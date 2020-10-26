using Dolphin.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dolphin.DevTools
{
    public class SaveImageService : ISaveImageService
    {

        private readonly ICaptureWindowService captureWindowService;
        private readonly string outputDirectoryBasePath;
        private string extractedFilesDirectory;

        public SaveImageService(ICaptureWindowService captureWindowService, string outputDirectoryBasePath = "ExtractedResource")
        {
            this.captureWindowService = captureWindowService;
            this.outputDirectoryBasePath = outputDirectoryBasePath;

            Directory.CreateDirectory(outputDirectoryBasePath);
        }

        public void SaveHealthbar(Bitmap bitmap)
        {
            var image = captureWindowService.CropHealth(bitmap);

            Directory.CreateDirectory($"{extractedFilesDirectory}/Player/PlayerHealth");

            image.Save($"{extractedFilesDirectory}/Player/PlayerHealth/H_FILLIN.png");
        }

        public void SavePlayerClass(Bitmap bitmap)
        {
            var image = captureWindowService.CropPlayerClass(bitmap);

            Directory.CreateDirectory($"{extractedFilesDirectory}/Player/PlayerClass");

            image.Save($"{extractedFilesDirectory}/Player/PlayerClass/PlayerClass.png");
        }

        public void SavePlayerResourcePrimary(Bitmap bitmap)
        {
            var image = captureWindowService.CropPrimaryResource(bitmap, PlayerClass.NecromancerFemale);

            Directory.CreateDirectory($"{extractedFilesDirectory}/Player/PlayerResource");

            image.Save($"{extractedFilesDirectory}/Player/PlayerResource/PrimaryResource_FILLIN.png");
        }

        public void SavePlayerResourcePrimaryDemonHunter(Bitmap bitmap)
        {
            var image = captureWindowService.CropPrimaryResource(bitmap, PlayerClass.DemonHunterFemale);

            Directory.CreateDirectory($"{extractedFilesDirectory}/Player/PlayerResource");

            image.Save($"{extractedFilesDirectory}/Player/PlayerResource/PrimaryResourceDemonHunter_FILLIN.png");
        }

        public void SavePlayerResourceSecondaryDemonHunter(Bitmap bitmap)
        {
            var image = captureWindowService.CropSecondaryResource(bitmap, PlayerClass.DemonHunterFemale);

            Directory.CreateDirectory($"{extractedFilesDirectory}/Player/PlayerResource");

            image.Save($"{extractedFilesDirectory}/Player/PlayerResource/SecondaryResourceDemonHunter_FILLIN.png");
        }

        public void SavePlayerSkills(Bitmap bitmap)
        {
            Directory.CreateDirectory($"{extractedFilesDirectory}/Skill");

            for (int i = 0; i < 4; i++)
            {
                var skill = captureWindowService.CropSkillbar(bitmap, i);

                skill.Save($"{extractedFilesDirectory}/Skill/Skill{i}.png");
            }
        }

        public void SavePlayerSkillsMouse(Bitmap bitmap)
        {
            Directory.CreateDirectory($"{extractedFilesDirectory}/Skill/Mouse");

            for (int i = 4; i < 6; i++)
            {
                var skill = captureWindowService.CropSkillbar(bitmap, i);

                skill.Save($"{extractedFilesDirectory}/Skill/Mouse/Skill{i}.png");
            }
        }

        public Bitmap TakePicture(string processName)
        {
            var time = DateTime.Now.ToString("hh_mm_ss");
            extractedFilesDirectory = $"{outputDirectoryBasePath}/{time}";

            Directory.CreateDirectory(extractedFilesDirectory);

            return captureWindowService.CaptureWindow(processName);
        }
    }
}
