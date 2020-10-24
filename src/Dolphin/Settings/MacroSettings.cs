using Dolphin.Enum;
using System.Collections;
using System.Collections.Generic;

namespace Dolphin
{
    public class MacroSettings
    {
        public ConvertingSpeed ConvertingSpeed { get; set; }

        public bool Empowered { get; set; }

        public bool PickGemYourself { get; set; }

        public ItemType SelectedGambleItem { get; set; }

        public uint SpareColumns { get; set; }

        public uint SwapItemsAmount { get; set; }

        public IList<Waypoint> Poolspots { get; set; }
    }
}