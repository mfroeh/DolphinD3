using Dolphin.Enum;
using System;

namespace Dolphin.Service
{
    public class ConditionFinderService : IConditionFinderService
    {
        private readonly ILogService logService;
        private readonly IModelService modelService;

        public ConditionFinderService(IModelService modelService, ILogService logService)
        {
            this.modelService = modelService;
            this.logService = logService;
        }

        public ConditionFunction FindCondition(SkillName skillName)
        {
            var condition = typeof(Condition).GetMethod(skillName.ToString());

            if (condition == null)
            {
                logService.AddEntry(this, $"{skillName} has no condition defined, defaulting to return cast inside grift / rift.", LogLevel.Debug);

                condition = typeof(Condition).GetMethod(nameof(Condition.DefaultAlways));
            }

            return (ConditionFunction)Delegate.CreateDelegate(typeof(ConditionFunction), condition);
        }
    }
}