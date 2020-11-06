using Dolphin.Enum;
using System.Windows.Forms;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin
{
    public interface ISettingsService
    {
        MacroSettings MacroSettings { get; }

        Settings Settings { get; }

        SmartActionSettings SmartActionSettings { get; }

        UiSettings UiSettings { get; }

        ActionName GetActionName(Hotkey hotkey);

        Keys GetKeybinding(CommandKeybinding command);

        ActionName GetSmartActionName(Window window);

        public void ResetSettings<T>();

        public void ResetSettings();

        void SetHotkeyValue(ActionName key, Hotkey value);

        void SetPaused(bool newPaused, bool isFromChanging);

        bool SkillIndexIsSuspended(int skillIndex);

        bool SkillIsEnabled(SkillName skill);
    }
}