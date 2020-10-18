using Dolphin.Enum;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace Dolphin.Service
{
    public class ResourceService : IResourceService
    {
        private readonly ICacheService cacheService;
        private readonly ILogService logService;

        public ResourceService(ICacheService cacheService, ILogService logService)
        {
            this.cacheService = cacheService;
            this.logService = logService;
        }

        public async Task<Bitmap> Load<T>(T enumValue)
        {
            var cachedBitmap = await cacheService.Get<T, Bitmap>(enumValue);
            if (cachedBitmap != null)
            {
                logService.AddEntry(this, $"Found Image for {(T)enumValue} in Cache.");
                return cachedBitmap;
            }

            var path = GetTypeBasedPath(enumValue);
            var bitmap = new Bitmap(path);

            if (bitmap.PixelFormat != PixelFormat.Format24bppRgb)
                bitmap = await ImageHelper.To24bppRgbFormat(bitmap);

            await cacheService.Add(enumValue, bitmap);
            logService.AddEntry(this, $"Loaded Image from [{path}] for {(T)enumValue} and added to Cache.");

            return bitmap;
        }

        private string GetTypeBasedPath<T>(T enumValue)
        {
            if (enumValue is BuffName buffName)
                return $"Resource/Buff/BuffName_{buffName}.png";
            else if (enumValue is SkillName skillName)
                return $"Resource/Skill/SkillName_{skillName}.png";
            else if (enumValue is WorldLocation location)
                return $"Resource/Location/WorldLocation_{location}.png";
            else if (enumValue is PlayerClass playerClass)
                return $"Resource/Player/PlayerClass/PlayerClass_{playerClass}.png";
            else if (enumValue is PlayerHealth health)
                return $"Resource/Player/PlayerHealth/PlayerHealth_{health}.png";
            else if (enumValue is PlayerResource resource)
                return $"Resource/Player/PlayerResource/PlayerResource_{resource}.png";

            throw new NotImplementedException("Didnt implement GetTypeBasedPath for given enum in ResourceService.cs yet.");
        }
    }
}