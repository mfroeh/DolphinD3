using System;

namespace Dolphin
{
    public static class Condition
    {
        public static Func<Player, World, bool> Punishment = ConditionFunction.PunishmentFunction;
        public static Func<Player, World, bool> Companion = ConditionFunction.CompanionFunction;
        public static Func<Player, World, bool> Vengance = ConditionFunction.VenganceFunction;
        public static Func<Player, World, bool> ShadowPower = ConditionFunction.ShadowPowerFunction;
    }
}