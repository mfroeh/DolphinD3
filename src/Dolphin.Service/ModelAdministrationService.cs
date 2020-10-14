using Dolphin.Enum;
using System.Collections.Generic;
using System.Linq;

namespace Dolphin.Service
{
    public class ModelAdministrationService : IModelAdministrationService
    {
        public ModelAdministrationService(Player player, World world)
        {
            this.Player = player;
            this.World = world;
        }

        public Player Player { get; }
        public World World { get; }

        public IEnumerable<SkillName> GetPossibleSkills()
        {
            foreach (var skillName in System.Enum.GetValues(typeof(SkillName)))
                if ((SkillName)skillName != SkillName.None) yield return (SkillName)skillName;
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

        public void SetSkill(int index, Skill skill)
        {
            if (index >= Player.Skills.Count)
                Player.Skills.Add(skill);
            else
                Player.Skills[index] = skill;
        }
    }
}