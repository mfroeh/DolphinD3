using Dolphin.Enum;
using System.Linq;
using System.Windows.Input;

namespace Dolphin.Ui.ViewModel
{
    public class FeatureTabViewModel : ViewModelBase
    {
        private readonly ISettingsService settingsService;

        private bool isOpenRift;
        private bool skillCastingEnabled;
        private ICommand skillCheckboxClicked;
        private ICommand smartActionCheckboxClicked;
        private bool smartActionsEnabled;

        public FeatureTabViewModel(ISettingsService settingsService)
        {
            this.settingsService = settingsService;

            EnabledSkills = new ObservableDictionary<SkillName, bool>();
            foreach (var playerClass in System.Enum.GetValues(typeof(PlayerClass)).Cast<PlayerClass>())
            {
                foreach (var skill in playerClass.PossibleSkills(false))
                {
                    EnabledSkills[skill] = settingsService.SkillIsEnabled(skill);
                }
            }

            EnabledSmartActions = new ObservableDictionary<SmartActionName, bool>();
            foreach (var actionName in System.Enum.GetValues(typeof(SmartActionName)).Cast<SmartActionName>())
            {
                if (actionName != SmartActionName.None)
                {
                    EnabledSmartActions[actionName] = settingsService.IsSmartActionEnabled(actionName);
                }
            }

            smartActionsEnabled = settingsService.SmartFeatureSettings.SmartActionsEnabled;
            skillCastingEnabled = settingsService.SmartFeatureSettings.SkillCastingEnabled;
            isOpenRift = settingsService.SmartFeatureSettings.IsOpenRift;
        }

        public ObservableDictionary<SkillName, bool> EnabledSkills { get; }

        public ObservableDictionary<SmartActionName, bool> EnabledSmartActions { get; set; }

        public bool IsOpenRift
        {
            get => isOpenRift;
            set
            {
                isOpenRift = value;
                settingsService.SmartFeatureSettings.IsOpenRift = value;
            }
        }

        public bool SkillCastingEnabled
        {
            get => skillCastingEnabled;
            set
            {
                skillCastingEnabled = value;
                settingsService.SmartFeatureSettings.SkillCastingEnabled = value;
                RaisePropertyChanged(nameof(SkillCastingEnabled));
            }
        }

        public ICommand SkillCheckboxClicked
        {
            get
            {
                skillCheckboxClicked = skillCheckboxClicked ?? new RelayCommand((o) => ChangeSkillEnabled((SkillName)o));
                return skillCheckboxClicked;
            }
        }

        public ICommand SmartActionCheckboxClicked
        {
            get
            {
                smartActionCheckboxClicked = smartActionCheckboxClicked ?? new RelayCommand((o) => ChangeSmartActionEnabled((SmartActionName)o));
                return smartActionCheckboxClicked;
            }
        }

        public bool SmartActionsEnabled
        {
            get => smartActionsEnabled;
            set
            {
                smartActionsEnabled = value;
                settingsService.SmartFeatureSettings.SmartActionsEnabled = value;
                RaisePropertyChanged(nameof(SmartActionsEnabled));
            }
        }

        private void ChangeSkillEnabled(SkillName name)
        {
            EnabledSkills[name] = !EnabledSkills[name];

            if (settingsService.SmartFeatureSettings.EnabledSkills.Contains(name))
            {
                settingsService.SmartFeatureSettings.EnabledSkills.Remove(name);
            }
            else
            {
                settingsService.SmartFeatureSettings.EnabledSkills.Add(name);
            }
        }

        private void ChangeSmartActionEnabled(SmartActionName smartAction)
        {
            EnabledSmartActions[smartAction] = !EnabledSmartActions[smartAction];

            if (settingsService.IsSmartActionEnabled(smartAction))
            {
                settingsService.SmartFeatureSettings.EnabledSmartActions.Remove(smartAction);
            }
            else
            {
                settingsService.SmartFeatureSettings.EnabledSmartActions.Add(smartAction);
            }
        }
    }
}