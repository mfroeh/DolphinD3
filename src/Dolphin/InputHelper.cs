﻿using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput;

namespace Dolphin
{
    public static class InputHelper
    {
        private const uint WM_LBUTTONDOWN = 0x201;
        private const uint WM_LBUTTONUP = 0x202;
        private const uint WM_RBUTTONDOWN = 0x204;
        private const uint WM_RBUTTONUP = 0x205;
        private const uint WM_MBUTTONDOWN = 0x207;
        private const uint WM_MBUTTONUP = 0x208;

        private const uint MK_LBUTTON = 0x0001;
        private const uint MK_RBUTTON = 0x0002;
        private const uint MK_MBUTTON = 0x0010;

        private const uint WM_KEYDOWN = 0x0100;
        private const uint WM_KEYUP = 0x0101;

        public static void MouseClick(IntPtr handle, MouseButtons button, Point point)
        {
            var lParam = MakeLParam(point);

            switch (button)
            {
                case MouseButtons.Left:
                    InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
                    PostMessage(handle, WM_LBUTTONDOWN, MK_LBUTTON, lParam);
                    PostMessage(handle, WM_LBUTTONUP, 0, lParam);
                    InputSimulator.SimulateKeyUp(VirtualKeyCode.SHIFT);
                    break;
                case MouseButtons.Right:
                    PostMessage(handle, WM_RBUTTONDOWN, MK_RBUTTON, lParam);
                    PostMessage(handle, WM_RBUTTONUP, 0, lParam);
                    break;
                case MouseButtons.Middle:
                    PostMessage(handle, WM_MBUTTONDOWN, MK_MBUTTON, lParam);
                    PostMessage(handle, WM_MBUTTONUP, 0, lParam);
                    break;
                default:
                    Console.WriteLine("Recived unkown MouseButton in MouseClick() in InputHelper.cs");
                    break;
            }
        }

        public static void PressKey(IntPtr handle, Keys key, bool pressAlt = false)
        {
            var lParam = 0;
            if (pressAlt) // TODO: Untested, what cases to we need?
                lParam = (0x01 << 28);

            PostMessage(handle, WM_KEYDOWN, (uint)key, lParam);
            PostMessage(handle, WM_KEYUP, (uint)key, lParam);
        }

        public static Point GetCursorPos()
        {
            var point = new Point();
            GetCursorPos(ref point);
            return point;
        }

        private static int MakeLParam(Point point)
        {
            return (point.Y << 16) | (point.X & 0xFFFF);
        }

        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hwnd, uint message, uint wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(ref Point lpPoint);
    }
}