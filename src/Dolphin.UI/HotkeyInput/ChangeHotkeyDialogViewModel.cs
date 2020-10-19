using MvvmDialogs;
using System.Windows.Input;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin.Ui.HotkeyInput
{
    public class ChangeHotkeyDialogViewModel : ViewModelBase, IModalDialogViewModel
    {
        private InputControlHotkey hotkey;
        public ChangeHotkeyDialogViewModel(Hotkey currentHotkey)
        {
            hotkey = new InputControlHotkey(currentHotkey);
        }

        public InputControlHotkey Hotkey
        {
            get => hotkey;
            set
            {
                hotkey = value;
                RaisePropertyChanged("Hotkey");
            }
        }

        public Hotkey GetHotkey => Hotkey.ToHotkey();

        public ICommand CancelCommand => new RelayCommand((_) => DialogResult = false);

        public ICommand SaveCommand => new RelayCommand((_) => DialogResult = true);

        public bool? DialogResult { get; set; }
    }
}
