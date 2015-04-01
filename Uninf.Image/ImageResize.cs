// ***********************************************************************
// Assembly         : Uninf.Images
// Author           : cz
// Created          : 01-21-2015
//
// Last Modified By : cz
// Last Modified On : 01-22-2015
// ***********************************************************************
// <copyright file="ImageResize.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.IO;
using System.Web;
using Jillzhang.GifUtility;

namespace Uninf.Images
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Linq;

    using CodeCarvings.Piczard;

    using Uninf.Upload;

    /// <summary>
    /// ImageResize. 类
    /// </summary>
    public class ImageResize
    {
        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="oldImgPath">源文件路径</param>
        /// <param name="saveRootDir">保存到的文件夹</param>
        /// <param name="saveFileName">保存的文件名</param>
        /// <param name="uploader">保存图片用的uploader</param>
        /// <param name="configs">缩放设置</param>
        /// <returns>文件存放地址</returns>
        /// <exception cref="System.Exception">原图不正确</exception>
        public virtual string Resize(
            string oldImgPath,
            string saveRootDir,
            string saveFileName,
            IUpload uploader,
            params ResizeConfig[] configs)
        {
            var oldImg = Image.FromFile(oldImgPath);
            if (oldImg == null) throw new Exception("原图不正确");

            return Resize(oldImg, saveRootDir, saveFileName, uploader, configs);
        }

        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="oldImg">原图</param>
        /// <param name="saveRootDir">保存到的文件夹</param>
        /// <param name="saveFileName">保存的文件名</param>
        /// <param name="uploader">保存图片用的uploader</param>
        /// <param name="configs">缩放设置</param>
        /// <returns>文件存放地址</returns>
        public virtual string Resize(Image oldImg, string saveRootDir, string saveFileName, IUpload uploader, params ResizeConfig[] configs)
        {
            if (!configs.Any())
            {
                return uploader.Upload(oldImg, saveRootDir, saveFileName, GetImageFormat(oldImg));
            }
            var result = "";
            var compute = true;
            var dir = saveRootDir;
            var saveOld = false;
            foreach (var config in configs)
            {
                if (config.ResizeToWidth < 0 || config.ResizeToHeight < 0)
                {
                    saveOld = true;
                }
                else
                {
                    var resizeName = "_" + config.ResizeToWidth + "x" + config.ResizeToHeight;
                    var resultName = saveFileName.Insert(saveFileName.LastIndexOf("."), resizeName);

                    var newimg = ResizeProcess(oldImg, config);

                    result = uploader.Upload(newimg, dir, resultName, GetImageFormat(oldImg), compute);
                    compute = false;
                    result = result.Replace(resizeName, "");
                    dir = result.Substring(0, result.LastIndexOf(@"\"));
                }
            }
            if (saveOld)
            {
                uploader.Upload(oldImg, dir, saveFileName, GetImageFormat(oldImg), compute);
            }

            var resultPath = result;
            return resultPath;
        }

        /// <summary>
        /// Resizes the process.
        /// </summary>
        /// <param name="old">原图</param>
        /// <param name="config">缩放设置</param>
        /// <returns>缩略图</returns>
        public virtual Image ResizeProcess(Image old, ResizeConfig config)
        {
            //执行缩略，并生成新图

            //不允许缩略小图
            if (!config.ResizeToBigger && old.Width <= config.ResizeToWidth && old.Height <= config.ResizeToHeight)
            {
                return old;
            }
            if (config.AllowGif && GetImageFormat(old).Equals(ImageFormat.Gif))
            {
                var oldwith = old.Width;
                var oldheight = old.Height;
                double r = 1.0D;
                if (oldwith > oldheight)
                {
                    r = Convert.ToDouble(config.ResizeToWidth) / Convert.ToDouble(oldwith);
                }
                else
                {
                    r = Convert.ToDouble(config.ResizeToHeight) / Convert.ToDouble(oldheight);
                }
                return ResizeGif(old, r);
            }
            switch (config.ResizeType)
            {
                case ResizeType.FixWidthAndHeight:
                    return FixWidthAndHeight(old, config);
                    break;
                case ResizeType.FixWidthAutoHeight:
                    return FixWidthAutoHeight(old, config);
                    break;
                case ResizeType.FixHeightAutoWidth:
                    return FixHeightAutoWidth(old, config);
                    break;
                case ResizeType.ByMax:
                    return ByMax(old, config);
                    break;
                case ResizeType.ByMin:
                    return ByMin(old, config);
                    break;
            }

            return old;
        }

        /// <summary>
        /// 旋转
        /// </summary>
        /// <param name="old">原图</param>
        /// <param name="degree">角度</param>
        /// <returns>Image.</returns>
        /// <exception cref="System.Exception">角度只支持0,90,180,270</exception>
        public virtual Image Rotate(Image old, int degree)
        {
            if (!new[] { 0, 90, 180, 270 }.Contains(degree))
            {
                throw new Exception("角度只支持0,90,180,270");
            }
            return new ImageTransformation(100, degree).GetProcessedImage(old);
        }

        /// <summary>
        /// Bies the minimum.
        /// </summary>
        /// <param name="old">The old.</param>
        /// <param name="config">The configuration.</param>
        /// <returns>Image.</returns>
        protected Image ByMin(Image old, ResizeConfig config)
        {
            var resultWidth = 0;
            var resultHeight = 0;
            var thumbWidth = config.ResizeToWidth;
            var thumbHeight = config.ResizeToHeight;
            var imgHeight = old.Height;
            var imgWidth = old.Width;

            if (imgWidth > imgHeight)
            {
                if (imgHeight > thumbHeight)
                {
                    resultHeight = thumbHeight;
                    resultWidth = imgWidth * thumbHeight / imgHeight;
                }
                else if (imgWidth > thumbWidth)
                {
                    resultWidth = thumbWidth;
                    resultHeight = imgHeight * thumbWidth / imgWidth;
                }
                else
                {
                    resultWidth = imgWidth;
                    resultHeight = imgHeight;
                }
            }
            else
            {
                if (imgWidth > thumbWidth)
                {
                    resultWidth = thumbWidth;
                    resultHeight = imgHeight * thumbWidth / imgWidth;
                }
                else
                {
                    resultWidth = imgWidth;
                    resultHeight = imgHeight;
                }
            }
            return new ScaledResizeConstraint(resultWidth, resultHeight).GetProcessedImage(old);
        }

        /// <summary>
        /// Bies the maximum.
        /// </summary>
        /// <param name="old">The old.</param>
        /// <param name="config">The configuration.</param>
        /// <returns>Image.</returns>
        protected Image ByMax(Image old, ResizeConfig config)
        {
            var resultWidth = 0;
            var resultHeight = 0;
            var thumbWidth = config.ResizeToWidth;
            var thumbHeight = config.ResizeToHeight;
            var imgHeight = old.Height;
            var imgWidth = old.Width;


            if (old.Width > old.Height)
            {
                if (old.Width > config.ResizeToWidth)
                {
                    resultWidth = thumbWidth;
                    resultHeight = imgHeight * thumbWidth / imgWidth;
                }
                else
                {
                    resultWidth = imgWidth;
                    resultHeight = imgHeight;
                }
            }
            else
            {
                if (imgHeight > thumbHeight)
                {
                    resultHeight = thumbHeight;
                    resultWidth = imgWidth * thumbHeight / imgHeight;
                }
                else if (imgWidth > thumbWidth)
                {
                    resultWidth = thumbWidth;
                    resultHeight = imgHeight * thumbWidth / imgWidth;
                }
                else
                {
                    resultWidth = imgWidth;
                    resultHeight = imgHeight;
                }
            }
            return new ScaledResizeConstraint(resultWidth, resultHeight).GetProcessedImage(old);
        }

        /// <summary>
        /// Fixes the width of the height automatic.
        /// </summary>
        /// <param name="old">The old.</param>
        /// <param name="config">The configuration.</param>
        /// <returns>Image.</returns>
        protected Image FixHeightAutoWidth(Image old, ResizeConfig config)
        {
            //throw new NotImplementedException();
            if (old.Height <= config.ResizeToHeight)
                return old;
            else
            {
                var thumbh = config.ResizeToHeight;
                var thumbw = old.Width*thumbh/old.Height;
                return new ScaledResizeConstraint(thumbw, thumbh).GetProcessedImage(old);
            }
        }

        /// <summary>
        /// Fixes the height of the width automatic.
        /// </summary>
        /// <param name="old">The old.</param>
        /// <param name="config">The configuration.</param>
        /// <returns>Image.</returns>
        protected Image FixWidthAutoHeight(Image old, ResizeConfig config)
        {
            //throw new NotImplementedException();
            if (old.Width <= config.ResizeToWidth)
                return old;
            else
            {
                var thumbw = config.ResizeToWidth;
                var thumbh = old.Height*thumbw/old.Width;
                return new ScaledResizeConstraint(thumbw, thumbh).GetProcessedImage(old);
            }
        }

        /// <summary>
        /// Fixes the height of the width and.
        /// </summary>
        /// <param name="old">The old.</param>
        /// <param name="config">The configuration.</param>
        /// <returns>Image.</returns>
        protected Image FixWidthAndHeight(Image old, ResizeConfig config)
        {
            var img = new ScaledResizeConstraint(config.ResizeToWidth, config.ResizeToHeight).GetProcessedImage(old);
            return img;
        }

        /// <summary>
        /// Resizes the GIF.
        /// </summary>
        /// <param name="old">The old.</param>
        /// <param name="rate">The rate.</param>
        /// <returns>Image.</returns>
        protected Image ResizeGif(Image old, double rate)
        {
            //throw new NotImplementedException();
            var path = HttpContext.Current.Server.MapPath("");
            if (Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var rnd = new Random();
            var name = DateTime.Now.ToString("yyyyMMddhhmmssffff");
            for (var i = 0; i < 5; i++)
            {
                name += rnd.Next(9);
            }
            
            var filename = path + name + ".gif";
            var filenewname = path + name + "new.gif";
            old.Save(filename);
            new GifHelper().GetThumbnail(filename, rate, filenewname);
            var ms = new MemoryStream(File.ReadAllBytes(path));
            var img = Image.FromStream(ms);
            return img;
        }

        /// <summary>
        /// Gets the image format.
        /// </summary>
        /// <param name="img">The img.</param>
        /// <returns>System.Drawing.Imaging.ImageFormat.</returns>
        public static System.Drawing.Imaging.ImageFormat GetImageFormat(System.Drawing.Image img)
        {
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
                return System.Drawing.Imaging.ImageFormat.Jpeg;
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Bmp))
                return System.Drawing.Imaging.ImageFormat.Bmp;
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
                return System.Drawing.Imaging.ImageFormat.Png;
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Emf))
                return System.Drawing.Imaging.ImageFormat.Emf;
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Exif))
                return System.Drawing.Imaging.ImageFormat.Exif;
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Gif))
                return System.Drawing.Imaging.ImageFormat.Gif;
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Icon))
                return System.Drawing.Imaging.ImageFormat.Icon;
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.MemoryBmp))
                return System.Drawing.Imaging.ImageFormat.MemoryBmp;
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Tiff))
                return System.Drawing.Imaging.ImageFormat.Tiff;
            else
                return System.Drawing.Imaging.ImageFormat.Wmf;
        }
    }
}