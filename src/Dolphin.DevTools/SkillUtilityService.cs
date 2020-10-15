using Dolphin.Enum;
using System.Drawing;
using System.Threading.Tasks;

namespace Dolphin.DevTools
{
    public class SkillUtilityService : ISkillUtilityService
    {
        private readonly ICaptureWindowService captureWindowService;
        private readonly IResourceService resourceService;
        private readonly ILogService logService;

        public SkillUtilityService(ICaptureWindowService captureWindowService, IResourceService resourceService, ILogService logService)
        {
            this.captureWindowService = captureWindowService;
            this.resourceService = resourceService;
            this.logService = logService;
        }

        public async Task SaveCurrentSkills(string path)
        {
            var handle = WindowHelper.GetHWND("Diablo III64");
            var picture = await captureWindowService.CaptureWindow(handle);

            if (picture == null)
            {
                logService.AddEntry(this, "Picture was null, make sure to open Diablo first.");
                return;
            }

            for (int i = 0; i < 6; i++)
            {
                var currentSkillBitmap = await GetSkillBitmap(i, picture);

                var savePath = $"{path}/SkillName_{i}.png";
                currentSkillBitmap.Save(savePath);
            }
        }

        public async Task TestSkillRecognition()
        {
            var handle = WindowHelper.GetHWND("Diablo III64");
            var picture = await captureWindowService.CaptureWindow(handle);

            if (picture == null)
            {
                logService.AddEntry(this, "Picture was null, make sure to open Diablo first.");
                return;
            }

            for (int i = 0; i < 6; i++)
            {
                var currentSkillBitmap = await GetSkillBitmap(i, picture);
                var identified = false;
                foreach (var skillName in System.Enum.GetValues(typeof(SkillName)))
                {
                    if ((SkillName)skillName == SkillName.None) continue;

                    var bitmap = await resourceService.Load((SkillName)skillName);
                    var similiaryPercentage = await ImageHelper.Compare(currentSkillBitmap, bitmap, 0);

                    if (similiaryPercentage > 0.9f)
                    {
                        identified = true;
                        if (similiaryPercentage >= 0.99f)
                            logService.AddEntry(this, $"Skill{i} is {skillName}.", LogLevel.Info);
                        else
                            logService.AddEntry(this, $"Skill{i} is {skillName}, but is either active or not castable.", LogLevel.Info);
                    }
                }
                if (!identified)
                    logService.AddEntry(this, $"Couldn't identify Skill{i}.", LogLevel.Info);
            }
        }

        private async Task<Bitmap> GetSkillBitmap(int index, Bitmap fullBitmap)
        {
            var point = new Point { X = 0, Y = 0 };
            var size = new Size { Height = 85, Width = 85 };
            switch (index)
            {
                case 0:
                    point = new Point { X = 835, Y = 1330 };
                    break;

                case 1:
                    point = new Point { X = 835 + 90, Y = 1330 };
                    break;

                case 2:
                    point = new Point { X = 835 + 2 * 90, Y = 1330 };
                    break;

                case 3:
                    point = new Point { X = 835 + 3 * 90, Y = 1330 };
                    break;

                case 4:
                    point = new Point { X = 835 + 4 * 90, Y = 1330 };
                    break;

                case 5:
                    point = new Point { X = 835 + 5 * 90, Y = 1330 };
                    break;
            }
            return await ImageHelper.CropImage(fullBitmap, new Rectangle(point, size));
        }
    }
}