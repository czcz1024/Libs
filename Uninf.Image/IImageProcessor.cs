namespace Uninf.Images
{
    using System.Drawing;

    public interface IImageProcessor
    {
        Image Process(Image img);
    }
}