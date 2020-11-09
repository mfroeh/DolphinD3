using Dolphin.Enum;
using System.Drawing;

namespace Dolphin
{
    public interface IResourceService
    {
        Bitmap Load<T>(T value);

        Bitmap LoadSkillBitmap(SkillName skillName, bool isMouse);
    }
}