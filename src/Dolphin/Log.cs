using Dolphin.Enum;
using System;
using System.Collections.Generic;

namespace Dolphin
{
    public class Log
    {
        public IList<LogEntry> Entries { get; set; } = new List<LogEntry>();
    }

    public class LogEntry
    {
        public string FullMessage { get; set; }

        public LogLevel LogLevel { get; set; }

        public string Message { get; set; }

        public DateTime Time { get; set; }

        public string Type { get; set; }
    }
}