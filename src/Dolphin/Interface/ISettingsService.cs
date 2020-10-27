using Dolphin.Enum;
using System.Windows.Forms;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin
{
    public interface ISettingsService
    {
        MacroSettings MacroSettings { get; }

        Settings Settings { get; }

        UiSettings UiSettings { get; }

        SmartActionSettings SmartActionSettings { get; }

        ActionName GetActionName(Hotkey hotkey);

        void SetPaused(bool newPaused, bool isFromChanging);

        public void ResetSettings<T>();

        public void ResetSettings();

        void SetHotkeyValue(ActionName key, Hotkey value);

        bool SkillIsEnabled(SkillName skill);

        Keys GetKeybinding(Command command);
    }
}