using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

namespace Dolphin
{
    public static class WindowHelper
    {
        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hwnd, ref Rectangle rect);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        public static IntPtr GetHWND(string processName)
        {
            var processes = Process.GetProcessesByName(processName);
            if (processes.Any())
            {
                return processes.First().MainWindowHandle;
            }

            return IntPtr.Zero;
        }

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rectangle rect);

        public static bool IsForeground(IntPtr hwnd)
        {
            return GetForegroundWindow() == hwnd;
        }

        [DllImport("user32.dll")]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

        [DllImport("user32.dll")]
        public static extern bool ScreenToClient(IntPtr hwnd, ref Point point);
    }
}