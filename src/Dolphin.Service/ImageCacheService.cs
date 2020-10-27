using Dolphin.Enum;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Dolphin.Service
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