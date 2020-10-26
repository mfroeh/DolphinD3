using System.Drawing;

namespace Dolphin
{
    /// <summary>
    /// Common Coordinates in 1920 x 1080
    /// </summary>
    public static class CommonCoordinate
    {
        #region General

        public static Point BlacksmithAnvil = new Point { X = 165, Y = 295 };
        public static Point BlacksmithMenu = new Point { X = 517, Y = 480 };
        public static Point CubeBackwards = new Point { X = 580, Y = 850 };
        public static Point CubeFill = new Point { X = 710, Y = 840 };
        public static Point CubeForwards = new Point { X = 850, Y = 850 };
        public static Point CubeTransmute = new Point { X = 250, Y = 830 };
        public static Point EscapeLeave = new Point { X = 230, Y = 475 };
        public static Point EscapeLowerDifficulty = new Point { X = 1700, Y = 400 };
        public static Point InventoryTopLeftSpot = new Point { X = 1425, Y = 580 }; // CubeTopLeftInventorySlot
        public static Point InventoryTopRightSpot = new Point { X = 1875, Y = 580 }; // BlacksmithTopRightInventorySlot // Y = 585
        public static Point PortalAccept = new Point { X = 260, Y = 850 };
        public static Point PortalGriftButton = new Point { X = 270, Y = 480 };
        public static Point PortalRiftButton = new Point { X = 270, Y = 300 };
        public static Point UrshiFirstGem = new Point { X = 100, Y = 640 };
        public static Point UrshiUpgrade = new Point { X = 280, Y = 550 };

        #endregion General

        #region Map

        public static Point MapAct1 = new Point { X = 740, Y = 620 };
        public static Point MapAct1Town = new Point { X = 1020, Y = 490 };
        public static Point MapAct2 = new Point { X = 1090, Y = 525 };
        public static Point MapAct2Town = new Point { X = 1040, Y = 780 };
        public static Point MapAct3 = new Point { X = 710, Y = 400 };
        public static Point MapAct3Town = new Point { X = 510, Y = 485 };
        public static Point MapAct4 = new Point { X = 1450, Y = 370 };
        public static Point MapAct4Town = new Point { X = 515, Y = 745 };
        public static Point MapAct5 = new Point { X = 590, Y = 550 };
        public static Point MapAct5Town = new Point { X = 1170, Y = 625 };
        public static Point MapBackwards = new Point { X = 895, Y = 125 };
        public static Point MapBrideOfKorsikk = new Point(800, 460);
        public static Point MapCemetryOfTheForsaken = new Point(630, 370);
        public static Point MapDrownedTemple = new Point(580, 250);
        public static Point MapFieldsOfMisery = new Point(450, 320);
        public static Point MapHowlingPlateau = new Point(880, 560);
        public static Point MapLeoricsManor = new Point(580, 585);
        public static Point MapLowerRealmOfInfernalFate = new Point(1480, 300);
        public static Point MapPandemoniumFortressLevel1 = new Point(800, 325);
        public static Point MapRakkisCrossing = new Point(870, 300);
        public static Point MapRealmOfFracturedFate = new Point(560, 510);
        public static Point MapRoadToAlcarnus = new Point(1290, 600);
        public static Point MapSouthernHighlands = new Point(810, 795);
        public static Point MapTheBattlefields = new Point(630, 330);
        public static Point MapTheWeepingHollow = new Point(690, 485);
        public static Point MapTowerOfTheCursedLevel1 = new Point(1320, 610);
        public static Point MapTowerOfTheDamnedLevel1 = new Point(1320, 340);

        #endregion Map

        #region Kadala

        public static Point KadalaItemLeft1 = new Point { X = 70, Y = 210 };
        public static Point KadalaItemLeft2 = new Point { X = 70, Y = 310 };
        public static Point KadalaItemLeft3 = new Point { X = 70, Y = 410 };
        public static Point KadalaItemLeft4 = new Point { X = 70, Y = 510 };
        public static Point KadalaItemLeft5 = new Point { X = 70, Y = 610 };
        public static Point KadalaItemRight1 = new Point { X = 290, Y = 210 };
        public static Point KadalaItemRight2 = new Point { X = 290, Y = 310 };
        public static Point KadalaItemRight3 = new Point { X = 290, Y = 410 };
        public static Point KadalaItemRight4 = new Point { X = 290, Y = 510 };
        public static Point KadalaItemRight5 = new Point { X = 290, Y = 610 };
        public static Point KadalaTab1 = new Point { X = 515, Y = 220 };
        public static Point KadalaTab2 = new Point { X = 515, Y = 350 };
        public static Point KadalaTab3 = new Point { X = 515, Y = 480 };

        #endregion Kadala

        #region Player

        public static Point HealthBarTopLeft = new Point { X = 32, Y = 122 };
        public static Point PlayerClassTopLeft = new Point { X = 59, Y = 80 };
        public static Point PrimaryResourceDemonHunterTopLeft = new Point { X = 1830, Y = 1230 }; // 1440p
        public static Point PrimaryResourceTopLeft = new Point { X = 1380, Y = 917 };
        public static Point SecondaryResourceDemonHunterTopLeft = new Point { X = 1850, Y = 1230 };  // 1440p

        #endregion Player

        #region Skill

        public static Point SkillbarSkill0TopLeft = new Point { X = 635, Y = 1005 };
        public static Point SkillbarSkill1TopLeft = new Point { X = 701, Y = 1005 };
        public static Point SkillbarSkill2TopLeft = new Point { X = 768, Y = 1005 };
        public static Point SkillbarSkill3TopLeft = new Point { X = 835, Y = 1005 };
        public static Point SkillbarSkill4TopLeft = new Point { X = 905, Y = 1006 };
        public static Point SkillbarSkill5TopLeft = new Point { X = 971, Y = 1006 };

        #endregion Skill
    }
}