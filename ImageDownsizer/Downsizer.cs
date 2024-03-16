using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ImageDownsizer
{
    public static class Downsizer
    {
        private static Color[,] GetPixels(Bitmap bitmap)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;

            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            IntPtr ptr = bmpData.Scan0;

            int bytes = Math.Abs(bmpData.Stride) * height;
            byte[] rgbValues = new byte[bytes];

            Marshal.Copy(ptr, rgbValues, 0, bytes);

            bitmap.UnlockBits(bmpData);

            Color[,] pixels = new Color[width, height];

            int stride = bmpData.Stride;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * stride + 3 * x;
                    byte b = rgbValues[index];
                    byte g = rgbValues[index + 1];
                    byte r = rgbValues[index + 2];
                    pixels[x, y] = Color.FromArgb(r, g, b);
                }
            }
            return pixels;
        }

        private static Bitmap GetBitmap(Color[,] pixels)
        {
            int width = pixels.GetLength(0);
            int height = pixels.GetLength(1);

            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            IntPtr ptr = bmpData.Scan0;

            int stride = bmpData.Stride;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * stride + 3 * x;
                    Color color = pixels[x, y];
                    byte[] pixel = { color.B, color.G, color.R };
                    Marshal.Copy(pixel, 0, ptr + index, 3);
                }
            }

            bitmap.UnlockBits(bmpData);

            return bitmap;
        }


        private static byte Interpolate(byte a, byte b, byte c, byte d, double xWeight, double yWeight)
        {
            return (byte)(a * (1 - xWeight) * (1 - yWeight) +
                           b * xWeight * (1 - yWeight) +
                           c * (1 - xWeight) * yWeight +
                           d * xWeight * yWeight);
        }


        // used source https://stackoverflow.com/questions/50346821/c-sharp-bilinear-interpolation-rgb
        // using bilinear interpolation but it is not appropriate for downscaling more than 50%
        public static Bitmap Bilinear(Bitmap originalBitmap, double factor)
        {
            Color[,] original = GetPixels(originalBitmap);

            int width = (int)(original.GetLength(0) * factor / 100);
            int hight = (int)(original.GetLength(1) * factor / 100);

            Color[,] result = new Color[width, hight];

            double scaleX = (double)original.GetLength(0) / result.GetLength(0);
            double scaleY = (double)original.GetLength(1) / result.GetLength(1);

            for (int y = 0; y < result.GetLength(1); y++)
            {
                for (int x = 0; x < result.GetLength(0); x++)
                {
                    int originalX = (int)(x * scaleX);
                    int originalY = (int)(y * scaleY);

                    Color topLeft = original[originalX, originalY];

                    Color topRight = originalX + 1 >= original.GetLength(0)
                        ? topLeft : original[originalX + 1, originalY];

                    Color bottomLeft = originalY + 1 >= original.GetLength(1)
                        ? topLeft : original[originalX, originalY + 1];

                    Color bottomRight = (originalX + 1 >= original.GetLength(0) || originalY + 1 >= original.GetLength(1))
                        ? topLeft : original[originalX + 1, originalY + 1];

                    double xWeight = (x * scaleX) - originalX;
                    double yWeight = (y * scaleY) - originalY;

                    byte red = Interpolate(topLeft.R, topRight.R, bottomLeft.R, bottomRight.R, xWeight, yWeight);
                    byte green = Interpolate(topLeft.G, topRight.G, bottomLeft.G, bottomRight.G, xWeight, yWeight);
                    byte blue = Interpolate(topLeft.B, topRight.B, bottomLeft.B, bottomRight.B, xWeight, yWeight);

                    result[x, y] = Color.FromArgb(red, green, blue);
                }
            }

            return GetBitmap(result);
        }


        // using pixel averaging algorithm
        //https://stackoverflow.com/questions/1068373/how-to-calculate-the-average-rgb-color-values-of-a-bitmap
        // this shows us how to calculate the average color the whole bitmap
        // in my solution I reworked it to work with 2x2 matrix of colors
        // this creating a new pixel with the average color and adding it to the new image
        public static Bitmap PixelAveraging(Bitmap originalBitmap, double downscalePercentage)
        {
            Color[,] originalPixels = GetPixels(originalBitmap);

            int newWidth = (int)(originalPixels.GetLength(0) * downscalePercentage / 100);
            int newHeight = (int)(originalPixels.GetLength(1) * downscalePercentage / 100);

            Color[,] resizedPixels = new Color[newWidth, newHeight];

            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    // get 2x2 from original image
                    int originalXStart = x * originalPixels.GetLength(0) / newWidth;
                    int originalXEnd = (x + 1) * originalPixels.GetLength(0) / newWidth;
                    int originalYStart = y * originalPixels.GetLength(1) / newHeight;
                    int originalYEnd = (y + 1) * originalPixels.GetLength(1) / newHeight;

                    int redSum = 0;
                    int greenSum = 0;
                    int blueSum = 0;
                    int pixelCount = 0;

                    for (int originalY = originalYStart; originalY < originalYEnd; originalY++)
                    {
                        for (int originalX = originalXStart; originalX < originalXEnd; originalX++)
                        {
                            Color originalColor = originalPixels[originalX, originalY];
                            redSum += originalColor.R;
                            greenSum += originalColor.G;
                            blueSum += originalColor.B;
                            pixelCount++;
                        }
                    }

                    int averageRed = redSum / pixelCount;
                    int averageGreen = greenSum / pixelCount;
                    int averageBlue = blueSum / pixelCount;

                    resizedPixels[x, y] = Color.FromArgb(averageRed, averageGreen, averageBlue);
                }
            }

            return GetBitmap(resizedPixels);
        }
    }
}
