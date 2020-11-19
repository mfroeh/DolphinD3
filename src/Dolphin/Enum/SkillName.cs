using System.Collections.Generic;

namespace Dolphin.Enum
{
    public enum SkillName
    {
        None = 0,
        Companion = 101,
        FanOfKnives = 102,
        Preperation = 103,
        ShadowPower = 104,
        Vengeance = 105,
        SmokeScreen = 106,
        BoneArmor = 201,
        CommandSkeletons = 202,
        Devour = 203,
        SkeletalMage = 204,
        Simulacrum = 205,
        LandOfTheDead = 206,
        IgnorePain = 301,
        BattleRage = 302,
        ThreateningShout = 303,
        WarCry = 304,
        WrathOfTheBerserker = 305,
        Sprint = 306,
        BlindingFlash = 401,
        Epiphany = 402,
        InnerSanctuary = 403,
        Serenity = 404,
        MantraOfHealing = 405,
        BreathOfHeaven = 406
    }

    public static class SkillNameExtensionMethods
    {
        public static IEnumerable<SkillName> PossibleSkills(this Player player)
        {
            return PossibleSkills(player.Class);
        }

        public static IEnumerable<SkillName> PossibleSkills(this PlayerClass @class)
        {
            switch (@class)
            {
                case PlayerClass.NecromancerMale:
                case PlayerClass.NecromancerFemale:
                    yield return SkillName.BoneArmor;
                    yield return SkillName.CommandSkeletons;
                    yield return SkillName.SkeletalMage;
                    yield return SkillName.LandOfTheDead;
                    yield return SkillName.Simulacrum;
                    yield return SkillName.Devour;
                    break;

                case PlayerClass.BarbarianFemale:
                case PlayerClass.BarbarianMale:
                    yield return SkillName.IgnorePain;
                    yield return SkillName.ThreateningShout;
                    yield return SkillName.WrathOfTheBerserker;
                    yield return SkillName.Sprint;
                    yield return SkillName.WarCry;
                    yield return SkillName.BattleRage;
                    break;
                case PlayerClass.CrusaderFemale:
                case PlayerClass.CrusaderMale:
                    yield break;
                case PlayerClass.WitchDoctorFemale:
                case PlayerClass.WitchDoctorMale:
                    yield break;
                case PlayerClass.DemonHunterFemale:
                case PlayerClass.DemonHunterMale:
                    yield return SkillName.ShadowPower;
                    yield return SkillName.Vengeance;
                    yield return SkillName.FanOfKnives;
                    yield return SkillName.Preperation;
                    yield return SkillName.Companion;
                    yield return SkillName.SmokeScreen;
                    yield break;
                case PlayerClass.MonkFemale:
                case PlayerClass.MonkMale:
                    yield return SkillName.BlindingFlash;
                    yield return SkillName.Epiphany;
                    yield return SkillName.Serenity;
                    yield return SkillName.MantraOfHealing;
                    yield return SkillName.BreathOfHeaven;
                    yield return SkillName.InnerSanctuary;
                    break;
                case PlayerClass.WizardFemale:
                case PlayerClass.WizardMale:
                    yield break;
                default:
                    yield break;
            }
        }
    }
}