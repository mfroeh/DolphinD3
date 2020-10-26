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

        public Bitmap CropSkillbar(Bitmap image, int index)
        {
            var skillbar = transformService.TransformCoordinate(CommonCoordinate.SkillbarSkill0TopLeft);
            var size = transformService.TransformSize(CommonSize.SkillbarSkillSize);

            switch (index)
            {
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
            }

            return ImageHelper.CropImage(image, new Rectangle(skillbar, size));
        }

        public Bitmap CropWindow(Bitmap image, Window window)
        {
            switch (window)
            {
                default:
                    throw new NotImplementedException();
            }
        }

        public Bitmap CropWorldLocation(Bitmap image, WorldLocation location)
        {
            switch (location)
            {
                default:
                    throw new NotImplementedException();
            }
        }
    }
}