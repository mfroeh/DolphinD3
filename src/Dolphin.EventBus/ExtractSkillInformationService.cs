using Dolphin.Enum;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace Dolphin.EventBus
{
    public class ExtractSkillInformationService : IExtractInformationService, IEventGenerator
    {
        private readonly IEventChannel eventChannel;
        private readonly IModelService modelService;
        private readonly IResourceService resourceService;
        private readonly ILogService logService;

        public ExtractSkillInformationService(IEventChannel eventChannel, IModelService modelService, IResourceService resourceService, ILogService logService)
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
                    var currentSkill = modelService.GetSkill(i);

                    if (newSkill?.Name != currentSkill?.Name)
                        GeneratePlayerInformationChangedEvent(new PlayerInformationEventArgs { SkillIndexChanged = i });

                    modelService.SetSkill(i, newSkill);
                    if (newSkill != null && newSkill.IsNotActiveAndCanBeCasted) // newSkill != null && newSkill.IsNotActiveAndCanBeCasted
                        await GenerateEvent(new SkillInformationEventArgs { Index = i, SkillName = newSkill.Name });
                }
            }
            else
                logService.AddEntry(this, "Bitmap was null", LogLevel.Info);
        }

        public async Task GenerateEvent(EventArgs e)
        {
            eventChannel.InvokeSkillCanBeCasted(this, e as SkillInformationEventArgs);
        }

        private void GeneratePlayerInformationChangedEvent(PlayerInformationEventArgs e)
        {
            logService.AddEntry(this, $"Skill{e.SkillIndexChanged} has changed!", LogLevel.Erorr);
            eventChannel.InvokePlayerInformationChanged(this, e);
        }

        // Returns null, if he cant identify the skill
        private async Task<Skill> ExtractSkill(Bitmap picturePart, int index)
        {
            foreach (var skillName in modelService.GetPossibleSkills())
            {
                var bitmap = await resourceService.Load(skillName);
                var similiaryPercentage = await ImageHelper.Compare(picturePart, bitmap, 0);

                // TODO: If similarityPercentage is > some value but not 1, then it must either be on not castable (due to either resources or cooldown) or it is currently still active.
                if (similiaryPercentage > 0.95f)
                {
                    var skill = new Skill { Name = skillName, Index = index };

                    if (similiaryPercentage >= 0.9975f)
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
            logService.AddEntry(this, $"Couldn't identify Skill{index}.", LogLevel.Info);
            return null;
        }

        private bool CanCast(Bitmap bitmap)
        {
            // TODO: 
            return default;
        }

        private bool IsActive(Bitmap bitmap)
        {
            // TODO: 
            return default;
        }

        private async Task<Bitmap> GetSkillBitmap(int index, Bitmap fullBitmap)
        {
            var point = new Point { X = 0, Y = 0 };
            var size = new Size { Height = 20, Width = 40 };
            switch (index)
            {
                case 0:
                    point = new Point { X = 846, Y = 1335 };
                    break;

                case 1:
                    point = new Point { X = 935, Y = 1335 };
                    break;

                case 2:
                    point = new Point { X = 1024, Y = 1335 };
                    break;

                case 3:
                    point = new Point { X = 1113, Y = 1335 };
                    break;

                case 4:
                    point = new Point { X = 1207, Y = 1335 };
                    break;

                case 5:
                    point = new Point { X = 1293, Y = 1335 };
                    break;
            }
            return await ImageHelper.CropImage(fullBitmap, new Rectangle(point, size));
        }
    }
}