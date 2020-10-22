using Dolphin.Enum;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin
{
    public interface ISettingsService
    {
        Settings Settings { get; }

        MacroSettings MacroSettings { get; }

        UiSettings UiSettings { get; }

        void SetHotkeyValue(ActionName key, Hotkey value);

        void NegateIsPaused();

        bool SkillIsEnabled(SkillName skill);

        ActionName GetActionName(Hotkey hotkey);
    }
}