using Dolphin.Ui.HotkeyInput;
using MvvmDialogs;
using System.Diagnostics;
using System.Windows.Input;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin.Ui
{
    public class HotkeyTabViewModel : ViewModelBase
    {
        private readonly IDialogService dialogService;

        public HotkeyTabViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        public ICommand ChangeHotkeyCommand => new RelayCommand((index) => ShowDialog(new Hotkey("Control+Shift+D4")));

        private void ShowDialog(Hotkey currentHotkey)
        {
            var dialogViewModel = new ChangeHotkeyDialogViewModel(currentHotkey);

            bool? success = dialogService.ShowDialog(this, dialogViewModel);

            Trace.WriteLine(success);

            if (success == true)
            {

            }
        }
    }
}