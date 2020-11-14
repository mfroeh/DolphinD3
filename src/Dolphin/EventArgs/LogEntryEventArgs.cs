using Dolphin.Enum;
using System;

namespace Dolphin
{
    public class LogEntryEventArgs : EventArgs
    {
        public LogEntry LogEntry { get; set; }

        public LogLevel LogLevel { get; set; }
    }
}