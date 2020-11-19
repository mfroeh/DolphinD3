using Dolphin.Ui.Dialog;
using MvvmDialogs.FrameworkDialogs.OpenFile;
using System;
using System.ComponentModel;
using System.Windows;

namespace Dolphin.Ui
{
    public interface IMessageBoxService
    {
        bool? ShowCustomDialog(INotifyPropertyChanged parentViewModel, string name, params object[] viewModelParams);

        Tuple<bool?, T> ShowCustomDialog<T>(INotifyPropertyChanged parentViewModel, params object[] viewModelParams) where T : IDialogViewModel;

        void ShowOK(INotifyPropertyChanged parentViewModel, string title, string message, Action<MessageBoxResult> afterDialog, MessageBoxImage icon = MessageBoxImage.Information);

        MessageBoxResult ShowOK(INotifyPropertyChanged parentViewModel, string title, string message, MessageBoxImage icon = MessageBoxImage.Information);

        MessageBoxResult ShowOKCancel(INotifyPropertyChanged parentViewModel, string title, string message, MessageBoxImage icon = MessageBoxImage.None);

        void ShowOKCancel(INotifyPropertyChanged parentViewModel, string title, string message, Action<MessageBoxResult> afterDialog, MessageBoxImage icon = MessageBoxImage.None);

        string ShowOpenFileDialog(INotifyPropertyChanged parentViewModel, OpenFileDialogSettings settings);

        MessageBoxResult ShowYesNo(INotifyPropertyChanged parentViewModel, string title, string message, MessageBoxImage icon = MessageBoxImage.None);

        void ShowYesNo(INotifyPropertyChanged parentViewModel, string title, string message, Action<MessageBoxResult> afterDialog, MessageBoxImage icon = MessageBoxImage.None);

        MessageBoxResult ShowYesNoCancel(INotifyPropertyChanged parentViewModel, string title, string message, MessageBoxImage icon = MessageBoxImage.Question);

        void ShowYesNoCancel(INotifyPropertyChanged parentViewModel, string title, string message, Action<MessageBoxResult> afterDialog, MessageBoxImage icon = MessageBoxImage.Question);
    }
}