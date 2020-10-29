using Dolphin.Enum;
using System;

namespace Dolphin.Service
{
    public class LogService : ILogService
    {
        private readonly Log internalLog;
        private readonly string savePath;

        public LogService(Log internalLog)
        {
            this.internalLog = internalLog;
            this.savePath = "log.txt";
        }

        public event EventHandler<LogEntryEventArgs> EntryAdded;

        public void AddEntry(object origin, string message, LogLevel logLevel = LogLevel.Info, Exception ex = null)
        {
            if (ex != null)
            {
                message += $", Exception: {ex}";
            }

            var fullMessage = $"[{DateTime.Now}]---LogLevel: {logLevel}, Type: {origin.GetType().FullName}, Message: {message}";

            internalLog.Entries.Add(fullMessage);
            if (internalLog.Entries.Count >= 2500)
            {
                SaveLog();
                internalLog.Entries.Clear();
            }

            Execute.OnUIThreadAsync(() => EntryAdded?.Invoke(this, new LogEntryEventArgs { Message = message, LogLevel = logLevel, FullMessage = fullMessage, Time = DateTime.Now, Type = origin.GetType().Name }));
        }

        public void SaveLog()
        {
            //TODO: File.AppendAllLines(savePath, internalLog.Entries);
        }
    }
}