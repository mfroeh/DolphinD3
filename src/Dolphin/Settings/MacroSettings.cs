using Dolphin.Enum;

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
    }
}