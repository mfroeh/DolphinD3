using Dolphin.Enum;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Dolphin.Service
{
    public class ResourceService : IResourceService
    {
        private readonly ICacheService cacheService;
        private readonly ILogService logService;
        private readonly IHandleService handleService;

        public ResourceService(ICacheService cacheService, IHandleService handleService, ILogService logService)
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
                logService.AddEntry(this, $"Found Image for {enumValue} in Cache.");

                return cachedBitmap;
            }

            var path = GetTypeBastedPath(enumValue);
            var bitmap = new Bitmap(path);

            if (bitmap.PixelFormat != PixelFormat.Format24bppRgb)
            {
                bitmap = ImageHelper.To24bppRgbFormat(bitmap);
            }

            cacheService.Add(enumValue, bitmap);
            logService.AddEntry(this, $"Loaded Image from [{path}] for {enumValue} and added to Cache.");

            return bitmap;
        }

        private string GetTypeBastedPath<T>(T enumValue)
        {
            switch (enumValue)
            {
                case BuffName buffName:
                    return $"Resource/Buff/BuffName_{buffName}.png";

                case SkillName skillName:
                    return $"Resource/Skill/SkillName_{skillName}.png";

                case WorldLocation location:
                    return $"Resource/Location/WorldLocation_{location}.png";

                case PlayerClass playerClass:
                    return $"Resource/Player/PlayerClass/PlayerClass_{playerClass}.png";

                case PlayerHealth health:
                    return $"Resource/Player/PlayerHealth/PlayerHealth_{health}.png";

                case Enum.PlayerResource resource:
                    return $"Resource/Player/PlayerResource/PlayerResource_{resource}.png";

                default:
                    throw new NotImplementedException($"Didnt implement GetTypeBasedKey for type {enumValue.GetType().Name} in CacheService yet.");
            }
        }
    }
}