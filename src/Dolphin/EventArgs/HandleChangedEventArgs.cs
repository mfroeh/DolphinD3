using System;

namespace Dolphin
{
    public class HandleChangedEventArgs : EventArgs
    {
        public WindowInformation NewHandle { get; set; }

        public string ProcessName { get; set; }
    }
}