using Dolphin.Enum;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;

namespace Dolphin.Service
{
    public class ModelService : IModelService
    {
        #region ResourceDictionary

        private static readonly IDictionary<PlayerClass, IList<Enum.PlayerResource>> ResourceDictionary = new Dictionary<PlayerClass, IList<Enum.PlayerResource>>
        {
            { PlayerClass.DemonHunterFemale, new List<Enum.PlayerResource>
            {
                Enum.PlayerResource.PrimaryHatred_0, Enum.PlayerResource.PrimaryHatred_20, Enum.PlayerResource.PrimaryHatred_40, Enum.PlayerResource.PrimaryHatred_60, Enum.PlayerResource.PrimaryHatred_80, Enum.PlayerResource.PrimaryHatred_100,
                Enum.PlayerResource.SecondaryDiscipline_0, Enum.PlayerResource.SecondaryDiscipline_20, Enum.PlayerResource.SecondaryDiscipline_40, Enum.PlayerResource.SecondaryDiscipline_60, Enum.PlayerResource.SecondaryDiscipline_80, Enum.PlayerResource.SecondaryDiscipline_100
            } },

            { PlayerClass.DemonHunterMale, new List<Enum.PlayerResource>
            {
                Enum.PlayerResource.PrimaryHatred_0, Enum.PlayerResource.PrimaryHatred_20, Enum.PlayerResource.PrimaryHatred_40, Enum.PlayerResource.PrimaryHatred_60, Enum.PlayerResource.PrimaryHatred_80, Enum.PlayerResource.PrimaryHatred_100,
                Enum.PlayerResource.SecondaryDiscipline_0, Enum.PlayerResource.SecondaryDiscipline_20, Enum.PlayerResource.SecondaryDiscipline_40, Enum.PlayerResource.SecondaryDiscipline_60, Enum.PlayerResource.SecondaryDiscipline_80, Enum.PlayerResource.SecondaryDiscipline_100
            } },

            { PlayerClass.NecromancerFemale, new List<PlayerResource>
            {
                PlayerResource.PrimaryEssence_100, PlayerResource.PrimaryEssence_80, PlayerResource.PrimaryEssence_60,PlayerResource.PrimaryEssence_40,PlayerResource.PrimaryEssence_20,PlayerResource.PrimaryEssence_0,
            } },

            { PlayerClass.NecromancerMale, new List<PlayerResource>
            {
                PlayerResource.PrimaryEssence_100, PlayerResource.PrimaryEssence_80, PlayerResource.PrimaryEssence_60,PlayerResource.PrimaryEssence_40,PlayerResource.PrimaryEssence_20,PlayerResource.PrimaryEssence_0,
            } },

            { PlayerClass.WizardFemale, new List<PlayerResource> {
                PlayerResource.PrimaryArcanePower_0, PlayerResource.PrimaryArcanePower_20, PlayerResource.PrimaryArcanePower_40, PlayerResource.PrimaryArcanePower_60, PlayerResource.PrimaryArcanePower_80 ,PlayerResource.PrimaryArcanePower_100
            } },

            { PlayerClass.WizardMale,  new List<PlayerResource>
            {
                PlayerResource.PrimaryArcanePower_0, PlayerResource.PrimaryArcanePower_20, PlayerResource.PrimaryArcanePower_40, PlayerResource.PrimaryArcanePower_60, PlayerResource.PrimaryArcanePower_80 ,PlayerResource.PrimaryArcanePower_100
            } },

            { PlayerClass.MonkFemale,  new List<PlayerResource>
            {
                PlayerResource.PrimarySpirit_0, PlayerResource.PrimarySpirit_20, PlayerResource.PrimarySpirit_40, PlayerResource.PrimarySpirit_60, PlayerResource.PrimarySpirit_80, PlayerResource.PrimarySpirit_100
            } },


            { PlayerClass.MonkMale,  new List<PlayerResource>
            {
                PlayerResource.PrimarySpirit_0, PlayerResource.PrimarySpirit_20, PlayerResource.PrimarySpirit_40, PlayerResource.PrimarySpirit_60, PlayerResource.PrimarySpirit_80, PlayerResource.PrimarySpirit_100
            } },


            { PlayerClass.CrusaderFemale,  new List<PlayerResource>
            {
                PlayerResource.PrimaryWrath_0, PlayerResource.PrimaryWrath_20, PlayerResource.PrimaryWrath_40, PlayerResource.PrimaryWrath_60, PlayerResource.PrimaryWrath_80, PlayerResource.PrimaryWrath_100
            } },

            { PlayerClass.CrusaderMale,  new List<PlayerResource>
            {
                PlayerResource.PrimaryWrath_0, PlayerResource.PrimaryWrath_20, PlayerResource.PrimaryWrath_40, PlayerResource.PrimaryWrath_60, PlayerResource.PrimaryWrath_80, PlayerResource.PrimaryWrath_100
            } },


            { PlayerClass.BarbarianFemale,  new List<PlayerResource>
            {
                PlayerResource.PrimaryRage_0, PlayerResource.PrimaryRage_20, PlayerResource.PrimaryRage_40, PlayerResource.PrimaryRage_60, PlayerResource.PrimaryRage_80, PlayerResource.PrimaryRage_100
            } },

            { PlayerClass.BarbarianMale,  new List<PlayerResource>
            {
                PlayerResource.PrimaryRage_0, PlayerResource.PrimaryRage_20, PlayerResource.PrimaryRage_40, PlayerResource.PrimaryRage_60, PlayerResource.PrimaryRage_80, PlayerResource.PrimaryRage_100
            } },

            { PlayerClass.WitchDoctorFemale,  new List<PlayerResource>
            {
                PlayerResource.PrimaryMana_0, PlayerResource.PrimaryMana_20, PlayerResource.PrimaryMana_40, PlayerResource.PrimaryMana_60, PlayerResource.PrimaryMana_80, PlayerResource.PrimaryMana_100
            } },

            { PlayerClass.WitchDoctorMale,  new List<PlayerResource>
            {
                PlayerResource.PrimaryMana_0, PlayerResource.PrimaryMana_20, PlayerResource.PrimaryMana_40, PlayerResource.PrimaryMana_60, PlayerResource.PrimaryMana_80, PlayerResource.PrimaryMana_100
            } },
        };

