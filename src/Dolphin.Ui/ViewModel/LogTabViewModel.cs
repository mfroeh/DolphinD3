using Dolphin.Enum;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Dolphin.Ui.ViewModel
{
    public class LogTabViewModel : ViewModelBase
    {
        #region Private Fields

        private readonly object logLock = new object();
        private readonly ILogService logService;
        private readonly ISettingsService settingsService;

        private ICommand clipLogEntryCommand;
        private LogLevel displayLogLevel;
        private bool logPaused;

        #endregion Private Fields

        #region Public Constructors

        public LogTabViewModel(ISettingsService settingsService, ILogService logService)
        {
            this.logService = logService;
            this.settingsService = settingsService;
            logService.EntryAdded += OnEntryAdded;

            displayLogLevel = settingsService.UiSettings.DisplayLogLevel;
            logPaused = settingsService.UiSettings.LogPaused;

            LogMessages = new ObservableCollection<LogEntry>();

            BindingOperations.EnableCollectionSynchronization(LogMessages, logLock);
        }

        #endregion Public Constructors

        #region Public Properties

        public ICommand ClipLogEntryCommand
        {
            get
            {
                clipLogEntryCommand = clipLogEntryCommand ?? new RelayCommand((o) => Clipboard.SetData(DataFormats.Text, SelectedLogItem.FullMessage));
                return clipLogEntryCommand;
            }
        }

        public LogLevel DisplayLogLevel
        {
            get => displayLogLevel;
            set
            {
                displayLogLevel = value;
                settingsService.Settings.UiSettings.DisplayLogLevel = value;
                RaisePropertyChanged(nameof(DisplayLogLevel));
                LogMessages = new ObservableCollection<LogEntry>(logService.InternalLog.Entries.Where(x => x.LogLevel.CompareTo(value) <= 0));
                RaisePropertyChanged(nameof(LogMessages));
            }
        }

        public ObservableCollection<LogEntry> LogMessages { get; set; }

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

        public IEnumerable<LogLevel> PossibleLogLevel => System.Enum.GetValues(typeof(LogLevel)).Cast<LogLevel>();

        public LogEntry SelectedLogItem { get; set; }

        #endregion Public Properties

        #region Private Methods

        private void OnEntryAdded(object sender, LogEntryEventArgs e)
        {
            if (!LogPaused && e.LogLevel.CompareTo(DisplayLogLevel) <= 0)
            {
                LogMessages.Add(e.LogEntry);
            }
        }

        #endregion Private Methods
    }
}