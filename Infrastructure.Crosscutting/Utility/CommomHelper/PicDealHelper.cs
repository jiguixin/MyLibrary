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
    /// 图片水印处理类  
    /// </summary>  
    public class ImageWatermark
    {
        /* 
  调用很简单 im.SaveWatermark(原图地址, 水印地址, 透明度, 水印位置, 边距,保存位置);  
  ImageWatermark im = new ImageWatermark(); 
  im.SaveWatermark(Server.MapPath("/原图.jpg"), Server.MapPath("/水印.jpg"), 0.5f, ImageWatermark.WatermarkPosition.RigthBottom, 10, Server.MapPath("/原图.jpg")); 
 */

        #region 变量声明

        /// <summary>  
        /// 枚举: 水印位置  
        /// </summary>  
        public enum WatermarkPosition
        {
            /// <summary>  
            /// 左上  
            /// </summary>  
            LeftTop,

            /// <summary>  
            /// 左中  
            /// </summary>  
            Left,

            /// <summary>  
            /// 左下  
            /// </summary>  
            LeftBottom,

            /// <summary>  
            /// 正上  
            /// </summary>  
            Top,

            /// <summary>  
            /// 正中  
            /// </summary>  
            Center,

            /// <summary>  
            /// 正下  
            /// </summary>  
            Bottom,

            /// <summary>  
            /// 右上  
            /// </summary>  
            RightTop,

            /// <summary>  
            /// 右中  
            /// </summary>  
            RightCenter,

            /// <summary>  
            /// 右下  
            /// </summary>  
            RigthBottom
        }

        #endregion

        #region 私有函数

        /// <summary>  
        /// 获取: 图片去扩展名(包含完整路径及其文件名)小写字符串  
        /// </summary>  
        /// <param name="path">图片路径(包含完整路径,文件名及其扩展名): string</param>  
        /// <returns>返回: 图片去扩展名(包含完整路径及其文件名)小写字符串: string</returns>  
        private string GetFileName(string path)
        {
            return path.Remove(path.LastIndexOf('.')).ToLower();
        }

        /// <summary>  
        /// 获取: 图片以'.'开头的小写字符串扩展名  
        /// </summary>  
        /// <param name="path">图片路径(包含完整路径,文件名及其扩展名): string</param>  
        /// <returns>返回: 图片以'.'开头的小写字符串扩展名: string</returns>  
        private string GetExtension(string path)
        {
            return path.Remove(0, path.LastIndexOf('.')).ToLower();
        }

        /// <summary>  
        /// 获取: 图片以 '.' 开头的小写字符串扩展名对应的 System.Drawing.Imaging.ImageFormat 对象  
        /// </summary>  
        /// <param name="format">以 '. '开头的小写字符串扩展名: string</param>  
        /// <returns>返回: 图片以 '.' 开头的小写字符串扩展名对应的 System.Drawing.Imaging.ImageFormat 对象: System.Drawing.Imaging.ImageFormat</returns>  
        private ImageFormat GetImageFormat(string format)
        {
            switch (format)
            {
                case ".bmp":
                    return ImageFormat.Bmp;
                case ".emf":
                    return ImageFormat.Emf;
                case ".exif":
                    return ImageFormat.Exif;
                case ".gif":
                    return ImageFormat.Gif;
                case ".ico":
                    return ImageFormat.Icon;
                case ".png":
                    return ImageFormat.Png;
                case ".tif":
                    return ImageFormat.Tiff;
                case ".tiff":
                    return ImageFormat.Tiff;
                case ".wmf":
                    return ImageFormat.Wmf;
                default:
                    return ImageFormat.Jpeg;
            }
        }

        /// <summary>  
        /// 获取: 枚举 ImageWatermark.WatermarkPosition 对应的 System.Drawing.Rectangle 对象
        /// </summary>  
        /// <param name="positon">枚举 ImageWatermark.WatermarkPosition: ImageWatermark.WatermarkPosition</param>  
        /// <param name="X">原图宽度: int</param>  
        /// <param name="Y">原图高度: int</param>  
        /// <param name="x">水印宽度: int</param>  
        /// <param name="y">水印高度: int</param>  
        /// <param name="i">边距: int</param>  
        /// <returns>返回: 枚举 ImageWatermark.WatermarkPosition 对应的 System.Drawing.Rectangle 对象: System.Drawing.Rectangle</returns>  
        private Rectangle GetWatermarkRectangle(WatermarkPosition positon, int X, int Y, int x, int y, int i)
        {
            switch (positon)
            {
                case WatermarkPosition.LeftTop:
                    return new Rectangle(i, i, x, y);
                case WatermarkPosition.Left:
                    return new Rectangle(i, (Y - y) / 2, x, y);
                case WatermarkPosition.LeftBottom:
                    return new Rectangle(i, Y - y - i, x, y);
                case WatermarkPosition.Top:
                    return new Rectangle((X - x) / 2, i, x, y);
                case WatermarkPosition.Center:
                    return new Rectangle((X - x) / 2, (Y - y) / 2, x, y);
                case WatermarkPosition.Bottom:
                    return new Rectangle((X - x) / 2, Y - y - i, x, y);
                case WatermarkPosition.RightTop:
                    return new Rectangle(X - x - i, i, x, y);
                case WatermarkPosition.RightCenter:
                    return new Rectangle(X - x - i, (Y - y) / 2, x, y);
                default:
                    return new Rectangle(X - x - i, Y - y - i, x, y);
            }
        }

        #endregion

        #region 设置透明度

        /// <summary>  
        /// 设置: 图片 System.Drawing.Bitmap 对象透明度  
        /// </summary>  
        /// <param name="sBitmap">图片 System.Drawing.Bitmap 对象: System.Drawing.Bitmap</param>  
        /// <param name="transparence">水印透明度(值越高透明度越低,范围在0.0f~1.0f之间): float</param>  
        /// <returns>图片 System.Drawing.Bitmap: System.Drawing.Bitmap</returns>  
        public Bitmap SetTransparence(Bitmap bm, float transparence)
        {
            if (transparence == 0.0f || transparence == 1.0f) throw new ArgumentException("透明度值只能在0.0f~1.0f之间");
            float[][] floatArray =
                {
                    new float[] { 1.0f, 0.0f, 0.0f, 0.0f, 0.0f },
                    new float[] { 0.0f, 1.0f, 0.0f, 0.0f, 0.0f },
                    new float[] { 0.0f, 0.0f, 1.0f, 0.0f, 0.0f },
                    new float[] { 0.0f, 0.0f, 0.0f, transparence, 0.0f },
                    new float[] { 0.0f, 0.0f, 0.0f, 0.0f, 1.0f }
                };
            ImageAttributes imageAttributes = new ImageAttributes();
            imageAttributes.SetColorMatrix(new ColorMatrix(floatArray), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            Bitmap bitmap = new Bitmap(bm.Width, bm.Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.DrawImage(
                bm,
                new Rectangle(0, 0, bm.Width, bm.Height),
                0,
                0,
                bm.Width,
                bm.Height,
                GraphicsUnit.Pixel,
                imageAttributes);
            graphics.Dispose();
            imageAttributes.Dispose();
            bm.Dispose();
            return bitmap;
        }

        /// <summary>  
        ///  设置: 图片 System.Drawing.Bitmap 对象透明度  
        /// </summary>  
        /// <param name="readpath">图片路径(包含完整路径,文件名及其扩展名): string</param>  
        /// <param name="transparence">水印透明度(值越高透明度越低,范围在0.0f~1.0f之间): float</param>  
        /// <returns>图片 System.Drawing.Bitmap: System.Drawing.Bitmap</returns>  
        public Bitmap SetTransparence(string readpath, float transparence)
        {
            return SetTransparence(new Bitmap(readpath), transparence);
        }

        #endregion

        #region 公共方法

        /// < summary/>  
        /// 将byte转换成Image文件   
        /// < param name="mybyte">byte[]变量</param>    
        /// < returns></returns>  
        public Image SetByteToImage(byte[] mybyte)
        {
            Image image;
            using (var mymemorystream = new MemoryStream(mybyte, 0, mybyte.Length))
            {
                image = Image.FromStream(mymemorystream);
                return image;
            }
        }

        /// <summary> 
        /// Function to download Image from website 
        /// </summary> 
        /// <param name="_URL">URL address to download image</param> 
        /// <returns>Image</returns>
        public Image DownloadImage(string _URL)
        {
            Image _tmpImage = null;

            try
            {
                // Open a connection
                System.Net.HttpWebRequest _HttpWebRequest =
                    (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(_URL);

                _HttpWebRequest.AllowWriteStreamBuffering = true;

                // You can also specify additional header values like the user agent or the referer: (Optional)
                _HttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";

                _HttpWebRequest.Referer = "http://www.baidu.com/";

                // set timeout for 20 seconds (Optional)
                _HttpWebRequest.Timeout = 20000;

                // Request response:
                System.Net.WebResponse _WebResponse = _HttpWebRequest.GetResponse();

                // Open data stream:
                System.IO.Stream _WebStream = _WebResponse.GetResponseStream();

                // convert webstream to image

                _tmpImage = Image.FromStream(_WebStream);

                // Cleanup
                _WebResponse.Close();
                _WebResponse.Close();
            }

            catch (Exception _Exception)
            {
                // Error
                Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
                return null;
            }

            return _tmpImage;
        }

        #endregion

        #region 添加水印

        /// <summary>  
        /// 生成: 原图绘制水印的 System.Drawing.Bitmap 对象  
        /// </summary>  
        /// <param name="sBitmap">原图 System.Drawing.Bitmap 对象: System.Drawing.Bitmap</param>  
        /// <param name="wBitmap">水印 System.Drawing.Bitmap 对象: System.Drawing.Bitmap</param>  
        /// <param name="position">枚举 ImageWatermark.WatermarkPosition : ImageWatermark.WatermarkPosition</param>  
        /// <param name="margin">水印边距: int</param>  
        /// <returns>返回: 原图绘制水印的 System.Drawing.Bitmap 对象 System.Drawing.Bitmap</returns>  
        public Bitmap CreateWatermark(Bitmap sBitmap, Bitmap wBitmap, WatermarkPosition position, int margin)
        {
            Graphics graphics = Graphics.FromImage(sBitmap);
            graphics.DrawImage(
                wBitmap,
                GetWatermarkRectangle(position, sBitmap.Width, sBitmap.Height, wBitmap.Width, wBitmap.Height, margin));
            graphics.Dispose();
            wBitmap.Dispose();
            return sBitmap;
        }

        /// <summary>
        /// 添加文字水印
        /// </summary>
        /// <param name="sBitmap">源图片</param>
        /// <param name="text">水印文字</param>
        /// <param name="position">位置</param>
        /// <param name="margin">边距</param>
        /// <returns></returns>
        public Bitmap CreateWatermark(Bitmap sBitmap, string text, WatermarkPosition position, int margin)
        {
            Graphics graphics = Graphics.FromImage(sBitmap);

            graphics.DrawString(
                text,
                new Font("arial", 12f, FontStyle.Bold),
                new SolidBrush(Color.SandyBrown),
                GetWatermarkRectangle(position, sBitmap.Width, sBitmap.Height, sBitmap.Width / 2, 22, margin));

            graphics.Dispose();

            return sBitmap;
        }

        #endregion

        #region 保存图片到文件

        #region 普通保存

        /// <summary>  
        /// 保存: System.Drawing.Bitmap 对象到图片文件  
        /// </summary>  
        /// <param name="bitmap">System.Drawing.Bitmap 对象: System.Drawing.Bitmap</param>  
        /// <param name="writepath">保存路径(包含完整路径,文件名及其扩展名): string</param>  
        public void Save(Bitmap bitmap, string writepath)
        {
            try
            {
                bitmap.Save(writepath, GetImageFormat(GetExtension(writepath)));
                bitmap.Dispose();
            }
            catch
            {
                throw new ArgumentException("图片保存错误");
            }
        }

        /// <summary>  
        /// 保存: 对象到图片文件  
        /// </summary>  
        /// <param name="readpath">原图路径(包含完整路径,文件名及其扩展名): string</param>  
        /// <param name="writepath">保存路径(包含完整路径,文件名及其扩展名): string</param>  
        public void Save(string readpath, string writepath)
        {
            if (string.Compare(readpath, writepath) == 0) throw new ArgumentException("源图片与目标图片地址相同");
            try
            {
                Save(new Bitmap(readpath), writepath);
            }
            catch
            {
                throw new ArgumentException("图片读取错误");
            }
        }

        #endregion

        #region 透明度调整保存

        /// <summary>  
        /// 保存: 设置透明度的对象到图片文件  
        /// </summary>  
        /// <param name="sBitmap">图片 System.Drawing.Bitmap 对象: System.Drawing.Bitmap</param>  
        /// <param name="transparence">水印透明度(值越高透明度越低,范围在0.0f~1.0f之间): float</param>  
        /// <param name="writepath">保存路径(包含完整路径,文件名及其扩展名): string</param>  
        public void SaveTransparence(Bitmap bitmap, float transparence, string writepath)
        {
            Save(SetTransparence(bitmap, transparence), writepath);
        }

        /// <summary>  
        /// 保存: 设置透明度的象到图片文件  
        /// </summary>  
        /// <param name="readpath">原图路径(包含完整路径,文件名及其扩展名): string</param>  
        /// <param name="transparence">水印透明度(值越高透明度越低,范围在0.0f~1.0f之间): float</param>  
        /// <param name="writepath">保存路径(包含完整路径,文件名及其扩展名): string</param>  
        public void SaveTransparence(string readpath, float transparence, string writepath)
        {
            Save(SetTransparence(readpath, transparence), writepath);
        }

        #endregion

        #region 水印图片保存

        /// <summary>  
        /// 保存: 绘制水印的对象到图片文件  
        /// </summary>  
        /// <param name="sBitmap">原图 System.Drawing.Bitmap 对象: System.Drawing.Bitmap</param>  
        /// <param name="wBitmap">水印 System.Drawing.Bitmap 对象: System.Drawing.Bitmap</param>  
        /// <param name="position">枚举 ImageWatermark.WatermarkPosition : ImageWatermark.WatermarkPosition</param>  
        /// <param name="margin">水印边距: int</param>  
        /// <param name="writepath">保存路径(包含完整路径,文件名及其扩展名): string</param>  
        public void SaveWatermark(
            Bitmap sBitmap,
            Bitmap wBitmap,
            WatermarkPosition position,
            int margin,
            string writepath)
        {
            Save(CreateWatermark(sBitmap, wBitmap, position, margin), writepath);
        }

        /// <summary>  
        /// 保存: 绘制水印的对象到图片文件  
        /// </summary>  
        /// <param name="readpath">图片路径(包含完整路径,文件名及其扩展名): string</param>  
        /// <param name="watermarkpath">水印图片路径(包含完整路径,文件名及其扩展名): string</param>  
        /// <param name="transparence">水印透明度(值越高透明度越低,范围在0.0f~1.0f之间): float</param>  
        /// <param name="position">枚举 ImageWatermark.WatermarkPosition : ImageWatermark.WatermarkPosition</param>  
        /// <param name="margin">水印边距: int</param>  
        /// <param name="writepath">保存路径(包含完整路径,文件名及其扩展名): string</param>  
        public void SaveWatermark(
            string readpath,
            string watermarkpath,
            float transparence,
            WatermarkPosition position,
            int margin,
            string writepath)
        {
            if (string.Compare(readpath, writepath) == 0) throw new ArgumentException("源图片与目标图片地址相同");
            if (transparence == 0.0f) Save(readpath, writepath);
            else if (transparence == 1.0f) SaveWatermark(new Bitmap(readpath), new Bitmap(watermarkpath), position, margin, writepath);
            else
                SaveWatermark(
                    new Bitmap(readpath),
                    SetTransparence(watermarkpath, transparence),
                    position,
                    margin,
                    writepath);

        }

        /// <summary>  
        /// 保存: 绘制水印的对象到图片文件  
        /// </summary>  
        /// <param name="sBitmap">原图 System.Drawing.Bitmap 对象: System.Drawing.Bitmap</param>  
        /// <param name="text">水印 文字</param>  
        /// <param name="position">枚举 WatermarkPosition</param>  
        /// <param name="margin">水印边距: int</param>  
        /// <param name="writepath">保存路径(包含完整路径,文件名及其扩展名): string</param>  
        public void SaveWatermarkText(
            Bitmap sBitmap,
            string text,
            WatermarkPosition position,
            int margin,
            string writepath)
        {
            Save(CreateWatermark(sBitmap, text, position, margin), writepath);
        }


        #endregion

        #endregion
    }
}

 