using Dolphin.Enum;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin.Ui.Dialog
{
    public class ChangeHotkeyDialogViewModel : ViewModelBase, IDialogViewModel
    {
        #region Private Fields

        private readonly IMessageBoxService messageBoxService;
        private readonly ISettingsService settingsService;

        private bool? dialogResult;
        private Hotkey hotkey;
        private Hotkey oldHotkey;

        #endregion Private Fields

        #region Public Constructors

        public ChangeHotkeyDialogViewModel(ISettingsService settingsService, IMessageBoxService messageBoxService)
        {
            this.settingsService = settingsService;
            this.messageBoxService = messageBoxService;
        }

        #endregion Public Constructors

        #region Public Properties

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
                RaisePropertyChanged(nameof(DialogResult));
            }
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

        public ICommand SaveCommand => new RelayCommand(SaveCommandAction);

        #endregion Public Properties

        #region Public Methods

        public void Initialize(params object[] @params)
        {
            oldHotkey = (Hotkey)@params[0];
            hotkey = (Hotkey)@params[0];

            EditingAction = (ActionName)@params[1];
        }

        #endregion Public Methods

        #region Private Methods

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
                    var result = messageBoxService.ShowYesNo(this, "Hotkey allocation found", $"{Hotkey} is already allocated to {actionWithSameHotkey}. Delete existing allocation?", MessageBoxImage.Warning);

                    if (result != MessageBoxResult.Yes) return;
                }
            }

            DialogResult = true;
        }

        #endregion Private Methods
    }
}