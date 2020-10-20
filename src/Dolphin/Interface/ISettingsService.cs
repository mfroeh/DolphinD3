using Dolphin.Enum;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin
{
    public interface ISettingsService
    {
        Settings Settings { get; }

        void SetHotkeyValue(ActionName key, Hotkey value);

        bool SkillIsEnabled(SkillName skill);
    }
}