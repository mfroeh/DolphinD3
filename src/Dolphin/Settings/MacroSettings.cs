using Dolphin.Enum;

namespace Dolphin
{
    public class MacroSettings
    {
        public bool PickGemYourself { get; set; }

        public ConvertingSpeed ConvertingSpeed { get; set; }

        public bool Empowered { get; set; }

        public ItemType SelectedGambleItem { get; set; }

        public uint SpareColumns { get; set; }
    }
}