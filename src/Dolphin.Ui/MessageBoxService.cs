using Dolphin.Ui.Dialog;
using MvvmDialogs;
using System;
using System.ComponentModel;
using System.Windows;
using Unity;

namespace Dolphin.Ui
{
    public class MessageBoxService : IMessageBoxService
    {
        private readonly IUnityContainer unityContainer;
        private readonly IDialogService dialogService;

        public MessageBoxService(IUnityContainer unityContainer, IDialogService dialogService)
        {
            this.unityContainer = unityContainer;
            this.dialogService = dialogService;
        }

        public bool? ShowCustomDialog(INotifyPropertyChanged parentViewModel, string name, params object[] viewModelParams)
        {
            var viewModel = unityContainer.Resolve<IDialogViewModel>(name);
            viewModel.Initialize(viewModelParams);

            return dialogService.ShowDialog(parentViewModel, viewModel);
        }

        public Tuple<bool?, T> ShowCustomDialog<T>(INotifyPropertyChanged parentViewModel, string name, params object[] viewModelParams) where T : IDialogViewModel
        {
            var viewModel = unityContainer.Resolve<IDialogViewModel>(name);
            viewModel.Initialize(viewModelParams);

            var result = dialogService.ShowDialog(parentViewModel, viewModel);

            return Tuple.Create(result, (T)viewModel);
        }

        public MessageBoxResult ShowOK(INotifyPropertyChanged parentViewmodel, string title, string message, MessageBoxImage icon = MessageBoxImage.Information)
        {
            return dialogService.ShowMessageBox(parentViewmodel, message, title, MessageBoxButton.OK, icon);
        }

        public void ShowOK(INotifyPropertyChanged parentViewModel, string title, string message, Action<MessageBoxResult> afterDialog, MessageBoxImage icon = MessageBoxImage.Information)
        {
            var result = ShowOK(parentViewModel, title, message, icon);

            afterDialog.Invoke(result);
        }

        public MessageBoxResult ShowOKCancel(INotifyPropertyChanged parentViewmodel, string title, string message, MessageBoxImage icon = MessageBoxImage.None)
        {
            return dialogService.ShowMessageBox(parentViewmodel, message, title, MessageBoxButton.OKCancel, icon);
        }

        public void ShowOKCancel(INotifyPropertyChanged parentViewModel, string title, string message, Action<MessageBoxResult> afterDialog, MessageBoxImage icon = MessageBoxImage.None)
        {
            var result = ShowOKCancel(parentViewModel, title, message, icon);

            afterDialog.Invoke(result);
        }

        public MessageBoxResult ShowYesNo(INotifyPropertyChanged parentViewmodel, string title, string message, MessageBoxImage icon = MessageBoxImage.None)
        {
            return dialogService.ShowMessageBox(parentViewmodel, message, title, MessageBoxButton.YesNo, icon);
        }

        public void ShowYesNo(INotifyPropertyChanged parentViewModel, string title, string message, Action<MessageBoxResult> afterDialog, MessageBoxImage icon = MessageBoxImage.None)
        {
            var result = ShowYesNo(parentViewModel, title, message, icon);

            afterDialog.Invoke(result);
        }

        public MessageBoxResult ShowYesNoCancel(INotifyPropertyChanged parentViewmodel, string title, string message, MessageBoxImage icon = MessageBoxImage.Question)
        {
            return dialogService.ShowMessageBox(parentViewmodel, message, title, MessageBoxButton.YesNoCancel, icon);
        }

        public void ShowYesNoCancel(INotifyPropertyChanged parentViewModel, string title, string message, Action<MessageBoxResult> afterDialog, MessageBoxImage icon = MessageBoxImage.Question)
        {
            var result = ShowYesNoCancel(parentViewModel, title, message, icon);

            afterDialog.Invoke(result);
        }
    }
}