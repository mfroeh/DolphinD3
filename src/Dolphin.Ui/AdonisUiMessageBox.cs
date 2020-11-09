using MvvmDialogs.FrameworkDialogs.MessageBox;
using System;
using System.Windows;

namespace Dolphin.Ui
{
    internal class AdonisUiMessageBox : IMessageBox
    {
        private readonly AdonisUI.Controls.MessageBoxModel messageBoxModel;
        private readonly MessageBoxSettings settings;

        public AdonisUiMessageBox(MessageBoxSettings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));

            messageBoxModel = new AdonisUI.Controls.MessageBoxModel();

            SetUpTitle();
            SetUpText();
            SetUpButtons();
            SetUpIcon();
        }

        public MessageBoxResult Show(Window owner)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));

            var result = AdonisUI.Controls.MessageBox.Show(owner, messageBoxModel);

            return ToMessageBoxResult(result);
        }

        private static MessageBoxResult ToMessageBoxResult(AdonisUI.Controls.MessageBoxResult result)
        {
            switch (result)
            {
                case AdonisUI.Controls.MessageBoxResult.Cancel:
                    return MessageBoxResult.Cancel;

                case AdonisUI.Controls.MessageBoxResult.No:
                    return MessageBoxResult.No;

                case AdonisUI.Controls.MessageBoxResult.OK:
                    return MessageBoxResult.OK;

                case AdonisUI.Controls.MessageBoxResult.Yes:
                    return MessageBoxResult.Yes;

                default:
                    return MessageBoxResult.None;
            }
        }

        private void SetUpButtons()
        {
            switch (settings.Button)
            {
                case MessageBoxButton.OKCancel:
                    messageBoxModel.Buttons = AdonisUI.Controls.MessageBoxButtons.OkCancel();
                    break;

                case MessageBoxButton.YesNo:
                    messageBoxModel.Buttons = AdonisUI.Controls.MessageBoxButtons.YesNo();
                    break;

                case MessageBoxButton.YesNoCancel:
                    messageBoxModel.Buttons = AdonisUI.Controls.MessageBoxButtons.YesNoCancel();
                    break;

                default:
                    messageBoxModel.Buttons = new[] { AdonisUI.Controls.MessageBoxButtons.Ok() };
                    break;
            }
        }

        private void SetUpIcon()
        {
            switch (settings.Icon)
            {
                case MessageBoxImage.Error:
                    messageBoxModel.Icon = AdonisUI.Controls.MessageBoxImage.Error;
                    break;

                case MessageBoxImage.Question:
                    messageBoxModel.Icon = AdonisUI.Controls.MessageBoxImage.Question;
                    break;

                case MessageBoxImage.Warning:
                    messageBoxModel.Icon = AdonisUI.Controls.MessageBoxImage.Warning;
                    break;

                case MessageBoxImage.Information:
                    messageBoxModel.Icon = AdonisUI.Controls.MessageBoxImage.Information;
                    break;

                default:
                    messageBoxModel.Icon = AdonisUI.Controls.MessageBoxImage.None;
                    break;
            }
        }

        private void SetUpText()
        {
            messageBoxModel.Text = settings.MessageBoxText ?? "";
        }

        private void SetUpTitle()
        {
            messageBoxModel.Caption = string.IsNullOrEmpty(settings.Caption) ?
                " " :
                settings.Caption;
        }
    }
}