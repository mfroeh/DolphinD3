using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Media.Imaging;

namespace Dolphin.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {

            new System.Windows.Application();
            var uri = new Uri(@"pack://application:,,,/Resource/test.png", UriKind.Absolute);
            var bitmap = new BitmapImage(uri);
            Console.WriteLine(uri.AbsolutePath);
            var watch = Stopwatch.StartNew();
            for (int i = 0; i < 100; i++)
            {
                var x = BitmapImage2Bitmap(bitmap);
            }
            watch.Stop();
            Trace.WriteLine(watch.ElapsedMilliseconds);
            Console.ReadLine();
        }

        private static Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            // BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return bitmap;
            }
        }
    }
}