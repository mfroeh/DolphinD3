using Dolphin.Enum;

namespace Dolphin
{
    public class Skill
    {
        public bool CanBeCasted { get; set; }

        public int Index { get; set; }

        public bool IsActive { get; set; }

        public SkillName Name { get; set; }
    }
}