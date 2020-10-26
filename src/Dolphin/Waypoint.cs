using System.ComponentModel;

namespace Dolphin
{
    public class Waypoint : INotifyPropertyChanged
    {
        public int Act { get; set; }

        private bool enabled;

        public bool Enabled
        {
            get => enabled;
            set
            {
                enabled = value;
                RaisePropertyChanged(nameof(Enabled));
            }
        }

        public string Name { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}