using Dolphin.Enum;
using System;

namespace Dolphin.Service
{
    public class LogService : ILogService
    {
        public event EventHandler<LogInformationEventArgs> EntryAdded;

        private readonly Log log;

        public LogService(Log log)
        {
            this.log = log;
        }

        public void AddEntry(object origin, string message, LogLevel logLevel, Exception ex = null)
        {
            // TODO: Add the origin to the logmesasge (Filename or something)

            var currentTime = DateTime.Now;
            var logMessage = $"[{currentTime}]---LogLevel: {logLevel}, Message: {message}, Exception: {ex}";

            log.Entries.Add(logMessage);
            EntryAdded?.Invoke(this, new LogInformationEventArgs { Message = logMessage, LogLevel = logLevel });
        }

        public void SaveLog(string path)
        {
            throw new NotImplementedException();
        }
    }
}