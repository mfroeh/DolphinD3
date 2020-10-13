using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace Dolphin
{
    public static class WindowInformationHelper
    {
        public static IntPtr? GetHWND(string name)
        {
            return Process.GetProcessesByName(name).FirstOrDefault()?.Handle;
        }

        public static bool IsForeground(IntPtr hwnd)
        {
            return GetForegroundWindow() != hwnd;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
    }
}