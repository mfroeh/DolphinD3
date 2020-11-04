using System.Collections.Generic;

namespace Dolphin.Enum
{
    public enum ActionName
    {
        None = 0,
        Pause = 1,
        CancelAction = 25,
        CubeConverterSingleSlot = 2,
        CubeConverterDualSlot = 3,
        RightClick = 4,
        LeftClick = 5,
        NormalizeDifficulty = 6,
        SwapArmour = 7,
        TravelAct1 = 8,
        TravelAct2 = 9,
        TravelAct34 = 10,
        TravelAct5 = 11,
        TravelPool = 12,
        OpenGrift = 13,
        UpgradeGem = 14,
        LeaveGame = 15,
        Salvage = 16,
        DropInventory = 17,
        Gamble = 18,
        Reforge = 19,
        Smart_AcceptGriftPopup = 20,
        Smart_StartGame = 21,
        Smart_OpenRift = 22,
        Smart_Gamble = 23,
        Smart_UpgradeGem = 24,
        Smart_OpenGrift = 26
    }

    public static class ActionNameExtensionMethods
    {
        private static IList<ActionName> cancelableActions = new List<ActionName>
        {
            ActionName.CubeConverterDualSlot,
            ActionName.CubeConverterSingleSlot,
            ActionName.UpgradeGem,
            ActionName.Smart_UpgradeGem
        };

        public static bool IsCancelable(this ActionName action)
        {
            return cancelableActions.Contains(action);
        }

        public static bool IsSmartAction(this ActionName action)
        {
            return action.ToString().StartsWith("Smart");
        }
    }
}