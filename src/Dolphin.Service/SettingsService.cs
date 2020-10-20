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
            Subscribe(hotkeySubscription);
        }

        public Settings Settings { get; }

        public void HotkeyPressedHandler(object o, HotkeyPressedEvent @event)
        {
            var pauseHotkey = Settings.Hotkeys[HotkeyName.Pause];

            if (@event.PressedHotkey == pauseHotkey)
            {
                Settings.IsPaused = !Settings.IsPaused;

                Publish(new PausedEvent { IsPaused = Settings.IsPaused });
            }
        }

        public void Publish(PausedEvent @event)
        {
            eventBus.Publish(@event);
        }

        public bool SkillIsEnabled(SkillName skill)
        {
            return Settings.EnabledSkills.Contains(skill);
        }
    }
}