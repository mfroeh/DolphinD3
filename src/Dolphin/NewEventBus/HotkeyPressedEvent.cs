using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin.NewEventBus
{
    public class HotkeyPressedEvent : IEvent
    {
        public Hotkey Hotkey { get; set; }
    }
}
