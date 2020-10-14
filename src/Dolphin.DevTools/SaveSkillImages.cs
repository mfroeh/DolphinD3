using Dolphin.Enum;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace Dolphin.DevTools
{
    public class SaveSkillImages : ISaveSkillImages
    {
        private readonly ICaptureWindowService captureWindowService;
        private readonly IResourceService resourceService;
        private readonly ILogService logService;

        public SaveSkillImages(ICaptureWindowService captureWindowService, IResourceService resourceService, ILogService logService)
        {
            this.captureWindowService = captureWindowService;
            this.resourceService = resourceService;
            this.logService = logService;
        }

        public async Task SaveAllCurrentSkills(string path, bool byName)
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
                if (byName)
                {
                    var skillName = await GetSkillName(currentSkillBitmap, i);
                    if (skillName != SkillName.None)
                        savePath = $"{path}/SkillName_{skillName}.png";
                }
                currentSkillBitmap.Save(savePath);
            }
        }

        private async Task<SkillName> GetSkillName(Bitmap picturePart, int index)
        {
            foreach (var skill in System.Enum.GetValues(typeof(SkillName)))
            {
                if ((SkillName)skill == SkillName.None) continue;

                var bitmap = await resourceService.Load((SkillName)skill);
                var similiaryPercentage = await ImageHelper.Compare(picturePart, bitmap, 0.5f);

                if (similiaryPercentage > 0.9)
                {
                    logService.AddEntry(this, $"Skill at index {index} has {similiaryPercentage} to {(SkillName)skill}.");
                    if (similiaryPercentage >= 0.99f)
                    {
                        logService.AddEntry(this, $"Skill at index {index} is {(SkillName)skill}");
                        return (SkillName)skill;
                    }
                }
            }
            Console.WriteLine($"Could not identify skill at index {index}");
            return default;
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
