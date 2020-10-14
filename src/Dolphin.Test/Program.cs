using Dolphin.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Unity;
using Unity.Lifetime;

namespace Dolphin.Test
{
    internal static class Program
    {
        public static void Print<T>(this IEnumerable<T> obj, Func<T, bool> func)
        {
            foreach (var x in obj)
                if (func(x))
                    System.Console.WriteLine(x.ToString());
        }

        private static async Task<int> Main(string[] args)
        {
            var container = new UnityContainer();
            container.RegisterInstance<Log>(new Log());
            container.RegisterType<ILogService, LogService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICaptureWindowService, CaptureWindowService>();

            var logService = container.Resolve<ILogService>();
            logService.EntryAdded += OutputMessage;

            var handle = WindowHelper.GetHWND("Diablo III64");
            Console.WriteLine(handle);

            var captureWindowService = container.Resolve<ICaptureWindowService>();
            var picture = await captureWindowService.CaptureWindow(handle);
            picture.Save("picture.png");

            var hungeringArrowBitmap = new Bitmap("../hungeringArrow.jpeg");
            //var rect = new Rectangle { Height = temp.Height, Width = temp.Width };
            //var hungeringArrowBitmap = temp.Clone(rect, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < 6; i++)
            {
                var skillBitmap = GetSkillSlotBitmap(i, picture);
                for (int j = 0; j < 12; j++)
                {
                    Console.WriteLine($"Similarity: {ImageHelper.Compare(skillBitmap, hungeringArrowBitmap, 0)}");
                }
            }
            watch.Stop();
            Console.WriteLine($"Elapsed Miliseconds Aforge: {watch.ElapsedMilliseconds}");
            Console.ReadLine();

            logService.SaveLog("log.txt");

            return 1;
        }

        public static Bitmap CropImage(Bitmap source, Rectangle section)
        {
            var bitmap = new Bitmap(section.Width, section.Height);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);
                return bitmap;
            }
        }

        private static Bitmap GetSkillSlotBitmap(int index, Bitmap fullBitmap)
        {
            var size = new Size { Height = 85, Width = 85 };
            var point = new Point { X = 0, Y = 0 };
            switch (index)
            {
                case 0:
                    point = new Point { X = 835, Y = 1330 };
                    break;

                case 1:
                    point = new Point { X = 835 + 90, Y = 1330 };
                    break;

                case 2:
                    point = new Point { X = 835 + 2 * 90, Y = 1330 };
                    break;

                case 3:
                    point = new Point { X = 835 + 3 * 90, Y = 1330 };
                    break;

                case 4:
                    point = new Point { X = 835 + 4 * 90, Y = 1330 };
                    break;

                case 5:
                    point = new Point { X = 835 + 5 * 90, Y = 1330 };
                    break;
            }

            var rectangle = new Rectangle(point, size);
            return CropImage(fullBitmap, rectangle);
        }

        public static void OutputMessage(object sender, LogEntryEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}