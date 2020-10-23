using System;

namespace Dolphin
{
    public interface IHandleService
    {
        event EventHandler<HandleChangedEventArgs> HandleStatusChanged;

        IntPtr GetHandle(string processName = "Dialo III64");

        void UpdateHandle(string processName = "Diablo III64");
    }
}