using System.Collections.Generic;

namespace Dolphin
{
    public class SkillCastSettings
    {
        public int SelectedIndex { get; set; }

        public SkillCastConfiguration SelectedSkillCastConfiguration => SelectedIndex >= 0 && SelectedIndex < SkillCastConfigurations.Count ? SkillCastConfigurations[SelectedIndex] : default;

        public IList<SkillCastConfiguration> SkillCastConfigurations { get; set; }
    }
}