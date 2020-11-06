using System;
using System.Windows.Input;

namespace Dolphin.DevUi
{
    internal class RelayCommand : ICommand
    {
        private readonly Action<object> a;

        public RelayCommand(Action<object> a)
        {
            this.a = a;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            a.Invoke(parameter);
        }
    }
}