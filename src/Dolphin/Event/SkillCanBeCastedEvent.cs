using Dolphin.Enum;

namespace Dolphin
{
    public class SkillCanBeCastedEvent : IEvent
    {
        public int SkillIndex { get; set; }

        public SkillName SkillName { get; set; }
    }
}