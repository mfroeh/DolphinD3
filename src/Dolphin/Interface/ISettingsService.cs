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

        ActionName GetActionName(Hotkey hotkey);

        Keys GetKeybinding(CommandKeybinding command);

        public void ResetSettings<T>();

        public void ResetSettings();

        void SetHotkeyValue(ActionName key, Hotkey value);

        void SetPaused(bool newPaused, bool isFromChanging);

        bool SkillIsEnabled(SkillName skill);

        ActionName GetSmartActionName(Window window);
    }
}