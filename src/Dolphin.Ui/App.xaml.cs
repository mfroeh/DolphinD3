using Dolphin.Image;
using Dolphin.Service;
using Dolphin.Ui.Dialog;
using Dolphin.Ui.View;
using Dolphin.Ui.ViewModel;
using MvvmDialogs;
using MvvmDialogs.DialogFactories;
using MvvmDialogs.DialogTypeLocators;
using MvvmDialogs.FrameworkDialogs;
using Newtonsoft.Json;
using RestoreWindowPlace;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using Unity;
using Unity.Lifetime;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin.Ui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IUnityContainer container = new UnityContainer();

        public WindowPlace WindowPlace { get; private set; }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            if (e.ApplicationExitCode != 2)
            {
                var settings = container.Resolve<Settings>();
                var json = JsonConvert.SerializeObject(settings);
                File.WriteAllText("settings.json", json);
                WindowPlace.Save();
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            WindowPlace = new WindowPlace("placement.config");

            var settings = LoadSettings();

            #region Register

            container.RegisterInstance(settings);

            container.RegisterInstance(new Player());
            container.RegisterInstance(new World());
            container.RegisterInstance(new Log());
            container.RegisterInstance(new HotkeyListener());

            container.RegisterType<IEventBus, EventBus>(new ContainerControlledLifetimeManager());

            container.RegisterType<IEventPublisher<PlayerInformationChangedEvent>, ExtractPlayerInformationService>("extractPlayerInformation");
            container.RegisterType<IEventPublisher<SkillCanBeCastedEvent>, ExtractSkillInformationService>("extractSkillInformation");
            container.RegisterType<IEventPublisher<SkillRecognitionChangedEvent>, ExtractSkillInformationService>("extractSkillInformation");
            container.RegisterType<IEventPublisher<HotkeyPressedEvent>, HotkeyListenerService>("hotkeyListener");
            container.RegisterType<IEventPublisher<WorldInformationChangedEvent>, ExtractWorldInformationService>("extractWorldInformation");

            container.RegisterType<IEventSubscriber, ExecuteActionService>("macro");
            container.RegisterType<IEventSubscriber, SkillCastingService>("skill");

            container.RegisterType<IExtractInformationService, ExtractPlayerInformationService>("extractPlayerInformation");
            container.RegisterType<IExtractInformationService, ExtractSkillInformationService>("extractSkillInformation");
            container.RegisterType<IExtractInformationService, ExtractWorldInformationService>("extractWorldInformation");

            container.RegisterType<IImageCacheService, ImageCacheService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICaptureWindowService, CaptureWindowService>();
            container.RegisterType<ICropImageService, CropImageService>();
            container.RegisterType<ILogService, LogService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IModelService, ModelService>();
            container.RegisterType<IResourceService, ResourceService>();
            container.RegisterType<IImageCacheService, ImageCacheService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISettingsService, SettingsService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IHandleService, HandleService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IActionFinderService, ActionFinderService>();
            container.RegisterType<ITransformService, TransformService>();
            container.RegisterType<IPoolSpotService, PoolSpotService>();
            container.RegisterType<ITravelInformationService, TravelInformationService>();
            container.RegisterType<IConditionFinderService, ConditionFinderService>();

            container.RegisterType<ActionService, ActionService>();

            container.RegisterType<IViewModel, MainViewModel>("main");
            container.RegisterType<IViewModel, HotkeyTabViewModel>("hotkeyTab");
            container.RegisterType<IViewModel, FeatureTabViewModel>("featureTab");
            container.RegisterType<IViewModel, LogTabViewModel>("logTab");
            container.RegisterType<IViewModel, SettingsTabViewModel>("settingsTab");
            container.RegisterType<IViewModel, OverviewTabViewModel>("overviewTab");

            container.RegisterType<IDialogService, DialogService>();
            container.RegisterType<IDialogFactory, ReflectionDialogFactory>();
            container.RegisterType<IDialogTypeLocator, NamingConventionDialogTypeLocator>();
            container.RegisterType<IFrameworkDialogFactory, CustomFrameworkDialogFactory>();

            container.RegisterType<IDialogViewModel, ChangeHotkeyDialogViewModel>("hotkey");
            container.RegisterType<IDialogViewModel, ChangeSkillCastProfileDialogViewModel>("skillCast");

            container.RegisterType<IMessageBoxService, MessageBoxService>();

            #endregion Register

            container.AddExtension(new Diagnostic());

            var mainVM = container.Resolve<IViewModel>("main");
            MainWindow = new MainWindow { DataContext = mainVM };
            MainWindow.Show();

            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += DoWork;
            backgroundWorker.RunWorkerAsync();

            // var task = Task.Factory.StartNew(() => TaskCreationOptions.LongRunning);

            container.Resolve<IEventSubscriber>("macro");
            container.Resolve<IEventSubscriber>("skill");
        }

        private void DoWork(object o, DoWorkEventArgs e)
        {
            var handleService = container.Resolve<IHandleService>();
            Trace.WriteLine(handleService.GetHashCode());
            var logService = container.Resolve<ILogService>();

            handleService.MainLoop("Diablo III64");

            var captureService = container.Resolve<ICaptureWindowService>();
            var extractSkillService = container.Resolve<IExtractInformationService>("extractSkillInformation");
            var extractPlayerService = container.Resolve<IExtractInformationService>("extractPlayerInformation");
            var extractWorldService = container.Resolve<IExtractInformationService>("extractWorldInformation");
            var _settings = container.Resolve<Settings>();

            while (true)
            {
                var watch = Stopwatch.StartNew();
                try
                {
                    var handle = handleService.GetHandle("Diablo III64");
                    if (handle?.Handle != default
                        && !_settings.IsPaused
                        && (_settings.SmartFeatureSettings.SmartActionsEnabled || _settings.SmartFeatureSettings.SkillCastingEnabled))
                    {
                        using (var image = captureService.CaptureWindow("Diablo III64"))
                        {
                            if (_settings.SmartFeatureSettings.SkillCastingEnabled)
                            {
                                extractSkillService.Extract(image);
                                extractPlayerService.Extract(image);
                            }

                            if (_settings.SmartFeatureSettings.SmartActionsEnabled)
                            {
                                // extractWorldService.Extract(image);
                            }
                            watch.Stop();
                            Trace.WriteLine(watch.ElapsedMilliseconds);
                        }
                    }
                    Thread.Sleep((int)_settings.SmartFeatureSettings.UpdateInterval);
                }
                catch (Exception ex)
                {
                    logService.AddEntry(this, "Caught exception in information extraction mmainloop", Enum.LogLevel.Error, ex);
                }
            }
        }

        private Settings LoadSettings()
        {
            var contractResolver = new ShouldSerializeContractResolver();
            var serializerSettings = new JsonSerializerSettings { ContractResolver = contractResolver };

            Settings settings;
            try
            {
                var json = File.ReadAllText("settings.json");

                settings = JsonConvert.DeserializeObject<Settings>(json, serializerSettings);
            }
            catch
            {
                settings = new Settings(true);
            }

            return settings;
        }

        /*
        var container = new UnityContainer();
        container.RegisterType<IUnityContainer, UnityContainer>();

        container.RegisterInstance(new Log());
        container.RegisterInstance(new Player());
        container.RegisterInstance(new World());
        // container.RegisterInstance(settings);
        container.RegisterInstance(new Settings());

        container.RegisterType<IEventChannel, EventChannel>(new ContainerControlledLifetimeManager());
        container.RegisterType<IExtractInformationService, ExtractPlayerInformationService>("player"); // Generator
        container.RegisterType<IExtractInformationService, ExtractSkillInformationService>("skill"); // Generator
        container.RegisterType<IActionAdministrationService, ActionAdministrationService>(); // Subscriber

        container.RegisterType<ILogService, LogService>(new ContainerControlledLifetimeManager());
        container.RegisterType<ICacheService, CacheService>(new ContainerControlledLifetimeManager());

        container.RegisterType<IResourceService, ResourceService>();
        container.RegisterType<ICaptureWindowService, CaptureWindowService>();
        container.RegisterType<IModelService, ModelService>();
        container.RegisterType<ISettingsService, SettingsService>();
        container.RegisterType<IActionExecutionService, ActionExecutionService>();

        container.RegisterType<ViewModelBase, MainWindowViewModel>("main");
        container.RegisterType<TabViewmodelBase, DataTabViewModel>("data");
        container.RegisterType<TabViewmodelBase, LogTabViewModel>("log");

        var mainVM = container.Resolve<ViewModelBase>("main");
        MainWindow = new MainWindow { DataContext = mainVM };
        MainWindow.Title = "Blub";
        MainWindow.Closed += (_, __) =>
        {
            throw new Exception("Main window closed");
        };
        MainWindow.Show();

        var logService = container.Resolve<ILogService>();
        logService.EntryAdded += (o, e) =>
        {
            if (e.LogLevel == LogLevel.Erorr)
                Console.WriteLine(e.Message);
        };

        var extractSkillService = container.Resolve<IExtractInformationService>("skill");
        var extractPlayerService = container.Resolve<IExtractInformationService>("player");
        var captureService = container.Resolve<ICaptureWindowService>();

        var actionAdministrationService = container.Resolve<IActionAdministrationService>();

        var task = Task.Factory.StartNew(async () =>
        {
            try
            {
                var diabloHandle = IntPtr.Zero;
                while (diabloHandle == IntPtr.Zero)
                {
                    logService.AddEntry(this, "Searching for Diablo64 Process", LogLevel.Info);
                    diabloHandle = WindowHelper.GetHWND();
                    Thread.Sleep(1000);
                }

                logService.AddEntry(this, $"Found Diablo Process!", LogLevel.Info);

                while (true)
                {
                    var image = await captureService.CaptureWindow("Diablo III64");

                    var task1 = extractSkillService.Extract(image);
                    var task2 = extractPlayerService.Extract(image);
                    var delay = Task.Delay(60);

                    await task1;
                    await task2;
                    await delay;
                }
            }
            catch (Exception ex)
            {
                logService.AddEntry(this, $"Caught Exception in Mainloop", LogLevel.Erorr, ex);
            }
            finally
            {
                //var _json = JsonConvert.SerializeObject(settings);
                //File.WriteAllText(_json, "settings.json");
                logService.SaveLog("log.txt");
                // MainWindow.Close();
            }
        }, TaskCreationOptions.LongRunning);
    }*/
    }
}