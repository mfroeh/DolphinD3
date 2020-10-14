using System.Drawing;
using System.Threading.Tasks;

namespace Dolphin
{
    public interface IExtractInformationService
    {
        /// <summary>
        /// Extracts Information from a given picture
        /// </summary>
        /// <param name="picture">The picture to extract information from</param>
        Task Extract(Bitmap picture);
    }
}