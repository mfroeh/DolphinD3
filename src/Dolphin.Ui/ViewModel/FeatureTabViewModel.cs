using Dolphin.Enum;
using Dolphin.Ui.Dialog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace Dolphin.Ui.ViewModel
{
    public class FeatureTabViewModel : ViewModelBase
    {
        #region Private Fields

        private readonly IMessageBoxService messageBoxService;
        private readonly ISettingsService settingsService;
        private ICommand addSkillCastProfile_;
        private ICommand changeSelectedSkillCastProfile_;
        private ICommand deleteSelectedSkillCastProfile_;
        private bool empowerGrifts;
        private bool isOpenRift;
        private ItemType selectedItem;
        private SkillCastConfiguration selectedSkillCastConfiguration_;
        private bool skillCastingEnabled;
        private ICommand skillCheckboxClicked;
        private ICommand smartActionCheckboxClicked;
        private bool smartActionsEnabled;
        private uint spareColumns;

        #endregion Private Fields

        #region Public Constructors

        public FeatureTabViewModel(ISettingsService settingsService, IMessageBoxService messageBoxService)
        {
            this.settingsService = settingsService;
            this.messageBoxService = messageBoxService;

            EnabledSkills = new ObservableDictionary<SkillName, bool>();
            foreach (var playerClass in EnumHelper.GetValues<PlayerClass>())
            {
                foreach (var skill in playerClass.PossibleSkills())
                {
                    EnabledSkills[skill] = settingsService.SkillIsEnabled(skill);
                }
            }

            EnabledSmartActions = new ObservableDictionary<SmartActionName, bool>();
            foreach (var actionName in EnumHelper.GetValues<SmartActionName>())
            {
                EnabledSmartActions[actionName] = settingsService.IsSmartActionEnabled(actionName);
            }

            smartActionsEnabled = settingsService.SmartFeatureSettings.SmartActionsEnabled;
            skillCastingEnabled = settingsService.SmartFeatureSettings.SkillCastingEnabled;
            isOpenRift = settingsService.SmartFeatureSettings.IsOpenRift;

            SkillCastProfiles_ = new ObservableCollection<SkillCastConfiguration>(settingsService.SkillCastSettings.SkillCastConfigurations);
            SelectedSkillCastProfile_ = SkillCastProfiles_.FirstOrDefault();
            EmpowerGrifts = settingsService.SmartFeatureSettings.EmpowerGrifts;

            // New
            PoolSpots = new BindingList<Waypoint>(settingsService.MacroSettings.Poolspots);
            PoolSpots.ListChanged += PoolSpotsChangedHandler;

            ItemTypes = EnumHelper.GetValues<ItemType>(false).ToList();
            selectedItem = settingsService.Settings.MacroSettings.SelectedGambleItem;
            spareColumns = settingsService.Settings.MacroSettings.SpareColumns;

            SkillsCheckboxEnabled = false;
            SmartActionCheckboxEnabled = false;
        }

        #endregion Public Constructors

        #region Public Properties

        public bool SkillsCheckboxEnabled { get; set; }

        public bool SmartActionCheckboxEnabled { get; set; }

        public ICommand AddSkillCastProfile_
        {
            get
            {
                addSkillCastProfile_ = addSkillCastProfile_ ?? new RelayCommand((o) =>
                {
                    var profile_ = new SkillCastConfiguration
                    {
                        Name = Guid.NewGuid().ToString(),
                        SkillIndices = new List<int>(),
                        Delays = new Dictionary<int, int>()
                    };
                    SkillCastProfiles_.Add(profile_);
                    settingsService.SkillCastSettings.SkillCastConfigurations.Add(profile_);

                    SelectedSkillCastProfile_ = profile_;
                    ChangeSkillCastProfileDialog_(profile_);
                });

                return addSkillCastProfile_;
            }
        }

        public ICommand ChangeSelectedSkillCastProfile_
        {
            get
            {
                changeSelectedSkillCastProfile_ = changeSelectedSkillCastProfile_ ?? new RelayCommand((o) =>
                {
                    if (SelectedSkillCastProfile_ != default)
                    {
                        ChangeSkillCastProfileDialog_(SelectedSkillCastProfile_);
                    }
                });

                return changeSelectedSkillCastProfile_;
            }
        }

        public ICommand DeleteSelectedSkillCastProfile_
        {
            get
            {
                deleteSelectedSkillCastProfile_ = deleteSelectedSkillCastProfile_ ?? new RelayCommand((o) =>
                {
                    settingsService.SkillCastSettings.SkillCastConfigurations.Remove(SelectedSkillCastProfile_);
                    SkillCastProfiles_.Remove(SelectedSkillCastProfile_);
                    SelectedSkillCastProfile_ = SkillCastProfiles_.FirstOrDefault();
                    RaisePropertyChanged(nameof(SelectedSkillCastProfile));
                    RaisePropertyChanged(nameof(SkillCastProfiles));
                });

                return deleteSelectedSkillCastProfile_;
            }
        }

        public bool EmpowerGrifts
        {
            get => empowerGrifts;
            set
            {
                empowerGrifts = value;
                settingsService.SmartFeatureSettings.EmpowerGrifts = value;
                RaisePropertyChanged(nameof(EmpowerGrifts));
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

        public IList<ItemType> ItemTypes { get; }

        public BindingList<Waypoint> PoolSpots { get; set; }

        public ItemType SelectedGambleItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                settingsService.Settings.MacroSettings.SelectedGambleItem = value;
                RaisePropertyChanged(nameof(SelectedGambleItem));
            }
        }

        public int SelectedSkillCastProfile
        {
            get => settingsService.SkillCastSettings.SelectedIndex;
            set
            {
                settingsService.SkillCastSettings.SelectedIndex = value;
                RaisePropertyChanged(nameof(SelectedSkillCastProfile));
            }
        }

        public SkillCastConfiguration SelectedSkillCastProfile_
        {
            get => selectedSkillCastConfiguration_;
            set
            {
                selectedSkillCastConfiguration_ = value;
                RaisePropertyChanged(nameof(SelectedSkillCastProfile_));
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

        public IList<string> SkillCastProfiles
        {
            get => settingsService.SkillCastSettings.SkillCastConfigurations.Select(x => x.Name).ToList();
        }

        public ObservableCollection<SkillCastConfiguration> SkillCastProfiles_ { get; set; }

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

        public uint SpareColumnIndex
        {
            get => spareColumns;
            set
            {
                spareColumns = value;
                settingsService.Settings.MacroSettings.SpareColumns = spareColumns;
                RaisePropertyChanged(nameof(SpareColumnIndex));
            }
        }

        #endregion Public Properties

        #region Private Methods

        private void ChangeSkillCastProfileDialog_(SkillCastConfiguration skillCastProfile)
        {
            var result = messageBoxService.ShowCustomDialog<ChangeSkillCastProfileDialogViewModel>(this, skillCastProfile);
            if (result.Item1 == true)
            {
                var index = SkillCastProfiles_.IndexOf(skillCastProfile);
                SkillCastProfiles_.Remove(skillCastProfile);
                SkillCastProfiles_.Insert(index, skillCastProfile);

                RaisePropertyChanged(nameof(SelectedSkillCastProfile));
                RaisePropertyChanged(nameof(SkillCastProfiles));
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

        private void PoolSpotsChangedHandler(object o, ListChangedEventArgs e)
        {
            settingsService.MacroSettings.Poolspots = PoolSpots.ToList();
        }

        #endregion Private Methods
    }
}