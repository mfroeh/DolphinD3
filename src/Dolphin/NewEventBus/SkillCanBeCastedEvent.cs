using Dolphin.Enum;

namespace Dolphin.NewEventBus
{
    public class SkillCanBeCastedEvent : IEvent
    {
        public SkillName Name { get; set; }

        public int Index { get; set; }
    }
}
