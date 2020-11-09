using Dolphin.Enum;
using MvvmDialogs;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin.Ui.Dialog
{
    public class ChangeHotkeyDialogViewModel : ViewModelBase, IModalDialogViewModel
    {
        private readonly IDialogService dialogService;
        private readonly ISettingsService settingsService;

        private bool? dialogResult;
        private Hotkey hotkey;
        private Hotkey oldHotkey;

        public ChangeHotkeyDialogViewModel(ISettingsService settingsService, IDialogService dialogService)
        {
            this.settingsService = settingsService;
            this.dialogService = dialogService;
        }

        public ICommand CancelCommand => new RelayCommand((_) => DialogResult = false);

        public bool? DialogResult
        {
            get
            {
                return dialogResult;
            }
            set
            {
                dialogResult = value;
                RaisePropertyChanged("DialogResult");
            }
        }

        public ActionName EditingAction { get; set; }

        public Hotkey Hotkey
        {
            get => hotkey;
            set
            {
                hotkey = value;
                RaisePropertyChanged("Hotkey");
            }
        }

        public ICommand RevertCommand => new RelayCommand((_) => Hotkey = oldHotkey);

        public ICommand SaveCommand => new RelayCommand(SaveCommandAction);

        public void Initialize(Hotkey currentHotkey, ActionName actionToEdit)
        {
            oldHotkey = currentHotkey;
            hotkey = currentHotkey;
            EditingAction = actionToEdit;
        }

        private void SaveCommandAction(object o)
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
                    var result = ShowWarningMessageBox($"{Hotkey} is already allocated to {actionWithSameHotkey}. Delete existing allocation?");

                    if (!result) return;
                }
            }

            DialogResult = true;
        }

        private bool ShowWarningMessageBox(string text)
        {
            var result = dialogService.ShowMessageBox(
                            this,
                            text,
                            "Existing Hotkey allocation found",
                            MessageBoxButton.OKCancel,
                            MessageBoxImage.Warning);

            return result == MessageBoxResult.OK;
        }
    }
}