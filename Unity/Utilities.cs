using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;

namespace Unity
{
    public static class Utilities
    {
        
        public static string BytesToString(long byteCount)
        {
            string[] strArray = new string[7]
            {
                "Unity_Utilities_unknown1",
                "Unity_Utilities_unknown2",
                "Unity_Utilities_unknown3",
                "Unity_Utilities_unknown4",
                "Unity_Utilities_unknown5",
                "Unity_Utilities_unknown6",
                "Unity_Utilities_unknown7"
            };
            if (byteCount == 0L)
                return "Unity_Utilities_unknown8" + strArray[0];
            long num1 = Math.Abs(byteCount);
            int int32 = Convert.ToInt32(Math.Floor(Math.Log((double)num1, 1024.0)));
            double num2 = Math.Round((double)num1 / Math.Pow(1024.0, (double)int32), 1);
            return ((double)Math.Sign(byteCount) * num2).ToString() + strArray[int32];
        }

        
        public static string ParseStringForIP(string ipAdress)
        {
            string str = "";
            MatchCollection matchCollection = new Regex("Unity_Utilities_unknown9").Matches(ipAdress);
            if (matchCollection[0] != null)
                str = matchCollection[0].ToString();
            return str;
        }

        
        public static string DateTo24HourTime(DateTime dt)
        {
            return dt.ToString("Unity_Utilities_unknown10");
        }

        
        public static string DateTo12HourTime(DateTime dt)
        {
            return dt.ToString("Unity_Utilities_unknown11");
        }

        
        public static Image resizeImage(int newWidth, int newHeight, string stPhotoPath)
        {
            Image img = Image.FromFile(stPhotoPath);
            return resizeImage(newWidth, newHeight, img);
        }

        
        public static Image resizeImage(int newWidth, int newHeight, Image img)
        {
            Image image = img;
            int width1 = image.Width;
            int height1 = image.Height;
            if (width1 < height1)
            {
                int num = newWidth;
                newWidth = newHeight;
                newHeight = num;
            }
            int x1 = 0;
            int y1 = 0;
            int x2 = 0;
            int y2 = 0;
            float num1 = newWidth / (float)width1;
            float num2 = newHeight / (float)height1;
            float num3;
            if ((double)num2 < (double)num1)
            {
                num3 = num2;
                x2 = (int)Convert.ToInt16((float)(((double)newWidth - (double)width1 * (double)num3) / 2.0));
            }
            else
            {
                num3 = num1;
                y2 = (int)Convert.ToInt16((float)(((double)newHeight - (double)height1 * (double)num3) / 2.0));
            }
            int width2 = (int)((double)width1 * (double)num3);
            int height2 = (int)((double)height1 * (double)num3);
            Bitmap bitmap = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);
            bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            Graphics graphics = Graphics.FromImage((Image)bitmap);
            graphics.Clear(Color.Black);
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.DrawImage(image, new Rectangle(x2, y2, width2, height2), new Rectangle(x1, y1, width1, height1), GraphicsUnit.Pixel);
            graphics.Dispose();
            image.Dispose();
            return (Image)bitmap;
        }

        
        public static byte[] ImageToByte(Image x)
        {
            return (byte[])new ImageConverter().ConvertTo((object)x, typeof(byte[]));
        }

        
        public static Image ByteArrayToImage(byte[] imageByte)
        {
            Image image = (Image)null;
            try
            {
                image = Image.FromStream((Stream)new MemoryStream(imageByte));
            }
            catch
            {
            }
            return image;
        }
    }
}
