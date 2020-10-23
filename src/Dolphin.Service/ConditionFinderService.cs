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

        public Func<bool> FindCondition(SkillName skillName)
        {
            var property = typeof(Condition).GetProperty(skillName.ToString(), typeof(Func<Player, World, bool>));

            if (property == null)
            {
                logService.AddEntry(this, $"{skillName} has no condition defined, defaulting to just return true.");
                return () => true;
            }

            var conditionFunction = (Func<Player, World, bool>)property?.GetValue(null);

            return () =>
            {
                return conditionFunction.Invoke(modelService.Player, modelService.World);
            };
        }
    }
}