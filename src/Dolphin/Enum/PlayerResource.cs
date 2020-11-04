using System.Collections;
using System.Collections.Generic;

namespace Dolphin.Enum
{
    public enum PlayerResource
    {
        None = 0,
        PrimaryHatred_100 = 1,
        PrimaryHatred_80 = 2,
        PrimaryHatred_60 = 3,
        PrimaryHatred_40 = 4,
        PrimaryHatred_20 = 5,
        PrimaryHatred_0 = 6,
        SecondaryDiscipline_100 = 7,
        SecondaryDiscipline_80 = 8,
        SecondaryDiscipline_60 = 9,
        SecondaryDiscipline_40 = 10,
        SecondaryDiscipline_20 = 11,
        SecondaryDiscipline_0 = 12,
        PrimaryEssence_100 = 13,
        PrimaryEssence_80 = 14,
        PrimaryEssence_60 = 15,
        PrimaryEssence_40 = 16,
        PrimaryEssence_20 = 17,
        PrimaryEssence_0 = 18,
        PrimaryMana_100 = 19,
        PrimaryMana_80 = 20,
        PrimaryMana_60 = 21,
        PrimaryMana_40 = 22,
        PrimaryMana_20 = 23,
        PrimaryMana_0 = 24,
        PrimaryRage_100 = 25,
        PrimaryRage_80 = 25,
        PrimaryRage_60 = 25,
        PrimaryRage_40 = 25,
        PrimaryRage_20 = 25,
        PrimaryRage_0 = 25,
        PrimarySpirit_100 = 26,
        PrimarySpirit_80 = 26,
        PrimarySpirit_60 = 26,
        PrimarySpirit_40 = 26,
        PrimarySpirit_20 = 26,
        PrimarySpirit_0 = 26,
        PrimaryWrath_100 = 27,
        PrimaryWrath_80 = 28,
        PrimaryWrath_60 = 29,
        PrimaryWrath_40 = 30,
        PrimaryWrath_20 = 31,
        PrimaryWrath_0 = 32,
        PrimaryArcanePower_100 = 33,
        PrimaryArcanePower_80 = 34,
        PrimaryArcanePower_60 = 35,
        PrimaryArcanePower_40 = 36,
        PrimaryArcanePower_20 = 37,
        PrimaryArcanePower_0 = 38
    }

    public static class ResourceExtensionMethods
    {
        public static IEnumerable<PlayerResource> PossiblePrimaryResource(this Player player)
        {
            return PossiblePrimaryResource(player.Class);
        }

        public static IEnumerable<PlayerResource> PossiblePrimaryResource(this PlayerClass @class)
        {
            switch (@class)
            {
                case PlayerClass.NecromancerMale:
                case PlayerClass.NecromancerFemale:
                    yield return PlayerResource.PrimaryEssence_0;
                    yield return PlayerResource.PrimaryEssence_20;
                    yield return PlayerResource.PrimaryEssence_40;
                    yield return PlayerResource.PrimaryEssence_60;
                    yield return PlayerResource.PrimaryEssence_80;
                    yield return PlayerResource.PrimaryEssence_100;
                    break;
                case PlayerClass.BarbarianFemale:
                case PlayerClass.BarbarianMale:
                    yield return PlayerResource.PrimaryRage_0;
                    yield return PlayerResource.PrimaryRage_20;
                    yield return PlayerResource.PrimaryRage_40;
                    yield return PlayerResource.PrimaryRage_60;
                    yield return PlayerResource.PrimaryRage_80;
                    yield return PlayerResource.PrimaryRage_100;
                    break;
                case PlayerClass.CrusaderFemale:
                case PlayerClass.CrusaderMale:
                    yield return PlayerResource.PrimaryWrath_0;
                    yield return PlayerResource.PrimaryWrath_20;
                    yield return PlayerResource.PrimaryWrath_40;
                    yield return PlayerResource.PrimaryWrath_60;
                    yield return PlayerResource.PrimaryWrath_80;
                    yield return PlayerResource.PrimaryWrath_100;
                    break;
                case PlayerClass.WitchDoctorFemale:
                case PlayerClass.WitchDoctorMale:
                    yield return PlayerResource.PrimaryMana_0;
                    yield return PlayerResource.PrimaryMana_20;
                    yield return PlayerResource.PrimaryMana_40;
                    yield return PlayerResource.PrimaryMana_60;
                    yield return PlayerResource.PrimaryMana_80;
                    yield return PlayerResource.PrimaryMana_100;
                    break;
                case PlayerClass.DemonHunterFemale:
                case PlayerClass.DemonHunterMale:
                    yield return PlayerResource.PrimaryHatred_0;
                    yield return PlayerResource.PrimaryHatred_20;
                    yield return PlayerResource.PrimaryHatred_40;
                    yield return PlayerResource.PrimaryHatred_60;
                    yield return PlayerResource.PrimaryHatred_80;
                    yield return PlayerResource.PrimaryHatred_100;
                    break;
                case PlayerClass.MonkFemale:
                case PlayerClass.MonkMale:
                    yield return PlayerResource.PrimarySpirit_0;
                    yield return PlayerResource.PrimarySpirit_20;
                    yield return PlayerResource.PrimarySpirit_40;
                    yield return PlayerResource.PrimarySpirit_60;
                    yield return PlayerResource.PrimarySpirit_80;
                    yield return PlayerResource.PrimarySpirit_100;
                    break;
                case PlayerClass.WizardFemale:
                case PlayerClass.WizardMale:
                    yield return PlayerResource.PrimaryArcanePower_0;
                    yield return PlayerResource.PrimaryArcanePower_20;
                    yield return PlayerResource.PrimaryArcanePower_40;
                    yield return PlayerResource.PrimaryArcanePower_60;
                    yield return PlayerResource.PrimaryArcanePower_80;
                    yield return PlayerResource.PrimaryArcanePower_100;
                    break;
                default:
                    yield break;
            }
        }

        public static IEnumerable<PlayerResource> PossibleSecondaryResource(this Player player)
        {
            return PossibleSecondaryResource(player.Class);
        }

        public static IEnumerable<PlayerResource> PossibleSecondaryResource(this PlayerClass @class)
        {
            switch (@class)
            {
                case PlayerClass.DemonHunterFemale:
                case PlayerClass.DemonHunterMale:
                    yield return PlayerResource.SecondaryDiscipline_0;
                    yield return PlayerResource.SecondaryDiscipline_20;
                    yield return PlayerResource.SecondaryDiscipline_40;
                    yield return PlayerResource.SecondaryDiscipline_60;
                    yield return PlayerResource.SecondaryDiscipline_80;
                    yield return PlayerResource.SecondaryDiscipline_100;
                    break;
                default:
                    yield break;
            }
        }
    }
}