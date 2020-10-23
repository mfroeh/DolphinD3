using WK.Libraries.HotkeyListenerNS;

namespace Dolphin
{
    public class PausedEvent : IEvent
    {
        public bool IsPaused { get; set; }

        public bool IsFromChanging { get; set; }
    }
}