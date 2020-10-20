using Dolphin.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dolphin.Service
{
    public class ModelService : IModelService
    {
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
            foreach (var @enum in StaticResource.ResourceByClass[Player.Class])
            {
                if (!nameof(@enum).StartsWith("Secondary"))
                {
                    yield return @enum;
                }
            }
        }

        public IEnumerable<Enum.PlayerResource> GetPossibleSecondary()
        {
            foreach (var @enum in StaticResource.ResourceByClass[Player.Class])
            {
                if (!nameof(@enum).StartsWith("Primary"))
                {
                    yield return @enum;
                }
            }
        }

        public IEnumerable<SkillName> GetPossibleSkills()
        {
            foreach (var @enum in StaticResource.SkillsByClass[Player.Class])
            {
                yield return @enum;
            }
        }

        public Skill GetSkill(SkillName name)
        {
            return Player.Skills.FirstOrDefault(x => x.Name == name);
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