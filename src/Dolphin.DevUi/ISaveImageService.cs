using Dolphin.Enum;
using System.Drawing;

namespace Dolphin.DevUi
{
    public interface ISaveImageService
    {
        void SaveHealthbar(Bitmap bitmap);

        void SavePlayerClass(Bitmap bitmap);

        void SavePlayerResourcePrimary(Bitmap bitmap);

        void SavePlayerResourcePrimaryDemonHunter(Bitmap bitmap);

        void SavePlayerResourceSecondaryDemonHunter(Bitmap bitmap);

        void SavePlayerSkills(Bitmap bitmap);

        void SavePlayerSkillsActive(Bitmap bitmap);

        void SavePlayerSkillsMouse(Bitmap bitmap);

        void SaveUrshiGemUp(Bitmap bitmap, int gemUps);

        void SaveWindow(Bitmap bitmap, Window window);

        void SaveWorldLocation(Bitmap bitmap, WorldLocation location);
    }
}