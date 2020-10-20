using Dolphin.Enum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dolphin.Service
{
    public class ExtractPlayerInformationService : IExtractInformationService, IEventPublisher<PlayerInformationChangedEvent>
    {
        private readonly IEventBus eventBus;
        private readonly ILogService logService;
        private readonly IModelService modelService;
        private readonly IResourceService resourceService;

        public ExtractPlayerInformationService(IEventBus eventBus, IModelService modelService, IResourceService resourceService, ILogService logService)
        {
            this.eventBus = eventBus;
            this.modelService = modelService;
            this.resourceService = resourceService;
            this.logService = logService;
        }

        public void Extract(Bitmap picture)
        {
            if (picture == null)
            {
                logService.AddEntry(this, $"Bitmap was null");
                return;
            }

            var classPart = ImageHelper.CropImage(picture, new Rectangle(new Point { X = 90, Y = 100 }, new Size { Width = 10, Height = 10 }));
            var healthPart = ImageHelper.CropImage(picture, new Rectangle(new Point { X = 43, Y = 164 }, new Size { Width = 81, Height = 5 }));
            var primaryPart = ImageHelper.CropImage(picture, new Rectangle(new Point { X = 1830, Y = 1230 }, new Size { Height = 194, Width = 2 }));
            var secondaryPart = ImageHelper.CropImage(picture, new Rectangle(new Point { X = 1850, Y = 1230 }, new Size { Height = 194, Width = 2 }));

            var classChanged = ExtractPlayerClass(classPart);
            var healthChanged = ExtractHealth(healthPart);
            var primaryResourceChanged = ExtractPrimaryResource(primaryPart);
            var secondaryResourceChanged = ExtractSecondaryResource(secondaryPart);

            var @event = new PlayerInformationChangedEvent();
            if (classChanged) @event.ChangedProperties.Add(nameof(Player.Class));
            if (healthChanged) @event.ChangedProperties.Add(nameof(Player.HealthPercentage));
            if (primaryResourceChanged) @event.ChangedProperties.Add(nameof(Player.PrimaryResourcePercentage));
            if (secondaryResourceChanged) @event.ChangedProperties.Add(nameof(Player.SecondaryRessourcePercentage));

            if (@event.ChangedProperties.Any())
            {
                Publish(@event);
            }

            logService.AddEntry(this, $"Extracted Playerinformation: [Class:{modelService.Player.Class}, HP: {modelService.Player.HealthPercentage}, PrimaryResource: {modelService.Player.PrimaryResourcePercentage}].");
        }

        public void Publish(PlayerInformationChangedEvent @event)
        {
            eventBus.Publish(@event);
        }

        private bool ExtractHealth(Bitmap picturePart)
        {
            var oldHealth = modelService.Player.HealthPercentage;

            var compareResult = GetHighestMatch(picturePart, modelService.GetPossibleHealthEnum());
            logService.AddEntry(this, $"Player health is most likley to be {compareResult.Item1}, with odds of {compareResult.Item2 * 100}%.");

            modelService.SetPlayerHealth(compareResult.Item1);

            return oldHealth != modelService.Player.HealthPercentage;
        }

        private bool ExtractPlayerClass(Bitmap picturePart)
        {
            var oldClass = modelService.Player.Class;

            var newPlayerClass = PlayerClass.None;
            foreach (var playerClass in System.Enum.GetValues(typeof(PlayerClass)).Cast<PlayerClass>())
            {
                if (playerClass is PlayerClass.None) continue;

                var template = resourceService.Load(playerClass);
                var match = ImageHelper.Compare(picturePart, template);

                if (match >= 0.99)
                {
                    newPlayerClass = playerClass;
                    break;
                }
            }

            modelService.Player.Class = newPlayerClass;
            logService.AddEntry(this, $"Extracted player class is {newPlayerClass}.");

            return newPlayerClass != oldClass;
        }

        private bool ExtractPrimaryResource(Bitmap picturePart)
        {
            var oldPrimary = modelService.Player.PrimaryResourcePercentage;

            var compareResult = GetHighestMatch(picturePart, modelService.GetPossiblePrimaryResourceEnum());
            logService.AddEntry(this, $"Player primary resource is most likley to be {compareResult.Item1}, with odds of {compareResult.Item2 * 100}%.");

            modelService.SetPlayerPrimaryResource(compareResult.Item1);

            return oldPrimary != modelService.Player.PrimaryResourcePercentage;
        }

        private bool ExtractSecondaryResource(Bitmap picturePart)
        {
            var oldSecondary = modelService.Player.SecondaryRessourcePercentage;

            var compareResult = GetHighestMatch(picturePart, modelService.GetPossibleSecondary());
            logService.AddEntry(this, $"Player secondary resource is most likley to be {compareResult.Item1}, with odds of {compareResult.Item2 * 100}%.");

            modelService.SetPlayerSecondaryResource(compareResult.Item1);

            return oldSecondary != modelService.Player.SecondaryRessourcePercentage;
        }

        private Tuple<T, float> GetHighestMatch<T>(Bitmap compareTo, IEnumerable<T> possibleEnums)
        {
            var matches = new Dictionary<T, float>();
            foreach (var enumValue in possibleEnums)
            {
                var template = resourceService.Load(enumValue);
                var match = ImageHelper.Compare(compareTo, template);

                matches[enumValue] = match;
            }

            var bestMatch = matches.OrderBy(x => x.Value).LastOrDefault();

            return Tuple.Create(bestMatch.Key, bestMatch.Value);
        }
    }
}