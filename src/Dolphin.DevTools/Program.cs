using Dolphin.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity;
using Unity.Lifetime;

namespace Dolphin.DevTools
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = GetContainer();

            var handleService = container.Resolve<IHandleService>();

            handleService.MainLoop();

            var imageService = container.Resolve<ISaveImageService>();

            while (true)
            {
                var image = imageService.TakePicture("Diablo III64");

                imageService.SaveHealthbar(image);
                imageService.SavePlayerClass(image);
                imageService.SavePlayerResourcePrimary(image);
                imageService.SavePlayerSkills(image);
                imageService.SavePlayerSkillsMouse(image);

                Thread.Sleep(1000);
            }
        }

        public static IUnityContainer GetContainer()
        {
            var container = new UnityContainer();

            container.RegisterInstance(new Log());

            container.RegisterType<IHandleService, HandleService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICaptureWindowService, CaptureWindowService>();
            container.RegisterType<ILogService, LogService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ITransformService, TransformService>();
            container.RegisterType<ISaveImageService, SaveImageService>();

            return container;
        }
    }
}
