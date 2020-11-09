using System.Collections.Generic;
using System.Linq;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin.Service
{
    public class HotkeyListenerService : EventSubscriberBase, IEventPublisher<HotkeyPressedEvent>
    {
        private readonly HotkeyListener hotkeyListener;
        private readonly ILogService logService;
        private readonly ISettingsService settingsService;

        public HotkeyListenerService(IEventBus eventBus, ISettingsService settingsService, HotkeyListener hotkeyListener, ILogService logService) : base(eventBus)
        {
            this.hotkeyListener = hotkeyListener;
            this.settingsService = settingsService;
            this.logService = logService;

            var pausedSubscription = new Subscription<PausedEvent>(PausedHandler);

            SubscribeBus(pausedSubscription);

            hotkeyListener.HotkeyPressed += OnHotkeyPressed;

            foreach (var hotkey in settingsService.Settings.Hotkeys.Values.ToList().Where(x => x != null))
            {
                AddHotkey(hotkey);
            }
        }

        public void AddHotkey(Hotkey hotkey)
        {
            var result = hotkeyListener.Add(hotkey);

            if (!result)
            {
                logService.AddEntry(this, $"Failed to add Hotkey {hotkey} to the listener", Enum.LogLevel.Error);
            }
        }

        public void OnHotkeyPressed(object o, HotkeyEventArgs e)
        {
            Publish(new HotkeyPressedEvent { PressedHotkey = e.Hotkey });
        }

        public void Publish(HotkeyPressedEvent @event)
        {
            eventBus.Publish(@event);
            logService.AddEntry(this, $"Published Hotkey pressed [{@event.PressedHotkey}]", Enum.LogLevel.Debug);
        }

        public void RefreshHotkeys(IList<Hotkey> newHotkeys)
        {
            var wasSuspended = hotkeyListener.Suspended;

            if (wasSuspended)
            {
                hotkeyListener.Resume();
            }

            hotkeyListener.RemoveAll();
            foreach (var hotkey in newHotkeys)
            {
                if (hotkey != null) AddHotkey(hotkey);
            }

            if (wasSuspended)
            {
                hotkeyListener.Suspend();
            }
        }

        public void ResumeListener()
        {
            if (hotkeyListener.Suspended)
            {
                hotkeyListener.Resume();
            }
        }

        public void SuspendListener()
        {
            if (!hotkeyListener.Suspended)
            {
                hotkeyListener.Suspend();
            }
            else
            {
                hotkeyListener.Remove(settingsService.Settings.Hotkeys[Enum.ActionName.Pause]);
            }
        }

        private void PausedHandler(object o, PausedEvent @event)
        {
            if (@event.IsPaused)
            {
                SuspendListener();
                if (!@event.IsFromChanging)
                {
                    hotkeyListener.Add(settingsService.Settings.Hotkeys[Enum.ActionName.Pause]);
                }
                logService.AddEntry(this, $"Pausing HotkeyListener...");
            }
            else
            {
                ResumeListener();
                logService.AddEntry(this, $"Resuming HotkeyListener...");
            }
        }
    }
}