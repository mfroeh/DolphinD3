/*
 * Developer    : Willy Kimura (WK).
 * Library      : HotkeyListener.
 * License      : MIT.
 *
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace WK.Libraries.HotkeyListenerNS
{
    /// <summary>
    /// Provides the base Hotkey handle for intercepting and receiving all registered global Hotkey events.
    /// </summary>
    [DebuggerStepThrough]
    internal sealed class HotkeyHandle : Control
    {
        #region Public Fields

        public Dictionary<int, string> Hotkeys;

        #endregion Public Fields

        #region Private Fields

        private const int WM_HOTKEY = 0x312;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HotkeyHandle"/> class.
        /// </summary>
        public HotkeyHandle()
        {
            SuspendLayout();
            Visible = false;

            ResumeLayout(false);

            Hotkeys = new Dictionary<int, string>();
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Adds a Hotkey to the global Key watcher.
        /// </summary>
        /// <param name="hotkey">The Hotkey string.</param>
        public bool AddKey(string hotkey)
        {
            if (!base.IsHandleCreated)
            {
                base.CreateControl();
            }

            return this.Register(hotkey);
        }

        /// <summary>
        /// Remove all the registered Hotkeys from the global Key watcher.
        /// </summary>
        public void RemoveAllKeys()
        {
            this.Unregister();
        }

        /// <summary>
        /// Removes any specified Hotkey from the global Key watcher.
        /// </summary>
        /// <param name="hotkey">The Hotkey to remove.</param>
        public void RemoveKey(string hotkey)
        {
            this.Unregister(hotkey);
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Overrides the default window message processing to detect the registered Hotkeys when pressed.
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                var hotkey = HotkeyRepresentation.AsHotkey(this.Hotkeys[m.WParam.ToInt32()]);
                HotkeyPressed?.Invoke(null, new HotkeyEventArgs { Hotkey = hotkey });
            }
            else
                base.WndProc(ref m);
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Validates and registers any given Hotkey.
        /// </summary>
        /// <param name="hotkey">The Hotkey string.</param>
        private bool Register(string hotkey)
        {
            this.Unregister(hotkey);

            int hotKeyId = HotkeyCore.GlobalAddAtom("RE:" + hotkey);

            if (hotKeyId == 0)
            {
                throw new Exception(
                    string.Format(
                        "Could not register atom for {0} Hotkey. " +
                        "Please try another Hotkey.", hotkey));
            }

            if (HotkeyCore.RegisterKey(this, hotKeyId, hotkey))
            {
                Hotkeys.Add(hotKeyId, hotkey);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Unregisters any registered Hotkeys.
        /// </summary>
        /// <param name="hotkey">The registered Hotkey.</param>
        private void Unregister(string hotkey)
        {
            int intKey = 0;

            foreach (KeyValuePair<int, string> hotKey in Hotkeys)
            {
                if (hotKey.Value == hotkey)
                {
                    intKey = hotKey.Key;

                    HotkeyCore.UnregisterKey(this, hotKey.Key);
                    HotkeyCore.GlobalDeleteAtom(hotKey.Key);

                    break;
                }
            }

            if (intKey > 0)
                Hotkeys.Remove(intKey);
        }

        /// <summary>
        /// Unregisters all the registered Hotkeys.
        /// </summary>
        private void Unregister()
        {
            foreach (KeyValuePair<int, string> hotKey in Hotkeys)
            {
                HotkeyCore.UnregisterKey(this, hotKey.Key);
                HotkeyCore.GlobalDeleteAtom(hotKey.Key);
            }

            Hotkeys.Clear();
        }

        #endregion Private Methods

        #region Public

        /// <summary>
        /// Raised whenever a registered Hotkey is pressed.
        /// </summary>
        public event EventHandler<HotkeyEventArgs> HotkeyPressed;

        #endregion Public
    }
}