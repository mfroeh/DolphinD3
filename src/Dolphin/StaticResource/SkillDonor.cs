using System;
using System.Windows.Forms;

namespace Dolphin
{
    public static class SkillDonor
    {
        public static Action GetAction(IntPtr handle, Keys key)
        {
            if (handle == IntPtr.Zero) return () => { Console.WriteLine("Got empty handle"); };

            return () =>
            {
                InputHelper.PressKey(handle, key);
            };
        }

        public static Action GetAction(IntPtr handle, MouseButtons button)
        {
            if (handle == IntPtr.Zero) return () => { Console.WriteLine("Got empty handle"); };

            var cursorPOs = InputHelper.GetCursorPos();

            return () =>
            {
                InputHelper.MouseClick(handle, button, cursorPOs);
            };
        }
    }
}