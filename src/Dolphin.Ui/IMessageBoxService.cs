using System;
using System.ComponentModel;
using System.Windows;

namespace Dolphin.Ui
{
    public interface IMessageBoxService
    {
        void ShowOK(INotifyPropertyChanged parentViewModel, string title, string message, Action<MessageBoxResult> afterDialog, MessageBoxImage icon = MessageBoxImage.Information);

        MessageBoxResult ShowOK(INotifyPropertyChanged parentViewModel, string title, string message, MessageBoxImage icon = MessageBoxImage.Information);

        MessageBoxResult ShowOKCancel(INotifyPropertyChanged parentViewModel, string title, string message, MessageBoxImage icon = MessageBoxImage.None);

        void ShowOKCancel(INotifyPropertyChanged parentViewModel, string title, string message, Action<MessageBoxResult> afterDialog, MessageBoxImage icon = MessageBoxImage.None);

        MessageBoxResult ShowYesNo(INotifyPropertyChanged parentViewModel, string title, string message, MessageBoxImage icon = MessageBoxImage.None);

        void ShowYesNo(INotifyPropertyChanged parentViewModel, string title, string message, Action<MessageBoxResult> afterDialog, MessageBoxImage icon = MessageBoxImage.None);

        MessageBoxResult ShowYesNoCancel(INotifyPropertyChanged parentViewModel, string title, string message, MessageBoxImage icon = MessageBoxImage.Question);

        void ShowYesNoCancel(INotifyPropertyChanged parentViewModel, string title, string message, Action<MessageBoxResult> afterDialog, MessageBoxImage icon = MessageBoxImage.Question);
    }
}