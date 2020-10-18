using System.ComponentModel;

namespace Dolphin.UI
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected readonly ISettingsService settingsService;
        public ViewModelBase(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
