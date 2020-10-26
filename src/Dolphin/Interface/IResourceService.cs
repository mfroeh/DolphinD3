using Dolphin.Enum;
using System.Drawing;

namespace Dolphin
{
    public interface IResourceService
    {
        Bitmap Load<TEnum>(TEnum enumValue);

        Bitmap LoadSkillBitmap(SkillName skillName, bool isMouse);
    }
}