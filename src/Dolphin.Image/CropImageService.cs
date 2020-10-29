using Dolphin.Enum;
using System.Drawing;

namespace Dolphin.Image
{
    public class CropImageService : ICropImageService
    {
        private readonly ILogService logService;
        private readonly ITransformService transformService;

        public CropImageService(ITransformService transformService, ILogService logService)
        {
            this.transformService = transformService;
            this.logService = logService;
        }

        public Bitmap CropHealthbar(Bitmap image)
        {
            var healthBar = transformService.TransformCoordinate(CommonImageCoordinate.HealthBarTopLeft);
            var size = transformService.TransformSize(CommonImageSize.HealthbarSize);

            return ImageHelper.CropImage(image, new Rectangle(healthBar, size));
        }

        public Bitmap CropPlayerClass(Bitmap image)
        {
            var playerClass = transformService.TransformCoordinate(CommonImageCoordinate.PlayerClassTopLeft);
            var size = transformService.TransformSize(CommonImageSize.PlayerClassSize);

            return ImageHelper.CropImage(image, new Rectangle(playerClass, size));
        }

        public Bitmap CropPrimaryResource(Bitmap image, PlayerClass playerClass)
        {
            var resource = transformService.TransformCoordinate(CommonImageCoordinate.PrimaryResourceTopLeft);
            var size = transformService.TransformSize(CommonImageSize.ResourceSize);

            if (playerClass == PlayerClass.DemonHunterFemale || playerClass == PlayerClass.DemonHunterMale)
            {
                resource = transformService.TransformCoordinate(CommonImageCoordinate.PrimaryResourceDemonHunterTopLeft);
            }

            return ImageHelper.CropImage(image, new Rectangle(resource, size));
        }

        public Bitmap CropSecondaryResource(Bitmap image, PlayerClass playerClass)
        {
            if (!(playerClass == PlayerClass.DemonHunterMale || playerClass == PlayerClass.DemonHunterFemale)) return null;

            var resource = transformService.TransformCoordinate(CommonImageCoordinate.SecondaryResourceDemonHunterTopLeft);
            var size = transformService.TransformSize(CommonImageSize.ResourceSize);

            return ImageHelper.CropImage(image, new Rectangle(resource, size));
        }

        // TODO: Overdo completely
        public Bitmap CropSkillActive(Bitmap image, int index)
        {
            var skillActiveSize = transformService.TransformSize(CommonImageSize.SkillActive);
            Point skillActive;

            switch (index)
            {
                case 0:
                    var skillbar = transformService.TransformCoordinate(CommonImageCoordinate.SkillbarSkill0TopLeft);
                    skillActive = transformService.TransformCoordinate(new Point { X = skillbar.X - 3, Y = skillbar.Y });
                    break;

                case 1:
                    skillbar = CommonImageCoordinate.SkillbarSkill1TopLeft;
                    skillActive = transformService.TransformCoordinate(new Point { X = skillbar.X - 3, Y = skillbar.Y });
                    break;

                case 2:
                    skillbar = transformService.TransformCoordinate(CommonImageCoordinate.SkillbarSkill2TopLeft);
                    skillActive = transformService.TransformCoordinate(new Point { X = skillbar.X - 3, Y = skillbar.Y });
                    break;

                case 3:
                    skillbar = transformService.TransformCoordinate(CommonImageCoordinate.SkillbarSkill3TopLeft);
                    skillActive = transformService.TransformCoordinate(new Point { X = skillbar.X - 3, Y = skillbar.Y });
                    break;

                case 4:
                    skillbar = transformService.TransformCoordinate(CommonImageCoordinate.SkillbarSkill4TopLeft);
                    skillActive = transformService.TransformCoordinate(new Point { X = skillbar.X - 4, Y = skillbar.Y });
                    break;

                case 5:
                    skillbar = transformService.TransformCoordinate(CommonImageCoordinate.SkillbarSkill5TopLeft);
                    skillActive = transformService.TransformCoordinate(new Point { X = skillbar.X - 4, Y = skillbar.Y });
                    break;

                default:
                    return null;
            }

            return ImageHelper.CropImage(image, new Rectangle(skillActive, skillActiveSize));
        }

