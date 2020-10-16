using System;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace Dolphin
{
    public interface IActionExecutionService
    {
        Task Execute(Action action, Func<bool> condition);
        Task Execute<T>(Action action, Func<T, bool> condition);
        Task Execute<T1, T2>(Action action, Func<T1, T2, bool> condition);

    }
}