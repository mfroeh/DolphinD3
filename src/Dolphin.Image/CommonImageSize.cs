using System.Drawing;

namespace Dolphin.Image
{
    /// <summary>
    /// Common Sizes for Image Searching in 2560 x 1440
    /// </summary>
    internal class CommonImageSize
    {
        #region General

        public static Size HealthbarSize = new Size { Width = 81, Height = 5 };
        public static Size PlayerClassSize = new Size { Width = 15, Height = 15 };
        public static Size ResourceSize = new Size { Height = 195, Width = 3 };
        public static Size SkillActive = new Size { Height = 10, Width = 4 }; // TODO
        public static Size SkillbarSkillSize = new Size { Width = 15, Height = 15 };

        #endregion General

        // TODO: STILL 1920 x 1080

        #region World

        public static Size LocationGriftText = new Size { Width = 39, Height = 11 };
        public static Size LocationMenu = new Size { Width = 60, Height = 28 };
        public static Size LocationRiftLevel = new Size { Width = 15, Height = 14 };
        public static Size WorldNephalemRift = new Size { Width = 25, Height = 14 };
        public static Size WorldRiftSpeakToOrek = new Size { Width = 35, Height = 14 };

        #endregion World

        #region Window

        public static Size WindowKadala = new Size { Width = 46, Height = 20 };
        public static Size WindowObelisk = new Size { Width = 55, Height = 9 };
        public static Size WindowStartGame = new Size { Width = 67, Height = 8 };
        public static Size WindowUrhsiGemUp = new Size { Width = 29, Height = 12 };
        public static Size WindowUrshi = new Size { Width = 46, Height = 15 };

        #endregion Window
    }
}