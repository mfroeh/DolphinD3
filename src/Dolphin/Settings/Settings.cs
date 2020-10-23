using Dolphin.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin
{
    public static class InitialSettings
    {
        public static IList<SkillName> EnabledSkills = new List<SkillName>();

        public static MacroSettings MacroSettings = new MacroSettings
        {
            ConvertingSpeed = ConvertingSpeed.Normal,
            SelectedGambleItem = ItemType.OneHandedWeapon,
            SpareColumns = 1,
            SwapItemsAmount = 3
        };

        public static Keys OpenInventoryKeybinding = Keys.C;

        public static Keys OpenMapKeybinding = Keys.M;

        public static IList<Keys> SkillKeybindigns = new Keys[] { Keys.D1, Keys.D2, Keys.D3, Keys.D4 };

        public static Keys TeleportToTownKeybinding = Keys.T;

        public static UiSettings UiSettings = new UiSettings
        {
            DisplayLogLevel = LogLevel.Warning,
            LogPaused = true
        };

        public static IDictionary<ActionName, Hotkey> Hotkeys
        {
            get
            {
                var dict = new Dictionary<ActionName, Hotkey>();
                foreach (var @enum in System.Enum.GetValues(typeof(ActionName)).Cast<ActionName>())
                {
                    if (@enum != ActionName.None)
                    {
                        dict[@enum] = @enum == ActionName.Pause ? new Hotkey(Keys.Control, Keys.C) : null;
                    }
                }
                return dict;
            }
        }
    }

    public class Settings
    {
        public IList<SkillName> EnabledSkills { get; set; } = InitialSettings.EnabledSkills;

        public IDictionary<Enum.ActionName, Hotkey> Hotkeys { get; set; } = InitialSettings.Hotkeys;

        public bool IsPaused { get; set; }

        public MacroSettings MacroSettings { get; set; } = InitialSettings.MacroSettings;

        public Keys OpenInventoryKeybinding { get; set; } = InitialSettings.OpenInventoryKeybinding;

        public Keys OpenMapKeybinding { get; set; } = InitialSettings.OpenMapKeybinding;

        public IList<Keys> SkillKeybindings { get; set; } = InitialSettings.SkillKeybindigns;

        public Keys TeleportToTownKeybinding { get; set; } = InitialSettings.TeleportToTownKeybinding;

        public UiSettings UiSettings { get; set; } = InitialSettings.UiSettings;
    }
}