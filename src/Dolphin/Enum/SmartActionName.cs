namespace Dolphin.Enum
{
    public enum SmartActionName
    {
        None = 0,
        AcceptGriftPopup = 1,
        StartGame = 2,
        OpenRiftGrift = 3,
        Gamble = 4,
        UpgradeGem = 5
    }

    public static class SmartActionExtensionMethods
    {
        public static bool IsCancelable(this SmartActionName action)
        {
            return action == SmartActionName.UpgradeGem;
        }
    }
}