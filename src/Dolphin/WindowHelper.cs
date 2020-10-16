using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

namespace Dolphin
{
    public static class WindowHelper
    {
        public static IntPtr GetHWND(string name = "Diablo III64")
        {
            var processes = Process.GetProcessesByName(name);

            if (processes.Any())
                return processes.First().MainWindowHandle;
            else
                return IntPtr.Zero;
        }

        public static bool IsForeground(IntPtr hwnd)
        {
            return GetForegroundWindow() == hwnd;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hwnd, ref Rectangle rect);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rectangle rect);

        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);
    }
}