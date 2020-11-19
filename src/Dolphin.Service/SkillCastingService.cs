using Dolphin.Enum;
using System;
using System.Windows.Forms;

namespace Dolphin.Service
{
    public class SkillCastingService : EventSubscriberBase
    {
        private readonly IConditionFinderService conditionFinderService;
        private readonly IHandleService handleService;
        private readonly ILogService logService;
        private readonly IModelService modelService;
        private readonly ISettingsService settingsService;

        public SkillCastingService(IEventBus eventBus, ISettingsService settingsService, IConditionFinderService conditionFinderService, IModelService modelService, IHandleService handleService, ILogService logService) : base(eventBus)
        {
            this.modelService = modelService;
            this.settingsService = settingsService;
            this.logService = logService;
            this.conditionFinderService = conditionFinderService;
            this.handleService = handleService;

            var skillSubscription = new Subscription<SkillCanBeCastedEvent>(OnSkilCanBeCasted);
            SubscribeBus(skillSubscription);
        }

        private void OnSkilCanBeCasted(object o, SkillCanBeCastedEvent @event)
        {
            var handle = handleService.GetHandle("Diablo III64");
            if (handle.IsDefault() ||
                !settingsService.SkillIsEnabled(@event.SkillName) ||
                settingsService.SkillIndexIsSuspended(@event.SkillIndex))
            {
                return;
            }

            Action action;
            if (@event.SkillIndex <= 3)
            {
                var key = settingsService.Settings.SkillKeybindings[@event.SkillIndex];
                action = () => InputHelper.SendKey(handle.Handle, key);
            }
            else if (@event.SkillIndex == 4)
            {
                action = () => InputHelper.SendClickAtCursorPos(handle.Handle, MouseButtons.Left);
            }
            else
            {
                action = () => InputHelper.SendClickAtCursorPos(handle.Handle, MouseButtons.Right);
            }

            var condition = conditionFinderService.FindCondition(@event.SkillName);
            if (condition.Invoke(modelService.Player,
                                 modelService.World,
                                 modelService.GetSkill(@event.SkillIndex)))
            {
                Execute.AndForgetAsync(action);
                logService.AddEntry(this, $"Clicking skill... [{@event.SkillName}][{@event.SkillIndex}]", LogLevel.Debug);
            }
            else
            {
                logService.AddEntry(this, $"Condition for skill not fulfilled... [{@event.SkillName}][{@event.SkillIndex}]", LogLevel.Debug);
            }
        }
    }
}