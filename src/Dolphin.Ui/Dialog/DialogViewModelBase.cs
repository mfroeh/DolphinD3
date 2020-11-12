using System.Windows.Input;

namespace Dolphin.Ui.Dialog
{
    public abstract class DialogViewModelBase : ViewModelBase, IDialogViewModel
    {
        private ICommand dialogCancelCommand;
        private ICommand dialogOkCommand;
        private bool? dialogResult;

        public ICommand DialogCancelCommand
        {
            get
            {
                dialogCancelCommand = dialogCancelCommand ?? new RelayCommand(DialogCancelClicked);

                return dialogCancelCommand;
            }
        }

        public ICommand DialogOkCommand
        {
            get
            {
                dialogOkCommand = dialogOkCommand ?? new RelayCommand(DialogOkClicked);

                return dialogOkCommand;
            }
        }

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

        public abstract void Initialize(params object[] @params);

        protected virtual void DialogCancelClicked(object o)
        {
            DialogResult = false;
        }

        protected virtual void DialogOkClicked(object o)
        {
            DialogResult = true;
        }
    }
}