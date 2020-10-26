using Dolphin.Enum;
using System;
using System.Diagnostics;
using System.Linq;

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
            var property = typeof(Condition).GetField(skillName.ToString());

            if (property == null)
            {
                logService.AddEntry(this, $"{skillName} has no condition defined, defaulting to just return true.");
                return () => true;
            }

            var conditionFunction = (Func<Player, World, Skill, bool>)property?.GetValue(null);

            var skill = modelService.GetSkill(skillName);

            return () =>
            {
                return conditionFunction.Invoke(modelService.Player, modelService.World, skill);
            };
        }
    }
}