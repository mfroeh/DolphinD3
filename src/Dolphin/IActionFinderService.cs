using Dolphin.Enum;
using System;
using System.Threading;

namespace Dolphin
{
    public interface IActionFinderService
    {
        Action FindAction(ActionName actionName, IntPtr handle, params object[] @params);

        Action FindAction(ActionName actionName, IntPtr handle, CancellationTokenSource tokenSource, params object[] @params);
    }
}