using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;

namespace Unity
{
    public static class Utilities
    {
        public static Image ByteArrayToImage(byte[] imageByte)
        {
            Image image = null;
            try
            {
                image = Image.FromStream(new MemoryStream(imageByte));
            }
            catch
            {
            }
            return image;
        }

        public static string BytesToString(long byteCount)
        {
            string str;
            string[] strArrays = new string[] { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            string[] strArrays1 = strArrays;
            if (byteCount != 0L)
            {
                long num = Math.Abs(byteCount);
                int num1 = Convert.ToInt32(Math.Floor(Math.Log((double)num, 1024)));
                double num2 = Math.Round((double)num / Math.Pow(1024, (double)num1), 1);
                double num3 = (double)Math.Sign(byteCount) * num2;
                str = string.Concat(num3.ToString(), strArrays1[num1]);
            }
            else
            {
                str = string.Concat("0", strArrays1[0]);
            }
            return str;
        }

        public static string DateTo12HourTime(DateTime dt)
        {
            return dt.ToString("hh:mm:ss tt");
        }

        public static string DateTo24HourTime(DateTime dt)
        {
            return dt.ToString("HH:mm:ss");
        }

        public static byte[] ImageToByte(Image x)
        {
            return (byte[])(new ImageConverter()).ConvertTo(x, typeof(byte[]));
        }

        public static string ParseStringForIP(string ipAdress)
        {
            string str = "";
            MatchCollection matchCollections = (new Regex("\\b\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\b")).Matches(ipAdress);
            if (matchCollections[0] != null)
            {
                str = matchCollections[0].ToString();
            }
            return str;
        }

        public static Image resizeImage(int newWidth, int newHeight, string stPhotoPath)
        {
            return Utilities.resizeImage(newWidth, newHeight, Image.FromFile(stPhotoPath));
        }

        public static Image resizeImage(int newWidth, int newHeight, Image img)
        {
            Image image = img;
            int width = image.Width;
            int height = image.Height;
            if (width < height)
            {
                int num = newWidth;
                newWidth = newHeight;
                newHeight = num;
            }
            int num1 = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            float single = 0f;
            float single1 = 0f;
            float single2 = 0f;
            single1 = (float)newWidth / (float)width;
            single2 = (float)newHeight / (float)height;
            if (single2 >= single1)
            {
                single = single1;
                num4 = Convert.ToInt16(((float)newHeight - (float)height * single) / 2f);
            }
            else
            {
                single = single2;
                num3 = Convert.ToInt16(((float)newWidth - (float)width * single) / 2f);
            }
            int num5 = (int)((float)width * single);
            int num6 = (int)((float)height * single);
            Bitmap bitmap = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);
            bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            Graphics graphic = Graphics.FromImage(bitmap);
            graphic.Clear(Color.Black);
            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.DrawImage(image, new Rectangle(num3, num4, num5, num6), new Rectangle(num1, num2, width, height), GraphicsUnit.Pixel);
            graphic.Dispose();
            image.Dispose();
            return bitmap;
        }
    }
}