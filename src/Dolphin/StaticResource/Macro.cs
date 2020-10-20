using System;
using System.Diagnostics;
using System.Threading;

namespace Dolphin
{
    public static class Macro
    {
        public static Action<IntPtr, CancellationTokenSource> CubeConverter = CubeConverter_;

        public static Action<IntPtr> LeftClick = LeftClick_;

        private static void CubeConverter_(IntPtr handle, CancellationTokenSource tokenSource)
        {
            for (int i = 0; i < 100; i++)
            {
                if (IsCancelled(tokenSource)) return;
                Trace.WriteLine(i);
            }
            Thread.Sleep(200);
        }

        private static bool IsCancelled(CancellationTokenSource tokenSource)
        {
            if (tokenSource.Token.IsCancellationRequested)
            {
                Trace.WriteLine("Cancellation requested!");
                tokenSource.Dispose();
                tokenSource = null;

                return true;
            }

            return false;
        }

        private static void LeftClick_(IntPtr handle)
        {
            var cursorPos = InputHelper.GetCursorPos();
            InputHelper.MouseClick(handle, System.Windows.Forms.MouseButtons.Left, cursorPos);
        }
    }
}