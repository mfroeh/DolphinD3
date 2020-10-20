using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin.Ui.HotkeyInput
{
    public class InputControlHotkey
    {
        public Key Key { get; }

        public ModifierKeys Modifiers { get; }

        public InputControlHotkey(Key key, ModifierKeys modifiers)
        {
            Key = key;
            Modifiers = modifiers;
        }

        public InputControlHotkey(Hotkey hotkey)
        {
            Key = KeyInterop.KeyFromVirtualKey((int)hotkey.KeyCode);
            Modifiers = (ModifierKeys)KeyInterop.KeyFromVirtualKey((int)hotkey.Modifiers);
        }

        public override string ToString()
        {
            var str = new StringBuilder();

            if (Modifiers.HasFlag(ModifierKeys.Control))
                str.Append("Ctrl + ");
            if (Modifiers.HasFlag(ModifierKeys.Shift))
                str.Append("Shift + ");
            if (Modifiers.HasFlag(ModifierKeys.Alt))
                str.Append("Alt + ");
            if (Modifiers.HasFlag(ModifierKeys.Windows))
                str.Append("Win + ");

            str.Append(Key);

            return str.ToString();
        }

        public Hotkey ToHotkey()
        {
            var formsKey = (System.Windows.Forms.Keys)KeyInterop.VirtualKeyFromKey(Key);
            var modifiers = Modifiers.ToWinforms();

            return new Hotkey(formsKey, modifiers);
        }
    }

    public static class KeyExtensionMethods
    {
        public static System.Windows.Forms.Keys ToWinforms(this ModifierKeys modifier)
        {
            var retVal = System.Windows.Forms.Keys.None;
            if (modifier.HasFlag(System.Windows.Input.ModifierKeys.Alt))
            {
                retVal |= System.Windows.Forms.Keys.Alt;
            }
            if (modifier.HasFlag(System.Windows.Input.ModifierKeys.Control))
            {
                retVal |= System.Windows.Forms.Keys.Control;
            }
            if (modifier.HasFlag(System.Windows.Input.ModifierKeys.None))
            {
                retVal |= System.Windows.Forms.Keys.None;
            }
            if (modifier.HasFlag(System.Windows.Input.ModifierKeys.Shift))
            {
                retVal |= System.Windows.Forms.Keys.Shift;
            }
            if (modifier.HasFlag(System.Windows.Input.ModifierKeys.Windows))
            {
            }
            return retVal;
        }
    }
}
