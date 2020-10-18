using System.ComponentModel;

namespace Dolphin.Ui
{
    public interface IViewModelBase : INotifyPropertyChanged
    {
        void RaisePropertyChanged(string propertyName);
    }
}