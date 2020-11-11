using Dolphin.Enum;
using System;
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

        private IDictionary<string, Graphics> graphics;
        private IDictionary<string, Bitmap> windowBitmaps;

        public CaptureWindowService(IHandleService handleService, ILogService logService)
        {
            this.handleService = handleService;
            this.logService = logService;

            windowBitmaps = new Dictionary<string, Bitmap>();
            graphics = new Dictionary<string, Graphics>();

            handleService.HandleStatusChanged += OnHandleChanged;
        }

        public Bitmap CaptureWindow(string processName)
        {
            var handle = handleService.GetHandle(processName);

            if (windowBitmaps.ContainsKey(processName) && !handle.IsDefault())
            {
                return CaptureWindow(handle.Handle, windowBitmaps[processName], graphics[processName]);
            }

            return CaptureWindow(handle.Handle);
        }

        // TODO: Make more performant. This eats most of the cpu usage.
        public Bitmap CaptureWindow(IntPtr hwnd)
        {
            var watch = Stopwatch.StartNew();
            if (hwnd == default)
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
            watch.Stop();
            Trace.WriteLine($"Image:{watch.ElapsedMilliseconds}");
            return bitmap;
        }

        public Bitmap CaptureWindow(IntPtr hwnd, Bitmap writtenTo, Graphics graphics)
        {
            var watch = Stopwatch.StartNew();

            var hdc = graphics.GetHdc();
            WindowHelper.PrintWindow(hwnd, hdc, 0);
            graphics.ReleaseHdc();

            watch.Stop();
            Trace.WriteLine($"Image:{watch.ElapsedMilliseconds}");

            return writtenTo;
        }

        private void OnHandleChanged(object o, HandleChangedEventArgs e)
        {
            if (!e.NewHandle.IsDefault())
            {
                var newHandle = e.NewHandle;
                var rect = newHandle.ClientRectangle;
                var bitmap = new Bitmap(rect.Width - rect.X, rect.Height - rect.Y, PixelFormat.Format24bppRgb);
                windowBitmaps[e.ProcessName] = bitmap;
                graphics[e.ProcessName] = Graphics.FromImage(bitmap);
            }
            else
            {
                windowBitmaps.Remove(e.ProcessName);
                graphics.Remove(e.ProcessName);
            }
        }
    }
}