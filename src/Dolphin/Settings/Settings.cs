using Dolphin.Enum;
using System.Collections.Generic;

namespace Dolphin
{
    public class Settings
    {
        public IList<SkillName> EnabledSkills { get; set; } = new List<SkillName>();
    }
}