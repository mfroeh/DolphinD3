using Dolphin.Service;
using Dolphin.Ui.Dialog;
using Dolphin.Ui.ViewModel;
using MvvmDialogs.FrameworkDialogs;
using System.Threading;
using System.Threading.Tasks;
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
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var container = new UnityContainer();
            container.RegisterInstance(new Player());
            container.RegisterInstance(new World());
            container.RegisterInstance(new Settings());
            container.RegisterInstance(new Log());
            container.RegisterInstance(new HotkeyListener());

            container.RegisterType<IEventBus, EventBus>(new ContainerControlledLifetimeManager());

            container.RegisterType<IEventPublisher<PlayerInformationChangedEvent>, ExtractPlayerInformationService>("extractPlayerInformation");
            container.RegisterType<IEventPublisher<SkillCanBeCastedEvent>, ExtractSkillInformationService>("extractSkillInformation");
            container.RegisterType<IEventPublisher<SkillRecognitionChangedEvent>, ExtractSkillInformationService>();
            container.RegisterType<IEventPublisher<HotkeyPressedEvent>, HotkeyListenerService>("hotkeyListener");

            container.RegisterType<IEventSubscriber, MacroExecutionService>();
            container.RegisterType<IEventSubscriber, SkillCastingService>();

            container.RegisterType<ICacheService, CacheService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICaptureWindowService, CaptureWindowService>();
            container.RegisterType<ILogService, LogService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IModelService, ModelService>();
            container.RegisterType<IResourceService, ResourceService>();
            container.RegisterType<ISettingsService, SettingsService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IHandleService, HandleService>(new ContainerControlledLifetimeManager());

            container.RegisterType<IViewModelBase, MainViewModel>("main");
            container.RegisterType<IViewModelBase, HotkeyTabViewModel>("hotkeyTab");
            container.RegisterType<IViewModelBase, FeatureTabViewModel>("featureTab");
            container.RegisterType<IViewModelBase, LogTabViewModel>("logTab");
            container.RegisterType<IViewModelBase, ChangeHotkeyDialogViewModel>("changeHotkey");
            container.RegisterType<IViewModelBase, SettingsTabViewModel>("settingsTab");

            container.RegisterType<IFrameworkDialogFactory, CustomFrameworkDialogFactory>();
            container.RegisterType<MvvmDialogs.IDialogService, MvvmDialogs.DialogService>();

            container.AddExtension(new Diagnostic());

            var mainVM = container.Resolve<IViewModelBase>("main");
            MainWindow = new MainWindow { DataContext = mainVM };
            MainWindow.Show();

            var handleService = container.Resolve<IHandleService>();

            Task.Run(() =>
            {
                while (true)
                {
                    handleService.UpdateHandle();
                    Thread.Sleep(1000);
                }
            });
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