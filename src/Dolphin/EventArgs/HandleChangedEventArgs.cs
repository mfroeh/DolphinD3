using System;

namespace Dolphin
{
    public class HandleChangedEventArgs
    {
        public string ProcessName { get; set; }
        public uint NewProcessId { get; set; }
        public IntPtr NewHandle { get; set; }
    }
}