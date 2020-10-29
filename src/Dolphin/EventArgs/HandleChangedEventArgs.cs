using System;

namespace Dolphin
{
    public class HandleChangedEventArgs : EventArgs
    {
        public IntPtr NewHandle { get; set; }

        public uint NewProcessId { get; set; }

        public string ProcessName { get; set; }
    }
}