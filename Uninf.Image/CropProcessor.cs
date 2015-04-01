using System.Drawing.Imaging;
using System.Linq;

namespace Uninf.Images
{
    using System.Drawing;
    using System.IO;

    using CodeCarvings.Piczard;
    /// <summary>
    /// 裁剪图片
    /// </summary>
    public class CropProcessor:IImageProcessor
    {
        private int width;

        private int height;


        public CropProcessor(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public Image Process(Image img)
        {
            var stream = new MemoryStream(); 
            new FixedCropConstraint(width, height).SaveProcessedImageToStream(img, stream);
            //FreeCropConstraint
            //FixedAspectRatioCropConstraint

            //CropConstraint cropconstraint = new FixedCropConstraint(width, height);
            //cropconstraint.DefaultImageSelectionStrategy = CropConstraintImageSelectionStrategy.Slice;
            //cropconstraint.SaveProcessedImageToStream(img, stream);
            return Image.FromStream(stream);
        }

        /// <summary>
        /// 裁剪图片
        /// </summary>
        /// <param name="img"></param>
        /// <param name="x">左上角x</param>
        /// <param name="y">左上角y</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <returns></returns>
        public Image Process(Image img,int x,int y,int width,int height)
        {
            return Process(img,GetCrop(x, y, width, height));
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

        public static ImageProcessingFilter GetCrop(int x, int y, int width, int height)
        {
            return new ImageCrop(x, y, width, height);
        }
    }
}