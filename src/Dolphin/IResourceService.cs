using System.Drawing;
using System.Threading.Tasks;

namespace Dolphin
{
    public interface IResourceService
    {
        Task<Bitmap> Load<TEnum>(TEnum enumValue);
    }
}