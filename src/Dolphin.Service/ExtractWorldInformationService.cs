using Dolphin.Enum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dolphin.Service
{
    public class ExtractWorldInformationService : IExtractInformationService, IEventPublisher<WorldInformationChangedEvent>
    {
        private readonly IEventBus eventBus;
        private readonly ILogService logService;
        private readonly ICaptureWindowService imageService;
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

            var window = ExtractWindow(picture);
            @event.IsWindowChanged = window != modelService.World.OpenWindow;
            modelService.World.OpenWindow = window;

            if (@event.IsLocationChanged || @event.IsWindowChanged)
            {
                Publish(@event);
            }
        }

        private Window ExtractWindow(Bitmap picture)
        {
            var location = modelService.World.CurrentLocation;

            if (location == WorldLocation.Menu)
            {
                var startGamePart = imageService.CropWindow(picture, Window.StartGame);
                var resourceImage = resourceService.Load(Window.StartGame);

                if (ImageHelper.Compare(startGamePart, resourceImage) > 0.95f) return Window.StartGame;
            }

            throw new NotImplementedException();

        }

        public void Publish(WorldInformationChangedEvent @event)
        {
            eventBus.Publish(@event);
        }

        private WorldLocation ExtractLocation(Bitmap picture)
        {
            var griftPart = imageService.CropWorldLocation(picture, WorldLocation.Grift);
            var resourceImage = resourceService.Load(WorldLocation.Grift);

            if (ImageHelper.Compare(griftPart, resourceImage) > 0.95f) return WorldLocation.Grift;

            var menu = imageService.CropWorldLocation(picture, WorldLocation.Menu);
            resourceImage = resourceService.Load(WorldLocation.Menu);

            if (ImageHelper.Compare(menu, resourceImage) > 0.96f) return WorldLocation.Menu;

            // TODO: 
            var riftPart = imageService.CropWorldLocation(picture, WorldLocation.Rift);
            for (int i = 0; i < 8; i++)
            {
                // resourceImage = resourceService.Load(WorldLocation.Rift);
                // if (ImageHelper.Compare(riftPart))
            }

            return default;
        }
    }
}
