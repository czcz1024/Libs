using System.Drawing.Imaging;
using System.Linq;

namespace Uninf.Images
{
    using System.Drawing;
    using System.IO;

    using CodeCarvings.Piczard;
    /// <summary>
    /// 调整图片尺寸
    /// </summary>
    public class ResizeProcessor:IImageProcessor
    {
        private ResizeConfig config;
        public ResizeProcessor(ResizeConfig config)
        {
            this.config = config;
        }

        public Image Process(Image img)
        {
            var stream = new MemoryStream();
            new ScaledResizeConstraint(280, 280).SaveProcessedImageToStream(img, stream);
            //FixedResizeConstraint
            return Image.FromStream(stream);
        }

        public Image Process(Image img, params ImageProcessingFilter[] filters)
        {
            if (!filters.Any()) return img;
            var stream = new MemoryStream();
            var job = new ImageProcessingJob();
            job.Filters.Add(filters);
            job.SaveProcessedImageToStream(img, stream);
            return Image.FromStream(stream);
        }

        public void Process(Image img,string savedir, ResizeConfig config, bool size=false)
        {
            var stream = new MemoryStream();
            var thumbWidth = config.ResizeToWidth;
            var thumbHeight = config.ResizeToHeight;
            //var imgHeight = img.Height;
            //var imgWidth = img.Width;
            //var resultWidth = thumbWidth < imgWidth ? thumbWidth : imgWidth;
            //var resultHeight = thumbHeight < imgHeight ? thumbHeight : imgHeight;
            var filename = size ? thumbWidth + "x" + thumbHeight : "";
            var resultWidth = thumbWidth;
            var resultHeight = thumbHeight;
            GetResultWidthAndHeight(img, config, out resultWidth, out resultHeight);

            
            if (img.Width <= resultWidth && img.Height <= resultHeight)
            {
                Upload(stream, savedir, filename);
                //return img;
            }
            
            if (config.ResizeType == ResizeType.FixWidthAndHeight)
            {
                new ScaledResizeConstraint(resultWidth, resultHeight).SaveProcessedImageToStream(img, stream);
                if (resultWidth > thumbWidth || resultHeight > thumbHeight)
                {
                    var twoimg = Image.FromStream(stream);
                    new FixedCropConstraint(thumbWidth, thumbHeight).SaveProcessedImageToStream(twoimg, stream);
                    Upload(stream, savedir, filename);
                    //return Image.FromStream(stream);
                }
                Upload(stream, savedir, filename);
                //return Image.FromStream(stream);
                
            }
            else
            {
                new ScaledResizeConstraint(resultWidth, resultHeight).SaveProcessedImageToStream(img, stream);
                Upload(stream, savedir, filename);
                //return Image.FromStream(stream);

            }

            
        }

