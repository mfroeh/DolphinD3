using Dolphin.Enum;

namespace Dolphin
{
    public class Skill
    {
        public SkillName Name { get; set; }
        public int Index { get; set; }
        public bool CanCast { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Skill otherSkill)
                return otherSkill.Name == Name && otherSkill.CanCast == CanCast && otherSkill.Index == Index;
            else
                return false;
        }
    }
}