using Dolphin.Enum;
using System.Windows.Forms;

namespace Dolphin
{
    public class Skill
    {
        public int Index { get; set; }

        public bool CanBeCasted { get; set; }

        public bool IsActive { get; set; }

        public SkillName Name { get; set; }
    }
}