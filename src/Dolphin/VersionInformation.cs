using System.Collections.Generic;
using System.Drawing;

namespace Dolphin
{
    public static class VersionInformation
    {
        public const string Version = "0.0.0.0";

        public static IList<Size> SupportedResolutions = new List<Size>
        {
            new Size { Height= 1440, Width=2560 }
        };
    }
}