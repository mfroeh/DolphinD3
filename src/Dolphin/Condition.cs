using Dolphin.Enum;
using System;

namespace Dolphin
{
    public static class Condition
    {
        public static Func<Player, World, Skill, bool> BoneArmor = BoneArmorFunction;
        public static Func<Player, World, Skill, bool> Companion = CompanionFunction;
        public static Func<Player, World, Skill, bool> Devour = DevourFunction;
        public static Func<Player, World, Skill, bool> LandOfTheDead = LandOfTheDeadFunction;
        public static Func<Player, World, Skill, bool> Punishment = PunishmentFunction;
        public static Func<Player, World, Skill, bool> ShadowPower = ShadowPowerFunction;
        public static Func<Player, World, Skill, bool> Simulacrum = SimulacrumFunction;
        public static Func<Player, World, Skill, bool> SkeletalMage = SkeletalMageFunction;
        public static Func<Player, World, Skill, bool> Vengeance = VenganceFunction;

        #region DemonHunter

        public static bool CompanionFunction(Player p, World w, Skill skill)
        {
            return w.IsRiftOrGrift() &&
                    p.NotDead() &&
                    !skill.IsActive;
        }

        public static bool PunishmentFunction(Player p, World w, Skill skill)
        {
            return w.IsRiftOrGrift() &&
                    p.NotDead() &&
                    p.PrimaryResourcePercentage < 80;
        }

        public static bool ShadowPowerFunction(Player p, World w, Skill skill)
        {
            return w.IsRiftOrGrift() &&
                    !skill.IsActive &&
                    p.NotDead() &&
                    p.SecondaryRessourcePercentage >= 60;
        }

        public static bool VenganceFunction(Player p, World w, Skill skill)
        {
            return !skill.IsActive &&
                    w.IsRiftOrGrift() &&
                    p.NotDead();
        }

        #endregion DemonHunter

        #region Necromancer

        public static bool BoneArmorFunction(Player p, World w, Skill skill)
        {
            return w.IsRiftOrGrift() &&
                   p.NotDead();
        }

        public static bool DevourFunction(Player p, World w, Skill skill)
        {
            return w.IsRiftOrGrift() &&
                   p.NotDead();
        }

        public static bool LandOfTheDeadFunction(Player p, World w, Skill skill)
        {
            return w.IsRiftOrGrift() &&
                   p.NotDead() &&
                   !skill.IsActive;
        }

        public static bool SimulacrumFunction(Player p, World w, Skill skill)
        {
            return w.IsRiftOrGrift() &&
                   p.NotDead() &&
                   !skill.IsActive;
        }

        public static bool SkeletalMageFunction(Player p, World w, Skill skill)
        {
            return w.IsRiftOrGrift() &&
                   p.NotDead() &&
                   p.PrimaryResourcePercentage == 100;
        }

        #endregion Necromancer
    }

    public static class ConditionExtensionMethod
    {
        public static bool IsRiftOrGrift(this World w)
        {
            return w.CurrentLocation == WorldLocation.Rift || w.CurrentLocation == WorldLocation.Grift;
        }

        public static bool NotDead(this Player p) => p.HealthPercentage > 0;
    }
}