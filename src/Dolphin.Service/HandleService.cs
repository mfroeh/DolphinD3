using System;
using System.Collections.Generic;

namespace Dolphin.Service
{
    public class HandleService : IHandleService
    {
        private IDictionary<string, IntPtr> handles = new Dictionary<string, IntPtr>();

        public IntPtr GetHandle(string processName = "Dialo III64")
        {
            handles.TryGetValue(processName, out IntPtr handle);

            return handle;
        }

        public void UpdateHandle(string processName = "Diablo III64")
        {
            handles[processName] = WindowHelper.GetHWND(processName);
        }
    }
}
