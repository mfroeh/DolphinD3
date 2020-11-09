using Dolphin.Enum;
using Dolphin.Ui.Dialog;
using MvvmDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Unity;

namespace Dolphin.Ui.ViewModel
{
    public class FeatureTabViewModel : ViewModelBase
    {
        private readonly IDialogService dialogService;
        private readonly ISettingsService settingsService;
        private readonly IUnityContainer unityContainer;
        private ICommand addSkillCastProfile;
        private ICommand changeSelectedSkillCastProfile;
        private ICommand deleteSelectedSkillCastProfile;
        private bool isOpenRift;
        private bool skillCastingEnabled;
        private ICommand skillCheckboxClicked;
        private ICommand smartActionCheckboxClicked;
        private bool smartActionsEnabled;

        public FeatureTabViewModel(ISettingsService settingsService, IUnityContainer unityContainer, IDialogService dialogService)
        {
            this.settingsService = settingsService;
            this.dialogService = dialogService;
            this.unityContainer = unityContainer;

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

            SkillCastProfiles = new ObservableCollection<SkillCastConfiguration>(settingsService.SkillCastSettings.SkillCastConfigurations);
            SelectedSkillCastProfile = SkillCastProfiles.FirstOrDefault();
        }

        public ICommand AddSkillCastProfile
        {
            get
            {

                addSkillCastProfile = addSkillCastProfile ?? new RelayCommand((o) =>
                {
                    var profile = new SkillCastConfiguration
                    {
                        Name = Guid.NewGuid().ToString(),
                        SkillIndices = new List<int>(),
                        Delays = new Dictionary<int, int>()
                    };

                    SkillCastProfileDialog(profile, true);
                });

                return addSkillCastProfile;
            }
        }

        public ICommand ChangeSelectedSkillCastProfile
        {
            get
            {
                changeSelectedSkillCastProfile = changeSelectedSkillCastProfile ?? new RelayCommand((o) =>
                {

                    if (SelectedSkillCastProfile != default)
                    {
                        SkillCastProfileDialog(SelectedSkillCastProfile, false);
                    }
                });

                return changeSelectedSkillCastProfile;
            }
        }

        public ICommand DeleteSelectedSkillCastProfile
        {
            get
            {
                deleteSelectedSkillCastProfile = deleteSelectedSkillCastProfile ?? new RelayCommand((o) =>
                {
                    settingsService.SkillCastSettings.SkillCastConfigurations.Remove(SelectedSkillCastProfile);
                    SkillCastProfiles.Remove(SelectedSkillCastProfile);

                    SelectedSkillCastProfile = SkillCastProfiles.FirstOrDefault();
                });

                return deleteSelectedSkillCastProfile;
            }
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

        private SkillCastConfiguration selectedSkillCastConfiguration;

        public SkillCastConfiguration SelectedSkillCastProfile
        {
            get => selectedSkillCastConfiguration;
            set
            {
                selectedSkillCastConfiguration = value;
                RaisePropertyChanged(nameof(SelectedSkillCastProfile));
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

        public ObservableCollection<SkillCastConfiguration> SkillCastProfiles { get; set; }

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

        private void SkillCastProfileDialog(SkillCastConfiguration skillCastProfile, bool isNew)
        {
            var dialogViewModel = unityContainer.Resolve<ChangeSkillCastProfileDialogViewModel>("changeSkillCastProfile");
            dialogViewModel.Initialize(skillCastProfile);

            bool? success = dialogService.ShowDialog(this, dialogViewModel);
            if (success == true)
            {
                if (isNew)
                {
                    SkillCastProfiles.Add(skillCastProfile);
                    settingsService.SkillCastSettings.SkillCastConfigurations.Add(skillCastProfile);
                }
                else
                {
                    var index = SkillCastProfiles.IndexOf(skillCastProfile);
                    SkillCastProfiles.Remove(skillCastProfile);
                    SkillCastProfiles.Insert(index, skillCastProfile);
                }
            }
        }
    }
}