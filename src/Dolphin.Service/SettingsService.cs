using Dolphin.Enum;

namespace Dolphin.Service
{
    public class SettingsService : EventSubscriberBase, ISettingsService, IEventPublisher<PausedEvent>
    {
        private readonly Subscription<HotkeyPressedEvent> hotkeySubscription;

        private readonly ILogService logService;

        public SettingsService(IEventBus eventBus, Settings settings, ILogService logService) : base(eventBus)
        {
            Settings = settings;
            this.logService = logService;

            hotkeySubscription = new Subscription<HotkeyPressedEvent>(HotkeyPressedHandler);
            SubscribeBus(hotkeySubscription);
        }

        public Settings Settings { get; }

        public MacroSettings MacroSettings => Settings.MacroSettings;

        public UiSettings UiSettings => Settings.UiSettings;

        private void HotkeyPressedHandler(object o, HotkeyPressedEvent @event)
        {
            var pauseHotkey = Settings.Hotkeys[ActionName.Pause];

            if (@event.PressedHotkey == pauseHotkey)
            {
                NegateIsPaused();
            }
        }

        public void NegateIsPaused()
        {
            Settings.IsPaused = !Settings.IsPaused;

            Publish(new PausedEvent { IsPaused = Settings.IsPaused });
        }

        public void Publish(PausedEvent @event)
        {
            eventBus.Publish(@event);
        }

        public void SetHotkeyValue(ActionName key, WK.Libraries.HotkeyListenerNS.Hotkey value)
        {
            Settings.Hotkeys[key] = value;
        }

        public bool SkillIsEnabled(SkillName skill)
        {
            return Settings.EnabledSkills.Contains(skill);
        }

        public ActionName GetActionName(WK.Libraries.HotkeyListenerNS.Hotkey hotkey)
        {
            foreach (var item in Settings.Hotkeys)
            {
                if (item.Value == hotkey) return item.Key;
            }

            return default;
        }
    }
}