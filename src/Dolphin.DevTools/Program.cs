using Dolphin.Enum;
using Dolphin.EventBus;
using Dolphin.Service;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Unity;
using Unity.Lifetime;

namespace Dolphin.DevTools
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var container = new UnityContainer();
            container.RegisterInstance(new Log());
            container.RegisterInstance(new Player());
            container.RegisterInstance(new World());
            container.RegisterType<ILogService, LogService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICacheService, CacheService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IEventChannel, EventChannel>(new ContainerControlledLifetimeManager());
            container.RegisterType<IResourceService, ResourceService>();
            container.RegisterType<IModelAdministrationService, ModelAdministrationService>();
            container.RegisterType<ICaptureWindowService, CaptureWindowService>();
            container.RegisterType<ISkillUtilityService, SkillUtilityService>();
            container.RegisterType<IPlayerUtilityService, PlayerUtilityService>();

            var logService = container.Resolve<ILogService>();
            logService.EntryAdded += (o, e) =>
            {
                if (e.LogLevel == LogLevel.Info)
                    Console.WriteLine(e.Message);
            };
            var service = container.Resolve<IPlayerUtilityService>();

            await service.SaveSecondaryResource("Extracted");

            while (true)
            {
                await service.TestSecondaryResource();
            }
            return 0;
        }

        public static async Task SavePlayerClass(UnityContainer container)
        {
            var service = container.Resolve<IPlayerUtilityService>();
            await service.SavePlayerClass("Extracted");
        }

        public static async Task TestPlayerClassRecognition(UnityContainer container)
        {
            var service = container.Resolve<IPlayerUtilityService>();
            await service.TestPlayerClassRecognition();
        }

        public static async Task TestSkillRecoginition(ISkillUtilityService service, ILogService logService)
        {
            var watch = Stopwatch.StartNew();
            await service.TestSkillRecognition();
            watch.Stop();
            logService.AddEntry(new object(), $"Took {watch.ElapsedMilliseconds}");
        }
    }
}