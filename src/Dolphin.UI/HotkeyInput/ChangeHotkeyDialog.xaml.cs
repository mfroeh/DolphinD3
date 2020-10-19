using System.Text;
using System.Windows;
using System.Windows.Input;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin.Ui.HotkeyInput
{
    /// <summary>
    /// Interaction logic for ChangeHotkeyModalView.xaml
    /// </summary>
    public partial class ChangeHotkeyDialog : Window
    {
        //public static readonly DependencyProperty HotkeyProperty =
        //    DependencyProperty.Register(nameof(Hotkey), typeof(InputControlHotkey),
        //        typeof(ChangeHotkeyDialogView),
        //        new FrameworkPropertyMetadata(default(InputControlHotkey),
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public ChangeHotkeyDialog()
        {
            InitializeComponent();
        }

        private void HotkeyTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var dataContext = DataContext as ChangeHotkeyDialogViewModel;
            // Don't let the event pass further
            // because we don't want standard textbox shortcuts working
            e.Handled = true;

            // Get modifiers and key data
            var modifiers = Keyboard.Modifiers;
            var key = e.Key;

            // When Alt is pressed, SystemKey is used instead
            if (key == Key.System)
            {
                key = e.SystemKey;
            }

            // Pressing delete, backspace or escape without modifiers clears the current value
            if (modifiers == ModifierKeys.None &&
                (key == Key.Delete || key == Key.Back || key == Key.Escape))
            {
                dataContext.Hotkey = null;
                return;
            }

            // If no actual key was pressed - return
            if (key == Key.LeftCtrl ||
                key == Key.RightCtrl ||
                key == Key.LeftAlt ||
                key == Key.RightAlt ||
                key == Key.LeftShift ||
                key == Key.RightShift ||
                key == Key.LWin ||
                key == Key.RWin ||
                key == Key.Clear ||
                key == Key.OemClear ||
                key == Key.Apps)
            {
                return;
            }

            // Update the value
            dataContext.Hotkey = new InputControlHotkey(key, modifiers);
        }
    }

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