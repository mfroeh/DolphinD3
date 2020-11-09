using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace WK.Libraries.HotkeyListenerNS
{
    /// <summary>
    /// Creates a standard hotkey for use with <see cref="HotkeyListener"/>.
    /// </summary>
    [Serializable]
    [DebuggerStepThrough]
    public class Hotkey
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Hotkey"/> class.
        /// </summary>
        public Hotkey() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hotkey"/> class.
        /// </summary>
        /// <param name="hotkey">The hotkey in string format.</param>
        public Hotkey(string hotkey)
        {
            var hotkeyObj = HotkeyRepresentation.AsHotkey(hotkey);

            KeyCode = hotkeyObj.KeyCode;
            Modifiers = hotkeyObj.Modifiers;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hotkey"/> class.
        /// </summary>
        /// <param name="keyCode">The hotkey's keyboard code.</param>
        public Hotkey(Keys keyCode = Keys.None)
        {
            KeyCode = keyCode;
            Modifiers = Keys.None;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hotkey"/> class.
        /// </summary>
        /// <param name="modifiers">
        /// The hotkey's modifier flags. The flags indicate which combination of CTRL, SHIFT, and
        /// ALT keys will be detected.
        /// </param>
        /// <param name="keyCode">The hotkey's keyboard code.</param>
        public Hotkey(Keys modifiers = Keys.None, Keys keyCode = Keys.None)
        {
            KeyCode = keyCode;
            Modifiers = modifiers;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the hotkey's keyboard code.
        /// </summary>
        public Keys KeyCode { get; set; }

        /// <summary>
        /// Gets or sets the hotkey's modifier flags. The flags indicate which combination of CTRL,
        /// SHIFT, and ALT keys will be detected.
        /// </summary>
        public Keys Modifiers { get; set; }

        /// <summary>
        /// Determines whether this hotkey has been suspended from use.
        /// </summary>
        public bool Suspended
        {
            get => HotkeyListener._suspendedKeys.Contains(ToString());
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Overrides the system-default object non-equality operator for a customized Hotkey
        /// non-equality-check operator.
        /// </summary>
        /// <returns></returns>
        public static bool operator !=(Hotkey x, Hotkey y)
        {
            return !(x == y);
        }

        /// <summary>
        /// Overrides the system-default object equality operator for a customized Hotkey
        /// equality-check operator.
        /// </summary>
        /// <returns></returns>
        public static bool operator ==(Hotkey x, Hotkey y)
        {
            if (x is null)
                return y is null;

            return x.Equals(y);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        /// true if the specified object is equal to the current object; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Hotkey))
                return false;

            var other = obj as Hotkey;

            return
                KeyCode == other.KeyCode &&
                Modifiers == other.Modifiers;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns a string conversion containing the Hotkey's <see cref="KeyCode"/> and <see
        /// cref="Modifiers"/> keys.
        /// </summary>
        /// <returns><see cref="String"/></returns>
        public override string ToString()
        {
            if (Modifiers == Keys.None)
                return KeyCode.ToString();
            else
                return HotkeyRepresentation.AsString(this);
        }

        #endregion Public Methods
    }
}