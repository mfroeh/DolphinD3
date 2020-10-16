using Dolphin.Enum;
using System;
using System.Linq;

namespace Dolphin
{
    public static class ConditionExtensionMethods
    {
        public static bool NotDead(this Player p) => p.HealthPercentage > 0;

        public static bool SkillNotActiveAndCanBeCasted(this Player p, SkillName name)
        {
            var skill = p.Skills.Where(x => x.Name == name).FirstOrDefault();

            if (skill != null && skill.IsNotActiveAndCanBeCasted)
                return true;
            return false;
        }

        public static bool IsRiftOrGrift(this World w)
        {
            return w.CurrentLocation == WorldLocation.Rift || w.CurrentLocation == WorldLocation.Grift;
        }

        public static Func<Player, World, bool> GetCondition(this SkillName skillName)
        {
            var property = typeof(Condition).GetProperty(skillName.ToString(), typeof(Func<Player, World, bool>));
            return (Func<Player, World, bool>)property?.GetValue(null);
        }
    }
}
