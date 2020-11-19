using System;
using System.Drawing;

namespace Dolphin
{
    public static class WindowInformationExtensionMethod
    {
        public static bool IsDefault(this WindowInformation info)
        {
            return info is null || info.Handle == default;
        }
    }

    public class WindowInformation
    {
        public Rectangle ClientRectangle { get; set; }

        public IntPtr Handle { get; set; }

        public uint ProcessId { get; set; }

        public string ProcessName { get; set; }
    }
}