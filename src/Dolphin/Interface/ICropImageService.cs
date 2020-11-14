using Dolphin.Enum;
using System.Drawing;

namespace Dolphin
{
    public interface ICropImageService
    {
        Bitmap CropHealthbar(Bitmap image);

        Bitmap CropPlayerClass(Bitmap image);

        Bitmap CropPrimaryResource(Bitmap image, PlayerClass playerClass);

        Bitmap CropSecondaryResource(Bitmap image, PlayerClass playerClass);

        Bitmap CropSkill(Bitmap image, int index);

        Bitmap CropSkillActive(Bitmap image, int index);

        Bitmap CropWindow(Bitmap image, Window window);

        Bitmap CropWorldLocation(Bitmap image, WorldLocation location);

        Bitmap CropWindowExtraInformation(Bitmap image, ExtraInformation extraInformation);

        Bitmap CropWindowExtraInformation(Bitmap image, Window window);
    }
}