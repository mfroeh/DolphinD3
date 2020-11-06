using Dolphin.Enum;
using MvvmDialogs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace Dolphin.Ui.ViewModel
{
    public class SettingsTabViewModel : ViewModelBase
    {
        private readonly IDialogService dialogService;
        private readonly ISettingsService settingsService;
        private IDictionary<CommandKeybinding, Keys> otherKeybindings;
        private ICollection<Keys> skillKeybindings;

        private uint updateInterval;

        public SettingsTabViewModel(ISettingsService settingsService, IDialogService dialogService)
        {
            this.settingsService = settingsService;
            this.dialogService = dialogService;

            PossibleKeys = System.Enum.GetValues(typeof(Keys)).Cast<Keys>().ToList();
            SkillKeybindings = new ObservableCollection<Keys>(settingsService.Settings.SkillKeybindings);
            OtherKeybindings = new ObservableDictionary<CommandKeybinding, Keys>(settingsService.Settings.OtherKeybindings);
            updateInterval = settingsService.Settings.UpdateInterval;
            PoolSpots = new BindingList<Waypoint>(settingsService.MacroSettings.Poolspots);

            PoolSpots.ListChanged += PoolSpotsChangedHandler;
        }

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
                return new RelayCommand(ResetSettings);
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
                settingsService.Settings.UpdateInterval = intValue;
                RaisePropertyChanged(nameof(UpdateInterval));
            }
        }

        protected void ShowRestartDialog()
        {
            dialogService.ShowMessageBox(this, "A restart is required in order for these changes to take effect. Restarting now.", "Restart required", MessageBoxButton.OK);
            var json = JsonConvert.SerializeObject(settingsService.Settings);
            File.WriteAllText("settings.json", json);

            System.Diagnostics.Process.Start(System.Windows.Application.ResourceAssembly.Location);
            System.Windows.Application.Current.Shutdown(2);
        }

        private void PoolSpotsChangedHandler(object o, ListChangedEventArgs e)
        {
            settingsService.MacroSettings.Poolspots = PoolSpots.ToList();
        }

        private void ResetSettings(object o)
        {
            var result = dialogService.ShowMessageBox(this, "Are you sure you want to reset all existing settings?",
                            "Reset settings",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                settingsService.ResetSettings();

                ShowRestartDialog();
            }
        }
    }
}