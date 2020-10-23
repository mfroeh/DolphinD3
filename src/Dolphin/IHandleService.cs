using System;

namespace Dolphin
{
    public interface IHandleService
    {
        IntPtr GetHandle(string processName = "Dialo III64");

        void UpdateHandle(string processName = "Diablo III64");
    }
}