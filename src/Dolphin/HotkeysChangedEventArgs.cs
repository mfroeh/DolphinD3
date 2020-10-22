using System;
using System.Collections;
using System.Collections.Generic;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin
{
    public class HotkeysChangedEventArgs : EventArgs
    {
        public IList<Hotkey> NewHotkeys { get; set; }
    }
}