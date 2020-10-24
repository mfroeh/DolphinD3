using MvvmDialogs.FrameworkDialogs;
using MvvmDialogs.FrameworkDialogs.MessageBox;

namespace Dolphin.Ui
{
    public class CustomFrameworkDialogFactory : DefaultFrameworkDialogFactory
    {
        public override IMessageBox CreateMessageBox(MessageBoxSettings settings)
        {
            return new CustomMessageBox(settings);
        }
    }
}