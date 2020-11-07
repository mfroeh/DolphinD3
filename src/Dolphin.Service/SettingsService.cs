using Dolphin.Enum;
using System.Windows.Forms;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin.Service
{
    public class SettingsService : EventSubscriberBase, ISettingsService, IEventPublisher<PausedEvent>
    {
        private readonly ILogService logService;

        public SettingsService(IEventBus eventBus, Settings settings, ILogService logService) : base(eventBus)
        {
            Settings = settings;
            this.logService = logService;

            var hotkeySubscription = new Subscription<HotkeyPressedEvent>(HotkeyPressedHandler);
            SubscribeBus(hotkeySubscription);
        }

        public MacroSettings MacroSettings => Settings.MacroSettings;

        public Settings Settings { get; private set; }

        public SkillCastSettings SkillCastSettings => Settings.SkillCastSettings;

        public SmartFeatureSettings SmartFeatureSettings => Settings.SmartFeatureSettings;

        public UiSettings UiSettings => Settings.UiSettings;

        public ActionName GetActionName(Hotkey hotkey)
        {
            foreach (var item in Settings.Hotkeys)
            {
                if (item.Value == hotkey) return item.Key;
            }

            return default;
        }

        public Keys GetKeybinding(CommandKeybinding command)
        {
            return Settings.OtherKeybindings[command];
        }

        public SmartActionName GetSmartActionName(Window window)
        {
            foreach (var action in window.AssociatedSmartActions())
            {
                return action;
            }

            return default;
        }

        public bool IsSmartActionEnabled(SmartActionName smartAction)
        {
            return SmartFeatureSettings.EnabledSmartActions.Contains(smartAction);
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
            Settings = new Settings(true);
        }

        // Todo: Implement for specific types?
        public void ResetSettings<T>()
        {
            ResetSettings();
        }

        public void SetHotkeyValue(ActionName key, Hotkey value)
        {
            Settings.Hotkeys[key] = value;
        }

        public void SetPaused(bool newPaused, bool isFromChanging)
        {
            Settings.IsPaused = newPaused;

            Publish(new PausedEvent { IsPaused = Settings.IsPaused, IsFromChanging = isFromChanging });
        }

        public bool SkillIndexIsSuspended(int skillIndex)
        {
            return SmartFeatureSettings.SkillSuspensionStatus[skillIndex];
        }

        public bool SkillIsEnabled(SkillName skill)
        {
            return SmartFeatureSettings.EnabledSkills.Contains(skill);
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