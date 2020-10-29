using Dolphin.Enum;
using System.Collections.Generic;

namespace Dolphin
{
    public class WorldInformationChangedEvent : IEvent
    {
        public bool IsLocationChanged { get; set; }

        public bool IsWindowChanged { get; set; }

        public WorldLocation NewLocation { get; set; }

        public Window NewOpenWindow { get; set; }

        public IList<object> WindowExtraInformation { get; set; } = new List<object>();
    }
}