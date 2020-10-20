using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Policy;
using System.Windows.Input;
using Unity;

namespace Dolphin.Ui
{
    public class MainViewModel : ViewModelBase
    {
        private string status = "Ready";

        public MainViewModel(IUnityContainer container)
        {
            var tab0 = container.Resolve<IViewModelBase>("hotkeyTab");
            tab0.Parent = this;
            Children.Add(tab0);
        }
        public ICollection<IViewModelBase> Children { get; } = new ObservableCollection<IViewModelBase>();

        public string Status { get { return status; } set { status = value; RaisePropertyChanged("Status"); } }
    }
}
