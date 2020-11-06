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
    }
}