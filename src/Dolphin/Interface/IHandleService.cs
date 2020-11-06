using System;
using System.Threading.Tasks;

namespace Dolphin
{
    public interface IHandleService
    {
        event EventHandler<HandleChangedEventArgs> HandleStatusChanged;

        WindowInformation GetHandle(string processName);

        Task MainLoop(params string[] processNames);

        void UpdateHandle(string processName);
    }
}