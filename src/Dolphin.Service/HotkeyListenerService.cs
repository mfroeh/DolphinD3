using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin.Service
{
    public class HotkeyListenerService : EventSubscriberBase, IEventPublisher<HotkeyPressedEvent>
    {
        private readonly HotkeyListener hotkeyListener;
        private readonly ISettingsService settingsService; // TODO: Remove if not needed

        private readonly Subscription<PausedEvent> pausedSubscription;

        public HotkeyListenerService(IEventBus eventBus, ISettingsService settingsService, HotkeyListener hotkeyListener) : base(eventBus)
        {
            this.hotkeyListener = hotkeyListener;
            this.settingsService = settingsService;

            hotkeyListener.HotkeyPressed += OnHotkeyPressed;

            pausedSubscription = new Subscription<PausedEvent>(PausedHandler);
            SubscribeBus(pausedSubscription);

            foreach (var hotkey in settingsService.Settings.Hotkeys.Values.ToList().Where(x => x != null))
            {
                AddHotkey(hotkey);
            }
        }

        private void PausedHandler(object o, PausedEvent @event)
        {
            if (@event.IsPaused)
            {
                SuspendListener();
            }
            else
            {
                ResumeListener();
            }
        }

        public void RefreshHotkeys(IList<Hotkey> newHotkeys)
        {
            hotkeyListener.RemoveAll();

            foreach (var hotkey in newHotkeys)
            {
                if (hotkey != null) AddHotkey(hotkey);
            }
        }

        public void AddHotkey(Hotkey hotkey)
        {
            hotkeyListener.Add(hotkey);
        }

        public void OnHotkeyPressed(object o, HotkeyEventArgs e)
        {
            Publish(new HotkeyPressedEvent { PressedHotkey = e.Hotkey });
        }

        public void Publish(HotkeyPressedEvent @event)
        {
            eventBus.Publish(@event);
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