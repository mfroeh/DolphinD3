using Dolphin.Enum;
using System.Collections.Generic;
using System.Drawing;

namespace Dolphin.Image
{
    public class ExtractWorldInformationService : IExtractInformationService, IEventPublisher<WorldInformationChangedEvent>
    {
        private readonly IEventBus eventBus;
        private readonly ICropImageService imageService;
        private readonly ILogService logService;
        private readonly IModelService modelService;
        private readonly IResourceService resourceService;

        public ExtractWorldInformationService(IEventBus eventBus, IModelService modelService, IResourceService resourceService, ICropImageService cropService, ILogService logService)
        {
            this.eventBus = eventBus;
            this.modelService = modelService;
            this.resourceService = resourceService;
            this.logService = logService;
            this.imageService = cropService;
        }

        public void Extract(Bitmap picture)
        {
            if (picture == null) return;

            var @event = new WorldInformationChangedEvent();

            var location = ExtractLocation(picture);
            @event.IsLocationChanged = location != modelService.World.CurrentLocation;
            modelService.World.CurrentLocation = location;
            @event.NewLocation = location;

            var window = ExtractWindow(picture, location);
            @event.IsWindowChanged = window != modelService.World.OpenWindow;
            modelService.World.OpenWindow = window;
            @event.NewOpenWindow = window;

            @event.WindowExtraInformation.Add(ExtractExtraInformation(picture, window));

            //if (@event.IsLocationChanged || @event.IsWindowChanged)
            Publish(@event);
        }

        public void Publish(WorldInformationChangedEvent @event)
        {
            eventBus.Publish(@event);
        }

        private int ExtractGemUpsLeft(Bitmap gemUpPart)
        {
            for (int i = 1; i < 6; i++)
            {
                var original = resourceService.Load(System.Enum.Parse(typeof(ExtraInformation), $"Urshi_{i}"));

                if (ImageHelper.Compare(gemUpPart, original) >= 0.95f) return i;
            }

            return default;
        }

        private object ExtractExtraInformation(Bitmap picture, Window window)
        {
            var imagePart = imageService.CropWindowExtraInformation(picture, Window.Urshi);

            switch (window)
            {
                case Window.Urshi:
                    return ExtractGemUpsLeft(imagePart);
                case Window.AcceptGrift:
                    return ExtractEmpoweredTicked(picture, false);
                    // case Window.Obelisk:
                    //    return ExtractEmpoweredTicked(picture, true);
            }

            return default;
        }

        private bool ExtractEmpoweredTicked(Bitmap picture, bool isObelisk)
        {
            var original = isObelisk ? resourceService.Load(ExtraInformation.ObeliskIsEmpowered) : resourceService.Load(ExtraInformation.AcceptGriftIsEmpowered);

            return ImageHelper.Compare(picture, original) >= 0.95f;
        }

        private WorldLocation ExtractLocation(Bitmap picture)
        {
            if (IsVisible(WorldLocation.LoadingScreen, picture, 0.95f)) return WorldLocation.LoadingScreen;

            if (IsVisible(WorldLocation.Grift, picture, 0.92f)) return WorldLocation.Grift;

            if (IsVisible(WorldLocation.Menu, picture, 0.96f)) return WorldLocation.Menu;

            // TODO:
            var riftPart = imageService.CropWorldLocation(picture, WorldLocation.Rift);
            for (int i = 0; i < 8; i++)
            {
                // resourceImage = resourceService.Load(WorldLocation.Rift); if (ImageHelper.Compare(riftPart))
            }

            return default;
        }

        private Window ExtractWindow(Bitmap picture, WorldLocation location)
        {
            if (location == WorldLocation.Menu)
            {
                if (IsVisible(Window.StartGame, picture, 0.95f)) return Window.StartGame;
            }

            var windows = new List<Window> { Window.Urshi, Window.Kadala, Window.AcceptGrift, Window.Obelisk };

            foreach (var window in windows)
            {
                if (IsVisible(window, picture, 0.95f)) return window;
            }

            return default;
        }

        private bool IsVisible<T>(T @enum, Bitmap picture, float threshold)
        {
            Bitmap part = null;
            Bitmap original = null;

            if (@enum is WorldLocation location)
            {
                part = imageService.CropWorldLocation(picture, location);
                original = resourceService.Load(location);
            }
            else if (@enum is Window window)
            {
                part = imageService.CropWindow(picture, window);
                original = resourceService.Load(window);
            }

            if (!(part == null && original == null))
            {
                return ImageHelper.Compare(part, original) >= threshold;
            }

            return default;
        }
    }
}