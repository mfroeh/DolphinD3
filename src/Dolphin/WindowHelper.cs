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

        public static IntPtr GetHWND(string name = "Diablo III64")
        {
            var processes = Process.GetProcessesByName(name);
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
        public static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

        /// <summary>
        /// </summary>
        /// <param name="sourceCoordinate">Source coordinate in 1920x1080</param>
        /// <returns></returns>
        public static Point TransformCoordinate(Point sourceCoordinate, CoordinatePosition coordinatePosition = default)
        {
            var clientRect = new Rectangle();
            GetClientRect(GetHWND(), ref clientRect);

            var scaledY = clientRect.Height / 1080 * sourceCoordinate.Y;
            
            int scaledX;
            if (coordinatePosition == CoordinatePosition.Left)
            {
                scaledX = clientRect.Height / 1080 * sourceCoordinate.X;
            }
            else if (coordinatePosition == CoordinatePosition.Right)
            {
                scaledX = clientRect.Width - (1920 - sourceCoordinate.X) * clientRect.Height / 1080;
            }
            else
            {
                scaledX = sourceCoordinate.X * clientRect.Height / 1080 + (clientRect.Width - 1920 * clientRect.Height / 1080) / 2;
            }

            return new Point { X = scaledX, Y = scaledY };
        }
    }

    public enum CoordinatePosition
    {
        Left = 0,
        Right = 1,
        Other = 2
    }
}