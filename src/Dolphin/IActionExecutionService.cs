using System;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace Dolphin
{
    public interface IActionExecutionService
    {
        Task Execute(Func<Task> action, Func<bool> condition);
        Task Execute<T>(Func<Task> action, Func<T, bool> condition);
        Task Execute(Func<Task> action, Func<Player, World, bool> condition);
        void Execute(Action action, Func<bool> condition);
        void Execute<T>(Action action, Func<T, bool> condition);
        void Execute(Action action, Func<Player, World, bool> condition);
    }
}