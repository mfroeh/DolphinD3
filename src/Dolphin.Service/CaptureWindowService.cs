using Dolphin.Enum;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Dolphin.Service
{
    public class CaptureWindowService : ICaptureWindowService
    {
        private readonly IHandleService handleService;
        private readonly ILogService logService;
        private readonly ITransformService transformService;

        public CaptureWindowService(IHandleService handleService, ITransformService transformService, ILogService logService)
        {
            this.transformService = transformService;
            this.handleService = handleService;
            this.logService = logService;
        }

        public Bitmap CaptureWindow(string processName)
        {
            var hwnd = handleService.GetHandle(processName);

            return CaptureWindow(hwnd);
        }

        public Bitmap CaptureWindow(IntPtr hwnd)
        {
            if (hwnd == IntPtr.Zero)
            {
                logService.AddEntry(this, $"Got empty handle when trying to capture window.");

                return null;
            }

            var rect = new Rectangle();
            WindowHelper.GetClientRect(hwnd, ref rect);

            var bitmap = new Bitmap(rect.Width - rect.X, rect.Height - rect.Y, PixelFormat.Format24bppRgb);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                var dc = graphics.GetHdc();
                WindowHelper.PrintWindow(hwnd, dc, 0);

                graphics.ReleaseHdc();
            }

            return bitmap;
        }

        public Bitmap CropHealth(Bitmap image)
        {
            var healthBar = transformService.TransformCoordinate(CommonCoordinate.HealthBarTopLeft);
            var size = transformService.TransformSize(CommonSize.HealthbarSize);

            return ImageHelper.CropImage(image, new Rectangle(healthBar, size));
        }

        public Bitmap CropPlayerClass(Bitmap image)
        {
            var playerClass = transformService.TransformCoordinate(CommonCoordinate.PlayerClassTopLeft);
            var size = transformService.TransformSize(CommonSize.PlayerClassSize);

            return ImageHelper.CropImage(image, new Rectangle(playerClass, size));
        }

        public Bitmap CropPrimaryResource(Bitmap image, PlayerClass playerClass)
        {
            var resource = transformService.TransformCoordinate(CommonCoordinate.PrimaryResourceTopLeft);
            var size = transformService.TransformSize(CommonSize.ResourceSize);

            if (playerClass == PlayerClass.DemonHunterFemale || playerClass == PlayerClass.DemonHunterMale)
            {
                resource = transformService.TransformCoordinate(CommonCoordinate.PrimaryResourceDemonHunterTopLeft);
            }

            return ImageHelper.CropImage(image, new Rectangle(resource, size));
        }

        public Bitmap CropSecondaryResource(Bitmap image, PlayerClass playerClass)
        {
            if (!(playerClass == PlayerClass.DemonHunterMale || playerClass == PlayerClass.DemonHunterFemale)) return null;

            var resource = transformService.TransformCoordinate(CommonCoordinate.SecondaryResourceDemonHunterTopLeft);
            var size = transformService.TransformSize(CommonSize.ResourceSize);

            return ImageHelper.CropImage(image, new Rectangle(resource, size));
        }

        public Bitmap CropSkillActive(Bitmap image, int index)
        {
            var size = new Size { Height = 1, Width = 5 }; // TODO: ?
            Point skillActive;

            switch (index)
            {
                case 0:
                    var skillbar = transformService.TransformCoordinate(CommonCoordinate.SkillbarSkill0TopLeft);
                    skillActive = transformService.TransformCoordinate(new Point { X = skillbar.X + 2, Y = skillbar.Y - 14 });
                    break;

                case 1:
                    skillbar = CommonCoordinate.SkillbarSkill1TopLeft;
                    skillActive = transformService.TransformCoordinate(new Point { X = skillbar.X + 2, Y = skillbar.Y - 14 });
                    break;

                case 2:
                    skillbar = transformService.TransformCoordinate(CommonCoordinate.SkillbarSkill2TopLeft);
                    skillActive = transformService.TransformCoordinate(new Point { X = skillbar.X + 2, Y = skillbar.Y - 14 });
                    break;

                case 3:
                    skillbar = transformService.TransformCoordinate(CommonCoordinate.SkillbarSkill3TopLeft);
                    skillActive = transformService.TransformCoordinate(new Point { X = skillbar.X + 2, Y = skillbar.Y - 14 });
                    break;

                case 4:
                    skillbar = transformService.TransformCoordinate(CommonCoordinate.SkillbarSkill4TopLeft);
                    skillActive = transformService.TransformCoordinate(new Point { X = skillbar.X - 1, Y = skillbar.Y - 15 });
                    break;

                case 5:
                    skillbar = transformService.TransformCoordinate(CommonCoordinate.SkillbarSkill5TopLeft);
                    skillActive = transformService.TransformCoordinate(new Point { X = skillbar.X - 1, Y = skillbar.Y - 15 });
                    break;

                default:
                    throw new NotImplementedException();
            }

            return ImageHelper.CropImage(image, new Rectangle(skillActive, size));
        }

        public Bitmap CropSkillbar(Bitmap image, int index)
        {
            Size size = transformService.TransformSize(CommonSize.SkillbarSkillSize);
            Point skillbar;

            switch (index)
            {
                case 0:
                    skillbar = transformService.TransformCoordinate(CommonCoordinate.SkillbarSkill0TopLeft);
                    break;

                case 1:
                    skillbar = transformService.TransformCoordinate(CommonCoordinate.SkillbarSkill1TopLeft);
                    break;

                case 2:
                    skillbar = transformService.TransformCoordinate(CommonCoordinate.SkillbarSkill2TopLeft);
                    break;

                case 3:
                    skillbar = transformService.TransformCoordinate(CommonCoordinate.SkillbarSkill3TopLeft);
                    break;

                case 4:
                    skillbar = transformService.TransformCoordinate(CommonCoordinate.SkillbarSkill4TopLeft);
                    size = transformService.TransformSize(CommonSize.SkillbarSkillSizeMouse);
                    break;

                case 5:
                    skillbar = transformService.TransformCoordinate(CommonCoordinate.SkillbarSkill5TopLeft);
                    size = transformService.TransformSize(CommonSize.SkillbarSkillSizeMouse);
                    break;

                default:
                    throw new NotImplementedException();
            }

            return ImageHelper.CropImage(image, new Rectangle(skillbar, size));
        }

        public Bitmap CropUrshiGemUp(Bitmap image)
        {
            var point = transformService.TransformCoordinate(CommonCoordinate.WindowUrshiGemUp);
            var size = transformService.TransformSize(CommonSize.WindowUrhsiGemUp);

            return ImageHelper.CropImage(image, new Rectangle(point, size));
        }

        public Bitmap CropWindow(Bitmap image, Window window)
        {
            Point point;
            Size size;

            switch (window)
            {
                case Window.StartGame:
                    size = transformService.TransformSize(CommonSize.WindowStartGame);
                    point = transformService.TransformCoordinate(CommonCoordinate.WindowStartGame);
                    break;

                case Window.Urshi:
                    size = transformService.TransformSize(CommonSize.WindowUrshi);
                    point = transformService.TransformCoordinate(CommonCoordinate.WindowUrshi);
                    break;

                case Window.Kadala:
                    size = transformService.TranformSize(CommonSize.WindowKadala);
                    point = transformService.TransformCoordinate(CommonCoordinate.WindowKadala);
                    break;

                case Window.Obelisk:
                    size = transformService.TransformSize(CommonSize.WindowObelisk);
                    point = transformService.TransformCoordinate(CommonCoordinate.WindowObelisk);
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
                    size = transformService.TransformSize(CommonSize.LocationGriftText);
                    point = transformService.TransformCoordinate(CommonCoordinate.LocationGrift);
                    break;

                case WorldLocation.Rift:
                    size = transformService.TransformSize(CommonSize.LocationRiftLevel);
                    point = transformService.TransformCoordinate(CommonCoordinate.LocationRiftLevel);
                    break;

                case WorldLocation.Menu:
                    size = transformService.TransformSize(CommonSize.LocationMenu);
                    point = transformService.TransformCoordinate(CommonCoordinate.LocationMenuSymbol);
                    break;

                default:
                    throw new NotImplementedException();
            }

            return ImageHelper.CropImage(image, new Rectangle(point, size));
        }
    }
}