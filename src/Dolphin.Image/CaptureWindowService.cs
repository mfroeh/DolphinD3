using Dolphin.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace Dolphin.Image
{
    public class CaptureWindowService : ICaptureWindowService
    {
        private readonly IHandleService handleService;
        private readonly ILogService logService;
        public event EventHandler<ImageUpdatedEventArgs> ImageUpdated;

        public CaptureWindowService(IHandleService handleService, ILogService logService)
        {
            this.handleService = handleService;
            this.logService = logService;
        }

        public Bitmap CaptureWindow(string processName)
        {
            var handle = handleService.GetHandle(processName);
            if (handle?.Handle == default) return default;

            return CaptureWindow(handle.Handle);
        }

        public bool UpdateImage(string processName)
        {
            var handle = handleService.GetHandle(processName);
            if (handle?.Handle == default) return default;

            return UpdateImage(handle.Handle);
        }

        public bool UpdateImage(IntPtr hwnd)
        {
            var image = CaptureWindow(hwnd);

            ImageUpdated?.Invoke(this, new ImageUpdatedEventArgs { Handle = hwnd, NewImage = image });

            return image != default;
        }

        // TODO: Make more performant. This eats most of the performance.
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