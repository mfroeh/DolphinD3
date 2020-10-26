using Dolphin.Enum;
using System.Drawing;

namespace Dolphin.DevTools
{
    public interface ISaveImageService
    {
        Bitmap TakePicture(string processName);

        void SaveHealthbar(Bitmap bitmap);

        void SavePlayerClass(Bitmap bitmap);

        void SavePlayerSkillsMouse(Bitmap bitmap);

        void SavePlayerSkills(Bitmap bitmap);

        void SavePlayerResourcePrimary(Bitmap bitmap);

        void SavePlayerResourcePrimaryDemonHunter(Bitmap bitmap);

        void SavePlayerResourceSecondaryDemonHunter(Bitmap bitmap);
    }
}
