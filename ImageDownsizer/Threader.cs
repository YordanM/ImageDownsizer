using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ImageDownsizer
{
    public static class Threader
    {
        static string workingDirectory = Environment.CurrentDirectory;
        static string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;

        public static void SingleThread(Bitmap originalImage, double downscalePercentage)
        {

            Bitmap resizedImage = Downsizer.PixelAveraging(originalImage, downscalePercentage);

            string outputPath = Path.Combine(projectDirectory, "images\\downsized_sequential.jpg");

            resizedImage.Save(outputPath, ImageFormat.Jpeg);
        }


        public static void MulthiThread(Bitmap originalImage, double downscalePercentage)
        {
            Bitmap[] smallerBitmaps = SplitBitmap(originalImage);

            Task<Bitmap>[] tasks = new Task<Bitmap>[4];

            tasks[0] = Task.Run(() => Downsizer.PixelAveraging(smallerBitmaps[0], downscalePercentage));
            tasks[1] = Task.Run(() => Downsizer.PixelAveraging(smallerBitmaps[1], downscalePercentage));
            tasks[2] = Task.Run(() => Downsizer.PixelAveraging(smallerBitmaps[2], downscalePercentage));
            tasks[3] = Task.Run(() => Downsizer.PixelAveraging(smallerBitmaps[3], downscalePercentage));

            Task.WaitAll(tasks);

            Bitmap[] downsizedBitmaps = new Bitmap[4];

            for (int i = 0; i < 4; i++)
            {
                downsizedBitmaps[i] = tasks[i].Result;
            }

            Bitmap largeBitmap = CombineBitmaps(downsizedBitmaps);

            string outputPath = Path.Combine(projectDirectory, "images\\downsized_parrallel.jpg");

            largeBitmap.Save(outputPath);
        }

        private static Bitmap[] SplitBitmap(Bitmap largeBitmap)
        {
            int width = largeBitmap.Width;
            int height = largeBitmap.Height;
            int halfWidth = (int)Math.Ceiling((double)width / 2);
            int halfHeight = (int)Math.Ceiling((double)height / 2);

            Bitmap[] resultBitmaps = new Bitmap[4];

            for (int i = 0; i < 4; i++)
            {
                resultBitmaps[i] = new Bitmap(halfWidth, halfHeight, PixelFormat.Format24bppRgb);
            }

            BitmapData originalData = largeBitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            BitmapData[] resultDatas = new BitmapData[4];
            for (int i = 0; i < 4; i++)
            {
                resultDatas[i] = resultBitmaps[i].LockBits(new Rectangle(0, 0, halfWidth, halfHeight), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int quadrant = (x < width / 2 ? 0 : 1) + (y < height / 2 ? 0 : 2); // determine quadrant

                    int largeIndex = y * originalData.Stride + x * 3;
                    int smallerX = x % halfWidth;
                    int smallerY = y % halfHeight;
                    int smallerIndex = smallerY * resultDatas[quadrant].Stride + smallerX * 3;

                    unsafe
                    {
                        byte* largePtr = (byte*)originalData.Scan0 + largeIndex;
                        byte* smallerPtr = (byte*)resultDatas[quadrant].Scan0 + smallerIndex;

                        smallerPtr[0] = largePtr[0]; // blue
                        smallerPtr[1] = largePtr[1]; // green
                        smallerPtr[2] = largePtr[2]; // red
                    }
                }
            }

            largeBitmap.UnlockBits(originalData);

            for (int i = 0; i < 4; i++)
            {
                resultBitmaps[i].UnlockBits(resultDatas[i]);
            }

            return resultBitmaps;
        }

        private static Bitmap CombineBitmaps(Bitmap[] smallerBitmaps)
        {
            int width = smallerBitmaps[0].Width * 2;
            int height = smallerBitmaps[0].Height * 2;

            Bitmap combinedBitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            BitmapData combinedData = combinedBitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            for (int i = 0; i < 4; i++)
            {
                Bitmap smallerBitmap = smallerBitmaps[i];
                BitmapData smallerData = smallerBitmap.LockBits(new Rectangle(0, 0, smallerBitmap.Width, smallerBitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                for (int y = 0; y < smallerBitmap.Height; y++)
                {
                    for (int x = 0; x < smallerBitmap.Width; x++)
                    {
                        int combinedX = (i % 2) * smallerBitmap.Width + x;
                        int combinedY = (i / 2) * smallerBitmap.Height + y;

                        int combinedIndex = combinedY * combinedData.Stride + combinedX * 3;
                        int smallerIndex = y * smallerData.Stride + x * 3;

                        unsafe
                        {
                            byte* combinedPtr = (byte*)combinedData.Scan0 + combinedIndex;
                            byte* smallerPtr = (byte*)smallerData.Scan0 + smallerIndex;

                            combinedPtr[0] = smallerPtr[0];
                            combinedPtr[1] = smallerPtr[1];
                            combinedPtr[2] = smallerPtr[2];
                        }
                    }
                }

                smallerBitmap.UnlockBits(smallerData);
            }

            combinedBitmap.UnlockBits(combinedData);

            return combinedBitmap;
        }
    }
}
