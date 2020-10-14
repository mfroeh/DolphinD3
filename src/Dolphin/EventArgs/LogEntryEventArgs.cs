using Dolphin.Enum;
using System;

namespace Dolphin
{
    public class LogEntryEventArgs : EventArgs
    {
        public string Message { get; set; }
        public LogLevel LogLevel { get; set; }
    }
}