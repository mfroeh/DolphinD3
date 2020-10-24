using Dolphin.Enum;
using MvvmDialogs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace Dolphin.Ui.ViewModel
{
    public class SettingsTabViewModel : ViewModelBase
    {
        private readonly ISettingsService settingsService;
        private readonly IDialogService dialogService;
        private IDictionary<Command, Keys> otherKeybindings;
        private ICollection<Keys> skillKeybindings;

        public SettingsTabViewModel(ISettingsService settingsService, IDialogService dialogService)
        {
            this.settingsService = settingsService;
            this.dialogService = dialogService;

            PossibleKeys = System.Enum.GetValues(typeof(Keys)).Cast<Keys>().ToList();
            SkillKeybindings = new ObservableCollection<Keys>(settingsService.Settings.SkillKeybindings);
            OtherKeybindings = new ObservableDictionary<Command, Keys>(settingsService.Settings.OtherKeybindings);
            updateInterval = settingsService.Settings.UpdateInterval;
        }

        public IDictionary<Command, Keys> OtherKeybindings
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

        public ICollection<Keys> PossibleKeys { get; }

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

        private uint updateInterval;

        public string UpdateInterval
        {
            get => updateInterval.ToString();
            set
            {
                var intValue = uint.Parse(value);
                updateInterval = intValue;
                settingsService.Settings.UpdateInterval = intValue;
                RaisePropertyChanged(nameof(UpdateInterval));
                ShowRestartDialog();
            }
        }

        public ICommand ResetSettingsCommand
        {
            get
            {
                return new RelayCommand(ResetSettings);
            }
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

        protected void ShowRestartDialog()
        {
            var result = dialogService.ShowMessageBox(this, "A restart is required in order for these changes to take effect. Restart now?", "Restart required", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                var json = JsonConvert.SerializeObject(settingsService.Settings);
                File.WriteAllText("settings.json", json);

                System.Diagnostics.Process.Start(System.Windows.Application.ResourceAssembly.Location);
                System.Windows.Application.Current.Shutdown(2);
            }
        }
    }
}