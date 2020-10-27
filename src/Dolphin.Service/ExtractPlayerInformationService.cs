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
        private readonly ICropImageService imageService;
        private readonly ILogService logService;
        private readonly IModelService modelService;
        private readonly IResourceService resourceService;

        public ExtractPlayerInformationService(IEventBus eventBus, IModelService modelService, IResourceService resourceService, ICropImageService imageService, ILogService logService)
        {
            this.eventBus = eventBus;
            this.modelService = modelService;
            this.resourceService = resourceService;
            this.logService = logService;
            this.imageService = imageService;
        }

        public void Extract(Bitmap picture)
        {
            if (picture == null)
            {
                logService.AddEntry(this, $"Bitmap was null");
                return;
            }

            var classPart = imageService.CropPlayerClass(picture);
            var healthPart = imageService.CropHealth(picture);
            var primaryPart = imageService.CropPrimaryResource(picture, modelService.Player.Class);
            var secondaryPart = imageService.CropSecondaryResource(picture, modelService.Player.Class);

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
            if (picturePart == null)
            {
                var old = modelService.Player.SecondaryRessourcePercentage;
                modelService.Player.SecondaryRessourcePercentage = 0;

                return old != 0;
            }

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