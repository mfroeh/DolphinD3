using Dolphin.Enum;
using System;
using System.Collections.Generic;
using Unity;

namespace Dolphin.EventBus
{
    public class ExtractSkillInformationService : IExtractInformationService, IEventGenerator
    {
        private readonly IEventChannel eventChannel;
        private readonly IModelAdministrationService modelService;
        private readonly ICaptureScreenService captureScreenService;
        private readonly ILogService logService;

        public ExtractSkillInformationService(IEventChannel eventChannel, IModelAdministrationService modelService,
                                                [Dependency("skill")] ICaptureScreenService captureScreenService, ILogService logService)
        {
            this.eventChannel = eventChannel;
            this.modelService = modelService;
            this.captureScreenService = captureScreenService;
            this.logService = logService;
        }

        public void Extract()
        {
            var picture = captureScreenService.Capture();

            if (picture != null)
            {
                var i = 0;
                foreach (var newSkill in ExtractSkills(picture))
                {
                    var currentSkill = modelService.GetSkill(i);
                    modelService.SetSkill(i, newSkill);

                    if (newSkill != currentSkill)
                        InvokeChannelEvent(new SkillInformationEventArgs { ReplacedIndex = i, NewSkill = newSkill });

                    i += 1;
                }
            }
            else
                logService.AddEntry(this, "The given picture to extract from was null", LogLevel.Warning);
        }

        private IEnumerable<Skill> ExtractSkills(object picture)
        {
            // Checks for all the images he has
            // calls ExtractSKillInformation with parts of the full picture
            yield return null;
        }

        // Returns null, if he cant identify the skill
        private Skill ExtractSkill(object picture)
        {
            return null;
        }

        public void InvokeChannelEvent(EventArgs e)
        {
            eventChannel.InvokeSkillInformationChanged(this, e as SkillInformationEventArgs);
        }
    }
}