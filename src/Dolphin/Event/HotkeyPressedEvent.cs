using WK.Libraries.HotkeyListenerNS;

namespace Dolphin
{
    public class HotkeyPressedEvent : IEvent
    {
        public Hotkey PressedHotkey { get; set; }
    }
}