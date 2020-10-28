using Dolphin.Enum;
using Dolphin.Service;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace Dolphin.Ui.ViewModel
{
    public class OverviewTabViewModel : ViewModelBase, IEventSubscriber
    {
        private readonly IEventBus eventBus;
        private readonly IHandleService handleService;
        private readonly IModelService modelService;
        private readonly Subscription<PlayerInformationChangedEvent> playerInformationChanged;
        private readonly Subscription<SkillRecognitionChangedEvent> skillRecognitionChanged;
        private readonly Subscription<WorldInformationChangedEvent> worldInformationChanged;
        private readonly Subscription<SkillCanBeCastedEvent> skillCanBeCasted;

        public OverviewTabViewModel(IEventBus eventBus, IModelService modelService, IHandleService handleService)
        {
            this.eventBus = eventBus;
            this.modelService = modelService;
            this.handleService = handleService;

            playerInformationChanged = new Subscription<PlayerInformationChangedEvent>(OnPlayerInformationChanged);
            skillRecognitionChanged = new Subscription<SkillRecognitionChangedEvent>(OnSkillRecognitionChanged);
            worldInformationChanged = new Subscription<WorldInformationChangedEvent>(OnWorldInformationChanged);
            skillCanBeCasted = new Subscription<SkillCanBeCastedEvent>(OnSkillCanBeCasted);

            SubscribeBus(playerInformationChanged);
            SubscribeBus(skillRecognitionChanged);
            SubscribeBus(worldInformationChanged);
            SubscribeBus(skillCanBeCasted);

            handleService.HandleStatusChanged += OnHandleChanged;

            CurrentSkills = new ObservableCollection<string>
            {
                "pack://application:,,,/Resource/Skill/EmptyFrame.png",
                "pack://application:,,,/Resource/Skill/EmptyFrame.png",
                "pack://application:,,,/Resource/Skill/EmptyFrame.png",
                "pack://application:,,,/Resource/Skill/EmptyFrame.png",
                "pack://application:,,,/Resource/Skill/EmptyFrame.png",
                "pack://application:,,,/Resource/Skill/EmptyFrame.png",
            };

            CurrentSkillState = new ObservableCollection<string> { "Cant cast", "Cant cast", "Cant cast", "Cant cast", "Cant cast", "Cant cast" };

            CurrentPlayerClass = "pack://application:,,,/Resource/Skill/EmptyFrame.png";
            CurrentHealth = 0;
            CurrentPrimaryResource = 0;
            CurrentPrimaryResource = 0;
        }

        public int CurrentHealth { get; set; }

        public string CurrentPlayerClass { get; set; }

        public int CurrentPrimaryResource { get; set; }

        public int CurrentSecondaryResource { get; set; }

        public ObservableCollection<string> CurrentSkills { get; set; }

        public uint DiabloProcessId { get; set; }

        public WorldLocation CurrentLocation { get; set; }

        public Window OpenWindow { get; set; }

        private string GetResourcePath(SkillName skillName)
        {
            if (skillName == SkillName.None)
            {
                return "pack://application:,,,/Resource/Skill/EmptyFrame.png";
            }
            else
            {
                return $"pack://application:,,,/Resource/Skill/{skillName}.png";
            }
        }

        private string GetResourcePath(PlayerClass playerClass)
        {
            if (playerClass == PlayerClass.None)
            {
                return "pack://application:,,,/Resource/Skill/EmptyFrame.png";
            }
            else
            {
                return $"pack://application:,,,/Resource/Class/{playerClass}.png";
            }
        }

        private void OnHandleChanged(object o, HandleChangedEventArgs e)
        {
            if (e.ProcessName == "Diablo III64")
            {
                PropertySetter(e.NewProcessId, nameof(DiabloProcessId));
            }
        }

        private void OnPlayerInformationChanged(object o, PlayerInformationChangedEvent @event)
        {
            if (@event.ChangedProperties.Contains(nameof(Player.Class)))
            {
                var resource = GetResourcePath(modelService.Player.Class);
                if (CurrentPlayerClass != resource)
                {
                    CurrentPlayerClass = resource;
                    RaisePropertyChanged(nameof(CurrentPlayerClass));
                }
            }

            // TODO: maybe its worth to filter these performance wise

            CurrentHealth = modelService.Player.HealthPercentage;
            CurrentPrimaryResource = modelService.Player.PrimaryResourcePercentage;
            CurrentSecondaryResource = modelService.Player.SecondaryRessourcePercentage;

            PropertySetter(modelService.Player.HealthPercentage, nameof(CurrentHealth));

            RaisePropertyChanged(nameof(CurrentHealth));
            RaisePropertyChanged(nameof(CurrentPrimaryResource));
            RaisePropertyChanged(nameof(CurrentSecondaryResource));
        }

        private void OnSkillRecognitionChanged(object o, SkillRecognitionChangedEvent @event)
        {
            var newResourcePath = GetResourcePath(@event.NewSkillName);
            if (CurrentSkills[@event.Index] != newResourcePath)
            {
                CurrentSkills[@event.Index] = newResourcePath;
                CurrentSkillState[@event.Index] = @event.NewSkillName == SkillName.None ? "" : "Cant be casted";
            }
        }

        private void OnSkillCanBeCasted(object o, SkillCanBeCastedEvent @event)
        {
            var skill = modelService.GetSkill(@event.SkillIndex);
            CurrentSkillState[@event.SkillIndex] = skill.IsActive ? "Can cast [Active]" : "Can cast";
        }

        public ObservableCollection<string> CurrentSkillState { get; set; }

        private void OnWorldInformationChanged(object o, WorldInformationChangedEvent @event)
        {
            if (@event.IsLocationChanged)
            {
                PropertySetter(@event.NewLocation, nameof(CurrentLocation));
            }

            if (@event.IsWindowChanged)
            {
                PropertySetter(@event.NewOpenWindow, nameof(OpenWindow));
            }
        }

        private void SubscribeBus<T>(Subscription<T> subscription) where T : IEvent
        {
            eventBus.Subscribe(subscription);
        }

        private void UnsubscribeBus<T>(Subscription<T> subscription) where T : IEvent
        {
            eventBus.Unsubscribe(subscription);
        }
    }
}