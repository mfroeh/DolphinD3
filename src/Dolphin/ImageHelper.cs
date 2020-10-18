using AForge.Imaging;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace Dolphin
{
    public static class ImageHelper
    {
        public static async Task<Bitmap> CropImage(Bitmap source, Rectangle section)
        {
            var bitmap = new Bitmap(section.Width, section.Height, PixelFormat.Format24bppRgb);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);
                return bitmap;
            }
        }

        public static async Task<float> Compare(Bitmap image, Bitmap template, float threshold)
        {
            var result = new ExhaustiveTemplateMatching(threshold)
                .ProcessImage(image, template)[0]
                .Similarity;
            return result;
        }

        public static async Task<Bitmap> To24bppRgbFormat(Bitmap img)
        {
            return img.Clone(new Rectangle(0, 0, img.Width, img.Height), PixelFormat.Format24bppRgb);
        }
    }
}