using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Dolphin.Ui.Dialog
{
    public class ChangeSkillCastProfileDialogViewModel : DialogViewModelBase, IDialogViewModel
    {
        private readonly IMessageBoxService messageBoxService;
        private readonly ISettingsService settingsService;

        public ChangeSkillCastProfileDialogViewModel(ISettingsService settingsService, IMessageBoxService messageBoxService)
        {
            this.settingsService = settingsService;
            this.messageBoxService = messageBoxService;
        }

        public SkillCastConfiguration CurrentConfiguration { get; set; }

        public ObservableCollection<int> Delays { get; set; }

        public string Name { get; set; }

        public override void Initialize(params object[] @params)
        {
            var configuration = (SkillCastConfiguration)@params[0];

            CurrentConfiguration = configuration;

            Name = configuration.Name;
            Delays = new ObservableCollection<int> { 0, 0, 0, 0, 0, 0 };
            foreach (var item in configuration.Delays)
            {
                Delays[item.Key] = item.Value;
            }
        }

        protected override void DialogOkClicked(object o)
        {
            if (settingsService.SkillCastSettings.SkillCastConfigurations.Any(x => x.Name == Name && x != CurrentConfiguration))
            {
                messageBoxService.ShowOK(this, "Profile with name exists", $"There already is a profile named {Name}. Pick a different name.");

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
    }
}