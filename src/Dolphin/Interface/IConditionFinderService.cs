using Dolphin.Enum;
using System;

namespace Dolphin
{
    public interface IConditionFinderService
    {
        ConditionFunction FindCondition(SkillName skillName);
    }
}