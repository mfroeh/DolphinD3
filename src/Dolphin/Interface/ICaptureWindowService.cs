using Dolphin.Enum;
using System;
using System.Drawing;

namespace Dolphin
{
    public interface ICaptureWindowService
    {
        Bitmap CaptureWindow(string processName);

        Bitmap CaptureWindow(IntPtr hwnd);
    }
}