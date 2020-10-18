using Dolphin.Enum;
using System.Linq;

namespace Dolphin
{
    public static class ConditionFunction
    {
        public static bool PunishmentFunction(Player p, World w)
        {
            if (w.IsRiftOrGrift() && p.NotDead() && p.PrimaryRessourcePercentage < 80 && p.SkillNotActiveAndCanBeCasted(SkillName.Preperation))
                return true;
            return false;
        }

        public static bool CompanionFunction(Player p, World w)
        {
            if (w.IsRiftOrGrift() && p.NotDead() && p.SkillNotActiveAndCanBeCasted(SkillName.Companion))
                return true;
            return false;
        }

        public static bool VenganceFunction(Player p, World w)
        {
            if (w.IsRiftOrGrift() && p.NotDead() && p.SkillNotActiveAndCanBeCasted(SkillName.Vengeance))
                return true;
            return false;
        }

        public static bool ShadowPowerFunction(Player p, World w)
        {
            if (w.IsRiftOrGrift() && p.NotDead() && p.SecondaryRessourcePercentage >= 60 && p.SkillNotActiveAndCanBeCasted(SkillName.ShadowPower))
                return true;
            return false;
        }
    }
}