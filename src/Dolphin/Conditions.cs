using System;

namespace Dolphin
{
    public class Conditions
    {
        public Func<Player, World, bool> Punishment { get; set; } = ConditionFunction.PunishmentFunction;
    }
}