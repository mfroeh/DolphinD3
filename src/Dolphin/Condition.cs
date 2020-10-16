using System;

namespace Dolphin
{
    public class Condition
    {
        public Func<Player, World, bool> Punishment { get; set; } = ConditionFunction.PunishmentFunction;
        public Func<Player, World, bool> Companion { get; set; } = ConditionFunction.CompanionFunction;
        public Func<Player, World, bool> Vengance { get; set; } = ConditionFunction.VenganceFunction;
        public Func<Player, World, bool> ShadowPower { get; set; } = ConditionFunction.ShadowPowerFunction;
    }
}