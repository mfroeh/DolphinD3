using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WK.Libraries.HotkeyListenerNS
{
    /// <summary>
    /// Helper class for changing Hotkey representations
    /// </summary>
    public static class HotkeyRepresentation
    {
        #region Private Properties

        private static IList<int> NeedNonAltGrModifier
        {
            get
            {
                var list = new List<int>();
                for (Keys k = Keys.D0; k <= Keys.D9; k++)
                {
                    list.Add((int)k);
                }

                return new List<int>();
            }
        }

        private static IList<int> NeedNonShiftModifier
        {
            get
            {
                var list = new List<int>();

                // Shift + 0 - 9, A - Z.
                //for (Keys k = Keys.D0; k <= Keys.Z; k++)
                //    list.Add((int)k);

                // Shift + Numpad keys.
                for (Keys k = Keys.NumPad0; k <= Keys.NumPad9; k++)
                    list.Add((int)k);

                // Shift + Misc (,;<./ etc).
                for (Keys k = Keys.Oem1; k <= Keys.OemBackslash; k++)
                    list.Add((int)k);

                // Shift + Space, PgUp, PgDn, End, Home.
                for (Keys k = Keys.Space; k <= Keys.Home; k++)
                    list.Add((int)k);

                // Misc keys that we can't loop through.
                list.Add((int)Keys.Insert);
                list.Add((int)Keys.Help);
                list.Add((int)Keys.Multiply);
                list.Add((int)Keys.Add);
                list.Add((int)Keys.Subtract);
                list.Add((int)Keys.Divide);
                list.Add((int)Keys.Decimal);
                list.Add((int)Keys.Return);
                list.Add((int)Keys.Escape);
                list.Add((int)Keys.NumLock);
                list.Add((int)Keys.Scroll);
                list.Add((int)Keys.Pause);

                return new List<int>(); // list;
            }
        }

        #endregion Private Properties

        #region Public Methods

        /// <summary>
        /// [Special] Converts a hotkey string to its variant <see cref="Hotkey"/> object.
        /// </summary>
        public static Hotkey AsHotkey(string hotkey)
        {
            Keys keyCode = Keys.None;
            Keys modifiers = Keys.None;

            hotkey = hotkey.Replace(" ", "");
            hotkey = hotkey.Replace(",", "");
            hotkey = hotkey.Replace("+", "");

            if (hotkey.Contains("Control"))
            {
                modifiers |= Keys.Control;
                hotkey = hotkey.Replace("Control", "");
            }

            if (hotkey.Contains("Shift"))
            {
                modifiers |= Keys.Shift;
                hotkey = hotkey.Replace("Shift", "");
            }

            if (hotkey.Contains("Alt"))
            {
                modifiers |= Keys.Alt;
                hotkey = hotkey.Replace("Alt", "");
            }

            keyCode = (Keys)Enum.Parse(typeof(Keys), hotkey, true);

            return new Hotkey(modifiers, keyCode);
        }

        /// <summary>
        /// [Helper] Converts keys or key combinations to their string types.
        /// </summary>
        /// <param name="hotkey">The hotkey to convert.</param>
        public static string AsString(Hotkey hotkey)
        {
            try
            {
                var _hotkey = hotkey.KeyCode;
                var _modifiers = hotkey.Modifiers;

                string parsedHotkey = string.Empty;

                // No modifier or shift only, and a hotkey that needs another modifier.
                if ((_modifiers == Keys.Shift || _modifiers == Keys.None))
                {
                    if (NeedNonShiftModifier != null && NeedNonShiftModifier.Contains((int)_hotkey))
                    {
                        if (_modifiers == Keys.None)
                        {
                            // Set Ctrl+Alt as the modifier unless Ctrl+Alt+<key> won't work.
                            if (NeedNonAltGrModifier.Contains((int)_hotkey) == false)
                            {
                                _modifiers = Keys.Alt | Keys.Control;
                            }
                            else
                            {
                                // ...In that case, use Shift+Alt instead.
                                _modifiers = Keys.Alt | Keys.Shift;
                            }
                        }
                        else
                        {
                            // User pressed Shift and an invalid key (e.g. a letter or a number),
                            // that needs another set of modifier keys.
                            _hotkey = Keys.None;
                        }
                    }
                }

                // Without this code, pressing only Ctrl will show up as "Control + ControlKey", etc.
                if (_hotkey == Keys.Menu || /* Alt */
                    _hotkey == Keys.ShiftKey ||
                    _hotkey == Keys.ControlKey)
                {
                    _hotkey = Keys.None;
                }

                if (_modifiers == Keys.None)
                {
                    // LWin/RWin don't work as hotkeys... (neither do they work as modifier keys in
                    // .NET 2.0).
                    if (_hotkey == Keys.None || _hotkey == Keys.LWin || _hotkey == Keys.RWin)
                    {
                        parsedHotkey = string.Empty;
                    }
                    else
                    {
                        parsedHotkey = _hotkey.ToString();
                    }
                }
                else
                {
                    parsedHotkey = _modifiers.ToString() + " + " + _hotkey.ToString();
                }

                return parsedHotkey;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        #endregion Public Methods
    }
}