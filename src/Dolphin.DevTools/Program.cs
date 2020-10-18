using Dolphin.Enum;
using Dolphin.EventBus;
using Dolphin.Service;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            container.RegisterType<IModelService, ModelService>();
            container.RegisterType<ICaptureWindowService, CaptureWindowService>();
            container.RegisterType<ISkillUtilityService, SkillUtilityService>();
            container.RegisterType<IPlayerUtilityService, PlayerUtilityService>();
            container.RegisterType<IExtractInformationService, ExtractSkillInformationService>("skill");
            container.RegisterType<IExtractInformationService, ExtractPlayerInformationService>("player");

            var logService = container.Resolve<ILogService>();
            logService.EntryAdded += (o, e) =>
            {
                if (e.LogLevel == LogLevel.Info)
                    Console.WriteLine(e.Message);
            };

            var handle = WindowHelper.GetHWND("Diablo III64");
            var service = container.Resolve<ISkillUtilityService>();
            await service.SaveCurrentSkills("Extracted");

            //while (true) {
            //    var t = service.TestSkillRecognition();

            //    await t;
            //    await Task.Delay(200);
            //}

            return 0;
        }

        public static async Task TestPerformance(UnityContainer container)
        {
            var captureService = container.Resolve<ICaptureWindowService>();
            var service1 = container.Resolve<IExtractInformationService>("skill");
            var service2 = container.Resolve<IExtractInformationService>("player");

            while (true)
            {
                var image = await captureService.CaptureWindow("Diablo III64");

                var task1 = service1.Extract(image);
                var task2 = service2.Extract(image);
                var delay = Task.Delay(100);

                await task1;
                await task2;
                await delay;
            }
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