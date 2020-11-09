using System.ComponentModel;

namespace Dolphin.Ui
{
    public interface IViewModel : INotifyPropertyChanged
    {
        IViewModel Parent { get; set; }

        void RaisePropertyChanged(string propertyName);
    }
}