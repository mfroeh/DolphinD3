using System;

namespace Dolphin
{
    public class PlayerInformationEventArgs : EventArgs
    {
        public string ChangedPropery { get; set; }
        public int SkillIndexChanged { get; set; } = -1;
    }
}