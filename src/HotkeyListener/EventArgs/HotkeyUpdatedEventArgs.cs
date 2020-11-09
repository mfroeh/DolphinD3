using System;

namespace WK.Libraries.HotkeyListenerNS
{
    /// <summary>
    /// Provides data for the <see cref="HotkeyListener.HotkeyUpdated"/> event.
    /// </summary>
    public class HotkeyUpdatedEventArgs : EventArgs
    {
        #region Public Constructors

        /// <summary>
        /// Provides data for the <see cref="HotkeyListener.HotkeyUpdated"/> event.
        /// </summary>
        /// <param name="updatedHotkey">The hotkey that has been updated.</param>
        /// <param name="newHotkey">The hotkey's newly updated value.</param>
        public HotkeyUpdatedEventArgs(Hotkey updatedHotkey, Hotkey newHotkey)
        {
            UpdatedHotkey = updatedHotkey;
            NewHotkey = newHotkey;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets the Hotkey's newly updated value.
        /// </summary>
        public Hotkey NewHotkey { get; }

        /// <summary>
        /// Gets the currently updated Hotkey.
        /// </summary>
        public Hotkey UpdatedHotkey { get; }

        #endregion Public Properties
    }
}