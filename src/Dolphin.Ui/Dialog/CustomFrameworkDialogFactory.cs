using MvvmDialogs.FrameworkDialogs;
using MvvmDialogs.FrameworkDialogs.MessageBox;

namespace Dolphin.Ui.Dialog
{
    public class CustomFrameworkDialogFactory : DefaultFrameworkDialogFactory
    {
        public override IMessageBox CreateMessageBox(MessageBoxSettings settings)
        {
            return new AdonisUiMessageBox(settings);
        }
    }
}