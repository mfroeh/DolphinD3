using System;
using System.Drawing;

namespace Dolphin
{
    public class WindowInformation
    {
        public Rectangle ClientRectangle { get; set; }

        public IntPtr Handle { get; set; }

        public uint ProcessId { get; set; }

        public string ProcessName { get; set; }

        public IntPtr GraphicsHdc { get; set; }

        public Graphics Graphics { get; set; }

        public Bitmap WindowBitmap { get; set; }
    }

    public static class WindowInformationExtensionMethod
    {
        public static bool IsDefault(this WindowInformation info)
        {
            return info is null || info.Handle == default;
        }
    }
}