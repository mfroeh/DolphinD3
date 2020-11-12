using System.Collections.Generic;
using System.Linq;

namespace Dolphin.Ui.Dialog
{
    public class ResetSettingsDialogViewModel : DialogViewModelBase
    {
        public IDictionary<string, bool> SettingsStatus { get; set; }

        public ResetSettingsDialogViewModel()
        {
            SettingsStatus = new Dictionary<string, bool>();
            SettingsStatus["All"] = default;
            SettingsStatus[nameof(Settings.Hotkeys)] = default;
            SettingsStatus[nameof(Settings.MacroSettings)] = default;
            SettingsStatus[nameof(Settings.OtherKeybindings)] = default;
            SettingsStatus[nameof(Settings.SkillCastSettings)] = default;
            SettingsStatus[nameof(Settings.SkillKeybindings)] = default;
            SettingsStatus[nameof(Settings.SmartFeatureSettings)] = default;
            SettingsStatus[nameof(Settings.UiSettings)] = default;
        }

        public IList<string> SettingsToReset { get; set; }

        public override void Initialize(params object[] @params)
        {
        }

        protected override void DialogOkClicked(object o)
        {
            SettingsToReset = SettingsStatus.Where(x => x.Value).Select(x => x.Key).ToList();

            base.DialogOkClicked(o);
        }
    }
}