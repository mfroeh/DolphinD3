using Dolphin.Enum;
using System;

namespace Dolphin
{
    public static class ConditionDonor
    {
        public static Func<Player, World, bool> GiveCondition(SkillName skill)
        {
            var property = typeof(Condition).GetProperty(skill.ToString(), typeof(Func<Player, World, bool>));

            return (Func<Player, World, bool>)property?.GetValue(null);
        }
    }
}