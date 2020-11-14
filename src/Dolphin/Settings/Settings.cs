using Dolphin.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin
{
    public static class InitialSettings
    {
        public static IDictionary<ActionName, Hotkey> Hotkeys
        {
            get
            {
                var dict = new Dictionary<ActionName, Hotkey>();
                foreach (var @enum in EnumHelper.GetValues<ActionName>())
                {
                    dict[@enum] = null;
                }

                dict[ActionName.Pause] = new Hotkey(Keys.F10);
                dict[ActionName.Suspend_0] = new Hotkey(Keys.Control, Keys.D1);
                dict[ActionName.Suspend_1] = new Hotkey(Keys.Control, Keys.D2);
                dict[ActionName.Suspend_2] = new Hotkey(Keys.Control, Keys.D3);
                dict[ActionName.Suspend_3] = new Hotkey(Keys.Control, Keys.D4);
                dict[ActionName.Suspend_4] = new Hotkey(Keys.Control, Keys.D5);
                dict[ActionName.Suspend_5] = new Hotkey(Keys.Control, Keys.D6);
                dict[ActionName.SkillCastLoop] = new Hotkey(Keys.Control, Keys.O);

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

        public static SkillCastSettings SkillCastSettings
        {
            get
            {
                var configuration = new SkillCastConfiguration
                {
                    SkillIndices = new List<int> { 0 },
                    Delays = new Dictionary<int, int> { { 0, 250 } },
                    Name = "Profile 1"
                };

                var configuration2 = new SkillCastConfiguration
                {
                    SkillIndices = new List<int> { 0 },
                    Delays = new Dictionary<int, int> { { 0, 250 } },
                    Name = "Profile 2"
                };

                var configuration3 = new SkillCastConfiguration
                {
                    SkillIndices = new List<int> { 0 },
                    Delays = new Dictionary<int, int> { { 0, 250 } },
                    Name = "Profile 3"
                };

                var configuration4 = new SkillCastConfiguration
                {
                    SkillIndices = new List<int> { 0 },
                    Delays = new Dictionary<int, int> { { 0, 250 } },
                    Name = "Profile 4"
                };

                return new SkillCastSettings
                {
                    SkillCastConfigurations = new List<SkillCastConfiguration> { configuration, configuration2, configuration3, configuration4 },
                    SelectedIndex = 0
                };
            }
        }

        public static IList<Keys> SkillKeybindings => new Keys[] { Keys.D1, Keys.D2, Keys.D3, Keys.D4 };

        public static SmartFeatureSettings SmartFeatureSettings
        {
            get
            {
                return new SmartFeatureSettings
                {
                    EnabledSkills = new List<SkillName>
                                                {
                                                    SkillName.Devour,
                                                    SkillName.LandOfTheDead,
                                                    SkillName.SkeletalMage,
                                                    SkillName.BoneArmor,
                                                    SkillName.Simulacrum
                                                },
                    EnabledSmartActions = new List<SmartActionName>
                                                    {
                                                        SmartActionName.Gamble,
                                                        SmartActionName.AcceptGriftPopup,
                                                        SmartActionName.OpenRiftGrift,
                                                        SmartActionName.UpgradeGem,
                                                        SmartActionName.StartGame
                                                    },
                    IsOpenRift = true,
                    SkillSuspensionStatus = new bool[6],
                    UpdateInterval = 100
                };
            }
        }

        public static UiSettings UiSettings => new UiSettings
        {
            DisplayLogLevel = LogLevel.Warning,
            LogPaused = false,
            IsDark = false,
            ExecuteablePaths = new Dictionary<string, string>
            {
                { "Diablo III64", "" },
                { "TurboHUD", "" }
            }
        };
    }

    public class Settings
    {
        public Settings(bool reset = false)
        {
            if (reset)
            {
                Hotkeys = InitialSettings.Hotkeys;
                MacroSettings = InitialSettings.MacroSettings;
                OtherKeybindings = InitialSettings.OtherKeybindings;
                SkillKeybindings = InitialSettings.SkillKeybindings;
                UiSettings = InitialSettings.UiSettings;
                SmartFeatureSettings = InitialSettings.SmartFeatureSettings;
                SkillCastSettings = InitialSettings.SkillCastSettings;
            }
        }

        public IDictionary<ActionName, Hotkey> Hotkeys { get; set; }

        public bool IsPaused { get; set; }

        public MacroSettings MacroSettings { get; set; }

        public IDictionary<CommandKeybinding, Keys> OtherKeybindings { get; set; }

        public SkillCastSettings SkillCastSettings { get; set; }

        public IList<Keys> SkillKeybindings { get; set; }

        public SmartFeatureSettings SmartFeatureSettings { get; set; }

        public UiSettings UiSettings { get; set; }
    }
}