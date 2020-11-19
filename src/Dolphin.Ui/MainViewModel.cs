using AdonisUI;
using System.Collections.ObjectModel;
using System.Windows;
using Unity;

namespace Dolphin.Ui.ViewModel
{
    public class MainViewModel : ViewModelBase, IEventSubscriber
    {
        private readonly IEventBus eventBus;
        private readonly Subscription<PausedEvent> pauseSubscription;
        private readonly ISettingsService settingsService;
        private bool isDark;

        public MainViewModel(IUnityContainer container, IEventBus eventBus, ISettingsService settingsService)
        {
            this.eventBus = eventBus;
            this.settingsService = settingsService;

            var hotkeyTab = container.Resolve<IViewModel>("hotkeyTab");
            hotkeyTab.Parent = this;
            Children.Add(hotkeyTab);

            var featureTab = container.Resolve<IViewModel>("featureTab");
            featureTab.Parent = this;
            Children.Add(featureTab);

            var logTab = container.Resolve<IViewModel>("logTab");
            logTab.Parent = this;
            Children.Add(logTab);

            var settingsTab = container.Resolve<IViewModel>("settingsTab");
            settingsTab.Parent = this;
            Children.Add(settingsTab);

            var overviewTab = container.Resolve<IViewModel>("overviewTab");
            overviewTab.Parent = this;
            Children.Add(overviewTab);

            IsDark = settingsService.Settings.UiSettings.IsDark;
            Status = "Ready";

            pauseSubscription = new Subscription<PausedEvent>(OnPause);
            SubscribeBus(pauseSubscription);
        }

        public ObservableCollection<IViewModel> Children { get; } = new ObservableCollection<IViewModel>();

        public bool IsDark
        {
            get => isDark;
            set
            {
                isDark = value;
                settingsService.Settings.UiSettings.IsDark = value;
                RaisePropertyChanged("IsDark");
                ChangeTheme(isDark);
            }
        }

        public string Status { get; set; }

        private void ChangeTheme(bool isDark)
        {
            ResourceLocator.SetColorScheme(Application.Current.Resources, isDark ? ResourceLocator.DarkColorScheme : ResourceLocator.LightColorScheme);
        }

        private void OnPause(object o, PausedEvent @event)
        {
            Status = @event.IsPaused ? "Paused..." : "Ready";
            RaisePropertyChanged("Status");
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