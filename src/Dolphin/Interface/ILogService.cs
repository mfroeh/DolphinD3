using Dolphin.Enum;
using System;

namespace Dolphin
{
    public interface ILogService
    {
        event EventHandler<LogEntryEventArgs> EntryAdded;

        public Log InternalLog { get; }

        void AddEntry(object origin, string message, LogLevel logLevel = LogLevel.Info, Exception ex = null);

        void SaveLog();
    }
}