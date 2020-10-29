using System;
using System.Windows.Forms;

namespace Dolphin.Service
{
    public class SkillCastingService : EventSubscriberBase
    {
        private readonly IConditionFinderService conditionFinderService;
        private readonly IHandleService handleService;
        private readonly ILogService logService;
        private readonly ISettingsService settingsService;
        private readonly Subscription<SkillCanBeCastedEvent> skillSubscription;

        public SkillCastingService(IEventBus eventBus, ISettingsService settingsService, IConditionFinderService conditionFinderService, IHandleService handleService, ILogService logService) : base(eventBus)
        {
            this.settingsService = settingsService;
            this.logService = logService;
            this.conditionFinderService = conditionFinderService;
            this.handleService = handleService;

            skillSubscription = new Subscription<SkillCanBeCastedEvent>(CastSkill);
            SubscribeBus(skillSubscription);
        }

        private void CastSkill(object o, SkillCanBeCastedEvent @event)
        {
            var handle = handleService.GetHandle("Diablo III64");

            if (handle == default || !settingsService.SkillIsEnabled(@event.SkillName)) return;

            Action action;
            if (@event.SkillIndex <= 3)
            {
                var key = settingsService.Settings.SkillKeybindings[@event.SkillIndex];
                action = () => InputHelper.SendKey(handle, key);
            }
            else if (@event.SkillIndex == 4)
            {
                action = () => InputHelper.SendClickAtCursorPos(handle, MouseButtons.Left);
            }
            else
            {
                action = () => InputHelper.SendClickAtCursorPos(handle, MouseButtons.Right);
            }

            var condition = conditionFinderService.FindCondition(@event.SkillName);
            if (condition.Invoke())
            {
                Execute.AndForgetAsync(action);
            }
        }
    }
}