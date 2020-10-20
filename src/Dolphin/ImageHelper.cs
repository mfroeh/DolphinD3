using AForge.Imaging;
using System.Drawing;
using System.Drawing.Imaging;

namespace Dolphin
{
    public static class ImageHelper
    {
        public static float Compare(Bitmap image, Bitmap template)
        {
            var result = new ExhaustiveTemplateMatching(0)
                            .ProcessImage(image, template)[0]
                            .Similarity;

            return result;
        }

        public static Bitmap CropImage(Bitmap source, Rectangle section)
        {
            var bitmap = new Bitmap(section.Width, section.Height, PixelFormat.Format24bppRgb);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);
            }

            return bitmap;
        }

        public static Bitmap To24bppRgbFormat(Bitmap img)
        {
            return img.Clone(new Rectangle(0, 0, img.Width, img.Height), PixelFormat.Format24bppRgb);
        }
    }
}