using AForge.Math.Metrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Dolphin
{
    public static class _Action
    {
        public static Action GetSkillCastAction(IntPtr handle, Keys key)
        {
            if (handle == IntPtr.Zero) return () => Console.WriteLine("Got empty handle");

            return async () =>
            {
                await InputHelper.PressKey(handle, key);
            };
        }

        public static Action GetSkillCastAction(IntPtr handle, MouseButtons button)
        {
            if (handle == IntPtr.Zero) return () => Console.WriteLine("Got empty handle");

            var cursorPOs = InputHelper.GetCursorPos();
            return async () =>
            {
                await InputHelper.MouseClick(handle, button, cursorPOs);
            };
        }
    }
}
