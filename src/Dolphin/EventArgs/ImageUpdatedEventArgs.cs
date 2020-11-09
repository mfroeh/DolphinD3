using System;
using System.Drawing;

namespace Dolphin
{
    public class ImageUpdatedEventArgs
    {
        public IntPtr Handle { get; set; }

        public Bitmap NewImage { get; set; }
    }
}