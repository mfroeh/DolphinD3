using Dolphin.Enum;
using System;

namespace Dolphin
{
    public interface ILogService
    {
        public Log InternalLog { get; }

        event EventHandler<LogEntryEventArgs> EntryAdded;

        void AddEntry(object origin, string message, LogLevel logLevel = LogLevel.Info, Exception ex = null);

        void SaveLog();
    }
}