using Dolphin.Enum;

namespace Dolphin
{
    public class SkillRecognitionChangedEvent : IEvent
    {
        public int Index { get; set; }

        public SkillName NewSkillName { get; set; }
    }
}