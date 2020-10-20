using Dolphin.Enum;
using System;

namespace Dolphin.Service
{
    public class SkillCastingService : EventSubscriberBase
    {
        private readonly ILogService logService;
        private readonly IModelService modelService;
        private readonly ISettingsService settingsService;
        private readonly Subscription<SkillCanBeCastedEvent> skillSubscription;

        public SkillCastingService(IEventBus eventBus, ISettingsService settingsService, IModelService modelService, ILogService logService) : base(eventBus)
        {
            this.settingsService = settingsService;
            this.modelService = modelService;
            this.logService = logService;

            skillSubscription = new Subscription<SkillCanBeCastedEvent>(CastSkill);
            Subscribe(skillSubscription);
        }

        private void CastSkill(object o, SkillCanBeCastedEvent @event)
        {
            if (!settingsService.SkillIsEnabled(@event.SkillName)) return;

            IntPtr handle = WindowHelper.GetHWND();
            Action action;

            if (@event.SkillIndex <= 3)
            {
                var key = settingsService.Settings.SkillKeybindings[@event.SkillIndex];
                action = SkillDonor.GetAction(handle, key);
            }
            else if (@event.SkillIndex == 4)
            {
                action = SkillDonor.GetAction(handle, System.Windows.Forms.MouseButtons.Left);
            }
            else
            {
                action = SkillDonor.GetAction(handle, System.Windows.Forms.MouseButtons.Right);
            }

            var condition = ConditionDonor.GiveCondition(@event.SkillName);
            if (condition == null)
            {
                logService.AddEntry(this, $"{@event.SkillName} has no condition defined, defaulting to just return true.", LogLevel.Info);
                condition = (_, __) => true;
            }

            if (condition.Invoke(modelService.Player, modelService.World))
            {
                Execute.AndForgetAsync(action);
            }
        }
    }
}