using Dolphin.Enum;
using System;

namespace Dolphin
{
    public class LogEntryEventArgs : EventArgs
    {

        public LogLevel LogLevel { get; set; }

        public LogEntry LogEntry { get; set; }
    }
}