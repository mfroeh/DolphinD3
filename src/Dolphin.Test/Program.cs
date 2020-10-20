using Newtonsoft.Json;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hotkeySelector = new HotkeySelector();

            var hk = new Hotkey(System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift, System.Windows.Forms.Keys.A);

            var key = KeyInterop.KeyFromVirtualKey((int)hk.KeyCode);

            var modifiers = hk.Modifiers.ToWinforms();

            Trace.WriteLine(hk);
        }

        public static System.Windows.Forms.Keys ToWinforms(this System.Windows.Input.ModifierKeys modifier)
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
                // Pointless I know
                retVal |= System.Windows.Forms.Keys.None;
            }
            if (modifier.HasFlag(System.Windows.Input.ModifierKeys.Shift))
            {
                retVal |= System.Windows.Forms.Keys.Shift;
            }
            if (modifier.HasFlag(System.Windows.Input.ModifierKeys.Windows))
            {
                // Not supported lel
            }
            return retVal;
        }
    }
}