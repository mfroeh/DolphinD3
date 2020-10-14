using Dolphin.Enum;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;

namespace Dolphin.Service
{
    public class ResourceService : IResourceService
    {
        private readonly ICacheService cacheService;
        private readonly ILogService logService;
        private readonly Settings settings; // TODO: Settings for path?

        public ResourceService(ICacheService cacheService, ILogService logService, Settings settings)
        {
            this.cacheService = cacheService;
            this.logService = logService;
            this.settings = settings;
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
            await cacheService.Add(enumValue, bitmap);
            logService.AddEntry(this, $"Loaded Image from [{path}] for {(T)enumValue} and added to Cache.");

            return bitmap;
        }

        private string GetTypeBasedPath<T>(T enumValue)
        {
            if (enumValue is BuffName buffName)
                return $"./Resource/Buff/BuffName_{buffName}.png";
            else if (enumValue is SkillName skillName)
                return $"./Resource/Skill/SkillName_{skillName}.png";
            else if (enumValue is WorldLocation location)
                return $"./Resource/Location/WorldLocation_{location}.png";

            return null;
        }
    }
}