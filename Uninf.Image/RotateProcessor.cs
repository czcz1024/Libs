namespace Uninf.Images
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Linq;

    using CodeCarvings.Piczard;
    /// <summary>
    /// 旋转图片
    /// </summary>
    public class RotateProcessor:IImageProcessor
    {
        private int degree;

        public RotateProcessor(int degree)
        {
            if (!new[] { 0, 90, 180, 270 }.Contains(degree))
            {
                throw new Exception("角度只支持0,90,180,270");
            }
            this.degree = degree;

        }

        public Image Process(Image img)
        {
            var stream = new MemoryStream();
            new ImageTransformation(100, degree).SaveProcessedImageToStream(img, stream);
            return Image.FromStream(stream);
        }

        /// <summary>
        /// 指定旋转角度进行图片旋转
        /// </summary>
        /// <param name="img"></param>
        /// <param name="rotate">旋转角度</param>
        /// <returns></returns>
        public Image Process(Image img,int rotate)
        {
            this.degree = rotate;
            return this.Process(img);
        }

        
    }
}