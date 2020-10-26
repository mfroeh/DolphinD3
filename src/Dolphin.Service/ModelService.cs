using Dolphin.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dolphin.Service
{
    public class ModelService : IModelService
    {
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
            } }
        };

        private static readonly IDictionary<PlayerClass, IList<SkillName>> SkillDictionary = new Dictionary<PlayerClass, IList<SkillName>>
        {
            { PlayerClass.DemonHunterFemale, new List<SkillName> {
                SkillName.Companion, SkillName.FanOfKnives, SkillName.Preperation, SkillName.ShadowPower, SkillName.Vengeance
            } },
        };

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
                if (!nameof(@enum).StartsWith("Secondary"))
                {
                    yield return @enum;
                }
            }
        }

        public IEnumerable<Enum.PlayerResource> GetPossibleSecondary()
        {
            if (Player.Class == default) yield break;

            foreach (var @enum in ResourceDictionary[Player.Class])
            {
                if (!nameof(@enum).StartsWith("Primary"))
                {
                    yield return @enum;
                }
            }
        }

        public IEnumerable<SkillName> GetPossibleSkills(bool isMouse)
        {
            if (Player.Class == default) yield break;

            switch (Player.Class)
            {
                case PlayerClass.NecromancerMale:
                case PlayerClass.NecromancerFemale:
                    if (isMouse)
                    {
                        yield return SkillName.BoneArmor;
                        yield return SkillName.BloodRush;
                        yield return SkillName.CommandSkeletons;
                        yield return SkillName.SkeletalMage;
                        yield return SkillName.LandOfTheDead;
                        yield return SkillName.Simulacrum;
                        yield return SkillName.Devour;
                    }
                    else
                    {
                        yield return SkillName.BoneArmor;
                        yield return SkillName.BloodRush;
                        yield return SkillName.CommandSkeletons;
                        yield return SkillName.SkeletalMage;
                        yield return SkillName.LandOfTheDead;
                        yield return SkillName.Simulacrum;
                        yield return SkillName.Devour;
                    }
                    break;
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

        public void SetPlayerPrimaryResource(Enum.PlayerResource primary)
        {
            if (primary.ToString().StartsWith("Secondary")) throw new Exception("Recived secondary resource in SetPlayerPrimaryResource()");

            if (primary == Enum.PlayerResource.None)
            {
                Player.PrimaryResourcePercentage = 0;
            }
            else
            {
                Player.PrimaryResourcePercentage = int.Parse(primary.ToString().Split('_')[1]);
            }
        }

        public void SetPlayerSecondaryResource(Enum.PlayerResource secondary)
        {
            if (secondary.ToString().StartsWith("Primary")) throw new Exception("Recived primary resource in SetPlayerSecondaryResource()");

            if (secondary == Enum.PlayerResource.None)
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