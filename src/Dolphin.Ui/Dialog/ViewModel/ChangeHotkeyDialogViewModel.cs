using Dolphin.Enum;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin.Ui.Dialog
{
    public class ChangeHotkeyDialogViewModel : DialogViewModelBase, IDialogViewModel
    {
        private readonly IMessageBoxService messageBoxService;
        private readonly ISettingsService settingsService;

        private Hotkey hotkey;
        private Hotkey oldHotkey;

        public ChangeHotkeyDialogViewModel(ISettingsService settingsService, IMessageBoxService messageBoxService)
        {
            this.settingsService = settingsService;
            this.messageBoxService = messageBoxService;
        }

        public ActionName EditingAction { get; set; }

        public Hotkey Hotkey
        {
            get => hotkey;
            set
            {
                hotkey = value;
                RaisePropertyChanged(nameof(Hotkey));
            }
        }

        public ICommand RevertCommand => new RelayCommand((_) => Hotkey = oldHotkey);

        public override void Initialize(params object[] @params)
        {
            oldHotkey = (Hotkey)@params[0];
            hotkey = (Hotkey)@params[0];

            EditingAction = (ActionName)@params[1];
        }

        protected override void DialogOkClicked(object o)
        {
            if (Hotkey != null)
            {
                var actionWithSameHotkey = ActionName.None;
                foreach (var item in settingsService.Settings.Hotkeys.Where(item => item.Key != EditingAction))
                {
                    if (item.Value == Hotkey)
                    {
                        actionWithSameHotkey = item.Key;
                        break;
                    }
                }

                if (actionWithSameHotkey != ActionName.None)
                {
                    var result = messageBoxService.ShowYesNo(this, "Hotkey allocation found", $"{Hotkey} is already allocated to {actionWithSameHotkey}. Delete existing allocation?", MessageBoxImage.Warning);

                    if (result != MessageBoxResult.Yes) return;
                }
            }

            DialogResult = true;
        }
    }
}