using AdonisUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Unity;

namespace Dolphin.Ui
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

            var hotkeyTab = container.Resolve<IViewModelBase>("hotkeyTab");
            hotkeyTab.Parent = this;
            Children.Add(hotkeyTab);

            var featureTab = container.Resolve<IViewModelBase>("featureTab");
            featureTab.Parent = this;
            Children.Add(featureTab);

            var logTab = container.Resolve<IViewModelBase>("logTab");
            logTab.Parent = this;
            Children.Add(logTab);

            var settingsTab = container.Resolve<IViewModelBase>("settingsTab");
            settingsTab.Parent = this;
            Children.Add(settingsTab);

            var overviewTab = container.Resolve<IViewModelBase>("overviewTab");
            overviewTab.Parent = this;
            Children.Add(overviewTab);

            IsDark = settingsService.Settings.UiSettings.IsDark;
            Status = "Ready";

            pauseSubscription = new Subscription<PausedEvent>(OnPause);
            SubscribeBus(pauseSubscription);
        }

        public ICollection<IViewModelBase> Children { get; } = new ObservableCollection<IViewModelBase>();

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