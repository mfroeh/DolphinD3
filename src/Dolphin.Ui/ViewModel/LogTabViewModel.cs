using Dolphin.Enum;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            BindingOperations.EnableCollectionSynchronization(LogMessages, logLock);

            this.logService = logService;
            this.settingsService = settingsService;
            logService.EntryAdded += OnEntryAdded;

            displayLogLevel = settingsService.UiSettings.DisplayLogLevel;
            logPaused = settingsService.UiSettings.LogPaused;
        }

        private LogLevel displayLogLevel;
        public LogLevel DisplayLogLevel
        {
            get => displayLogLevel;
            set
            {
                displayLogLevel = value;
                settingsService.Settings.UiSettings.DisplayLogLevel = value;
                RaisePropertyChanged(nameof(DisplayLogLevel));
            }
        }

        public ObservableCollection<LogEntry> LogMessages { get; } = new ObservableCollection<LogEntry>();

        private bool logPaused;
        public bool LogPaused
        {
            get => logPaused;
            set
            {
                logPaused = value;
                settingsService.UiSettings.LogPaused = value;
                RaisePropertyChanged(nameof(LogPaused));
            }
        }

        public IEnumerable<LogLevel> PossibleLogLevel
        {
            get
            {
                return System.Enum.GetValues(typeof(LogLevel)).Cast<LogLevel>();
            }
        }

        private void OnEntryAdded(object sender, LogEntryEventArgs e)
        {
            if (!LogPaused)
            {
                if (e.LogLevel.CompareTo(DisplayLogLevel) <= 0 || e.LogLevel == LogLevel.Error)
                {
                    if (LogMessages.Count > 500)
                    {
                        LogMessages.Clear();
                    }

                    LogMessages.Add(new LogEntry { Message = e.Message, LogLevel = e.LogLevel, Time = e.Time.ToString("HH:mm:ss") }); // TODO: Prepend
                }
            }
        }
    }
}