using Dolphin.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Dolphin.Ui.ViewModel
{
    public class FeatureTabViewModel : ViewModelBase
    {
        private readonly ISettingsService settingsService;

        private ICommand skillCheckboxClicked;

        public FeatureTabViewModel(ISettingsService settingsService)
        {
            this.settingsService = settingsService;

            EnabledSkills = new ObservableDictionary<SkillName, bool>();

            foreach (var skillName in System.Enum.GetValues(typeof(SkillName)).Cast<SkillName>())
            {
                if (skillName != SkillName.None)
                {
                    EnabledSkills.Add(skillName, settingsService.SkillIsEnabled(skillName));
                }
            }
        }

        public ObservableDictionary<SkillName, bool> EnabledSkills { get; }

        public ICommand SkillCheckboxClicked
        {
            get
            {
                skillCheckboxClicked = skillCheckboxClicked ?? new RelayCommand((o) => ChangeSkillEnabled((SkillName)o));
                return skillCheckboxClicked;
            }
        }

        private void ChangeSkillEnabled(SkillName name)
        {
            EnabledSkills[name] = !EnabledSkills[name];

            if (settingsService.Settings.EnabledSkills.Contains(name))
            {
                settingsService.Settings.EnabledSkills.Remove(name);
            }
            else
            {
                settingsService.Settings.EnabledSkills.Add(name);
            }
        }
    }
}