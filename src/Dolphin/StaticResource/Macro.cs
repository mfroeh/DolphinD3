using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Dolphin
{
    public static class Macro
    {
        public static Action<IntPtr, CancellationTokenSource> CubeConverterSingleSlot = CubeConverterSingleSlot_;

        public static Action<IntPtr> LeftClick = LeftClick_;

        private static void CubeConverterSingleSlot_(IntPtr handle, CancellationTokenSource tokenSource)
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
            }

            return tokenSource.Token.IsCancellationRequested;
        }

        private static void LeftClick_(IntPtr handle)
        {
            var cursorPos = InputHelper.GetCursorPos();
            InputHelper.MouseClick(handle, System.Windows.Forms.MouseButtons.Left, cursorPos);
            Thread.Sleep(50);
        }
    }
}