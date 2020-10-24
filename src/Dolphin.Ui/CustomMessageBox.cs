using MvvmDialogs.FrameworkDialogs.MessageBox;
using System;
using System.Windows;

namespace Dolphin.Ui
{
    internal class CustomMessageBox : IMessageBox
    {
        private readonly AdonisUI.Controls.MessageBoxModel messageBoxModel;
        private readonly MessageBoxSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomMessageBox"/> class.
        /// </summary>
        /// <param name="settings">The settings for the folder browser dialog.</param>
        public CustomMessageBox(MessageBoxSettings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));

            messageBoxModel = new AdonisUI.Controls.MessageBoxModel();

            SetUpTitle();
            SetUpText();
            SetUpButtons();
            SetUpIcon();
        }

        /// <summary>
        /// Opens a message box with specified owner.
        /// </summary>
        /// <param name="owner">
        /// Handle to the window that owns the dialog.
        /// </param>
        /// <returns>
        /// A <see cref="MessageBoxResult"/> value that specifies which message box button is
        /// clicked by the user.
        /// </returns>
        public System.Windows.MessageBoxResult Show(Window owner)
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
                    messageBoxModel.Buttons = new[] { AdonisUI.Controls.MessageBoxButtons.Yes() };
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

                case MessageBoxImage.Information:
                    messageBoxModel.Icon = AdonisUI.Controls.MessageBoxImage.Information;
                    break;

                case MessageBoxImage.Warning:
                    messageBoxModel.Icon = AdonisUI.Controls.MessageBoxImage.Warning;
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