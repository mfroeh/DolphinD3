using System.ComponentModel;

namespace Dolphin.Ui
{
    public interface IViewModelBase : INotifyPropertyChanged
    {
        IViewModelBase Parent { get; set; }

        void RaisePropertyChanged(string propertyName);
    }
}