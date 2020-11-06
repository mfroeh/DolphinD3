using System.Collections.Generic;

namespace Dolphin.Enum
{
    public enum SkillName
    {
        None = 0,
        Companion = 1,
        FanOfKnives = 2,
        Preperation = 3,
        ShadowPower = 4,
        Vengeance = 5,
        BoneArmor = 6,
        CommandSkeletons = 9,
        Devour = 10,
        SkeletalMage = 11,
        Simulacrum = 12,
        LandOfTheDead = 13
    }

    public static class SkillNameExtensionMethods
    {
        public static IEnumerable<SkillName> PossibleSkills(this Player player, bool isMouse)
        {
            return PossibleSkills(player.Class, isMouse);
        }

        public static IEnumerable<SkillName> PossibleSkills(this PlayerClass @class, bool isMouse)
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
                    yield break;
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
                    yield break;
                case PlayerClass.MonkFemale:
                case PlayerClass.MonkMale:
                    yield break;
                case PlayerClass.WizardFemale:
                case PlayerClass.WizardMale:
                    yield break;
                default:
                    yield break;
            }
        }
    }
}