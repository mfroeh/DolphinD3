﻿using AdonisUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Policy;
using System.Windows;
using System.Windows.Input;
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

            var tab0 = container.Resolve<IViewModelBase>("hotkeyTab");
            tab0.Parent = this;
            Children.Add(tab0);

            var tab1 = container.Resolve<IViewModelBase>("featureTab");
            tab1.Parent = this;
            Children.Add(tab1);

            var tab2 = container.Resolve<IViewModelBase>("logTab");
            tab2.Parent = this;
            Children.Add(tab2);

            var tab3 = container.Resolve<IViewModelBase>("settingsTab");
            tab3.Parent = this;
            Children.Add(tab3);

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