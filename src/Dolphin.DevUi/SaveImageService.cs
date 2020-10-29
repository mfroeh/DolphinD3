using Dolphin.Enum;
using System;
using System.Drawing;
using System.IO;

namespace Dolphin.DevUi
{
    public class SaveImageService : ISaveImageService
    {
        private readonly ICropImageService cropWindowService;

        public SaveImageService(ICropImageService cropImageService)
        {
            this.cropWindowService = cropImageService;
            Directory.CreateDirectory(Properties.Settings.Default.OutputDirectory);
        }

        private string CurrentTime => DateTime.Now.ToString("HH_mm_ss");

        private string OutputDirectory => Properties.Settings.Default.OutputDirectory;

        private string OutputDirectoryDated => $"{OutputDirectory}/{CurrentTime}";

        public void SaveHealthbar(Bitmap bitmap)
        {
            var image = cropWindowService.CropHealthbar(bitmap);

            Directory.CreateDirectory($"{OutputDirectoryDated}/Player/PlayerHealth");

            image.Save($"{OutputDirectoryDated}/Player/PlayerHealth/H_FILLIN.png");
        }

        public void SavePlayerClass(Bitmap bitmap)
        {
            var image = cropWindowService.CropPlayerClass(bitmap);

            Directory.CreateDirectory($"{OutputDirectoryDated}/Player/PlayerClass");

            image.Save($"{OutputDirectoryDated}/Player/PlayerClass/PlayerClass.png");
        }

        public void SavePlayerResourcePrimary(Bitmap bitmap)
        {
            var image = cropWindowService.CropPrimaryResource(bitmap, PlayerClass.NecromancerFemale);

            Directory.CreateDirectory($"{OutputDirectoryDated}/Player/PlayerResource");

            image.Save($"{OutputDirectoryDated}/Player/PlayerResource/PrimaryFILLIN_FILLIN.png");
        }

        public void SavePlayerResourcePrimaryDemonHunter(Bitmap bitmap)
        {
            var image = cropWindowService.CropPrimaryResource(bitmap, PlayerClass.DemonHunterFemale);

            Directory.CreateDirectory($"{OutputDirectoryDated}/Player/PlayerResource");

            image.Save($"{OutputDirectoryDated}/Player/PlayerResource/PrimaryHatred_FILLIN.png");
        }

        public void SavePlayerResourceSecondaryDemonHunter(Bitmap bitmap)
        {
            var image = cropWindowService.CropSecondaryResource(bitmap, PlayerClass.DemonHunterFemale);

            Directory.CreateDirectory($"{OutputDirectoryDated}/Player/PlayerResource");

            image.Save($"{OutputDirectoryDated}/Player/PlayerResource/SecondaryDiscipline_FILLIN.png");
        }

        public void SavePlayerSkills(Bitmap bitmap)
        {
            Directory.CreateDirectory($"{OutputDirectoryDated}/Skill");

            for (int i = 0; i < 4; i++)
            {
                var skill = cropWindowService.CropSkill(bitmap, i);

                skill.Save($"{OutputDirectoryDated}/Skill/Skill{i}.png");
            }
        }

        public void SavePlayerSkillsActive(Bitmap bitmap)
        {
            Directory.CreateDirectory($"{OutputDirectoryDated}/Skill");

            for (int i = 0; i < 6; i++)
            {
                var skill = cropWindowService.CropSkillActive(bitmap, i);

                skill.Save($"{OutputDirectoryDated}/Skill{i}_Active.png");
            }
        }

        public void SavePlayerSkillsMouse(Bitmap bitmap)
        {
            Directory.CreateDirectory($"{OutputDirectoryDated}/Skill/Mouse");

            for (int i = 4; i < 6; i++)
            {
                var skill = cropWindowService.CropSkill(bitmap, i);

                skill.Save($"{OutputDirectoryDated}/Skill/Mouse/Skill{i}.png");
            }
        }

        public void SaveUrshiGemUp(Bitmap bitmap, int gemUp)
        {
            Directory.CreateDirectory($"{OutputDirectoryDated}/ExtraInformation");

            var image = cropWindowService.CropUrshiGemUp(bitmap);

            image.Save($"{OutputDirectoryDated}/ExtraInformation/Urshi_{gemUp}.png");
        }

        public void SaveWindow(Bitmap bitmap, Window window)
        {
            Directory.CreateDirectory($"{OutputDirectoryDated}/Window");

            var image = cropWindowService.CropWindow(bitmap, window);

            image.Save($"{OutputDirectoryDated}/Window/{window}.png");
        }

        public void SaveWorldLocation(Bitmap bitmap, WorldLocation location)
        {
            Directory.CreateDirectory($"{OutputDirectoryDated}/WorldLocation");

            var image = cropWindowService.CropWorldLocation(bitmap, location);

            image.Save($"{OutputDirectoryDated}/WorldLocation/{location}.png");
        }
    }
}