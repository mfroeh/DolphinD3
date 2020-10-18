using Dolphin.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace Dolphin.UI
{
    public class DataTabViewModel : TabViewmodelBase, IEventSubscriber
    {
        private EventHandler<PlayerInformationEventArgs> PlayerInformationChangedHandler;

        private readonly IEventChannel eventChannel;
        private readonly IModelService modelService;

        private readonly IList<SkillName> currentDisplayedSkills = new SkillName[6];
        private readonly ICommand[] skillClickCommands = new RelayCommand[6];

        public DataTabViewModel(IEventChannel eventChannel, IModelService modelService, ISettingsService settingsService) : base(settingsService)
        {
            this.eventChannel = eventChannel;
            this.modelService = modelService;

            PlayerInformationChangedHandler = OnPlayerInformationChanged;
            eventChannel.Subscribe(PlayerInformationChangedHandler);
            Title = "Current Data";
        }


        // TODO: Use Resource Service for caching these? Most likely dont have to
        public string PlayerClass
        {
            get
            {
                var playerClass = modelService.Player.Class;
                if (playerClass != Enum.PlayerClass.None)
                    return $"pack://application:,,,/Resource/UI/Class/{playerClass}.png";
                return "pack://application:,,,/Resource/UI/Skill/EmptyFrame.png";
            }
        }

        public int PlayerHealth => modelService.Player.HealthPercentage;

        public int PlayerResourcePrimary => modelService.Player.PrimaryRessourcePercentage;

        public int PlayerResourceSecondary => modelService.Player.SecondaryRessourcePercentage;

        public string Skill0ImagePath => GetSkillImagePath(0);
        public string Skill1ImagePath => GetSkillImagePath(1);
        public string Skill2ImagePath => GetSkillImagePath(2);
        public string Skill3ImagePath => GetSkillImagePath(3);
        public string Skill4ImagePath => GetSkillImagePath(4);
        public string Skill5ImagePath => GetSkillImagePath(5);

        public Brush Skill0Active => GetSkillActiveBrush(0);
        public Brush Skill1Active => GetSkillActiveBrush(1);
        public Brush Skill2Active => GetSkillActiveBrush(2);
        public Brush Skill3Active => GetSkillActiveBrush(3);
        public Brush Skill4Active => GetSkillActiveBrush(4);
        public Brush Skill5Active => GetSkillActiveBrush(5);

        public ICommand Skill0ClickCommand => GetSkillClickCommand(0);
        public ICommand Skill1ClickCommand => GetSkillClickCommand(1);
        public ICommand Skill2ClickCommand => GetSkillClickCommand(2);
        public ICommand Skill3ClickCommand => GetSkillClickCommand(3);
        public ICommand Skill4ClickCommand => GetSkillClickCommand(4);
        public ICommand Skill5ClickCommand => GetSkillClickCommand(5);

        private void OnPlayerInformationChanged(object sender, PlayerInformationEventArgs e)
        {
            RaisePropertyChanged(e.ChangedPropery); // ChangedProperty is PlayerClass, PlayerHealth, etc.

            if (e.SkillIndexChanged != -1)
                RaisePropertyChanged($"Skill{e.SkillIndexChanged}ImagePath");
        }

        private string GetSkillImagePath(int index)
        {
            var uriPrefix = "pack://application:,,,/";
            var skill = modelService.GetSkill(index);

            if (skill != null)
                currentDisplayedSkills[index] = skill.Name;

            if (currentDisplayedSkills[index] == SkillName.None)
                return uriPrefix + "Resource/UI/Skill/EmptyFrame.png";

            return uriPrefix + $"Resource/UI/Skill/{currentDisplayedSkills[index]}.png";
        }

        private Brush GetSkillActiveBrush(int index)
        {
            var skillName = currentDisplayedSkills[index];
            if (skillName == SkillName.None) return Brushes.Transparent;

            var isEnabled = settingsService.Settings.EnabledSkills.Contains(skillName);

            if (isEnabled)
                return Brushes.Green;
            else
                return Brushes.Red;
        }

        private ICommand GetSkillClickCommand(int index)
        {
            var currentCommand = skillClickCommands[index];
            if (currentCommand == null)
            {
                skillClickCommands[index] = new RelayCommand((_) =>
                {
                    var skillName = currentDisplayedSkills[index];
                    if (skillName != SkillName.None)
                    {
                        if (settingsService.Settings.EnabledSkills.Contains(skillName))
                            settingsService.Settings.EnabledSkills.Remove(skillName);
                        else
                            settingsService.Settings.EnabledSkills.Add(skillName);
                        RaisePropertyChanged($"Skill{index}Active");
                    }
                });
            }
            return skillClickCommands[index];
        }
    }
}
