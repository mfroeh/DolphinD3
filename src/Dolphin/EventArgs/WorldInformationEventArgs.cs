using Dolphin.Enum;
using System;

namespace Dolphin
{
    public class WorldInformationEventArgs : EventArgs
    {
        public WorldLocation Location { get; set; }
    }
}