using Dolphin.Enum;
using Dolphin.EventBus;
using Dolphin.Service;
using System;
using System.Diagnostics;
using System.Threading;
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
            container.RegisterType<ISaveSkillImages, SaveSkillImages>();

            var logService = container.Resolve<ILogService>();
            logService.EntryAdded += (o, e) =>
            {
                //if (e.LogLevel == LogLevel.Info)
                Console.WriteLine(e.Message);
            };

            var saveSkillImages = container.Resolve<ISaveSkillImages>();

            while (true)
            {
                await saveSkillImages.SaveAllCurrentSkills("Extracted", true);
                Thread.Sleep(500);
            }

            return 0;
        }

        private static void SaveCurrentSkillImages()
        {

        }
    }
}