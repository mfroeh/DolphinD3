using MvvmDialogs;
using System.Linq;
using System.Reflection;
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

        public void SetHotkey(Hotkey currentHotkey)
        {
            oldHotkey = currentHotkey;
            hotkey = currentHotkey;
        }

        private void SaveCommandAction(object o)
        {
            if (settingsService.Settings.Hotkeys.Values.Contains(Hotkey) && Hotkey != null)
            {
                var actionName = settingsService.Settings.Hotkeys.Where(item => item.Value == Hotkey).First().Key; // TODO: Also need the actionname here
                var result = ShowWarningMessageBox($"{Hotkey} is already allocated to {actionName}. Delete existing allocation?");

                if (!result) return;
            }

            DialogResult = true;
        }

        private bool ShowWarningMessageBox(string text)
        {
            var result = dialogService.ShowMessageBox(
                            this,
                            text,
                            "Existing Hotkey allocation found",
                            MessageBoxButton.OKCancel);

            return result == MessageBoxResult.OK;
        }
    }
}