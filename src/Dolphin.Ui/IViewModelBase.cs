using System.ComponentModel;

namespace Dolphin.Ui
{
    public interface IViewModelBase : INotifyPropertyChanged
    {
        void RaisePropertyChanged(string propertyName);
        
        IViewModelBase Parent { get; set; }
    }
}