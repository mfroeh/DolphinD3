using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dolphin.Service
{
    public class HandleService : IHandleService
    {
        private IDictionary<string, IntPtr> handles = new Dictionary<string, IntPtr>();

        public event EventHandler<HandleChangedEventArgs> HandleStatusChanged;

        public IntPtr GetHandle(string processName = "Diablo III64")
        {
            handles.TryGetValue(processName, out IntPtr handle);

            return handle;
        }

        public void UpdateHandle(string processName = "Diablo III64")
        {
            var handle = WindowHelper.GetHWND(processName);

            if (GetHandle(processName) != handle)
            {
                handles[processName] = handle;

                WindowHelper.GetWindowThreadProcessId(handle, out var processId);

                HandleStatusChanged?.Invoke(this, new HandleChangedEventArgs { ProcessName = processName, NewHandle = handle, NewProcessId = processId });
            }
        }

        public Task MainLoop()
        {
            return Task.Run(() =>
            {
                while (true)
                {
                    UpdateHandle();
                    Thread.Sleep(1000);
                }
            });
        }
    }
}