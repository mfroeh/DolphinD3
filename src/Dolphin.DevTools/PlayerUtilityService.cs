using Dolphin.Enum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Dolphin.DevTools
{
    public class PlayerUtilityService : IPlayerUtilityService
    {
        private readonly ICaptureWindowService captureWindowService;
        private readonly ILogService logService;
        private readonly IResourceService resourceService;

        public PlayerUtilityService(ICaptureWindowService captureWindowService, ILogService logService, IResourceService resourceService)
        {
            this.captureWindowService = captureWindowService;
            this.logService = logService;
            this.resourceService = resourceService;
        }

        public async Task SaveHealth(string path)
        {
            var handle = WindowHelper.GetHWND("Diablo III64");
            var picture = await captureWindowService.CaptureWindow(handle);

            var point = new Point { X = 43, Y = 164 };
            var size = new Size { Width = 81, Height = 5 };
            var healthBitmap = await ImageHelper.CropImage(picture, new Rectangle(point, size));

            healthBitmap.Save($"{path}/PlayerHealth_{Guid.NewGuid()}.png");
        }

        public async Task SavePlayerClass(string path)
        {
            var handle = WindowHelper.GetHWND("Diablo III64");
            var picture = await captureWindowService.CaptureWindow(handle);

            var point = new Point { X = 90, Y = 100 };
            var size = new Size { Height = 10, Width = 10 };
            var playerClassBitmap = await ImageHelper.CropImage(picture, new Rectangle(point, size));

            playerClassBitmap.Save($"{path}/PlayerClass_{Guid.NewGuid()}.png");
        }

        public async Task SavePrimaryResource(string path)
        {
            var handle = WindowHelper.GetHWND("Diablo III64");
            var picture = await captureWindowService.CaptureWindow(handle);

            var point = new Point { X = 1830, Y = 1230 };
            var size = new Size { Height = 194, Width = 2 };
            var primaryBitmap = await ImageHelper.CropImage(picture, new Rectangle(point, size));

            primaryBitmap.Save($"{path}/PlayerResource_{Guid.NewGuid()}.png");
        }

        public async Task SaveSecondaryResource(string path)
        {
            var handle = WindowHelper.GetHWND("Diablo III64");
            var picture = await captureWindowService.CaptureWindow(handle);

            var point = new Point { X = 1850, Y = 1230 };
            var size = new Size { Height = 194, Width = 2 };
            var primaryBitmap = await ImageHelper.CropImage(picture, new Rectangle(point, size));

            primaryBitmap.Save($"{path}/PlayerResource_SecondaryDiscipline_{Guid.NewGuid()}.png");
        }

        public async Task TestHealthRecognition()
        {
            var handle = WindowHelper.GetHWND("Diablo III64");
            var picture = await captureWindowService.CaptureWindow(handle);

            var point = new Point { X = 43, Y = 164 };
            var size = new Size { Width = 81, Height = 5 };
            var healthBitmap = await ImageHelper.CropImage(picture, new Rectangle(point, size));

            var matches = new Dictionary<PlayerHealth, float>();
            foreach (var x in System.Enum.GetValues(typeof(PlayerHealth)))
            {
                if ((PlayerHealth)x == PlayerHealth.None)
                    continue;

                var bitmap = await resourceService.Load((PlayerHealth)x);
                var match = await ImageHelper.Compare(bitmap, healthBitmap, 0);
                matches.Add((PlayerHealth)x, match);
            }
            var bestMatch = matches.OrderBy(x => x.Value).Last();
            logService.AddEntry(this, $"Health is most likely to be {bestMatch.Key} with matchingPercentage of {bestMatch.Value}.", LogLevel.Info);
        }

        public async Task TestImageRecognition<T>() where T : System.Enum
        {
            // Potentially
        }

        public async Task TestPlayerClassRecognition()
        {
            var handle = WindowHelper.GetHWND("Diablo III64");
            var picture = await captureWindowService.CaptureWindow(handle);

            var point = new Point { X = 90, Y = 100 };
            var size = new Size { Height = 10, Width = 10 };
            var playerClassBitmap = await ImageHelper.CropImage(picture, new Rectangle(point, size));

            var matchFound = false;
            foreach (var playerClass in System.Enum.GetValues(typeof(PlayerClass)))
            {
                if ((PlayerClass)playerClass == PlayerClass.None)
                    continue;

                var bitmap = await resourceService.Load(playerClass);
                var match = await ImageHelper.Compare(playerClassBitmap, bitmap, 0);

                if (match >= 0.9)
                {
                    if (match >= 0.99)
                    {
                        matchFound = true;
                        logService.AddEntry(this, $"Current playerclass is {playerClass}.", LogLevel.Info);
                    }
                    else
                        logService.AddEntry(this, $"Current playerClass has {match * 100}% to {playerClass}.", LogLevel.Info);
                }
            }
            if (!matchFound)
                logService.AddEntry(this, $"Couldent identify current PlayerClass.", LogLevel.Info);
        }

        public async Task TestPrimaryResource()
        {
            var handle = WindowHelper.GetHWND("Diablo III64");
            var picture = await captureWindowService.CaptureWindow(handle);

            var point = new Point { X = 1830, Y = 1230 };
            var size = new Size { Height = 194, Width = 2 };
            var primaryBitmap = await ImageHelper.CropImage(picture, new Rectangle(point, size));

            var matches = new Dictionary<Enum.PlayerResource, float>();
            foreach (var x in System.Enum.GetValues(typeof(Enum.PlayerResource)))
            {
                if ((Enum.PlayerResource)x == Enum.PlayerResource.None || ((Enum.PlayerResource)x).ToString().StartsWith("Secondary"))
                    continue;

                var bitmap = await resourceService.Load((Enum.PlayerResource)x);
                var match = await ImageHelper.Compare(bitmap, primaryBitmap, 0);
                matches.Add((Enum.PlayerResource)x, match);
            }
            var bestMatch = matches.OrderBy(x => x.Value).Last();
            logService.AddEntry(this, $"Primary Resource is most likely to be {bestMatch.Key} with matchingPercentage of {bestMatch.Value * 100}%.", LogLevel.Info);
        }

        public async Task TestSecondaryResource()
        {
            var handle = WindowHelper.GetHWND("Diablo III64");
            var picture = await captureWindowService.CaptureWindow(handle);

            var point = new Point { X = 1850, Y = 1230 };
            var size = new Size { Height = 194, Width = 2 };
            var secondaryBitmap = await ImageHelper.CropImage(picture, new Rectangle(point, size));

            var matches = new Dictionary<Enum.PlayerResource, float>();
            foreach (var x in System.Enum.GetValues(typeof(Enum.PlayerResource)))
            {
                if ((Enum.PlayerResource)x == Enum.PlayerResource.None || ((Enum.PlayerResource)x).ToString().StartsWith("Primary"))
                    continue;

                var bitmap = await resourceService.Load((Enum.PlayerResource)x);
                var match = await ImageHelper.Compare(bitmap, secondaryBitmap, 0);
                matches.Add((Enum.PlayerResource)x, match);
            }
            var bestMatch = matches.OrderBy(x => x.Value).Last();
            logService.AddEntry(this, $"Secondary Resource is most likely to be {bestMatch.Key} with matchingPercentage of {bestMatch.Value * 100}%.", LogLevel.Info);
        }
    }
}