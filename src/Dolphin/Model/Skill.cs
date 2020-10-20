using Dolphin.Enum;

namespace Dolphin
{
    public class Skill
    {
        public int Index { get; set; }

        public bool IsNotActiveAndCanBeCasted { get; set; }

        public SkillName Name { get; set; }
    }
}