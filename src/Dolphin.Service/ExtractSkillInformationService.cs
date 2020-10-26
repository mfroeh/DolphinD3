using Dolphin.Enum;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Dolphin.Service
{
    public class ExtractSkillInformationService : IExtractInformationService, IEventPublisher<SkillCanBeCastedEvent>, IEventPublisher<SkillRecognitionChangedEvent>
    {
        private readonly IEventBus eventBus;
        private readonly ILogService logService;
        private readonly ICaptureWindowService imageService;
        private readonly IModelService modelService;
        private readonly IResourceService resourceService;

        public ExtractSkillInformationService(IEventBus eventBus, IModelService modelService, IResourceService resourceService, ICaptureWindowService imageService, ILogService logService)
        {
            this.eventBus = eventBus;
            this.modelService = modelService;
            this.resourceService = resourceService;
            this.logService = logService;
            this.imageService = imageService;
        }

        public void Extract(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                logService.AddEntry(this, "Bitmap was null");
                return;
            }

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

        private bool CanCast(Bitmap bitmap)
        {
            // TODO:
            return default;
        }

        private Skill ExtractSkill(Bitmap picturePart, int index, Bitmap fullBitmap)
        {
            foreach (var skillName in modelService.GetPossibleSkills(index >= 4))
            {
                var template = resourceService.Load(skillName);
                var similiaryPercentage = ImageHelper.Compare(picturePart, template);

                // If similarityPercentage is > some value but not 1, then it must either be on not castable (due to resources / cooldown) or it is active
                if (similiaryPercentage > 0.95f)
                {
                    var skill = new Skill { Name = skillName, Index = index };

                    skill.IsActive = IsActive(imageService.CropSkillActive(fullBitmap, index));

                    if (similiaryPercentage >= 0.9975f)
                    {
                        skill.CanBeCasted = true;
                        logService.AddEntry(this, $"Skill{index} is {skillName}.");
                    }
                    else
                    {
                        logService.AddEntry(this, $"Skill{index} is {skillName}, but is either active or not castable.");
                    }

                    return skill;
                }
            }

            logService.AddEntry(this, $"Couldn't identify Skill{index}.");

            return null;
        }

        private bool IsActive(Bitmap bitmap)
        {
            var result = ImageHelper.Compare(bitmap, new Bitmap("Resource/Skill/SkillActive.png"));

            return result > 0.9f;
        }
    }
}