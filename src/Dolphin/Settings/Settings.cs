using Dolphin.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin
{
    public static class InitialSettings
    {
        public static IList<SkillName> EnabledSkills => new List<SkillName>
        {
            SkillName.Devour,
            SkillName.LandOfTheDead,
            SkillName.SkeletalMage,
            SkillName.BoneArmor,
            SkillName.Simulacrum
        };

        public static IList<SmartActionName> EnabledSmartActions
        {
            get
            {
                return new List<SmartActionName>
                {
                    SmartActionName.Gamble,
                    SmartActionName.AcceptGriftPopup,
                    SmartActionName.OpenRiftGrift,
                    SmartActionName.UpgradeGem,
                    SmartActionName.StartGame
                };
            }
        }

        public static IDictionary<ActionName, Hotkey> Hotkeys
        {
            get
            {
                var dict = new Dictionary<ActionName, Hotkey>();
                foreach (var @enum in System.Enum.GetValues(typeof(ActionName)).Cast<ActionName>())
                {
                    if (@enum != ActionName.None)
                    {
                        dict[@enum] = null;
                    }
                }

                dict[ActionName.Pause] = new Hotkey(Keys.F10);
                dict[ActionName.CancelAction] = new Hotkey(Keys.Escape);
                dict[ActionName.Suspend_0] = new Hotkey(Keys.Control, Keys.D1);
                dict[ActionName.Suspend_1] = new Hotkey(Keys.Control, Keys.D2);
                dict[ActionName.Suspend_2] = new Hotkey(Keys.Control, Keys.D3);
                dict[ActionName.Suspend_3] = new Hotkey(Keys.Control, Keys.D4);
                dict[ActionName.Suspend_4] = new Hotkey(Keys.Control, Keys.D5);
                dict[ActionName.Suspend_5] = new Hotkey(Keys.Control, Keys.D6);

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
                new Waypoint { Act = 1, Name = "CemetryOfTheForsaken"},
                new Waypoint { Act = 1, Name = "LeoricsManor" },
                new Waypoint { Act = 1, Name = "DrownedTemple" },
                new Waypoint { Act = 1, Name = "FieldsOfMisery" },
                new Waypoint { Act = 1, Name = "SouthernHighlands" },
                new Waypoint { Act = 1, Name = "TheWeepingHollow" },
                new Waypoint { Act = 2, Name = "HowlingPlateau" },
                new Waypoint { Act = 2, Name = "RoadToAlcarnus" },
                new Waypoint { Act = 3, Name = "BrideOfKorsikk" },
                new Waypoint { Act = 3, Name = "TheBattlefields" },
                new Waypoint { Act = 3, Name = "TowerOfTheCursedLevel1" },
                new Waypoint { Act = 3, Name = "TowerOfTheDamnedLevel1" },
                new Waypoint { Act = 3, Name = "RakkisCrossing" },
                new Waypoint { Act = 4, Name = "RealmOfFracturedFate" },
                new Waypoint { Act = 4, Name = "LowerRealmOfInfernalFate" },
                new Waypoint { Act = 4, Name = "PandemoniumFortressLevel1" },
            }
        };

        public static IDictionary<CommandKeybinding, Keys> OtherKeybindings
        {
            get
            {
                return new Dictionary<CommandKeybinding, Keys>
                {
                    { CommandKeybinding.TeleportToTown, Keys.T },
                    { CommandKeybinding.OpenMap, Keys.M },
                    { CommandKeybinding.OpenInventory, Keys.C }
                };
            }
        }

        public static IList<Keys> SkillKeybindigns => new Keys[] { Keys.D1, Keys.D2, Keys.D3, Keys.D4 };

        public static SmartActionSettings SmartActionSettings => new SmartActionSettings { IsOpenRift = true };

        public static IList<bool> SuspendedSkillIndices => new bool[6];

        public static UiSettings UiSettings => new UiSettings
        {
            DisplayLogLevel = LogLevel.Warning,
            LogPaused = false,
            IsDark = false
        };

        public static uint UpdateInterval => 100;
    }

    public class Settings
    {
        public Settings(bool reset = false)
        {
            if (reset)
            {
                EnabledSkills = InitialSettings.EnabledSkills;
                Hotkeys = InitialSettings.Hotkeys;
                MacroSettings = InitialSettings.MacroSettings;
                OtherKeybindings = InitialSettings.OtherKeybindings;
                SkillKeybindings = InitialSettings.SkillKeybindigns;
                UiSettings = InitialSettings.UiSettings;
                UpdateInterval = InitialSettings.UpdateInterval;
                EnabledSmartActions = InitialSettings.EnabledSmartActions;
                SmartActionSettings = InitialSettings.SmartActionSettings;
                SkillSuspensionStatus = InitialSettings.SuspendedSkillIndices;
            }
        }

        public IList<SkillName> EnabledSkills { get; set; }

        public IList<SmartActionName> EnabledSmartActions { get; set; }

        public IDictionary<ActionName, Hotkey> Hotkeys { get; set; }

        public bool IsPaused { get; set; }

        public MacroSettings MacroSettings { get; set; }

        public IDictionary<CommandKeybinding, Keys> OtherKeybindings { get; set; }

        public bool SkillCastingEnabled { get; set; }

        public IList<Keys> SkillKeybindings { get; set; }

        public IList<bool> SkillSuspensionStatus { get; set; }

        public bool SmartActionsEnabled { get; set; }

        public SmartActionSettings SmartActionSettings { get; set; }

        public UiSettings UiSettings { get; set; }

        public uint UpdateInterval { get; set; }
    }
}