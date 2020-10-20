using System.Collections.Generic;

namespace Dolphin
{
    public class PlayerInformationChangedEvent : IEvent
    {
        public IList<string> ChangedProperties { get; set; } = new List<string>();
    }
}