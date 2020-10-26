using System.Drawing;

namespace Dolphin
{
    public static class CommonSize
    {
        #region General

        public static Size InventoryStepSize = new Size { Width = 50, Height = 50 };

        #endregion General

        public static Size HealthbarSize = new Size { Width = 61, Height = 5 };
        public static Size PlayerClassSize = new Size { Width = 15, Height = 11 };
        public static Size ResourceSize = new Size { Height = 146, Width = 3 };
        public static Size SkillbarSkillSize = new Size { Width = 48, Height = 15 }; // 46
        public static Size SkillbarSkillSizeMouse = new Size { Width = 46, Height = 15 };
    }
}