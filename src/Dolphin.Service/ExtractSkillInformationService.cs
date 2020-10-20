using Dolphin.Enum;
using System.Collections.Generic;
using System.Drawing;

namespace Dolphin.Service
{
    public class ExtractSkillInformationService : IExtractInformationService, IEventPublisher<SkillCanBeCastedEvent>, IEventPublisher<SkillRecognitionChangedEvent>
    {
        private readonly IEventBus eventBus;

        // TODO: Get this from somewhere else
        private readonly IDictionary<int, Point> leftUpperCorners = new Dictionary<int, Point>
                                                                        {
                                                                            { 0, new Point { X = 846, Y = 1335 } },
                                                                            { 1, new Point { X = 935, Y = 1335 } },
                                                                            { 2, new Point { X = 1024, Y = 1335 } },
                                                                            { 3, new Point { X = 1113, Y = 1335 } },
                                                                            { 4, new Point { X = 1207, Y = 1335 } },
                                                                            { 5, new Point { X = 1293, Y = 1335 } },
                                                                        };

        private readonly ILogService logService;
        private readonly IModelService modelService;
        private readonly IResourceService resourceService;

        public ExtractSkillInformationService(IEventBus eventBus, IModelService modelService, IResourceService resourceService, ILogService logService)
        {
            this.eventBus = eventBus;
            this.modelService = modelService;
            this.resourceService = resourceService;
            this.logService = logService;
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

                var visibleSkillBitmap = GetSkillBitmap(i, bitmap);
                var newSkill = ExtractSkill(visibleSkillBitmap, i);

                modelService.SetSkill(i, newSkill);

                if (newSkill?.IsNotActiveAndCanBeCasted == true)
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

        private Skill ExtractSkill(Bitmap picturePart, int index)
        {
            foreach (var skillName in modelService.GetPossibleSkills())
            {
                var template = resourceService.Load(skillName);
                var similiaryPercentage = ImageHelper.Compare(picturePart, template);

                // If similarityPercentage is > some value but not 1, then it must either be on not castable (due to resources / cooldown) or it is active
                if (similiaryPercentage > 0.95f)
                {
                    var skill = new Skill { Name = skillName, Index = index };

                    if (similiaryPercentage >= 0.9975f)
                    {
                        skill.IsNotActiveAndCanBeCasted = true;
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

        private Bitmap GetSkillBitmap(int index, Bitmap fullBitmap)
        {
            var size = new Size { Height = 20, Width = 40 };
            var rect = new Rectangle(leftUpperCorners[index], size);

            return ImageHelper.CropImage(fullBitmap, rect);
        }

        private bool IsActive(Bitmap bitmap)
        {
            // TODO:
            return default;
        }
    }
}