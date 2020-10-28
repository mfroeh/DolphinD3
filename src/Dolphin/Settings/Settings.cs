using Dolphin.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin
{
    public static class InitialSettings
    {
        private static IList<ActionName> _nonMacroableActions = new List<ActionName> { ActionName.None, ActionName.Smart_OpenRift, ActionName.Smart_AcceptGriftPopup, ActionName.Smart_StartGame };

        public static uint UpdateInterval => 100;

        public static IList<SkillName> EnabledSkills => new List<SkillName>() { SkillName.Devour, SkillName.LandOfTheDead,
                                        SkillName.SkeletalMage, SkillName.BoneArmor, SkillName.Simulacrum };

        public static IDictionary<ActionName, Hotkey> Hotkeys
        {
            get
            {
                var dict = new Dictionary<ActionName, Hotkey>();
                foreach (var @enum in System.Enum.GetValues(typeof(ActionName)).Cast<ActionName>())
                {
                    if (!_nonMacroableActions.Contains(@enum))
                    {
                        Hotkey hotkey = null;
                        if (@enum == ActionName.CancelAction)
                        {
                            hotkey = new Hotkey(Keys.Escape);
                        }
                        else if (@enum == ActionName.Pause)
                        {
                            hotkey = new Hotkey(Keys.F10);
                        }

                        dict[@enum] = hotkey;
                    }
                }
                return dict;
            }
        }

        public static MacroSettings MacroSettings => new MacroSettings
        {
            ConvertingSpeed = ConvertingSpeed.Normal,
            SelectedGambleItem = ItemType.OneHandedWeapon,
            SpareColumns = 1,
            SwapItemsAmount = 3,
            Poolspots = new List<Waypoint>
            {
                new Waypoint{Act = 1, Name = "CemetryOfTheForsaken" },
                new Waypoint{Act = 2, Name = "HowlingPlateau"}
            }
        };

        public static IDictionary<Command, Keys> OtherKeybindings
        {
            get
            {
                return new Dictionary<Command, Keys>
                {
                    { Command.TeleportToTown, Keys.T },
                    { Command.OpenMap, Keys.M },
                    { Command.OpenInventory, Keys.C }
                };
            }
        }

        public static IList<Keys> SkillKeybindigns => new Keys[] { Keys.D1, Keys.D2, Keys.D3, Keys.D4 };

        public static UiSettings UiSettings => new UiSettings
        {
            DisplayLogLevel = LogLevel.Warning,
            LogPaused = false,
            IsDark = false
        };

        public static SmartActionSettings SmartActionSettings
        {
            get
            {
                return new SmartActionSettings();
            }
        }
    }

    public class Settings
    {
        public IList<SkillName> EnabledSkills { get; set; }

        public IDictionary<Enum.ActionName, Hotkey> Hotkeys { get; set; }

        [JsonIgnore]
        public bool IsPaused { get; set; }

        public MacroSettings MacroSettings { get; set; }

        public IDictionary<Command, Keys> OtherKeybindings { get; set; }
        public IList<Keys> SkillKeybindings { get; set; }

        public UiSettings UiSettings { get; set; }

        public SmartActionSettings SmartActionSettings { get; set; }

        public uint UpdateInterval { get; set; }

        public Settings(bool initialize = false)
        {
            if (initialize)
            {
                EnabledSkills = InitialSettings.EnabledSkills;
                Hotkeys = InitialSettings.Hotkeys;
                MacroSettings = InitialSettings.MacroSettings;
                OtherKeybindings = InitialSettings.OtherKeybindings;
                SkillKeybindings = InitialSettings.SkillKeybindigns;
                UiSettings = InitialSettings.UiSettings;
                UpdateInterval = InitialSettings.UpdateInterval;
                SmartActionSettings = InitialSettings.SmartActionSettings;
            }
        }
    }
}