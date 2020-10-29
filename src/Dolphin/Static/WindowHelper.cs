using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

namespace Dolphin
{
    public static class WindowHelper
    {
        public static Rectangle GetClientRect(IntPtr hwnd)
        {
            var rect = new Rectangle();
            GetClientRect(hwnd, ref rect);

            return rect;
        }

        public static Point GetCursorPos()
        {
            var p = new Point();
            GetCursorPos(ref p);

            return p;
        }

        public static IntPtr GetHWND(string processName)
        {
            var processes = Process.GetProcessesByName(processName);
            if (processes.Any())
            {
                return processes.First().MainWindowHandle;
            }

            return IntPtr.Zero;
        }

        public static uint GetWindowThreadProcessId(IntPtr hwnd)
        {
            GetWindowThreadProcessId(hwnd, out var id);

            return id;
        }

        public static bool IsForeground(IntPtr hwnd)
        {
            return GetForegroundWindow() == hwnd;
        }

        public static Point ScreenToClient(IntPtr hwnd, Point point)
        {
            ScreenToClient(hwnd, ref point);

            return point;
        }

        #region DLL imports

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hwnd, uint message, uint wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

        [DllImport("user32.dll")]
        private static extern bool GetClientRect(IntPtr hwnd, ref Rectangle rect);

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(ref Point point);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [DllImport("user32.dll")]
        private static extern bool ScreenToClient(IntPtr hwnd, ref Point point);

        #endregion DLL imports
    }
}