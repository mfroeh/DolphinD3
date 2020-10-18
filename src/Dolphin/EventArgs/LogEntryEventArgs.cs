using Dolphin.Enum;
using System;

namespace Dolphin
{
    public class LogEntryEventArgs : EventArgs
    {
        public string Message { get; set; }
        public string FullMessage { get; set; }
        public LogLevel LogLevel { get; set; }
        public DateTime Time { get; set; }
    }
}