using Dolphin.Enum;
using System.Windows.Forms;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin
{
    public interface ISettingsService
    {
        MacroSettings MacroSettings { get; }

        Settings Settings { get; }

        SkillCastSettings SkillCastSettings { get; }

        SmartFeatureSettings SmartFeatureSettings { get; }

        UiSettings UiSettings { get; }

        ActionName GetActionName(Hotkey hotkey);

        Keys GetKeybinding(CommandKeybinding command);

        SmartActionName GetSmartActionName(Window window);

        bool IsSmartActionEnabled(SmartActionName smartAction);

        public void ResetSettings(string propertyName);

        void SetHotkeyValue(ActionName key, Hotkey value);

        void SetPaused(bool newPaused, bool isFromChanging);

        bool SkillIndexIsSuspended(int skillIndex);

        bool SkillIsEnabled(SkillName skill);
    }
}