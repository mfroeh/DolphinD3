using Dolphin.Enum;
using Dolphin.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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

            //while (true)
            //{
            //    var image = imageService.TakePicture("Diablo III64");

            //    imageService.SaveHealthbar(image);
            //    imageService.SavePlayerClass(image);
            //    imageService.SavePlayerResourcePrimary(image);
            //    imageService.SavePlayerSkills(image);
            //    imageService.SavePlayerSkillsMouse(image);
            //    imageService.SavePlayerSkillsActive(image);


            //    Thread.Sleep(1000);
            //}


            //imageService.SaveWindow(image, Enum.Window.StartGame);
            //imageService.SaveWorldLocation(image, Enum.WorldLocation.Grift);
            var captureSErvice = container.Resolve<ICaptureWindowService>();

            var image = imageService.TakePicture("Diablo III64");

            imageService.SaveWindow(image, Window.Urshi);

            while (true)
            {
                image = imageService.TakePicture("Diablo III64");
                var now = captureSErvice.CropWorldLocation(image, WorldLocation.Grift); // 90% + 
                var bitmap = new Bitmap("Resource/WorldLocation/Grift.png");
                Console.WriteLine($"Grift chance: {ImageHelper.Compare(now, bitmap)}");

                now = captureSErvice.CropWorldLocation(image, WorldLocation.Menu); // 95% +
                bitmap = new Bitmap("Resource/WorldLocation/Menu.png");
                Console.WriteLine($"Menu chance: {ImageHelper.Compare(now, bitmap)}");

                now = captureSErvice.CropWindow(image, Window.StartGame);
                bitmap = new Bitmap("Resource/Window/StartGame.png");
                Console.WriteLine($"Start game chance: {ImageHelper.Compare(now, bitmap)}"); // 90+

                Thread.Sleep(500);
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
