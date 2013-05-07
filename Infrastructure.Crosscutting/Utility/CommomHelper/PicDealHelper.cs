using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using Infrastructure.Crosscutting.Declaration;

namespace Infrastructure.Crosscutting.Utility.CommomHelper
{
    /// <summary>
    /// 操作图片类, 生成缩略图,添加水印
    /// </summary>
    public static class PicDealHelper
    {
        private static Hashtable htmimes = new Hashtable();
        internal static readonly string AllowExt = ".jpe|.jpeg|.jpg|.png|.tif|.tiff|.bmp";

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static bool MakeThumbnail(string originalImagePath, int width, int height, ThumbnailMod mode)
        {
            string filename = originalImagePath.Substring(0, originalImagePath.LastIndexOf('.')) + "s.jpg";
            Image image = Image.FromFile(originalImagePath);
            int num = width;
            int num2 = height;
            int x = 0;
            int y = 0;
            int num3 = image.Width;
            int num4 = image.Height;
            switch (mode)
            {
                case ThumbnailMod.W:
                    {
                        num2 = image.Height * width / image.Width;
                        break;
                    }
                case ThumbnailMod.H:
                    {
                        num = image.Width * height / image.Height;
                        break;
                    }
                case ThumbnailMod.Cut:
                    {
                        if ((double)image.Width / (double)image.Height > (double)num / (double)num2)
                        {
                            num4 = image.Height;
                            num3 = image.Height * num / num2;
                            y = 0;
                            x = (image.Width - num3) / 2;
                        }
                        else
                        {
                            num3 = image.Width;
                            num4 = image.Width * height / num;
                            x = 0;
                            y = (image.Height - num4) / 2;
                        }
                        break;
                    }
            }
            Image image2 = new Bitmap(num, num2);
            Graphics graphics = Graphics.FromImage(image2);
            graphics.InterpolationMode = InterpolationMode.High;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.Clear(Color.Transparent);
            graphics.DrawImage(image, new Rectangle(0, 0, num, num2), new Rectangle(x, y, num3, num4), GraphicsUnit.Pixel);
            bool result = false;
            try
            {
                image2.Save(filename, ImageFormat.Jpeg);
                result = true;
            }
            catch (Exception)
            {
            }
            finally
            {
                image.Dispose();
                image2.Dispose();
                graphics.Dispose();
            }
            return result;
        }

        /// <summary>
        /// 在图片上生成图片水印
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_sypf">水印图片路径</param>
        public static void AddWaterPic(string Path, string Path_sypf)
        {
            try
            {
                Image image = Image.FromFile(Path);
                Image image2 = Image.FromFile(Path_sypf);
                Graphics graphics = Graphics.FromImage(image);
                graphics.DrawImage(image2, new Rectangle(image.Width - image2.Width, image.Height - image2.Height, image2.Width, image2.Height), 0, 0, image2.Width, image2.Height, GraphicsUnit.Pixel);
                graphics.Dispose();
                image.Save(Path + ".temp");
                image.Dispose();
                File.Delete(Path);
                File.Move(Path + ".temp", Path);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 公共方法
        /// </summary>
        private static void GetImgType()
        {
            PicDealHelper.htmimes[".jpe"] = "image/jpeg";
            PicDealHelper.htmimes[".jpeg"] = "image/jpeg";
            PicDealHelper.htmimes[".jpg"] = "image/jpeg";
            PicDealHelper.htmimes[".png"] = "image/png";
            PicDealHelper.htmimes[".tif"] = "image/tiff";
            PicDealHelper.htmimes[".tiff"] = "image/tiff";
            PicDealHelper.htmimes[".bmp"] = "image/bmp";
        }

        /// <summary>
        /// 返回新图片尺寸
        /// </summary>
        /// <param name="width">原始宽</param>
        /// <param name="height">原始高</param>
        /// <param name="maxWidth">新图片最大宽</param>
        /// <param name="maxHeight">新图片最大高</param>
        /// <returns></returns>
        public static Size ResizeImage(int width, int height, int maxWidth, int maxHeight)
        {
            decimal num = maxWidth;
            decimal d = maxHeight;
            decimal d2 = num / d;
            decimal d3 = width;
            decimal num2 = height;
            int width2;
            int height2;
            if (d3 > num || num2 > d)
            {
                if (d3 / num2 > d2)
                {
                    decimal d4 = d3 / num;
                    width2 = Convert.ToInt32(d3 / d4);
                    height2 = Convert.ToInt32(num2 / d4);
                }
                else
                {
                    decimal d4 = num2 / d;
                    width2 = Convert.ToInt32(d3 / d4);
                    height2 = Convert.ToInt32(num2 / d4);
                }
            }
            else
            {
                width2 = width;
                height2 = height;
            }
            return new Size(width2, height2);
        }
    }
}

 