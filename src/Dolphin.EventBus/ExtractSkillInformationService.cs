using Dolphin.Enum;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace Dolphin.EventBus
{
    public class ExtractSkillInformationService : IExtractInformationService, IEventGenerator
    {
        private readonly IEventChannel eventChannel;
        private readonly IModelAdministrationService modelService;
        private readonly IResourceService resourceService;
        private readonly ILogService logService;

        public ExtractSkillInformationService(IEventChannel eventChannel, IModelAdministrationService modelService, IResourceService resourceService, ILogService logService)
        {
            this.eventChannel = eventChannel;
            this.modelService = modelService;
            this.resourceService = resourceService;
            this.logService = logService;
        }

        public async Task Extract(Bitmap bitmap)
        {
            if (bitmap != null)
            {
                for (int i = 0; i < 6; i++)
                {
                    var currentSkillBitmap = await GetSkillBitmap(i, bitmap);
                    var newSkill = await ExtractSkill(currentSkillBitmap, i);
                    modelService.SetSkill(i, newSkill);

                    if (newSkill != null && newSkill.IsNotActiveAndCanBeCasted)
                        await GenerateEvent(new SkillInformationEventArgs { Index = i, SkillName = newSkill.Name });
                }
            }
            else
                logService.AddEntry(this, "The given bitmap to extract from was null", LogLevel.Warning);
        }

        public async Task GenerateEvent(EventArgs e)
        {
            eventChannel.InvokeSkillCanBeCasted(this, e as SkillInformationEventArgs);
        }

        // Returns null, if he cant identify the skill
        private async Task<Skill> ExtractSkill(Bitmap picturePart, int index)
        {
            foreach (var skillName in modelService.GetPossibleSkills())
            {
                var bitmap = await resourceService.Load(skillName);
                var similiaryPercentage = await ImageHelper.Compare(picturePart, bitmap, 0);

                // TODO: If similarityPercentage is > some value but not 1, then it must either be on not castable (due to either resources or cooldown) or it is currently still active.
                if (similiaryPercentage > 0.9f)
                {
                    var skill = new Skill { Name = skillName, Index = index };
                    Console.WriteLine($"Skill at index {index} is {skillName}");

                    if (similiaryPercentage >= 0.99f)
                    {
                        skill.IsNotActiveAndCanBeCasted = true;
                        logService.AddEntry(this, $"Skill{index} is {skillName}.", LogLevel.Info);
                    }
                    else
                    {
                        logService.AddEntry(this, $"Skill{index} is {skillName}, but is either active or not castable.", LogLevel.Info);
                    }
                    return skill;
                }
            }
            logService.AddEntry(this, $"Couldn't identify Skill{index}.");
            return null;
        }

        private async Task<bool> CanCast(Bitmap bitmap)
        {
            return default;
        }

        private async Task<bool> IsActive(Bitmap bitmap)
        {
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