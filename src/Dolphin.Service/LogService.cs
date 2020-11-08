using Dolphin.Enum;
using System;
using System.IO;
using System.Linq;

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

        public Log InternalLog => internalLog;

        public event EventHandler<LogEntryEventArgs> EntryAdded;

        public void AddEntry(object origin, string message, LogLevel logLevel = LogLevel.Info, Exception ex = null)
        {
            message = ex == null ? message : message + $", Exception: {ex}";

            var entry = new LogEntry
            {
                FullMessage = $"[{DateTime.Now}]---LogLevel: {logLevel}, Type: {origin.GetType().FullName}, Message: {message}",
                LogLevel = logLevel,
                Message = message,
                Time = DateTime.Now,
                Type = origin.GetType().Name
            };

            internalLog.Entries.Add(entry);
            if (internalLog.Entries.Count >= 2500)
            {
                SaveLog();
                internalLog.Entries.Clear();
            }

            Execute.OnUIThreadAsync(() => EntryAdded?.Invoke(this, new LogEntryEventArgs { LogLevel = logLevel, LogEntry = entry }));
        }

        public void SaveLog()
        {
            lock (internalLog)
            {
                File.AppendAllLines(savePath, internalLog.Entries.Select(x => x.FullMessage));
            }
            //TODO: File.AppendAllLines(savePath, internalLog.Entries);
        }
    }
}