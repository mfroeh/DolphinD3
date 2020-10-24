using Dolphin.Enum;
using System;

namespace Dolphin
{
    public class LogEntryEventArgs : EventArgs
    {
        public string FullMessage { get; set; }

        public LogLevel LogLevel { get; set; }

        public string Message { get; set; }

        public DateTime Time { get; set; }

        public string Type { get; set; }
    }
}