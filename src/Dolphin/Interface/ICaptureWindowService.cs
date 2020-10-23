using Dolphin.Enum;
using System;
using System.Drawing;

namespace Dolphin
{
    public interface ICaptureWindowService
    {
        Bitmap CaptureWindow(string processName);

        Bitmap CaptureWindow(IntPtr hwnd);

        public Bitmap CropHealth(Bitmap image);

        public Bitmap CropPlayerClass(Bitmap image);

        public Bitmap CropPrimaryResource(Bitmap image, PlayerClass playerClass);

        public Bitmap CropSecondaryResource(Bitmap image, PlayerClass playerClass);

        public Bitmap CropSkillbar(Bitmap image, int index);

        public Bitmap CropWindow(Bitmap image, Window window);

        public Bitmap CropWorldLocation(Bitmap image, WorldLocation location);
    }
}