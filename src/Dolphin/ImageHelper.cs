using AForge.Imaging;
using System.Drawing;
using System.Threading.Tasks;

namespace Dolphin
{
    public static class ImageHelper
    {
        public static async Task<Bitmap> CropImage(Bitmap source, Rectangle section)
        {
            var bitmap = new Bitmap(section.Width, section.Height);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);
                return bitmap;
            }
        }

        public static async Task<float> Compare(Bitmap image1, Bitmap image2, float threshold)
        {
            return new ExhaustiveTemplateMatching(threshold)
                .ProcessImage(await To24bppRgbFormat(image1), await To24bppRgbFormat(image2))[0]
                .Similarity;
        }

        public static async Task<Bitmap> To24bppRgbFormat(Bitmap img)
        {
            return img.Clone(new Rectangle(0, 0, img.Width, img.Height),
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        }
    }
}