using System.Collections.Generic;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin.Service
{
    public class HotkeyListenerService : IEventPublisher<HotkeyPressedEvent>
    {
        private readonly IEventBus eventBus;
        private readonly HotkeyListener hotkeyListener;

        private IList<Hotkey> hotkeys = new List<Hotkey>();

        public HotkeyListenerService(IEventBus eventBus, HotkeyListener hotkeyListener)
        {
            this.eventBus = eventBus;
            this.hotkeyListener = hotkeyListener;

            hotkeyListener.HotkeyPressed += OnHotkeyPressed;
            // Add all the hotkeys
        }

        public void AddHotkey(Hotkey hotkey)
        {
            hotkeys.Add(hotkey);
            // Also add
        }

        public void OnHotkeyChanged(object o, HotkeyChangedEventArgs e)
        {
            // remove a hotkey, add a hotkey, ...
        }

        public void OnHotkeyPressed(object o, HotkeyEventArgs e)
        {
            Publish(new HotkeyPressedEvent { PressedHotkey = e.Hotkey });
        }

        public void Publish(HotkeyPressedEvent @event)
        {
            eventBus.Publish(@event);
        }

        public void RemoveHotkey(Hotkey hotkey)
        {
            hotkeys.Remove(hotkey);
        }

        public void ResumeListener()
        {
            if (hotkeyListener.Suspended)
                hotkeyListener.Resume();
        }

        public void SuspendListener()
        {
            if (!hotkeyListener.Suspended)
                hotkeyListener.Suspend();
        }
    }
}