using System.Drawing;

namespace Dolphin
{
    public interface IResourceService
    {
        Bitmap Load<TEnum>(TEnum enumValue);
    }
}