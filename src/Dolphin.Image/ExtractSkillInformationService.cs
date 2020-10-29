using Dolphin.Enum;
using System.Diagnostics;
using System.Drawing;

namespace Dolphin.Image
{
    // TODO: OVERDO
    public class ExtractSkillInformationService : IExtractInformationService, IEventPublisher<SkillCanBeCastedEvent>, IEventPublisher<SkillRecognitionChangedEvent>
    {
        private readonly IEventBus eventBus;
        private readonly ICropImageService imageService;
        private readonly ILogService logService;
        private readonly IModelService modelService;
        private readonly IResourceService resourceService;

        public ExtractSkillInformationService(IEventBus eventBus, IModelService modelService, IResourceService resourceService, ICropImageService imageService, ILogService logService)
        {
            this.eventBus = eventBus;
            this.modelService = modelService;
            this.resourceService = resourceService;
            this.logService = logService;
            this.imageService = imageService;
        }

        public void Extract(Bitmap bitmap)
        {
            if (bitmap == null) return;

            for (int i = 0; i < 6; i++)
            {
                var oldSkill = modelService.GetSkill(i);

                var visibleSkillBitmap = imageService.CropSkillbar(bitmap, i);
                var newSkill = ExtractSkill(visibleSkillBitmap, i, bitmap);

                modelService.SetSkill(i, newSkill);

                if (newSkill?.CanBeCasted == true)
                {
                    Publish(new SkillCanBeCastedEvent { SkillIndex = i, SkillName = newSkill.Name });
                }

                if (newSkill?.Name != oldSkill?.Name)
                {
                    Publish(new SkillRecognitionChangedEvent { Index = i, NewSkillName = newSkill?.Name ?? SkillName.None });
                }
            }
        }

        public void Publish(SkillCanBeCastedEvent @event)
        {
            eventBus.Publish(@event);
        }

        public void Publish(SkillRecognitionChangedEvent @event)
        {
            eventBus.Publish(@event);
        }

        private Skill ExtractSkill(Bitmap picturePart, int index, Bitmap fullBitmap)
        {
            foreach (var skillName in modelService.GetPossibleSkills(index >= 4))
            {
                var template = resourceService.LoadSkillBitmap(skillName, index >= 4);
                var similiaryPercentage = ImageHelper.Compare(picturePart, template);

                // If similarityPercentage is > some value but not 1, then it must either be on not castable (due to resources / cooldown) or it is active
                if (similiaryPercentage > 0.95f)
                {
                    var skill = new Skill { Name = skillName, Index = index };

                    skill.IsActive = IsActive(imageService.CropSkillActive(fullBitmap, index), index >= 4);

                    Trace.WriteLine($"{skillName} is active: {skill.IsActive}");

                    if (similiaryPercentage >= 0.99f)
                    {
                        skill.CanBeCasted = true;
                        logService.AddEntry(this, $"Skill{index} is {skillName}.", LogLevel.Debug);
                    }
                    else
                    {
                        logService.AddEntry(this, $"Skill{index} is {skillName}, but is either active or not castable.", LogLevel.Debug);
                    }

                    return skill;
                }
            }

            logService.AddEntry(this, $"Couldn't identify Skill{index}.", LogLevel.Debug);

            return null;
        }

        private bool IsActive(Bitmap bitmap, bool isMouse)
        {
            if (isMouse)
            {
                var result = ImageHelper.Compare(bitmap, new Bitmap("Resource/Skill/Mouse/SkillActive.png"));

                Trace.WriteLine(result);

                return result > 0.95f;
            }
            else
            {
                var x = ImageHelper.Compare(bitmap, new Bitmap("Resource/Skill/SkillActive.png"));
                Trace.WriteLine(x);

                return x > 0.9f;
            }
        }
    }
}