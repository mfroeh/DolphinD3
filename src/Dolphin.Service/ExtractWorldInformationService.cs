using Dolphin.Enum;
using System.Drawing;

namespace Dolphin.Service
{
    public class ExtractWorldInformationService : IExtractInformationService, IEventPublisher<WorldInformationChangedEvent>
    {
        private readonly IEventBus eventBus;
        private readonly ICaptureWindowService imageService;
        private readonly ILogService logService;
        private readonly IModelService modelService;
        private readonly IResourceService resourceService;

        public ExtractWorldInformationService(IEventBus eventBus, IModelService modelService, IResourceService resourceService, ICaptureWindowService imageService, ILogService logService)
        {
            this.eventBus = eventBus;
            this.modelService = modelService;
            this.resourceService = resourceService;
            this.logService = logService;
            this.imageService = imageService;
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

            if (window == Window.Urshi)
            {
                @event.WindowExtraInformation.Add(ExtractGemUpsLeft(picture));
            }

            if (@event.IsLocationChanged || @event.IsWindowChanged)
            {
                Publish(@event);
            }
        }

        public void Publish(WorldInformationChangedEvent @event)
        {
            eventBus.Publish(@event);
        }

        private int ExtractGemUpsLeft(Bitmap picture)
        {
            var imagePart = imageService.CropUrshiGemUp(picture);

            for (int i = 1; i < 6; i++)
            {
                var original = resourceService.Load(System.Enum.Parse(typeof(ExtraInformation), $"Urshi_{i}"));

                if (ImageHelper.Compare(imagePart, original) >= 0.95f) { System.Diagnostics.Trace.WriteLine($"Is {i}"); return i; }
            }

            System.Diagnostics.Trace.WriteLine($"Is none");
            return default;
        }

        private WorldLocation ExtractLocation(Bitmap picture)
        {
            if (IsVisible(WorldLocation.Grift, picture, 0.92f)) return WorldLocation.Grift;

            if (IsVisible(WorldLocation.Menu, picture, 0.96f)) return WorldLocation.Menu;

            // TODO:
            var riftPart = imageService.CropWorldLocation(picture, WorldLocation.Rift);
            for (int i = 0; i < 8; i++)
            {
                // resourceImage = resourceService.Load(WorldLocation.Rift);
                // if (ImageHelper.Compare(riftPart))
            }

            return default;
        }

        private Window ExtractWindow(Bitmap picture, WorldLocation location)
        {
            if (location == WorldLocation.Menu)
            {
                if (IsVisible(Window.StartGame, picture, 0.95f)) return Window.StartGame;
            }

            if (IsVisible(Window.Urshi, picture, 0.95f)) return Window.Urshi;

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