using Dolphin.Enum;
using System;

namespace Dolphin
{
    public class SkillInformationEventArgs : EventArgs
    {
        public SkillName SkillName { get; set; }
        public int Index { get; set; } = -1;
    }
}