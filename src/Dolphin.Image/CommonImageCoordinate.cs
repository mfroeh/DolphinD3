using System.Drawing;

namespace Dolphin.Image
{
    /// <summary>
    /// Common Coordinates for Image Searching in 2560 x 1440
    /// </summary>
    internal class CommonImageCoordinate
    {
        #region Player

        public static Point HealthBarTopLeft = new Point { X = 43, Y = 163 };
        public static Point PlayerClassTopLeft = new Point { X = 82, Y = 109 };
        public static Point PrimaryResourceDemonHunterTopLeft = new Point { X = 1820, Y = 1225 };
        public static Point PrimaryResourceTopLeft = new Point { X = 1840, Y = 1225 };
        public static Point SecondaryResourceDemonHunterTopLeft = new Point { X = 1860, Y = 1225 };

        #endregion Player

        #region Skill

        public static Point SkillbarSkill0Active = new Point { X = 842, Y = 1345 };
        public static Point SkillbarSkill0TopLeft = new Point { X = 846, Y = 1340 };
        public static Point SkillbarSkill1Active = new Point { X = 931, Y = 1345 };
        public static Point SkillbarSkill1TopLeft = new Point { X = 935, Y = 1340 };
        public static Point SkillbarSkill2Active = new Point { X = 1020, Y = 1345 };
        public static Point SkillbarSkill2TopLeft = new Point { X = 1024, Y = 1340 };
        public static Point SkillbarSkill3Active = new Point { X = 1109, Y = 1345 };
        public static Point SkillbarSkill3TopLeft = new Point { X = 1113, Y = 1340 };
        public static Point SkillbarSkill4Active = new Point { X = 1201, Y = 1345 };
        public static Point SkillbarSkill4TopLeft = new Point { X = 1208, Y = 1342 };
        public static Point SkillbarSkill5Active = new Point { X = 1290, Y = 1345 };
        public static Point SkillbarSkill5TopLeft = new Point { X = 1295, Y = 1342 };

        public static Point SkillMouseOffset = new Point(22, 12);
        public static Point SkillOffset = new Point(25, 15);

        #endregion Skill

        #region WorldLocation

        public static Point LocationGrift = new Point { X = 2320, Y = 40 };  // TODO: Doesnt work
        public static Point LocationMenuSymbol = new Point { X = 2000, Y = 1328 };
        public static Point LocationRiftLevel = new Point { X = 1884, Y = 7 };  // TODO: STILL 1920 x 1080
        public static Point LocationRiftSpeakToOrek = new Point { X = 1680, Y = 428 };  // TODO: STILL 1920 x 1080

        #endregion WorldLocation

        #region Window

        public static Point LocationLoadingScreen = new Point(1260, 722);
        public static Point WindowAcceptGrift = new Point(0, 0); // TODO
        public static Point WindowAcceptGriftEmpowered = new Point(0, 0); // TODO
        public static Point WindowKadala = new Point { X = 330, Y = 87 };
        public static Point WindowObelisk = new Point { X = 330, Y = 87 };
        public static Point WindowObeliskEmpowered = new Point { X = 290, Y = 820 };
        public static Point WindowStartGame = new Point { X = 226, Y = 679 };
        public static Point WindowUrshi = new Point { X = 330, Y = 87 };
        public static Point WindowUrshiGemUp = new Point { X = 389, Y = 722 };

        #endregion Window
    }
}