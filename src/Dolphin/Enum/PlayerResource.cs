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
        PrimaryRage_80 = 26,
        PrimaryRage_60 = 27,
        PrimaryRage_40 = 28,
        PrimaryRage_20 = 29,
        PrimaryRage_0 = 30,
        PrimarySpirit_100 = 31,
        PrimarySpirit_80 = 32,
        PrimarySpirit_60 = 33,
        PrimarySpirit_40 = 34,
        PrimarySpirit_20 = 35,
        PrimarySpirit_0 = 36,
        PrimaryWrath_100 = 37,
        PrimaryWrath_80 = 38,
        PrimaryWrath_60 = 39,
        PrimaryWrath_40 = 40,
        PrimaryWrath_20 = 41,
        PrimaryWrath_0 = 42,
        PrimaryArcanePower_100 = 43,
        PrimaryArcanePower_80 = 44,
        PrimaryArcanePower_60 = 45,
        PrimaryArcanePower_40 = 46,
        PrimaryArcanePower_20 = 47,
        PrimaryArcanePower_0 = 48
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