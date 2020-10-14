using System;
using System.Drawing;
using System.Threading.Tasks;

namespace Dolphin
{
    public class CaptureWindowService : ICaptureWindowService
    {
        private readonly ILogService logService;

        public CaptureWindowService(ILogService logService)
        {
            this.logService = logService;
        }

        public async Task<Bitmap> CaptureWindow(string processName)
        {
            var hwnd = WindowHelper.GetHWND(processName);
            return await CaptureWindow(hwnd);
        }

        public async Task<Bitmap> CaptureWindow(IntPtr hwnd)
        {
            if (hwnd != IntPtr.Zero)
            {
                logService.AddEntry(this, $"Got handle ({hwnd})");
                var rect = new Rectangle();
                WindowHelper.GetWindowRect(hwnd, ref rect);

                var bitmap = new Bitmap(rect.Width - rect.X, rect.Height - rect.Y);

                using (var graphics = Graphics.FromImage(bitmap))
                {
                    var dc = graphics.GetHdc();
                    WindowHelper.PrintWindow(hwnd, dc, 0);

                    graphics.ReleaseHdc();
                }
                //var grayBitmap = Grayscale.CommonAlgorithms.BT709.Apply(bitmap);
                return bitmap;
            }
            logService.AddEntry(this, $"Got empty handle when trying to capture window.");
            return null;
        }
    }
}