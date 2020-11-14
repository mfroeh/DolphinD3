using MvvmDialogs;

namespace Dolphin.Ui.Dialog
{
    public interface IDialogViewModel : IModalDialogViewModel
    {
        void Initialize(params object[] @params);
    }
}