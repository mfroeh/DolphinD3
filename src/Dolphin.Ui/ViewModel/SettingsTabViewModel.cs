using Dolphin.Enum;
using Dolphin.Ui.Dialog;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;

namespace Dolphin.Ui.ViewModel
{
    public class SettingsTabViewModel : ViewModelBase
    {
        #region Private Fields

        private readonly IMessageBoxService messageBoxService;
        private readonly ISettingsService settingsService;
        private IDictionary<CommandKeybinding, Keys> otherKeybindings;
        private ICollection<Keys> skillKeybindings;

        private uint updateInterval;

        #endregion Private Fields

        #region Public Constructors

        public SettingsTabViewModel(ISettingsService settingsService, IMessageBoxService messageBoxService)
        {
            this.settingsService = settingsService;
            this.messageBoxService = messageBoxService;

            PossibleKeys = EnumHelper.GetValues<Keys>().ToList();
            SkillKeybindings = new ObservableCollection<Keys>(settingsService.Settings.SkillKeybindings);
            OtherKeybindings = new ObservableDictionary<CommandKeybinding, Keys>(settingsService.Settings.OtherKeybindings);
            updateInterval = settingsService.SmartFeatureSettings.UpdateInterval;
            PoolSpots = new BindingList<Waypoint>(settingsService.MacroSettings.Poolspots);
            ExecuteablePaths = new Dictionary<string, string>(settingsService.UiSettings.ExecuteablePaths);

            PoolSpots.ListChanged += PoolSpotsChangedHandler;
        }

        #endregion Public Constructors

        #region Public Properties

        private ICommand changePathCommand;

        public ICommand ChangePathCommand
        {
            get
            {
                changePathCommand = changePathCommand ?? new RelayCommand((o) => ChangePath((string)o));

                return changePathCommand;
            }
        }

        public IDictionary<string, string> ExecuteablePaths { get; set; }

        public IDictionary<CommandKeybinding, Keys> OtherKeybindings
        {
            get
            {
                settingsService.Settings.OtherKeybindings = otherKeybindings; // This cant be the right way. How does this work?

                return otherKeybindings;
            }
            set
            {
                otherKeybindings = value;
                RaisePropertyChanged(nameof(OtherKeybindings));
            }
        }

        public BindingList<Waypoint> PoolSpots { get; set; }

        public ICollection<Keys> PossibleKeys { get; }

        public ICommand ResetSettingsCommand
        {
            get
            {
                return new RelayCommand(ShowResetDialog);
            }
        }

        public ICollection<Keys> SkillKeybindings
        {
            get
            {
                settingsService.Settings.SkillKeybindings = skillKeybindings.ToList(); // This cant be the right way

                return skillKeybindings;
            }
            set
            {
                skillKeybindings = value;
                RaisePropertyChanged(nameof(SkillKeybindings));
            }
        }

        public string UpdateInterval
        {
            get => updateInterval.ToString();
            set
            {
                var intValue = uint.Parse(value);
                updateInterval = intValue;
                settingsService.SmartFeatureSettings.UpdateInterval = intValue;
                RaisePropertyChanged(nameof(UpdateInterval));
            }
        }

        #endregion Public Properties

        #region Private Methods

        private void ChangePath(string name)
        {
            var initialDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            settingsService.UiSettings.ExecuteablePaths.TryGetValue(name, out var currentPath);
            if (!(string.IsNullOrEmpty(currentPath) || !Directory.Exists(Path.GetDirectoryName(currentPath))))
            {
                initialDirectory = Path.GetDirectoryName(currentPath);
            }

            var settings = new MvvmDialogs.FrameworkDialogs.OpenFile.OpenFileDialogSettings
            {
                Multiselect = false,
                CheckFileExists = true,
                Filter = "Executables (.exe)|*.exe",
                InitialDirectory = initialDirectory
            };

            var newPath = messageBoxService.ShowOpenFileDialog(this, settings);

            if (string.IsNullOrEmpty(newPath)) return;

            settingsService.UiSettings.ExecuteablePaths[name] = newPath;
            ExecuteablePaths[name] = newPath;
            RaisePropertyChanged(nameof(ExecuteablePaths));
        }

        private void PoolSpotsChangedHandler(object o, ListChangedEventArgs e)
        {
            settingsService.MacroSettings.Poolspots = PoolSpots.ToList();
        }

        private void ShowResetDialog(object o)
        {
            //messageBoxService.ShowYesNo(this, "Reset settings", "Are you sure you want to reset the settings?", MessageBoxImage.Warning);

            var result = messageBoxService.ShowCustomDialog<ResetSettingsDialogViewModel>(this);
            if (result.Item1 == true)
            {
                foreach (var s in result.Item2.SettingsToReset)
                {
                    settingsService.ResetSettings(s);
                }

                messageBoxService.ShowOK(this, "Restart required", "A restart is required in order for these changes to take effect. Restarting now.");

                var json = JsonConvert.SerializeObject(settingsService.Settings);
                File.WriteAllText("settings.json", json);

                System.Diagnostics.Process.Start(System.Windows.Application.ResourceAssembly.Location);
                System.Windows.Application.Current.Shutdown(2);
            }
        }

        #endregion Private Methods
    }
}