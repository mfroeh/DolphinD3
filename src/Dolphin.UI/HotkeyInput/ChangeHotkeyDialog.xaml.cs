using System.Text;
using System.Windows;
using System.Windows.Input;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin.Ui.HotkeyInput
{
    /// <summary>
    /// Interaction logic for ChangeHotkeyDialog.xaml
    /// </summary>
    public partial class ChangeHotkeyDialog : Window
    {
        public ChangeHotkeyDialog()
        {
            InitializeComponent();
        }

        private void HotkeyTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var dataContext = DataContext as ChangeHotkeyDialogViewModel;
            e.Handled = true;

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

            dataContext.Hotkey = new InputControlHotkey(key, modifiers);
        }
    }
}