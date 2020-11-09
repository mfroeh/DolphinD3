using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dolphin.Service
{
    public class HandleService : IHandleService
    {
        private IDictionary<string, WindowInformation> handles = new Dictionary<string, WindowInformation>();
        private readonly ILogService logService;

        public event EventHandler<HandleChangedEventArgs> HandleStatusChanged;

        public HandleService(ILogService logService)
        {
            this.logService = logService;
        }

        public WindowInformation GetHandle(string processName)
        {
            handles.TryGetValue(processName, out WindowInformation handle);

            return handle;
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

        public void UpdateHandle(string processName)
        {
            var hwnd = WindowHelper.GetHWND(processName);
            var currentHandle = GetHandle(processName);

            var processId = WindowHelper.GetWindowThreadProcessId(hwnd);
            var clientRect = WindowHelper.GetClientRect(hwnd);

            if (currentHandle == default ||
                currentHandle.Handle != hwnd ||
                currentHandle.ProcessId != processId ||
                currentHandle.ClientRectangle != clientRect)
            {
                var handle = new WindowInformation
                {
                    ClientRectangle = WindowHelper.GetClientRect(hwnd),
                    Handle = hwnd,
                    ProcessId = WindowHelper.GetWindowThreadProcessId(hwnd),
                    ProcessName = processName
                };
                handles[processName] = handle;

                HandleStatusChanged?.Invoke(this, new HandleChangedEventArgs { ProcessName = processName, NewHandle = handle });
                logService.AddEntry(this, $"Handle information changed [{processName}][{hwnd}][{clientRect}]");
            }
        }
    }
}