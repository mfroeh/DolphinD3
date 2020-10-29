using Dolphin.Enum;
using System.Drawing;

namespace Dolphin
{
    public interface ICropImageService
    {
        public Bitmap CropHealthbar(Bitmap image);

        public Bitmap CropPlayerClass(Bitmap image);

        public Bitmap CropPrimaryResource(Bitmap image, PlayerClass playerClass);

        public Bitmap CropSecondaryResource(Bitmap image, PlayerClass playerClass);

        public Bitmap CropSkillActive(Bitmap image, int index);

        public Bitmap CropSkill(Bitmap image, int index);

        public Bitmap CropUrshiGemUp(Bitmap image);

        public Bitmap CropWindow(Bitmap image, Window window);

        public Bitmap CropWorldLocation(Bitmap image, WorldLocation location);
    }
}