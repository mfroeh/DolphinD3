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

        #region World

        public static Size LocationGriftText = new Size { Width = 50, Height = 10 };  // TODO: STILL 1920 x 1080
        public static Size LocationMenu = new Size { Width = 32, Height = 42 };
        public static Size LocationRiftLevel = new Size { Width = 15, Height = 14 };  // TODO: STILL 1920 x 1080
        public static Size WorldNephalemRift = new Size { Width = 25, Height = 14 };  // TODO: STILL 1920 x 1080
        public static Size WorldRiftSpeakToOrek = new Size { Width = 35, Height = 14 };  // TODO: STILL 1920 x 1080

        #endregion World

        #region Window

        public static Size WindowKadala = new Size { Width = 30, Height = 30 };
        public static Size WindowObelisk = new Size { Width = 30, Height = 30 };
        public static Size WindowStartGame = new Size { Width = 180, Height = 10 };
        public static Size WindowUrhsiGemUp = new Size { Width = 48, Height = 20 };
        public static Size WindowUrshi = new Size { Width = 30, Height = 30 };

        public static Size LocationLoadingScreen = new Size { Width = 50, Height = 23 };

        #endregion Window
    }
}