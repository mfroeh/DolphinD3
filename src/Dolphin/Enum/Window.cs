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
        public static IEnumerable<ActionName> AssociatedActionNames(this Window window)
        {
            switch (window)
            {
                case Window.Kadala:
                    yield return ActionName.Smart_Gamble;
                    break;

                case Window.AcceptGrift:
                    yield return ActionName.Smart_AcceptGriftPopup;
                    break;

                case Window.Urshi:
                    yield return ActionName.Smart_UpgradeGem;
                    break;

                case Window.StartGame:
                    yield return ActionName.Smart_StartGame;
                    break;

                case Window.Obelisk:
                    yield return ActionName.Smart_OpenRiftGrift;
                    break;

                default:
                    yield break;
            }
        }
    }
}