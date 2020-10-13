using Dolphin.Enum;
using System;

namespace Dolphin
{
    public class LogInformationEventArgs : EventArgs
    {
        public string Message { get; set; }
        public LogLevel LogLevel { get; set; }
    }
}