using Dolphin.Enum;
using System.Collections;
using System.Collections.Generic;

namespace Dolphin
{
    public class UiSettings
    {
        public LogLevel DisplayLogLevel { get; set; }

        public bool IsDark { get; set; }

        public bool LogPaused { get; set; }

        public IDictionary<string, string> ExecuteablePaths { get; set; }
    }
}