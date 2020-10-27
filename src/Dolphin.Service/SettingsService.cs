using Dolphin.Enum;
using System.Windows.Forms;

namespace Dolphin.Service
{
    public class SettingsService : EventSubscriberBase, ISettingsService, IEventPublisher<PausedEvent>
    {
        private readonly Subscription<HotkeyPressedEvent> hotkeySubscription;
        private readonly ILogService logService;
        private Settings settings;

        public SettingsService(IEventBus eventBus, Settings settings, ILogService logService) : base(eventBus)
        {
            this.settings = settings;
            this.logService = logService;

            hotkeySubscription = new Subscription<HotkeyPressedEvent>(HotkeyPressedHandler);
            SubscribeBus(hotkeySubscription);
        }

        public MacroSettings MacroSettings => Settings.MacroSettings;

        public Settings Settings => settings;

        public SmartActionSettings SmartActionSettings => Settings.SmartActionSettings;

        public UiSettings UiSettings => Settings.UiSettings;

        public ActionName GetActionName(WK.Libraries.HotkeyListenerNS.Hotkey hotkey)
        {
            foreach (var item in Settings.Hotkeys)
            {
                if (item.Value == hotkey) return item.Key;
            }

            return default;
        }

        public Keys GetKeybinding(Command command)
        {
            return Settings.OtherKeybindings[command];
        }

        public void NegateIsPaused(bool isFromChanging)
        {
            Settings.IsPaused = !Settings.IsPaused;

            Publish(new PausedEvent { IsPaused = Settings.IsPaused, IsFromChanging = isFromChanging });
        }

        public void Publish(PausedEvent @event)
        {
            eventBus.Publish(@event);
        }

        public void ResetSettings()
        {
            settings = new Settings(true);
        }

        // Todo: Implement for specific types?
        public void ResetSettings<T>()
        {
            ResetSettings();
        }

        public void SetHotkeyValue(ActionName key, WK.Libraries.HotkeyListenerNS.Hotkey value)
        {
            Settings.Hotkeys[key] = value;
        }

        public void SetPaused(bool newPaused, bool isFromChanging)
        {
            Settings.IsPaused = newPaused;

            Publish(new PausedEvent { IsPaused = Settings.IsPaused, IsFromChanging = isFromChanging });
        }

        public bool SkillIsEnabled(SkillName skill)
        {
            return Settings.EnabledSkills.Contains(skill);
        }

        private void HotkeyPressedHandler(object o, HotkeyPressedEvent @event)
        {
            var pauseHotkey = Settings.Hotkeys[ActionName.Pause];

            if (@event.PressedHotkey == pauseHotkey)
            {
                NegateIsPaused(false);
            }
        }
    }
}