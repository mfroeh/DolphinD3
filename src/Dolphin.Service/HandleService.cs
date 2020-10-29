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

        public IntPtr GetHandle(string processName)
        {
            handles.TryGetValue(processName, out IntPtr handle);

            return handle;
        }

        public void UpdateHandle(string processName)
        {
            var handle = WindowHelper.GetHWND(processName);

            if (GetHandle(processName) != handle)
            {
                handles[processName] = handle;

                var processId = WindowHelper.GetWindowThreadProcessId(handle);

                HandleStatusChanged?.Invoke(this, new HandleChangedEventArgs { ProcessName = processName, NewHandle = handle, NewProcessId = processId });
            }
        }

        public Task MainLoop(params string[] processNames)
        {
            return Task.Run(() =>
            {
                while (true)
                {
                    foreach (var processName in processNames)
                    {
                        UpdateHandle(processName);
                    }

                    Thread.Sleep(1000);
                }
            });
        }
    }
}