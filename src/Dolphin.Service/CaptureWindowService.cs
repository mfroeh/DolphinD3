using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Dolphin.Service
{
    public class CaptureWindowService : ICaptureWindowService
    {
        private readonly ILogService logService;

        public CaptureWindowService(ILogService logService)
        {
            this.logService = logService;
        }

        public Bitmap CaptureWindow(string processName)
        {
            var hwnd = WindowHelper.GetHWND(processName);

            return CaptureWindow(hwnd);
        }

        public Bitmap CaptureWindow(IntPtr hwnd)
        {
            if (hwnd == IntPtr.Zero)
            {
                logService.AddEntry(this, $"Got empty handle when trying to capture window.");

                return null;
            }

            var rect = new Rectangle();
            WindowHelper.GetWindowRect(hwnd, ref rect);

            var bitmap = new Bitmap(rect.Width - rect.X, rect.Height - rect.Y, PixelFormat.Format24bppRgb);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                var dc = graphics.GetHdc();
                WindowHelper.PrintWindow(hwnd, dc, 0);

                graphics.ReleaseHdc();
            }

            return bitmap;
        }
    }
}