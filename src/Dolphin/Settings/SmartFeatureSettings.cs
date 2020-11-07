using Dolphin.Enum;
using System.Collections.Generic;

namespace Dolphin
{
    public class SmartFeatureSettings
    {
        public bool IsOpenRift { get; set; }

        public IList<bool> SkillSuspensionStatus { get; set; }

        public bool SmartActionsEnabled { get; set; }

        public uint UpdateInterval { get; set; }

        public bool SkillCastingEnabled { get; set; }

        public IList<SkillName> EnabledSkills { get; set; }

        public IList<SmartActionName> EnabledSmartActions { get; set; }
    }
}