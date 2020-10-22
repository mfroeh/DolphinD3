using Dolphin.Enum;
using System;

namespace Dolphin.Service
{
    public class ConditionFinderService : IConditionFinderService
    {
        private readonly IModelService modelService;

        public ConditionFinderService(IModelService modelService)
        {
            this.modelService = modelService;
        }

        public Func<bool> FindCondition(SkillName skillName)
        {
            var property = typeof(Condition).GetProperty(skillName.ToString(), typeof(Func<Player, World, bool>));

            if (property == null) return () => true;

            var conditionFunction = (Func<Player, World, bool>)property?.GetValue(null);

            return () =>
            {
                return conditionFunction.Invoke(modelService.Player, modelService.World);
            };
        }
    }
}
