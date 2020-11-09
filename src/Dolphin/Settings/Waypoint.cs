using System.ComponentModel;

namespace Dolphin
{
    public class Waypoint : INotifyPropertyChanged
    {
        private bool enabled = true;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Act { get; set; }

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

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}