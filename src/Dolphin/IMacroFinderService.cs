using Dolphin.Enum;
using System;
using System.Threading;

namespace Dolphin
{
    public interface IMacroFinderService
    {
        Action FindAction(ActionName actionName);

        Action FindAction(ActionName actionName, CancellationTokenSource tokenSource);
    }
}