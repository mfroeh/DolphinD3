using Dolphin.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolphin.DevUi
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly ISaveImageService imageService;
        private readonly ICaptureWindowService captureWindowService;
        public MainViewModel(ICaptureWindowService captureWindowService, ISaveImageService imageService)
        {
            this.imageService = imageService;
            this.captureWindowService = captureWindowService;
        }

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
