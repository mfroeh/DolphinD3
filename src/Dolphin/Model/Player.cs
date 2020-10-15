using Dolphin.Enum;
using System.Collections.Generic;

namespace Dolphin
{
    public class Player
    {
        public PlayerClass Class { get; set; }
        public int HealthPercentage { get; set; }
        public int PrimaryRessourcePercentage { get; set; }
        public int SecondaryRessourcePercentage { get; set; }
        public IList<Skill> Skills { get; set; } = new List<Skill>();
        public IList<Buff> ActiveBuffs { get; set; } = new List<Buff>();
    }
}