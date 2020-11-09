using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace WK.Libraries.HotkeyListenerNS
{
    /// <summary>
    /// A library that provides support for registering and attaching events to global hotkeys in
    /// .NET applications.
    /// </summary>
    [DebuggerStepThrough]
    [Description("A library that provides support for registering and " +
                     "attaching events to global hotkeys in .NET applications.")]
    public partial class HotkeyListener
    {
        #region Internal Fields

        // This is the handle that will be used to register, unregister, and listen to the hotkey triggers.
        internal static HotkeyHandle _handle = new HotkeyHandle();

        // Saves the list of hotkeys suspended.
        internal static List<string> _suspendedKeys =
            new List<string>();

        #endregion Internal Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HotkeyListener"/> class.
        /// </summary>
        public HotkeyListener()
        {
            SetDefaults();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HotkeyListener"/> class.
        /// </summary>
        /// <param name="hotkeys">A collection of hotkeys to add to listening.</param>
        public HotkeyListener(ICollection<Hotkey> hotkeys)
        {
            SetDefaults();
            foreach (var hotkey in hotkeys)
            {
                Add(hotkey);
            }
        }

        #endregion Public Constructors

        #region Public Delegates

        /// <summary>
        /// Represents the method that will handle a <see cref="HotkeyPressed"/> event that has no
        /// event data.
        /// </summary>
        /// <param name="sender">The hotkey sender object.</param>
        /// <param name="e">The <see cref="HotkeyEventArgs"/> data.</param>
        public delegate void HotkeyEventHandler(object sender, HotkeyEventArgs e);

        #endregion Public Delegates

        #region Public Events

        /// <summary>
        /// Raised whenever a registered Hotkey is pressed.
        /// </summary>
        [Category("HotkeyListener Events")]
        [Description("Raised whenever a registered Hotkey is pressed.")]
        public event HotkeyEventHandler HotkeyPressed;

        /// <summary>
        /// Raised whenever a registered Hotkey has been updated.
        /// </summary>
        [Category("HotkeyListener Events")]
        [Description("Raised whenever a registered Hotkey has been updated.")]
        public event EventHandler<HotkeyUpdatedEventArgs> HotkeyUpdated = null;

        #endregion Public Events

        #region Public Properties

        /// <summary>
        /// Gets a value determining whether the hotkeys set have been suspended.
        /// </summary>
        public bool Suspended { get; private set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Adds a hotkey to the global Key watcher.
        /// </summary>
        /// <param name="hotkey">The hotkey to add.</param>
        /// <returns>
        /// True if successful or false if not. Ensure you inform the user if the hotkey fails to be
        /// registered. This is mostly due to a hotkey being already in use by another application.
        /// </returns>
        public bool Add(Hotkey hotkey)
        {
            if (hotkey.Modifiers == Keys.LWin || hotkey.Modifiers == Keys.RWin)
                return false;

            return Add(HotkeyRepresentation.AsString(hotkey));
        }

        /// <summary>
        /// Adds a list of hotkeys to the global Key watcher.
        /// </summary>
        /// <param name="hotkeys">The hotkeys to add.</param>
        /// <returns>
        /// The list of hotkeys passed and their results when trying to register them. Their results
        /// will each denote a true if successful or false if not. Ensure you inform the user if one
        /// hotkey fails to be registered. This is mostly due to a hotkey being already in use by
        /// another application.
        /// </returns>
        public Dictionary<string, bool> Add(Hotkey[] hotkeys)
        {
            Dictionary<string, bool> keyValues = new Dictionary<string, bool>();

            foreach (var key in hotkeys)
            {
                keyValues.Add(key.ToString(), Add(key));
            }

            return keyValues;
        }

        /// <summary>
        /// Removes any specific hotkey from the global Key watcher.
        /// </summary>
        /// <param name="hotkey">The hotkey to remove.</param>
        public void Remove(Hotkey hotkey)
        {
            _handle.RemoveKey(HotkeyRepresentation.AsString(hotkey));
        }

        /// <summary>
        /// Removes a list of hotkeys from the global Key watcher.
        /// </summary>
        /// <param name="hotkeys">The hotkeys to remove.</param>
        public void Remove(Hotkey[] hotkeys)
        {
            foreach (var key in hotkeys)
            {
                Remove(key);
            }
        }

        /// <summary>
        /// Remove all the registered hotkeys from the global Key watcher.
        /// </summary>
        public void RemoveAll()
        {
            _handle.RemoveAllKeys();
        }

        /// <summary>
        /// Resumes using the hotkey(s) that were set in the global Key watcher.
        /// </summary>
        public bool Resume()
        {
            if (Suspended)
            {
                foreach (var key in _suspendedKeys.ToList())
                {
                    if (!_handle.Hotkeys.ContainsValue(key))
                    {
                        Add(key);
                    }
                }

                Suspended = false;

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Resumes listening to a specific hotkey that was suspended from the global Key watcher.
        /// </summary>
        /// <param name="hotkey">The hotkey to resume using.</param>
        public bool Resume(Hotkey hotkey)
        {
            string hotkeyString = hotkey.ToString();

            if (!_handle.Hotkeys.ContainsValue(hotkeyString) &&
                _suspendedKeys.Contains(hotkeyString))
            {
                _suspendedKeys.Remove(hotkeyString);

                Add(hotkey);

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Suspends the hotkey(s) set in the global Key watcher.
        /// </summary>
        public bool Suspend()
        {
            if (!Suspended)
            {
                _suspendedKeys.Clear();

                foreach (var item in _handle.Hotkeys)
                {
                    _suspendedKeys.Add(item.Value);
                }

                foreach (var key in _handle.Hotkeys.Values.ToList())
                {
                    Remove(key);
                }

                Suspended = true;

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Suspends a specific hotkey set from the global Key watcher.
        /// </summary>
        /// <param name="hotkey">The hotkey to suspend.</param>
        public bool Suspend(Hotkey hotkey)
        {
            string hotkeyString = hotkey.ToString();

            if (_handle.Hotkeys.ContainsValue(hotkeyString) &&
                !_suspendedKeys.Contains(hotkeyString))
            {
                _suspendedKeys.Add(hotkeyString);

                Remove(hotkey);

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Updates an existing hotkey in the global Key watcher.
        /// </summary>
        /// <param name="currentHotkey">The hotkey to modify.</param>
        /// <param name="newHotkey">The new hotkey to be set.</param>
        public void Update(Hotkey currentHotkey, Hotkey newHotkey)
        {
            Update(currentHotkey.ToString(), newHotkey.ToString());

            HotkeyUpdated?.Invoke(this, new HotkeyUpdatedEventArgs(currentHotkey, newHotkey));
        }

        /// <summary>
        /// Updates an existing hotkey in the global Key watcher.
        /// </summary>
        /// <param name="currentHotkey">A reference to the variable containing the hotkey to modify.</param>
        /// <param name="newHotkey">The new hotkey to be set.</param>
        public void Update(ref Hotkey currentHotkey, Hotkey newHotkey)
        {
            currentHotkey = newHotkey;

            Update(currentHotkey.ToString(), newHotkey.ToString());

            HotkeyUpdated?.Invoke(this, new HotkeyUpdatedEventArgs(currentHotkey, newHotkey));
        }

        /// <summary>
        /// Updates an existing hotkey in the global Key watcher.
        /// </summary>
        /// <param name="currentHotkey">A reference to the variable containing the hotkey to modify.</param>
        /// <param name="newHotkey">
        /// A reference to the variable containing the new hotkey to be set.
        /// </param>
        public void Update(ref Hotkey currentHotkey, ref Hotkey newHotkey)
        {
            currentHotkey = newHotkey;

            Update(currentHotkey.ToString(), newHotkey.ToString());

            HotkeyUpdated?.Invoke(this, new HotkeyUpdatedEventArgs(currentHotkey, newHotkey));
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Adds a hotkey to the global Key watcher.
        /// </summary>
        /// <param name="hotkey">The hotkey to add.</param>
        private bool Add(string hotkey)
        {
            return _handle.AddKey(hotkey);
        }

        /// <summary>
        /// Adds a list of hotkeys to the global Key watcher.
        /// </summary>
        /// <param name="hotkeys">The hotkeys to add.</param>
        private void Add(string[] hotkeys)
        {
            foreach (string key in hotkeys)
            {
                Add(key);
            }
        }

        /// <summary>
        /// Attaches the major hotkey events to the Hotkey Listener.
        /// </summary>
        private void AttachEvents()
        {
            _handle.HotkeyPressed += (s, e) =>
            {
                HotkeyPressed?.Invoke(null, new HotkeyEventArgs { Hotkey = e.Hotkey });
            };
        }

        /// <summary>
        /// Removes any specific hotkey from the global Key watcher.
        /// </summary>
        /// <param name="hotkey">The hotkey to remove.</param>
        private void Remove(string hotkey)
        {
            _handle.RemoveKey(hotkey);
        }

        /// <summary>
        /// Removes a list of hotkeys from the global Key watcher.
        /// </summary>
        /// <param name="hotkeys">The hotkeys to remove.</param>
        private void Remove(string[] hotkeys)
        {
            foreach (string key in hotkeys)
            {
                Remove(key);
            }
        }

        /// <summary>
        /// Applies the library's default options and settings.
        /// </summary>
        private void SetDefaults()
        {
            AttachEvents();
        }

        /// <summary>
        /// Updates an existing hotkey in the global Key watcher.
        /// </summary>
        /// <param name="currentHotkey">The hotkey to modify.</param>
        /// <param name="newHotkey">The new hotkey to be set.</param>
        private void Update(string currentHotkey, string newHotkey)
        {
            try
            {
                if (!Suspended)
                {
                    Remove(currentHotkey);
                    Add(newHotkey);
                }
                else
                {
                    _suspendedKeys.Remove(currentHotkey);
                    _suspendedKeys.Add(newHotkey);
                }

                currentHotkey = newHotkey;
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Updates an existing hotkey in the global Key watcher.
        /// </summary>
        /// <param name="currentHotkey">A reference to the variable containing the hotkey to modify.</param>
        /// <param name="newHotkey">The new hotkey to be set.</param>
        private void Update(ref string currentHotkey, string newHotkey)
        {
            try
            {
                if (!Suspended)
                {
                    Remove(currentHotkey);
                    Add(newHotkey);
                }
                else
                {
                    _suspendedKeys.Remove(currentHotkey);
                    _suspendedKeys.Add(newHotkey);
                }

                currentHotkey = newHotkey;
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Updates an existing hotkey in the global Key watcher.
        /// </summary>
        /// <param name="currentHotkey">A reference to the variable containing the hotkey to modify.</param>
        /// <param name="newHotkey">
        /// A reference to the variable containing the new hotkey to be set.
        /// </param>
        private void Update(ref string currentHotkey, ref string newHotkey)
        {
            try
            {
                if (!Suspended)
                {
                    Remove(currentHotkey);
                    Add(newHotkey);
                }
                else
                {
                    _suspendedKeys.Remove(currentHotkey);
                    _suspendedKeys.Add(newHotkey);
                }

                currentHotkey = newHotkey;
            }
            catch (Exception) { }
        }

        #endregion Private Methods
    }
}