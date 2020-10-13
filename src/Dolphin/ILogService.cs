using Dolphin.Enum;
using System;

namespace Dolphin
{
    public interface ILogService
    {
        event EventHandler<LogInformationEventArgs> EntryAdded;

        void AddEntry(object origin, string message, LogLevel logLevel, Exception ex = null);

        void SaveLog(string path);
    }
}