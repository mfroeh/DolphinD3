using System.Drawing;

namespace Dolphin
{
    public static class CommonSize
    {
        #region General

        public static Size HealthbarSize = new Size { Width = 61, Height = 5 };
        public static Size InventoryStepSize = new Size { Width = 50, Height = 50 };

        #endregion General
        public static Size PlayerClassSize = new Size { Width = 15, Height = 11 };
        public static Size ResourceSize = new Size { Height = 146, Width = 3 };
        public static Size SkillbarSkillSize = new Size { Width = 48, Height = 15 };
        public static Size SkillbarSkillSizeMouse = new Size { Width = 46, Height = 15 };

        #region World
        public static Size WorldNephalemRift = new Size { Width = 25, Height = 14 };
        public static Size WorldRiftSpeakToOrek = new Size { Width = 35, Height = 14 };

        public static Size LocationGriftText = new Size { Width = 39, Height = 11 };
        public static Size LocationRiftLevel = new Size { Width = 15, Height = 14 };
        public static Size LocationMenu = new Size { Width = 60, Height = 28 };
        #endregion

        #region Window
        public static Size WindowStartGame = new Size { Width = 67, Height = 8 };
        public static Size WindowUrshi = new Size { Width = 46, Height = 15 };
        public static Size WindowUrhsiGemUp = new Size { Width = 29, Height = 12 };
        public static Size WindowKadala = new Size { Width = 46, Height = 20 };

        public static Size WindowObelisk = new Size { Width = 55, Height = 9 };
        #endregion
    }
}