        public static void GetResultWidthAndHeight(Image img, ResizeConfig config, out int resultWidth, out int resultHeight)
        {
            var thumbWidth = config.ResizeToWidth;
            var thumbHeight = config.ResizeToHeight;
            var imgHeight = img.Height;
            var imgWidth = img.Width;
            resultWidth = thumbWidth < imgWidth ? thumbWidth : imgWidth;
            resultHeight = thumbHeight < imgHeight ? thumbHeight : imgHeight;
            switch (config.ResizeType)
            {
                case ResizeType.ByMax:
                    if (imgWidth > imgHeight)
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
                    break;
                case ResizeType.ByMin:
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
                    break;
                case ResizeType.FixWidthAutoHeight:
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
                    break;
                case ResizeType.FixHeightAutoWidth:
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
                    break;
                case ResizeType.FixWidthAndHeight:
                    resultWidth = thumbWidth;
                    resultHeight = thumbHeight;
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="saveDir">原图保存地址</param>
        /// <param name="filename">缩略图尺寸信息（50x50）</param>
        /// <returns></returns>
        public string Upload(Stream stream, string saveDir, string filename)
        {

            var fileex = saveDir.Substring(saveDir.LastIndexOf(".", System.StringComparison.Ordinal));
            var savepath = saveDir.Substring(0, saveDir.LastIndexOf(".", System.StringComparison.Ordinal));
            var save = saveDir;
            if (!string.IsNullOrEmpty(filename))
            {
                save = savepath+"_"+filename+fileex;
            }
            stream.Position = 0;
            var buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            var fs = new FileStream(save, FileMode.OpenOrCreate, FileAccess.Write);
            try
            {
                fs.Write(buffer, 0, buffer.Length);
                fs.Flush();
            }
            finally
            {
                fs.Close();
            }

            return save;
        }

        //public Image Process(Image img, int width, int height, FixedResizeConstraintImagePosition position = FixedResizeConstraintImagePosition.Fit)
        //{
        //    //return Process(img, GetSize(width,height,position));
        //    //var frc = new FixedResizeConstraint(width, height);
        //    //if (config.ResizeToBigger)
        //    //{
        //    //    frc.EnlargeSmallImages = true;
        //    //    frc.ImagePosition = FixedResizeConstraintImagePosition.Fill;
        //    //}
        //    //else
        //    //{
        //    //    frc.ImagePosition = FixedResizeConstraintImagePosition.Fit;
        //    //    frc.CanvasColor = config.ResizeToBiggerColor;
        //    //}
        //    if (img.Width <= width && img.Height <= height)//小图片不处理
        //    {
        //        return img;
        //    }

        //    var src = new ScaledResizeConstraint(config.ResizeToWidth, config.ResizeToHeight);
        //    switch (config.ResizeType)
        //    {
        //        case ResizeType.ByMax:
        //            //ScaledResizeConstraint.GetMaxWidth(img.Width > img.Height ? img.Width : img.Height);
        //            if (img.Width >= img.Height)
        //            {
        //                var dd = ScaledResizeConstraint.GetMaxWidth(config.ResizeToWidth);
                        
        //            }
        //            else
        //            {
        //                ScaledResizeConstraint.GetMaxHeight(config.ResizeToHeight);
        //            }
        //            break;
        //        case ResizeType.ByMin:
        //            //ScaledResizeConstraint.GetMaxWidth(img.Width < img.Height ? img.Width : img.Height);
        //            if (img.Width >= img.Height)
        //            {
        //                ScaledResizeConstraint.GetMaxHeight(img.Height);
        //            }
        //            else
        //            {
        //                ScaledResizeConstraint.GetMaxWidth(img.Width);
        //            }
        //            break;
        //        case ResizeType.FixHeightAutoWidth:
        //            var fixheight = config.ResizeToHeight;
        //            ScaledResizeConstraint.GetMaxWidth(fixheight);
        //            break;
        //        case ResizeType.FixWidthAndHeight:
        //            var frc = new FixedResizeConstraint(width, height);
        //            if (config.ResizeToBigger)
        //            {
        //                frc.EnlargeSmallImages = true;
        //                frc.ImagePosition = FixedResizeConstraintImagePosition.Fill;
        //            }
        //            else
        //            {
        //                frc.ImagePosition = FixedResizeConstraintImagePosition.Fit;
        //                frc.CanvasColor = config.ResizeToBiggerColor;
        //            }
        //            break;
        //        case ResizeType.FixWidthAutoHeight:
        //            var fixwidth = config.ResizeToWidth;
        //            ScaledResizeConstraint.GetMaxWidth(fixwidth);
        //            break;
        //    }
        //    //frc.ImagePosition = position;
        //    return Process(img);
        //}

        //public ImageProcessingFilter GetSize(int width, int height, FixedResizeConstraintImagePosition position = FixedResizeConstraintImagePosition.Fit)
        //{
        //    var frc = new FixedResizeConstraint(width, height) {ImagePosition = position};
        //    return frc;
        //}

        public ImageProcessingFilter GetFixSize(int width, int height)
        {
            var frc = new FixedResizeConstraint(width, height);
            return frc;
        }

        public ImageProcessingFilter GetScaledSize()
        {
            return ScaledResizeConstraint.GetMaxWidth(100);

        }
    }
}