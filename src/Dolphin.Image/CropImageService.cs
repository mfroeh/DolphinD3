using Dolphin.Enum;
using System;
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
            var healthBar = TransformFrom1440p(CommonImageCoordinate.HealthBarTopLeft);
            var size = TransformFrom1440p(CommonImageSize.HealthbarSize);

            return ImageHelper.CropImage(image, new Rectangle(healthBar, size));
        }

        public Bitmap CropPlayerClass(Bitmap image)
        {
            var playerClass = TransformFrom1440p(CommonImageCoordinate.PlayerClassTopLeft);
            var size = TransformFrom1440p(CommonImageSize.PlayerClassSize);

            return ImageHelper.CropImage(image, new Rectangle(playerClass, size));
        }

        public Bitmap CropPrimaryResource(Bitmap image, PlayerClass playerClass)
        {
            var resource = TransformFrom1440p(CommonImageCoordinate.PrimaryResourceTopLeft);
            var size = TransformFrom1440p(CommonImageSize.ResourceSize);

            if (playerClass == PlayerClass.DemonHunterFemale || playerClass == PlayerClass.DemonHunterMale)
            {
                resource = TransformFrom1440p(CommonImageCoordinate.PrimaryResourceDemonHunterTopLeft);
            }

            return ImageHelper.CropImage(image, new Rectangle(resource, size));
        }

        public Bitmap CropSecondaryResource(Bitmap image, PlayerClass playerClass)
        {
            if (!(playerClass == PlayerClass.DemonHunterMale || playerClass == PlayerClass.DemonHunterFemale)) return null;

            var resource = TransformFrom1440p(CommonImageCoordinate.SecondaryResourceDemonHunterTopLeft);
            var size = TransformFrom1440p(CommonImageSize.ResourceSize);

            return ImageHelper.CropImage(image, new Rectangle(resource, size));
        }

        public Bitmap CropSkill(Bitmap image, int index)
        {
            Size size = TransformFrom1440p(CommonImageSize.SkillbarSkillSize);
            Point skillbar;

            switch (index)
            {
                case 0:
                    skillbar = TransformFrom1440p(CommonImageCoordinate.SkillbarSkill0TopLeft);
                    break;

                case 1:
                    skillbar = TransformFrom1440p(CommonImageCoordinate.SkillbarSkill1TopLeft);
                    break;

                case 2:
                    skillbar = TransformFrom1440p(CommonImageCoordinate.SkillbarSkill2TopLeft);
                    break;

                case 3:
                    skillbar = TransformFrom1440p(CommonImageCoordinate.SkillbarSkill3TopLeft);
                    break;

                case 4:
                    skillbar = TransformFrom1440p(CommonImageCoordinate.SkillbarSkill4TopLeft);
                    break;

                case 5:
                    skillbar = TransformFrom1440p(CommonImageCoordinate.SkillbarSkill5TopLeft);
                    break;

                default:
                    return null;
            }

            Point offset;
            if (index >= 4)
            {
                offset = TransformFrom1440p(CommonImageCoordinate.SkillMouseOffset);
            }
            else
            {
                offset = TransformFrom1440p(CommonImageCoordinate.SkillOffset);
            }

            var skillMiddleRectangle = new Point(skillbar.X + offset.X, skillbar.Y + offset.Y);

            return ImageHelper.CropImage(image, new Rectangle(skillMiddleRectangle, size));
        }

        public Bitmap CropSkillActive(Bitmap image, int index)
        {
            var skillActiveSize = TransformFrom1440p(CommonImageSize.SkillActive);
            Point skillActive;

            switch (index)
            {
                case 0:
                    skillActive = TransformFrom1440p(CommonImageCoordinate.SkillbarSkill0Active); ;
                    break;

                case 1:
                    skillActive = TransformFrom1440p(CommonImageCoordinate.SkillbarSkill1Active); ;
                    break;

                case 2:
                    skillActive = TransformFrom1440p(CommonImageCoordinate.SkillbarSkill2Active); ;
                    break;

                case 3:
                    skillActive = TransformFrom1440p(CommonImageCoordinate.SkillbarSkill3Active); ;
                    break;

                case 4:
                    skillActive = TransformFrom1440p(CommonImageCoordinate.SkillbarSkill4Active); ;
                    break;

                case 5:
                    skillActive = TransformFrom1440p(CommonImageCoordinate.SkillbarSkill5Active); ;
                    break;

                default:
                    return null;
            }

            return ImageHelper.CropImage(image, new Rectangle(skillActive, skillActiveSize));
        }

        public Bitmap CropWindow(Bitmap image, Window window)
        {
            Point point;
            Size size;

            switch (window)
            {
                case Window.StartGame:
                    size = TransformFrom1440p(CommonImageSize.WindowStartGame);
                    point = TransformFrom1440p(CommonImageCoordinate.WindowStartGame);
                    break;

                case Window.Urshi:
                    size = TransformFrom1440p(CommonImageSize.WindowUrshi);
                    point = TransformFrom1440p(CommonImageCoordinate.WindowUrshi);
                    break;

                case Window.Kadala:
                    size = TransformFrom1440p(CommonImageSize.WindowKadala);
                    point = TransformFrom1440p(CommonImageCoordinate.WindowKadala);
                    break;

                case Window.Obelisk:
                    size = TransformFrom1440p(CommonImageSize.WindowObelisk);
                    point = TransformFrom1440p(CommonImageCoordinate.WindowObelisk);
                    break;

                default:
                    return null;
            }

            return ImageHelper.CropImage(image, new Rectangle(point, size));
        }

        public Bitmap CropWindowExtraInformation(Bitmap image, Window window)
        {
            switch (window)
            {
                case Window.Urshi:
                    return CropWindowExtraInformation(image, ExtraInformation.Urshi_1);

                case Window.Obelisk:
                    return CropWindowExtraInformation(image, ExtraInformation.ObeliskIsEmpowered);

                case Window.AcceptGrift:
                    return CropWindowExtraInformation(image, ExtraInformation.AcceptGriftIsEmpowered);

                default:
                    throw new NotImplementedException();
            }
        }

        public Bitmap CropWindowExtraInformation(Bitmap image, ExtraInformation extraInformation)
        {
            Point point;
            Size size;

            switch (extraInformation)
            {
                case ExtraInformation.Urshi_1:
                case ExtraInformation.Urshi_2:
                case ExtraInformation.Urshi_3:
                case ExtraInformation.Urshi_4:
                case ExtraInformation.Urshi_5:
                    point = TransformFrom1440p(CommonImageCoordinate.WindowUrshiGemUp);
                    size = TransformFrom1440p(CommonImageSize.WindowUrhsiGemUp);
                    break;

                case ExtraInformation.ObeliskIsEmpowered:
                    point = TransformFrom1440p(CommonImageCoordinate.WindowObeliskEmpowered);
                    size = TransformFrom1440p(CommonImageSize.WindowObeliskEmpowered);
                    break;

                case ExtraInformation.AcceptGriftIsEmpowered:
                    point = TransformFrom1440p(CommonImageCoordinate.WindowAcceptGriftEmpowered);
                    size = TransformFrom1440p(CommonImageSize.WindowAcceptGriftEmpowered);
                    break;

                default:
                    throw new NotImplementedException();
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
                    size = TransformFrom1440p(CommonImageSize.LocationGriftText);
                    point = TransformFrom1440p(CommonImageCoordinate.LocationGrift);
                    break;

                case WorldLocation.Rift:
                    size = TransformFrom1440p(CommonImageSize.LocationRiftLevel);
                    point = TransformFrom1440p(CommonImageCoordinate.LocationRiftLevel);
                    break;

                case WorldLocation.Menu:
                    size = TransformFrom1440p(CommonImageSize.LocationMenu);
                    point = TransformFrom1440p(CommonImageCoordinate.LocationMenuSymbol);
                    break;

                case WorldLocation.LoadingScreen:
                    size = TransformFrom1440p(CommonImageSize.LocationLoadingScreen);
                    point = TransformFrom1440p(CommonImageCoordinate.LocationLoadingScreen);
                    break;

                default:
                    return null;
            }

            return ImageHelper.CropImage(image, new Rectangle(point, size));
        }

        private Point TransformFrom1440p(Point point)
        {
            return transformService.TransformCoordinate(point, originalWidth: 2560, originalHeight: 1440);
        }

        private Size TransformFrom1440p(Size size)
        {
            return transformService.TransformSize(size, originalWidth: 2560, originalHeight: 1440);
        }
    }
}