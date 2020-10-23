using Dolphin.Enum;
using System.Drawing;

namespace Dolphin
{
    public class UiSettings
    {
        public LogLevel DisplayLogLevel { get; set; }

        public bool IsDark { get; set; }

        public bool LogPaused { get; set; }

        public Rectangle MainWindowPosition { get; set; }
    }
}