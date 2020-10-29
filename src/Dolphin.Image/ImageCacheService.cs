using Dolphin.Enum;
using System.Drawing;

namespace Dolphin.Image
{
    public class ImageCacheService : CacheServiceBase, IImageCacheService
    {
        public void AddSkillBitmap(SkillName skillName, bool isMouse, Bitmap bitmap)
        {
            var key = isMouse ? $"SkillName_Mouse_{skillName}" : $"SkillName_{skillName}";

            Add(key, bitmap);
        }

        public Bitmap GetSkillBitmap(SkillName skillName, bool isMouse)
        {
            var key = isMouse ? $"SkillName_Mouse_{skillName}" : $"SkillName_{skillName}";

            return Get<Bitmap>(key);
        }
    }
}