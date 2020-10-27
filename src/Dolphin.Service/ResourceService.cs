using Dolphin.Enum;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Dolphin.Service
{
    public class ResourceService : IResourceService
    {
        private readonly IImageCacheService cacheService;
        private readonly IHandleService handleService; // Todo: By resolution
        private readonly ILogService logService;

        public ResourceService(IImageCacheService cacheService, IHandleService handleService, ILogService logService)
        {
            this.cacheService = cacheService;
            this.logService = logService;
            this.handleService = handleService;
        }

        public Bitmap Load<T>(T enumValue)
        {
            var cachedBitmap = cacheService.Get<T, Bitmap>(enumValue);
            if (cachedBitmap != null)
            {
                logService.AddEntry(this, $"Found Image for {enumValue} in Cache.", LogLevel.Debug);

                return cachedBitmap;
            }

            var path = GetTypeBastedPath(enumValue);
            var bitmap = new Bitmap(path);

            if (bitmap.PixelFormat != PixelFormat.Format24bppRgb)
            {
                bitmap = ImageHelper.To24bppRgbFormat(bitmap);
            }

            cacheService.Add(enumValue, bitmap);
            logService.AddEntry(this, $"Loaded Image from [{path}] for {enumValue} and added to Cache.", LogLevel.Debug);

            return bitmap;
        }

        public Bitmap LoadSkillBitmap(SkillName skillName, bool isMouse)
        {
            var cachedBitmap = cacheService.GetSkillBitmap(skillName, isMouse);
            if (cachedBitmap != null)
            {
                logService.AddEntry(this, $"Found Image for {skillName} in Cache.", LogLevel.Debug);

                return cachedBitmap;
            }

            var path = isMouse ? $"Resource/Skill/Mouse/{skillName}.png" : $"Resource/Skill/{skillName}.png";

            var bitmap = new Bitmap(path);
            if (bitmap.PixelFormat != PixelFormat.Format24bppRgb)
            {
                bitmap = ImageHelper.To24bppRgbFormat(bitmap);
            }

            cacheService.AddSkillBitmap(skillName, isMouse, bitmap);
            logService.AddEntry(this, $"Loaded Image from [{path}] for {skillName} and added to Cache.", LogLevel.Debug);

            return bitmap;
        }

        private string GetTypeBastedPath<T>(T enumValue)
        {
            switch (enumValue)
            {
                case BuffName buffName:
                    return $"Resource/Buff/{buffName}.png";

                case WorldLocation location:
                    return $"Resource/WorldLocation/{location}.png";

                case Window window:
                    return $"Resource/Window/{window}.png";


                case PlayerClass playerClass:
                    return $"Resource/Player/PlayerClass/{playerClass}.png";

                case PlayerHealth health:
                    return $"Resource/Player/PlayerHealth/{health}.png";

                case Enum.PlayerResource resource:
                    return $"Resource/Player/PlayerResource/{resource}.png";

                case ExtraInformation information:
                    return $"Resource/ExtraInformation/{information}.png";

                default:
                    throw new NotImplementedException($"Didnt implement GetTypeBasedKey for type {enumValue.GetType().Name} in ResourceService yet.");
            }
        }
    }
}