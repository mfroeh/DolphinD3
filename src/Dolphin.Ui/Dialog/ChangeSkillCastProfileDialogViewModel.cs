using MvvmDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Dolphin.Ui.Dialog
{
    public class ChangeSkillCastProfileDialogViewModel : ViewModelBase, IModalDialogViewModel
    {
        private readonly IDialogService dialogService;
        private readonly ISettingsService settingsService;

        private bool? dialogResult;

        public ChangeSkillCastProfileDialogViewModel(ISettingsService settingsService, IDialogService dialogService)
        {
            this.settingsService = settingsService;
            this.dialogService = dialogService;
        }

        public ICommand CancelCommand => new RelayCommand((_) => DialogResult = false);

        public SkillCastConfiguration CurrentConfiguration { get; set; }

        public void Initialize(SkillCastConfiguration currentConfiguration)
        {
            this.CurrentConfiguration = currentConfiguration;

            Name = currentConfiguration.Name;

            Delays = new ObservableCollection<int> { 0, 0, 0, 0, 0, 0 };
            foreach (var item in currentConfiguration.Delays)
            {
                Delays[item.Key] = item.Value;
            }
        }

        public bool? DialogResult
        {
            get
            {
                return dialogResult;
            }
            set
            {
                dialogResult = value;
                RaisePropertyChanged("DialogResult");
            }
        }

        public string Name { get; set; }

        public ObservableCollection<int> Delays { get; set; }

        public ICommand SaveCommand => new RelayCommand(SaveCommandAction);

        private void SaveCommandAction(object o)
        {
            if (settingsService.SkillCastSettings.SkillCastConfigurations.Any(x => x.Name == Name && x != CurrentConfiguration))
            {
                ShowWarningMessageBox($"There already is a profile named {Name}. Pick a different name.");

                return;
            }

            CurrentConfiguration.Name = Name;
            CurrentConfiguration.Delays = new Dictionary<int, int>();
            CurrentConfiguration.SkillIndices = new List<int>();

            var arr = Delays.ToArray();
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] > 0)
                {
                    CurrentConfiguration.Delays[i] = arr[i];
                    CurrentConfiguration.SkillIndices.Add(i);
                }
            }

            DialogResult = true;
        }

        private bool ShowWarningMessageBox(string text)
        {
            var result = dialogService.ShowMessageBox(
                            this,
                            text,
                            "Profile with name exists",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);

            return result == MessageBoxResult.OK;
        }
    }
}
