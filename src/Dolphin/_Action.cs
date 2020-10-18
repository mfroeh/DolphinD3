using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dolphin
{
    public static class _Action
    {
        public static Action GetSkillCastAction(IntPtr handle, Keys key)
        {
            if (handle == IntPtr.Zero) return () => { Console.WriteLine("Got empty handle"); };

            return () =>
            {
                InputHelper.PressKey(handle, key);
            };
        }

        public static Action GetSkillCastAction(IntPtr handle, MouseButtons button)
        {
            if (handle == IntPtr.Zero) return () => { Console.WriteLine("Got empty handle"); };

            var cursorPOs = InputHelper.GetCursorPos();
            return () =>
            {
                InputHelper.MouseClick(handle, button, cursorPOs );
            };
        }
    }
}
