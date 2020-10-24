using Dolphin.Enum;
using System;
using System.Threading;

namespace Dolphin
{
    public interface IActionFinderService
    {
        Action FindAction(ActionName actionName, IntPtr handle);

        Action FindAction(ActionName actionName, IntPtr handle, CancellationTokenSource tokenSource);
    }
}