        public Bitmap CropSkillbar(Bitmap image, int index)
        {
            Size size = transformService.TransformSize(CommonImageSize.SkillbarSkillSize);
            Point skillbar;

            switch (index)
            {
                case 0:
                    skillbar = transformService.TransformCoordinate(CommonImageCoordinate.SkillbarSkill0TopLeft);
                    break;

                case 1:
                    skillbar = transformService.TransformCoordinate(CommonImageCoordinate.SkillbarSkill1TopLeft);
                    break;

                case 2:
                    skillbar = transformService.TransformCoordinate(CommonImageCoordinate.SkillbarSkill2TopLeft);
                    break;

                case 3:
                    skillbar = transformService.TransformCoordinate(CommonImageCoordinate.SkillbarSkill3TopLeft);
                    break;

                case 4:
                    skillbar = transformService.TransformCoordinate(CommonImageCoordinate.SkillbarSkill4TopLeft);
                    size = transformService.TransformSize(CommonImageSize.SkillbarSkillSizeMouse);
                    break;

                case 5:
                    skillbar = transformService.TransformCoordinate(CommonImageCoordinate.SkillbarSkill5TopLeft);
                    size = transformService.TransformSize(CommonImageSize.SkillbarSkillSizeMouse);
                    break;

                default:
                    return null;
            }

            return ImageHelper.CropImage(image, new Rectangle(skillbar, size));
        }

        public Bitmap CropUrshiGemUp(Bitmap image)
        {
            var point = transformService.TransformCoordinate(CommonImageCoordinate.WindowUrshiGemUp);
            var size = transformService.TransformSize(CommonImageSize.WindowUrhsiGemUp);

            return ImageHelper.CropImage(image, new Rectangle(point, size));
        }

        public Bitmap CropWindow(Bitmap image, Window window)
        {
            Point point;
            Size size;

            switch (window)
            {
                case Window.StartGame:
                    size = transformService.TransformSize(CommonImageSize.WindowStartGame);
                    point = transformService.TransformCoordinate(CommonImageCoordinate.WindowStartGame);
                    break;

                case Window.Urshi:
                    size = transformService.TransformSize(CommonImageSize.WindowUrshi);
                    point = transformService.TransformCoordinate(CommonImageCoordinate.WindowUrshi);
                    break;

                case Window.Kadala:
                    size = transformService.TransformSize(CommonImageSize.WindowKadala);
                    point = transformService.TransformCoordinate(CommonImageCoordinate.WindowKadala);
                    break;

                case Window.Obelisk:
                    size = transformService.TransformSize(CommonImageSize.WindowObelisk);
                    point = transformService.TransformCoordinate(CommonImageCoordinate.WindowObelisk);
                    break;

                default:
                    return null;
            }

            return ImageHelper.CropImage(image, new Rectangle(point, size));
        }

        public Bitmap CropWorldLocation(Bitmap image, WorldLocation location)
        {
            Point point;
            Size size;

            switch (location)
            {
                case WorldLocation.Grift:
                    size = transformService.TransformSize(CommonImageSize.LocationGriftText);
                    point = transformService.TransformCoordinate(CommonImageCoordinate.LocationGrift);
                    break;

                case WorldLocation.Rift:
                    size = transformService.TransformSize(CommonImageSize.LocationRiftLevel);
                    point = transformService.TransformCoordinate(CommonImageCoordinate.LocationRiftLevel);
                    break;

                case WorldLocation.Menu:
                    size = transformService.TransformSize(CommonImageSize.LocationMenu);
                    point = transformService.TransformCoordinate(CommonImageCoordinate.LocationMenuSymbol);
                    break;

                default:
                    return null;
            }

            return ImageHelper.CropImage(image, new Rectangle(point, size));
        }
    }
}