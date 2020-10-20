using Dolphin.Enum;
using System;
using System.Threading;

namespace Dolphin
{
    public static class MacroDonor
    {
        public static Action<IntPtr, CancellationTokenSource> GetCancellableMacro(HotkeyName macro, Settings settings = null)
        {
            if (settings == null)
            {
                var property = typeof(Macro).GetProperty(macro.ToString(), typeof(Action<IntPtr, CancellationTokenSource>));

                return (Action<IntPtr, CancellationTokenSource>)property?.GetValue(null);
            }

            return null;
        }

        public static Action<IntPtr> GetMacro(HotkeyName macro, Settings settings = null)
        {
            if (settings == null)
            {
                var property = typeof(Macro).GetProperty(macro.ToString(), typeof(Action<IntPtr>));

                return (Action<IntPtr>)property?.GetValue(null);
            }

            return null;
        }
    }
}