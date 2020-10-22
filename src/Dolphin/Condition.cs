using Dolphin.Enum;
using System;
using System.Linq;

namespace Dolphin
{
    public static class Condition
    {
        public static Func<Player, World, bool> Companion = CompanionFunction;
        public static Func<Player, World, bool> Punishment = PunishmentFunction;
        public static Func<Player, World, bool> ShadowPower = ShadowPowerFunction;
        public static Func<Player, World, bool> Vengance = VenganceFunction;

        public static bool CompanionFunction(Player p, World w)
        {
            return w.IsRiftOrGrift() &&
                    p.NotDead() &&
                    p.SkillNotActiveAndCanBeCasted(SkillName.Companion);
        }

        public static bool PunishmentFunction(Player p, World w)
        {
            return w.IsRiftOrGrift() &&
                    p.NotDead() &&
                    p.PrimaryResourcePercentage < 80 &&
                    p.SkillNotActiveAndCanBeCasted(SkillName.Preperation);
        }

        public static bool ShadowPowerFunction(Player p, World w)
        {
            return w.IsRiftOrGrift() &&
                    p.NotDead() &&
                    p.SecondaryRessourcePercentage >= 60 &&
                    p.SkillNotActiveAndCanBeCasted(SkillName.ShadowPower);
        }

        public static bool VenganceFunction(Player p, World w)
        {
            return w.IsRiftOrGrift() &&
                    p.NotDead() &&
                    p.SkillNotActiveAndCanBeCasted(SkillName.Vengeance);
        }
    }

    public static class ConditionExtensionMethod
    {
        public static bool IsRiftOrGrift(this World w)
        {
            return w.CurrentLocation == WorldLocation.Rift || w.CurrentLocation == WorldLocation.Grift;
        }

        public static bool NotDead(this Player p) => p.HealthPercentage > 0;

        public static bool SkillNotActiveAndCanBeCasted(this Player p, SkillName name)
        {
            var skill = p.Skills.Where(x => x.Name == name).FirstOrDefault();

            if (skill != null && skill.IsNotActiveAndCanBeCasted)
            {
                return true;
            }

            return false;
        }
    }
}