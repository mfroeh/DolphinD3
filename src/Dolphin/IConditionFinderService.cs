using Dolphin.Enum;
using System;

namespace Dolphin
{
    public interface IConditionFinderService
    {
        Func<bool> FindCondition(SkillName skillName);
    }
}