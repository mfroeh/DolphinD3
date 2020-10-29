using System.Drawing;

namespace Dolphin.Image
{
    /// <summary>
    /// Common Coordinates for Image Searching in 2560 x 1440
    /// </summary>
    internal class CommonImageCoordinate
    {
        #region Player

        public static Point HealthBarTopLeft = new Point { X = 32, Y = 122 };
        public static Point PlayerClassTopLeft = new Point { X = 59, Y = 80 };
        public static Point PrimaryResourceDemonHunterTopLeft = new Point { X = 1368, Y = 919 };
        public static Point PrimaryResourceTopLeft = new Point { X = 1380, Y = 917 };
        public static Point SecondaryResourceDemonHunterTopLeft = new Point { X = 1388, Y = 919 };

        #endregion Player

        #region Skill

        public static Point SkillbarSkill0TopLeft = new Point { X = 635, Y = 1005 };
        public static Point SkillbarSkill1TopLeft = new Point { X = 701, Y = 1005 };
        public static Point SkillbarSkill2TopLeft = new Point { X = 768, Y = 1005 };
        public static Point SkillbarSkill3TopLeft = new Point { X = 835, Y = 1005 };
        public static Point SkillbarSkill4TopLeft = new Point { X = 905, Y = 1006 };
        public static Point SkillbarSkill5TopLeft = new Point { X = 971, Y = 1006 };

        #endregion Skill

        #region WorldLocation

        public static Point LocationGrift = new Point { X = 1738, Y = 31 };
        public static Point LocationMenuSymbol = new Point { X = 1481, Y = 985 };
        public static Point LocationRiftLevel = new Point { X = 1884, Y = 7 };
        public static Point LocationRiftSpeakToOrek = new Point { X = 1680, Y = 428 };

        #endregion WorldLocation

        #region Window

        public static Point WindowKadala = new Point { X = 243, Y = 47 };
        public static Point WindowObelisk = new Point { X = 298, Y = 128 };
        public static Point WindowStartGame = new Point { X = 169, Y = 508 };
        public static Point WindowUrshi = new Point { X = 185, Y = 127 };
        public static Point WindowUrshiGemUp = new Point { X = 295, Y = 542 };

        #endregion Window
    }
}