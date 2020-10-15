using Dolphin.Enum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Dolphin.EventBus
{
    public class ExtractPlayerInformationService : IExtractInformationService, IEventGenerator
    {
        private readonly IEventChannel eventChannel;
        private readonly IModelAdministrationService modelService;
        private readonly IResourceService resourceService;
        private readonly ILogService logService;

        public ExtractPlayerInformationService(IEventChannel eventChannel, IModelAdministrationService modelService, IResourceService resourceService, ILogService logService)
        {
            this.eventChannel = eventChannel;
            this.modelService = modelService;
            this.resourceService = resourceService;
            this.logService = logService;
        }

        public async Task Extract(Bitmap picture)
        {
            var classPart = ImageHelper.CropImage(picture, new Rectangle(new Point { X = 90, Y = 100 }, new Size { Width = 10, Height = 10 }));
            var healthPart = ImageHelper.CropImage(picture, new Rectangle(new Point { X = 43, Y = 164 }, new Size { Width = 81, Height = 5 }));
            var primaryPart = ImageHelper.CropImage(picture, new Rectangle(new Point { X = 1830, Y = 1230 }, new Size { Height = 194, Width = 2 }));
            var secondaryPart = ImageHelper.CropImage(picture, new Rectangle(new Point { X = 1850, Y = 1230 }, new Size { Height = 194, Width = 2 }));

            var classTask = ExtractPlayerClass(await classPart);
            var healthTask = ExtractHealthPercentage(await healthPart);
            var primaryTask = ExtractPrimaryResourcePercentage(await primaryPart);
            var secondaryTask = ExtractSecondaryResourcePercentage(await secondaryPart);

            await classTask;
            await primaryTask;
            await healthTask;
            await secondaryTask;
        }

        public async Task GenerateEvent(EventArgs e)
        {
            eventChannel.InvokePlayerInformationChanged(this, e as PlayerInformationEventArgs);
        }

        private async Task ExtractPlayerClass(Bitmap picturePart)
        {
            var oldPlayerClass = modelService.Player.Class;

            var found = false;
            foreach (var playerClass in System.Enum.GetValues(typeof(PlayerClass)).Cast<PlayerClass>())
            {
                if (playerClass is PlayerClass.None) continue;

                var bitmap = await resourceService.Load(playerClass);
                var match = await ImageHelper.Compare(bitmap, picturePart, 0);

                if (match >= 0.99)
                {
                    modelService.Player.Class = playerClass;
                    found = true;
                    break;
                }
            }

            if (!found)
                modelService.Player.Class = PlayerClass.None;
            logService.AddEntry(this, $"Current playerclass is {modelService.Player.Class}.");

            if (oldPlayerClass != modelService.Player.Class)
                await GenerateEvent(new PlayerInformationEventArgs { });
        }

        private async Task ExtractHealthPercentage(Bitmap picturePart)
        {
            var healthEnum = await GetHighestMatch(picturePart, modelService.GetPossibleHealthPercentage());
            logService.AddEntry(this, $"Player health is most likley to be {healthEnum.Item1}, with odds of {healthEnum.Item2 * 100}%.");

            var currentHealth = modelService.Player.HealthPercentage;
            modelService.SetPlayerHealth(healthEnum.Item1);

            if (currentHealth != modelService.Player.HealthPercentage)
                await GenerateEvent(new PlayerInformationEventArgs { });
        }

        private async Task ExtractPrimaryResourcePercentage(Bitmap picturePart)
        {
            var primaryEnum = await GetHighestMatch(picturePart, modelService.GetPossiblePrimaryResource());
            logService.AddEntry(this, $"Player primary resource is most likley to be {primaryEnum.Item1}, with odds of {primaryEnum.Item2 * 100}%.");

            var currentPrimary = modelService.Player.PrimaryRessourcePercentage;
            modelService.SetPlayerPrimaryResource(primaryEnum.Item1);

            if (currentPrimary != modelService.Player.PrimaryRessourcePercentage)
                await GenerateEvent(new PlayerInformationEventArgs { });
        }

        private async Task ExtractSecondaryResourcePercentage(Bitmap picturePart)
        {
            var secondaryEnum = await GetHighestMatch(picturePart, modelService.GetPossibleSecondaryResource());
            logService.AddEntry(this, $"Player secondary resource is most likley to be {secondaryEnum.Item1}, with odds of {secondaryEnum.Item2 * 100}%.");

            var currentSecondary = modelService.Player.SecondaryRessourcePercentage;
            modelService.SetPlayerSecondaryResource(secondaryEnum.Item1);

            if (currentSecondary != modelService.Player.SecondaryRessourcePercentage)
                await GenerateEvent(new PlayerInformationEventArgs { });
        }

        private async Task<Tuple<T, float>> GetHighestMatch<T>(Bitmap compareTo, IEnumerable<T> possibleEnums)
        {
            var matches = new Dictionary<T, float>();
            foreach (var enumValue in possibleEnums)
            {
                var bitmap = await resourceService.Load(enumValue);
                var match = await ImageHelper.Compare(compareTo, bitmap, 0);

                matches[enumValue] = match;
            }
            var bestMatch = matches.OrderBy(x => x.Value).Last();
            return Tuple.Create(bestMatch.Key, bestMatch.Value);
        }
    }
}