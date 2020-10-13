using System.Collections.Generic;

namespace Dolphin
{
    public class Player
    {
        public int PrimaryRessource { get; set; }
        public int SecondaryRessource { get; set; }
        public int Health { get; set; }
        public IList<Skill> Skills { get; set; } = new List<Skill>();
        public IList<Buff> ActiveBuffs { get; set; } = new List<Buff>();
    }
}