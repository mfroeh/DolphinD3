using Dolphin.Enum;

namespace Dolphin
{
    public delegate bool ConditionFunction(Player p, World w, Skill s);

    public static class Condition
    {
        public static bool DefaultAlways(Player p, World w, Skill s)
        {
            return w.IsRiftOrGrift() && p.NotDead();
        }

        public static bool DefaultNotActive(Player p, World w, Skill skill)
        {
            return w.IsRiftOrGrift() && p.NotDead() && !skill.IsActive;
        }

        #region DemonHunter

        public static bool Companion(Player p, World w, Skill skill)
        {
            return DefaultNotActive(p, w, skill);
        }

        public static bool FanOfKnives(Player p, World w, Skill skill)
        {
            return DefaultNotActive(p, w, skill);
        }

        public static bool Punishment(Player p, World w, Skill skill)
        {
            return DefaultAlways(p,w,skill) && p.PrimaryResourcePercentage < 80;
        }

        public static bool ShadowPower(Player p, World w, Skill skill)
        {
            return DefaultNotActive(p, w, skill) && p.SecondaryRessourcePercentage >= 60;
        }

        public static bool SmokeScreen(Player p, World w, Skill skill)
        {
            return DefaultNotActive(p, w, skill) && p.SecondaryRessourcePercentage >= 40;
        }

        public static bool Vengance(Player p, World w, Skill skill)
        {
            return DefaultNotActive(p, w, skill);
        }

        #endregion DemonHunter

        #region Necromancer

        public static bool BoneArmor(Player p, World w, Skill skill)
        {
            return DefaultAlways(p,w,skill) && skill.IsActive;
        }

        public static bool Devour(Player p, World w, Skill skill)
        {
            return DefaultAlways(p, w, skill);
        }

        public static bool LandOfTheDead(Player p, World w, Skill skill)
        {
            return DefaultNotActive(p, w, skill);
        }

        public static bool Simulacrum(Player p, World w, Skill skill)
        {
            return DefaultNotActive(p, w, skill);
        }

        public static bool SkeletalMage(Player p, World w, Skill skill)
        {
            return DefaultAlways(p,w,skill) && p.PrimaryResourcePercentage == 100;
        }

        public static bool CommandSkeletons(Player p, World w, Skill skill)
        {
            return DefaultAlways(p, w, skill) && p.PrimaryResourcePercentage >= 40;
        }

        #endregion Necromancer

        #region Barbarian

        public static bool IgnorePain(Player p, World w, Skill skill)
        {
            return DefaultAlways(p, w, skill);
        }

        public static bool WarCry(Player p, World w, Skill skill)
        {
            return DefaultAlways(p, w, skill);
        }

        public static bool ThreateningShout(Player p, World w, Skill skill)
        {
            return DefaultAlways(p, w, skill);
        }

        public static bool BattleRage(Player p, World w, Skill skill)
        {
            return false; // if rage avalible
        }

        public static bool Sprint(Player p, World w, Skill skill)
        {
            return DefaultAlways(p, w, skill) && p.PrimaryResourcePercentage >= 40;
        }

        public static bool WrathOfTheBerserker(Player p, World w, Skill skill)
        {
            return DefaultNotActive(p, w, skill);
        }

        #endregion

        #region Monk

        public static bool BlindingFlash(Player p, World w, Skill skill)
        {
            return DefaultNotActive(p, w, skill);
        }

        public static bool Epiphany(Player p, World w, Skill skill)
        {
            return DefaultNotActive(p, w, skill);
        }

        public static bool InnerSanctuary(Player p, World w, Skill skill)
        {
            return false; // todo
        }

        public static bool Serenity(Player p, World w, Skill skill)
        {
            return DefaultAlways(p, w, skill);
        }

        public static bool MantraOfHealing(Player p, World w, Skill skill)
        {
            return DefaultAlways(p, w, skill) && p.PrimaryResourcePercentage >= 0;
        }

        public static bool BreathOfHeaven(Player p, World w, Skill skill)
        {
            return DefaultNotActive(p, w, skill) && p.PrimaryResourcePercentage >= 60;
        }

        #endregion
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