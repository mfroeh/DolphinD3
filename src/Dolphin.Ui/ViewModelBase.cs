using System.ComponentModel;

namespace Dolphin.Ui
{
    public abstract class ViewModelBase : IViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public IViewModel Parent { get; set; }

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void PropertySetter<T>(T value, string propertyName, bool raisePropertyChanged = true)
        {
            GetType().GetProperty(propertyName).SetValue(this, value);

            if (raisePropertyChanged)
            {
                RaisePropertyChanged(propertyName);
            }
        }
    }
}