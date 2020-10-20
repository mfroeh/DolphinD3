using System.Drawing;

namespace Dolphin
{
    public interface IExtractInformationService
    {
        /// <summary>
        /// Extracts Information from a given picture
        /// </summary>
        /// <param name="picture">The picture to extract information from</param>
        void Extract(Bitmap picture);
    }
}