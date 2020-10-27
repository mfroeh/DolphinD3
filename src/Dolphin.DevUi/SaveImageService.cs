using Dolphin.Enum;
using System;
using System.Drawing;
using System.IO;

namespace Dolphin.DevUi
{
    public class SaveImageService : ISaveImageService
    {
        private readonly ICropImageService captureWindowService;

        public SaveImageService(ICropImageService cropImageService)
        {
            this.captureWindowService = cropImageService;

            Directory.CreateDirectory(Properties.Settings.Default.OutputDirectory);
        }

        public void SaveHealthbar(Bitmap bitmap)
        {
            var image = captureWindowService.CropHealth(bitmap);

            Directory.CreateDirectory($"{Properties.Settings.Default.OutputDirectory}/Player/PlayerHealth");

            image.Save($"{Properties.Settings.Default.OutputDirectory}/Player/PlayerHealth/H_FILLIN.png");
        }

        public void SavePlayerClass(Bitmap bitmap)
        {
            var image = captureWindowService.CropPlayerClass(bitmap);

            Directory.CreateDirectory($"{Properties.Settings.Default.OutputDirectory}/Player/PlayerClass");

            image.Save($"{Properties.Settings.Default.OutputDirectory}/Player/PlayerClass/PlayerClass.png");
        }

        public void SavePlayerResourcePrimary(Bitmap bitmap)
        {
            var image = captureWindowService.CropPrimaryResource(bitmap, PlayerClass.NecromancerFemale);

            Directory.CreateDirectory($"{Properties.Settings.Default.OutputDirectory}/Player/PlayerResource");

            image.Save($"{Properties.Settings.Default.OutputDirectory}/Player/PlayerResource/PrimaryResource_FILLIN.png");
        }

        public void SavePlayerResourcePrimaryDemonHunter(Bitmap bitmap)
        {
            var image = captureWindowService.CropPrimaryResource(bitmap, PlayerClass.DemonHunterFemale);

            Directory.CreateDirectory($"{Properties.Settings.Default.OutputDirectory}/Player/PlayerResource");

            image.Save($"{Properties.Settings.Default.OutputDirectory}/Player/PlayerResource/PrimaryResourceDemonHunter_FILLIN.png");
        }

        public void SavePlayerResourceSecondaryDemonHunter(Bitmap bitmap)
        {
            var image = captureWindowService.CropSecondaryResource(bitmap, PlayerClass.DemonHunterFemale);

            Directory.CreateDirectory($"{Properties.Settings.Default.OutputDirectory}/Player/PlayerResource");

            image.Save($"{Properties.Settings.Default.OutputDirectory}/Player/PlayerResource/SecondaryResourceDemonHunter_FILLIN.png");
        }

        public void SavePlayerSkills(Bitmap bitmap)
        {
            Directory.CreateDirectory($"{Properties.Settings.Default.OutputDirectory}/Skill");

            for (int i = 0; i < 4; i++)
            {
                var skill = captureWindowService.CropSkillbar(bitmap, i);

                skill.Save($"{Properties.Settings.Default.OutputDirectory}/Skill/Skill{i}.png");
            }
        }

        public void SavePlayerSkillsActive(Bitmap bitmap)
        {
            for (int i = 0; i < 6; i++)
            {
                var skill = captureWindowService.CropSkillActive(bitmap, i);

                skill.Save($"{Properties.Settings.Default.OutputDirectory}Skill{i}_Active.png");
            }
        }

        public void SavePlayerSkillsMouse(Bitmap bitmap)
        {
            Directory.CreateDirectory($"{Properties.Settings.Default.OutputDirectory}/Skill/Mouse");

            for (int i = 4; i < 6; i++)
            {
                var skill = captureWindowService.CropSkillbar(bitmap, i);

                skill.Save($"{Properties.Settings.Default.OutputDirectory}/Skill/Mouse/Skill{i}.png");
            }
        }

        public void SaveUrshiGemUp(Bitmap bitmap, int gemUp)
        {
            Directory.CreateDirectory($"{Properties.Settings.Default.OutputDirectory}/ExtraInformation");

            var image = captureWindowService.CropUrshiGemUp(bitmap);

            image.Save($"{Properties.Settings.Default.OutputDirectory}/ExtraInformation/Urshi_{gemUp}.png");
        }

        public void SaveWindow(Bitmap bitmap, Window window)
        {
            Directory.CreateDirectory($"{Properties.Settings.Default.OutputDirectory}/Window");

            var image = captureWindowService.CropWindow(bitmap, window);

            image.Save($"{Properties.Settings.Default.OutputDirectory}/Window/{window}.png");
        }

        public void SaveWorldLocation(Bitmap bitmap, WorldLocation location)
        {
            Directory.CreateDirectory($"{Properties.Settings.Default.OutputDirectory}/WorldLocation");

            var image = captureWindowService.CropWorldLocation(bitmap, location);

            image.Save($"{Properties.Settings.Default.OutputDirectory}/WorldLocation/{location}.png");
        }
    }
}