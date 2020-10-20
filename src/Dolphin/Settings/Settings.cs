using Dolphin.Enum;
using System.Collections.Generic;
using System.Windows.Forms;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin
{
    public class Settings
    {
        public IList<SkillName> EnabledSkills { get; set; } = new List<SkillName>();

        public IDictionary<Enum.ActionName, Hotkey> Hotkeys { get; set; } = new Dictionary<ActionName, Hotkey>() { { ActionName.Pause, new Hotkey("D") }, { ActionName.CubeConverter, new Hotkey("D") } };

        public bool IsPaused { get; set; }

        public MacroSettings MacroSettings { get; set; } = new MacroSettings();

        public IList<Keys> SkillKeybindings { get; set; } = new Keys[] { Keys.D1, Keys.D2, Keys.D3, Keys.D4 };

        public UiSettings UiSettings { get; set; } = new UiSettings();
    }
}