using System.Collections.Generic;

namespace Dolphin.Enum
{
    public enum Window
    {
        None = 0,
        AcceptGrift = 1,
        StartGame = 2,
        Urshi = 3,
        Kadala = 4,
        Obelisk = 5
    }

    public static class WindowExtensionMethods
    {
        public static IEnumerable<SmartActionName> AssociatedSmartActions(this Window window)
        {
            switch (window)
            {
                case Window.Kadala:
                    yield return SmartActionName.Gamble;
                    break;

                case Window.AcceptGrift:
                    yield return SmartActionName.AcceptGriftPopup;
                    break;

                case Window.Urshi:
                    yield return SmartActionName.UpgradeGem;
                    break;

                case Window.StartGame:
                    yield return SmartActionName.StartGame;
                    break;

                case Window.Obelisk:
                    yield return SmartActionName.OpenRiftGrift;
                    break;

                default:
                    yield break;
            }
        }
    }
}