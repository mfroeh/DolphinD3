using System.Collections.Generic;

namespace Dolphin
{
    public class SkillCastSettings
    {
        public SkillCastConfiguration SelectedSkillCastConfiguration => SelectedIndex >= 0 && SelectedIndex < SkillCastConfigurations.Count ? SkillCastConfigurations[SelectedIndex] : default;

        public int SelectedIndex { get; set; }

        public IList<SkillCastConfiguration> SkillCastConfigurations { get; set; }
    }
}