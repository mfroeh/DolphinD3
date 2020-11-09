using System;

namespace WK.Libraries.HotkeyListenerNS
{
    /// <summary>
    /// Provides data for the <see cref="HotkeyListener.HotkeyPressed"/> event.
    /// </summary>
    public class HotkeyEventArgs : EventArgs
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HotkeyEventArgs"/> class.
        /// </summary>
        public HotkeyEventArgs() { }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets the hotkey that was pressed.
        /// </summary>
        public Hotkey Hotkey { get; internal set; }

        #endregion Public Properties
    }
}