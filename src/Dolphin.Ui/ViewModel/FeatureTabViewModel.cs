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
        #region Private Fields

        private readonly IDialogService dialogService;
        private readonly IMessageBoxService messageBoxService;
        private readonly ISettingsService settingsService;
        private readonly IUnityContainer unityContainer;
        private ICommand addSkillCastProfile;
        private ICommand changeSelectedSkillCastProfile;
        private ICommand deleteSelectedSkillCastProfile;
        private bool isOpenRift;
        private SkillCastConfiguration selectedSkillCastConfiguration;
        private bool skillCastingEnabled;
        private ICommand skillCheckboxClicked;
        private ICommand smartActionCheckboxClicked;
        private bool smartActionsEnabled;

        #endregion Private Fields

        #region Public Constructors

        public FeatureTabViewModel(ISettingsService settingsService, IUnityContainer unityContainer, IDialogService dialogService, IMessageBoxService messageBoxService)
        {
            this.settingsService = settingsService;
            this.dialogService = dialogService;
            this.unityContainer = unityContainer;
            this.messageBoxService = messageBoxService;

            EnabledSkills = new ObservableDictionary<SkillName, bool>();
            foreach (var playerClass in System.Enum.GetValues(typeof(PlayerClass)).Cast<PlayerClass>())
            {
                foreach (var skill in playerClass.PossibleSkills())
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

        #endregion Public Constructors

        #region Public Properties

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
                    SkillCastProfiles.Add(profile);
                    settingsService.SkillCastSettings.SkillCastConfigurations.Add(profile);

                    SelectedSkillCastProfile = profile;
                    ChangeSkillCastProfileDialog(profile);
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
                        ChangeSkillCastProfileDialog(SelectedSkillCastProfile);
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

        #endregion Public Properties

        #region Private Methods

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

        private void ChangeSkillCastProfileDialog(SkillCastConfiguration skillCastProfile)
        {
            var dialogViewModel = unityContainer.Resolve<IDialogViewModel>("skillCast");
            dialogViewModel.Initialize(skillCastProfile);

            bool? success = dialogService.ShowDialog(this, dialogViewModel);
            if (success == true)
            {
                var index = SkillCastProfiles.IndexOf(skillCastProfile);
                SkillCastProfiles.Remove(skillCastProfile);
                SkillCastProfiles.Insert(index, skillCastProfile);
            }
        }

        #endregion Private Methods
    }
}