using Dolphin.Enum;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Dolphin.Image
{
    public class ResourceService : IResourceService
    {
        private readonly IImageCacheService cacheService;
        private readonly IHandleService handleService;
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

            if (cachedBitmap != null) return cachedBitmap;

            var path = GetTypeBastedPath(enumValue);
            try
            {
                var bitmap = new Bitmap(path);

                if (bitmap.PixelFormat != PixelFormat.Format24bppRgb)
                {
                    bitmap = ImageHelper.To24bppRgbFormat(bitmap);
                }

                cacheService.Add(enumValue, bitmap);
                logService.AddEntry(this, $"Loaded Image from [{path}] for {enumValue} and added to Cache.", LogLevel.Debug);

                return bitmap;
            }
            catch (Exception ex)
            {
                logService.AddEntry(this, $"Couldent load image for {enumValue}", LogLevel.Warning, ex);
            }

            return default;
        }

        public Bitmap LoadSkillBitmap(SkillName skillName, bool isMouse)
        {
            var cachedBitmap = cacheService.GetSkillBitmap(skillName, isMouse);
            if (cachedBitmap != null) return cachedBitmap;

            var path = isMouse ? $"Resource/Skill/Mouse/{skillName}.png" : $"Resource/Skill/{skillName}.png";
            try
            {
                var bitmap = new Bitmap(path);
                if (bitmap.PixelFormat != PixelFormat.Format24bppRgb)
                {
                    bitmap = ImageHelper.To24bppRgbFormat(bitmap);
                }

                cacheService.AddSkillBitmap(skillName, isMouse, bitmap);
                logService.AddEntry(this, $"Loaded Image from [{path}] for {skillName} and added to Cache.", LogLevel.Debug);

                return bitmap;
            }
            catch (Exception ex)
            {
                logService.AddEntry(this, $"Couldent load image for {skillName}", LogLevel.Warning, ex);
            }

            return default;
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