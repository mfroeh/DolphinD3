using Dolphin.Enum;
using Dolphin.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace Dolphin.Ui.ViewModel
{
    public class LogEntry
    {
        public LogLevel LogLevel { get; set; }

        public string Message { get; set; }

        public string Time { get; set; }
    }

    public class LogTabViewModel : ViewModelBase
    {
        private readonly object logLock = new object();
        private readonly ILogService logService;

        private readonly ISettingsService settingsService;

        public LogTabViewModel(ISettingsService settingsService, ILogService logService)
        {
            this.logService = logService;
            this.settingsService = settingsService;
            logService.EntryAdded += OnEntryAdded;

            BindingOperations.EnableCollectionSynchronization(LogMessages, logLock);
        }

        public LogLevel DisplayLogLevel
        {
            get => settingsService.Settings.UiSettings.DisplayLogLevel;
            set
            {
                settingsService.Settings.UiSettings.DisplayLogLevel = value;
                LogMessages.Clear(); // TODO: remove later
                RaisePropertyChanged(nameof(DisplayLogLevel));
            }
        }

        public ICollection<LogEntry> LogMessages { get; } = new ObservableCollection<LogEntry>();

        public IEnumerable<LogLevel> PossibleLogLevel
        {
            get
            {
                return System.Enum.GetValues(typeof(LogLevel)).Cast<LogLevel>();
            }
        }

        public ICommand AddEntryCommand
        {
            get
            {
                return new RelayCommand((_) => logService.AddEntry(this, "Test message", LogLevel.Error));
            }
        }

        private void OnEntryAdded(object sender, LogEntryEventArgs e)
        {
            if (e.LogLevel.CompareTo(DisplayLogLevel) <= 0)
            {
                LogMessages.Add(new LogEntry { Message = e.Message, LogLevel = e.LogLevel, Time = e.Time.ToString("HH:mm:ss") }); // TODO: Prepend
            }
        }
    }
}