using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolphin.Ui
{
    public class LogTabViewModel : ViewModelBase
    {
        // private readonly ILogService logService;

        private readonly object logLock = new object();

        public LogTabViewModel()// ISettingsService settingsService, ILogService logService) : base(settingsService)
        {
            this.logService = logService;

            logService.EntryAdded += OnEntryAdded;
            Title = "Log";

            BindingOperations.EnableCollectionSynchronization(LogMessages, logLock);
        }

        public LogLevel DisplayLogLevel
        {
            get
            {
                return settingsService.UISettings.DisplayLogLevel;
            }
            set
            {
                settingsService.UISettings.DisplayLogLevel = value;
                LogMessages.Clear(); // TODO: remove later
                RaisePropertyChanged("DisplayLogLevel");
            }
        }

        public IEnumerable<LogLevel> PossibleLogLevel
        {
            get
            {
                return System.Enum.GetValues(typeof(LogLevel)).Cast<LogLevel>();
            }
        }

        public ICollection<LogEntry> LogMessages { get; } = new ObservableCollection<LogEntry>();

        private void OnEntryAdded(object sender, LogEntryEventArgs e)
        {
            if (e.LogLevel.CompareTo(DisplayLogLevel) <= 0)
            {
                LogMessages.Add(new LogEntry { Message = e.Message, LogLevel = e.LogLevel, Time = e.Time.ToString("HH:mm:ss") }); // TODO: Prepend
            }
        }
    }

    public class LogEntry
    {
        public string Message { get; set; }
        public LogLevel LogLevel { get; set; }
        public string Time { get; set; }
    }
}
}
