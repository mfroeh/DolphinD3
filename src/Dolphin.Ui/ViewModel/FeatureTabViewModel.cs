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

            EnabledSmartActions = new ObservableDictionary<ActionName, bool>();
            foreach (var actionName in System.Enum.GetValues(typeof(ActionName)).Cast<ActionName>())
            {
                if (actionName.IsSmartAction())
                {
                    EnabledSmartActions[actionName] = settingsService.Settings.EnabledSmartActions.Contains(actionName);
                }
            }

            smartActionsEnabled = settingsService.Settings.SmartActionsEnabled;
            skillCastingEnabled = settingsService.Settings.SkillCastingEnabled;
            isOpenRift = settingsService.SmartActionSettings.IsOpenRift;
        }

        public ObservableDictionary<SkillName, bool> EnabledSkills { get; }

        public ObservableDictionary<ActionName, bool> EnabledSmartActions { get; set; }

        public bool IsOpenRift
        {
            get => isOpenRift;
            set
            {
                isOpenRift = value;
                settingsService.SmartActionSettings.IsOpenRift = value;
            }
        }

        public bool SkillCastingEnabled
        {
            get => skillCastingEnabled;
            set
            {
                skillCastingEnabled = value;
                settingsService.Settings.SkillCastingEnabled = value;
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
                smartActionCheckboxClicked = smartActionCheckboxClicked ?? new RelayCommand((o) => ChangeSmartActionEnabled((ActionName)o));
                return smartActionCheckboxClicked;
            }
        }

        public bool SmartActionsEnabled
        {
            get => smartActionsEnabled;
            set
            {
                smartActionsEnabled = value;
                settingsService.Settings.SmartActionsEnabled = value;
                RaisePropertyChanged(nameof(SmartActionsEnabled));
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

        private void ChangeSmartActionEnabled(ActionName smartAction)
        {
            EnabledSmartActions[smartAction] = !EnabledSmartActions[smartAction];

            if (settingsService.Settings.EnabledSmartActions.Contains(smartAction))
            {
                settingsService.Settings.EnabledSmartActions.Remove(smartAction);
            }
            else
            {
                settingsService.Settings.EnabledSmartActions.Add(smartAction);
            }
        }
    }
}