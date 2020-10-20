using Dolphin.Enum;
using Dolphin.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Dolphin.Ui
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
            get
            {
                return settingsService.Settings.UiSettings.DisplayLogLevel;
            }
            set
            {
                settingsService.Settings.UiSettings.DisplayLogLevel = value;
                LogMessages.Clear(); // TODO: remove later
                RaisePropertyChanged("DisplayLogLevel");
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

        private void OnEntryAdded(object sender, LogEntryEventArgs e)
        {
            if (e.LogLevel.CompareTo(DisplayLogLevel) <= 0)
            {
                LogMessages.Add(new LogEntry { Message = e.Message, LogLevel = e.LogLevel, Time = e.Time.ToString("HH:mm:ss") }); // TODO: Prepend
            }
        }
    }
}