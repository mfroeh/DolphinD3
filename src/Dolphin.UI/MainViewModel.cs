using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Unity;

namespace Dolphin.Ui
{
    public class MainViewModel : ViewModelBase
    {
        public ICollection<IViewModelBase> Children { get; } = new ObservableCollection<IViewModelBase>();

        public MainViewModel(IUnityContainer container)
        {
            var tab0 = container.Resolve<IViewModelBase>("hotkeyTab");
            tab0.Parent = this;
            Children.Add(tab0);
        }

        public ICommand Command => new RelayCommand((_) => Trace.WriteLine("Wrong vm"));
    }
}
