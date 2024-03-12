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
    }
}
