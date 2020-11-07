using System.Collections.Generic;

namespace Dolphin
{
    public class SkillCastSettings
    {
        public SkillCastConfiguration SelectedSkillCastConfiguration { get; set; }

        public IList<SkillCastConfiguration> SkillCastConfigurations { get; set; }
    }
}