using Dolphin.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dolphin
{
    public interface IMacroFinderService
    {
        Action FindMacro(ActionName actionName);

        Action FindMacroCancelable(ActionName actionName, CancellationTokenSource tokenSource);

    }
}
