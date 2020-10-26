using System;
using System.Threading.Tasks;

namespace Dolphin
{
    public interface IHandleService
    {
        event EventHandler<HandleChangedEventArgs> HandleStatusChanged;

        IntPtr GetHandle(string processName = "Diablo III64");

        void UpdateHandle(string processName = "Diablo III64");

        Task MainLoop();
    }
}