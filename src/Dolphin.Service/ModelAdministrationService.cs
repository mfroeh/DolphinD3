using Dolphin.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dolphin.Service
{
    public class ModelAdministrationService : IModelAdministrationService
    {
        public ModelAdministrationService(Player player, World world)
        {
            Player = player;
            World = world;
        }

        public Player Player { get; }
        public World World { get; }

        public IEnumerable<PlayerHealth> GetPossibleHealthPercentage()
        {
            foreach (var health in System.Enum.GetValues(typeof(PlayerHealth)).Cast<PlayerHealth>())
                if (health != PlayerHealth.None) yield return health;
        }

        public IEnumerable<PlayerResource> GetPossiblePrimaryResource()
        {
            switch (Player.Class)
            {
                case PlayerClass.DemonHunterFemale:
                case PlayerClass.DemonHunterMale:
                    yield return PlayerResource.PrimaryHatred_100;
                    yield return PlayerResource.PrimaryHatred_80;
                    yield return PlayerResource.PrimaryHatred_60;
                    yield return PlayerResource.PrimaryHatred_40;
                    yield return PlayerResource.PrimaryHatred_20;
                    yield return PlayerResource.PrimaryHatred_0;
                    break;

                default:
                    throw new NotImplementedException($"GetPossiblePrimaryResource() in ModelAdministrationService.cs is not yet implemented for class {Player.Class}.");
            }
        }

        public IEnumerable<PlayerResource> GetPossibleSecondaryResource()
        {
            switch (Player.Class)
            {
                case PlayerClass.DemonHunterFemale:
                case PlayerClass.DemonHunterMale:
                    yield return PlayerResource.SecondaryDiscipline_100;
                    yield return PlayerResource.SecondaryDiscipline_80;
                    yield return PlayerResource.SecondaryDiscipline_60;
                    yield return PlayerResource.SecondaryDiscipline_40;
                    yield return PlayerResource.SecondaryDiscipline_20;
                    yield return PlayerResource.SecondaryDiscipline_0;
                    break;

                default:
                    yield break;
            }
        }

        public IEnumerable<SkillName> GetPossibleSkills()
        {
            switch (Player.Class)
            {
                case PlayerClass.DemonHunterFemale:
                case PlayerClass.DemonHunterMale:
                    yield return SkillName.Companion;
                    yield return SkillName.Preperation;
                    yield return SkillName.ShadowPower;
                    yield return SkillName.Strafe;
                    yield return SkillName.FanOfKnives;
                    break;

                default:
                    throw new NotImplementedException($"GetPossibleSkills() in ModelAdministrationService.cs is not yet implemented for class {Player.Class}.");
            }
        }

        public void SetPlayerHealth(PlayerHealth health)
        {
            if (health == PlayerHealth.None)
                Player.HealthPercentage = 0;
            else
                Player.HealthPercentage = int.Parse(health.ToString().Split('_')[1]);
        }

        public void SetPlayerPrimaryResource(PlayerResource primary)
        {
            if (primary.ToString().StartsWith("Secondary")) throw new Exception("Recived secondary resource in SetPlayerPrimaryResource()");

            if (primary == PlayerResource.None)
                Player.PrimaryRessourcePercentage = 0;
            else
                Player.PrimaryRessourcePercentage = int.Parse(primary.ToString().Split('_')[1]);
        }

        public void SetPlayerSecondaryResource(PlayerResource secondary)
        {
            if (secondary.ToString().StartsWith("Primary")) throw new Exception("Recived primary resource in SetPlayerSecondaryResource()");

            if (secondary == PlayerResource.None)
                Player.SecondaryRessourcePercentage = 0;
            else
                Player.SecondaryRessourcePercentage = int.Parse(secondary.ToString().Split('_')[1]);
        }

        public void SetSkill(int index, Skill skill)
        {
            if (index >= Player.Skills.Count)
                Player.Skills.Add(skill);
            else
                Player.Skills[index] = skill;
        }

        public Skill GetSkill(SkillName name)
        {
            return Player.Skills.FirstOrDefault(x => x.Name == name);
        }

        public Skill GetSkill(int index)
        {
            if (index < Player.Skills.Count)
                return Player.Skills[index];
            else
                return null;
        }
    }
}