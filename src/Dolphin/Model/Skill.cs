using Dolphin.Enum;

namespace Dolphin
{
    public class Skill
    {
        public SkillName Name { get; set; }
        public int Index { get; set; }
        public bool IsNotActiveAndCanBeCasted { get; set; }
    }
}