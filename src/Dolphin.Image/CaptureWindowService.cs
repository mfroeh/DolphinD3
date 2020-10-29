using Dolphin.Enum;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Dolphin.Image
{
    public class CaptureWindowService : ICaptureWindowService
    {
        private readonly IHandleService handleService;
        private readonly ILogService logService;

        public CaptureWindowService(IHandleService handleService, ILogService logService)
        {
            this.handleService = handleService;
            this.logService = logService;
        }

        public Bitmap CaptureWindow(string processName)
        {
            var hwnd = handleService.GetHandle(processName);

            return CaptureWindow(hwnd);
        }

        public Bitmap CaptureWindow(IntPtr hwnd)
        {
            if (hwnd == IntPtr.Zero)
            {
                logService.AddEntry(this, $"Got empty handle when trying to capture window.", LogLevel.Debug);

                return null;
            }

            var rect = WindowHelper.GetClientRect(hwnd);

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