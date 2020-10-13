using System;

namespace Dolphin
{
    public class SkillInformationEventArgs : EventArgs
    {
        public Skill NewSkill { get; set; }
        public int ReplacedIndex { get; set; } = -1; // Is - 1 if no replacement else the index of the skill
    }
}