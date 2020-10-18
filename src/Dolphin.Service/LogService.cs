using Dolphin.Enum;
using System;
using System.IO;

namespace Dolphin.Service
{
    public class LogService : ILogService
    {
        public event EventHandler<LogEntryEventArgs> EntryAdded;

        private readonly Log log;

        public LogService(Log log)
        {
            this.log = log;
        }

        public void AddEntry(object origin, string message, LogLevel logLevel, Exception ex = null)
        {
            // TODO: Add the origin to the logmesasge (Filename or something)

            var currentTime = DateTime.Now;
            var fullMessage = $"[{currentTime}]---LogLevel: {logLevel}, Message: {message}";

            if (ex != null)
            {
                fullMessage += $", Exception: {ex}";
                message += $", Exception: {ex}";
            }

            log.Entries.Add(fullMessage);
            EntryAdded?.Invoke(this, new LogEntryEventArgs { Message = message, LogLevel = logLevel, FullMessage = fullMessage, Time = currentTime });
        }

        public void SaveLog(string path)
        {
            File.AppendAllLines(path, log.Entries);
        }
    }
}