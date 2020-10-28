using System;
using System.CodeDom;
using System.Windows.Input;

namespace Dolphin.DevUi
{
    internal class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly Action<object> a;

        public RelayCommand(Action<object> a)
        {
            this.a = a;
        }

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