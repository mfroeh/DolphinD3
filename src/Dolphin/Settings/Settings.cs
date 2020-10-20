using Dolphin.Enum;
using System.Collections.Generic;
using System.Windows.Forms;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin
{
    public class Settings
    {
        public IDictionary<Enum.HotkeyName, Hotkey> Hotkeys = new Dictionary<HotkeyName, Hotkey>();

        public IList<SkillName> EnabledSkills { get; set; } = new List<SkillName>();

        public bool IsPaused { get; set; }

        public IList<Keys> SkillKeybindings { get; set; } = new Keys[] { Keys.D1, Keys.D2, Keys.D3, Keys.D4 };

        public UiSettings UISettings { get; set; } = new UiSettings();
    }
}