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

        public Skill GetSkill(SkillName name)
        {
            return Player.Skills.FirstOrDefault(x => x?.Name == name);
        }

        public Skill GetSkill(int index)
        {
            if (index < Player.Skills.Count) return Player.Skills[index];

            return null;
        }

        public IEnumerable<PlayerHealth> PossibleHealthEnum()
        {
            yield return PlayerHealth.H_0;
            yield return PlayerHealth.H_20;
            yield return PlayerHealth.H_40;
            yield return PlayerHealth.H_60;
            yield return PlayerHealth.H_80;
            yield return PlayerHealth.H_100;
        }

        public IEnumerable<PlayerResource> PossiblePrimaryResourceEnum()
        {
            return Player.PossiblePrimaryResource();
        }

        public IEnumerable<PlayerResource> PossibleSecondaryResourceEnum()
        {
            return Player.PossibleSecondaryResource();
        }

        public IEnumerable<SkillName> PossibleSkills()
        {
            return Player.PossibleSkills();
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