        #endregion

        private readonly ILogService logService;

        public ModelService(Player player, World world, ILogService logService)
        {
            Player = player;
            World = world;

            this.logService = logService;
        }

        public Player Player { get; }

        public World World { get; }

        public IEnumerable<PlayerHealth> GetPossibleHealthEnum()
        {
            foreach (var health in System.Enum.GetValues(typeof(PlayerHealth)).Cast<PlayerHealth>())
                if (health != PlayerHealth.None) yield return health;
        }

        public IEnumerable<Enum.PlayerResource> GetPossiblePrimaryResourceEnum()
        {
            if (Player.Class == default) yield break;

            foreach (var @enum in ResourceDictionary[Player.Class])
            {
                if (@enum.ToString().StartsWith("Primary"))
                {
                    yield return @enum;
                }
            }
        }

        public IEnumerable<Enum.PlayerResource> GetPossibleSecondaryResourceEnum()
        {
            if (Player.Class == default) yield break;

            foreach (var @enum in ResourceDictionary[Player.Class])
            {
                if (@enum.ToString().StartsWith("Secondary"))
                {
                    yield return @enum;
                }
            }
        }

        public IEnumerable<SkillName> GetPossibleSkills(bool isMouse) // TODO: Can all skills be on the right mouse button? then this isnt needed here
        {
            if (Player.Class == default) yield break;

            switch (Player.Class)
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

        public Skill GetSkill(SkillName name)
        {
            return Player.Skills.FirstOrDefault(x => x?.Name == name);
        }

        public Skill GetSkill(int index)
        {
            if (index < Player.Skills.Count) return Player.Skills[index];

            return null;
        }

        public void SetPlayerHealth(PlayerHealth health)
        {
            if (health == PlayerHealth.None)
            {
                Player.HealthPercentage = 0;
            }
            else
            {
                Player.HealthPercentage = int.Parse(health.ToString().Split('_')[1]);
            }
        }

        public void SetPlayerPrimaryResource(PlayerResource primary)
        {
            if (primary.ToString().StartsWith("Secondary")) throw new Exception("Recived secondary resource in SetPlayerPrimaryResource()");

            if (primary == PlayerResource.None)
            {
                Player.PrimaryResourcePercentage = 0;
            }
            else
            {
                Player.PrimaryResourcePercentage = int.Parse(primary.ToString().Split('_')[1]);
            }
        }

        public void SetPlayerSecondaryResource(PlayerResource secondary)
        {
            if (secondary.ToString().StartsWith("Primary")) throw new Exception("Recived primary resource in SetPlayerSecondaryResource()");

            if (secondary == PlayerResource.None)
            {
                Player.SecondaryRessourcePercentage = 0;
            }
            else
            {
                Player.SecondaryRessourcePercentage = int.Parse(secondary.ToString().Split('_')[1]);
            }
        }

        public void SetSkill(int index, Skill skill)
        {
            if (index >= Player.Skills.Count)
            {
                Player.Skills.Add(skill);
            }
            else
            {
                Player.Skills[index] = skill;
            }
        }
    }
}