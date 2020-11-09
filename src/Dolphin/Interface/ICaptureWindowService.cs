using System;
using System.Drawing;

namespace Dolphin
{
    public interface ICaptureWindowService
    {
        event EventHandler<ImageUpdatedEventArgs> ImageUpdated;

        Bitmap CaptureWindow(string processName);

        Bitmap CaptureWindow(IntPtr hwnd);

        bool UpdateImage(string processName);

        bool UpdateImage(IntPtr hwnd);
    }
}