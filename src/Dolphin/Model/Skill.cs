using Dolphin.Enum;

namespace Dolphin
{
    public class Skill
    {
        public SkillName Name { get; set; }
        public int Index { get; set; }
        public bool IsNotActiveAndCanBeCasted { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Skill otherSkill)
                return otherSkill.Name == Name && otherSkill.IsNotActiveAndCanBeCasted == IsNotActiveAndCanBeCasted && otherSkill.Index == Index;
            else
                return false;
        }
    }
}