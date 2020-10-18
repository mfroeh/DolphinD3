using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolphin.Service
{
    public class ActionExecutionService : IActionExecutionService
    {
        private readonly IModelService modelService;

        public ActionExecutionService(IModelService modelService)
        {
            this.modelService = modelService;
        }

        public void Execute(Action action, Func<bool> condition)
        {
            if (condition.Invoke())
                action.Invoke();
        }

        public void Execute<T>(Action action, Func<T, bool> condition)
        {
            throw new NotImplementedException();
        }

        public void Execute(Action action, Func<Player, World, bool> condition)
        {
            if (condition.Invoke(modelService.Player, modelService.World))
                action.Invoke();
        }

        public async Task Execute(Func<Task> action, Func<bool> condition)
        {
            if (condition.Invoke())
                await action.Invoke();
        }

        public async Task Execute<T>(Func<Task> action, Func<T, bool> condition)
        {
            throw new NotImplementedException();
        }

        public async Task Execute(Func<Task> action, Func<Player, World, bool> condition)
        {
            if (condition.Invoke(modelService.Player, modelService.World))
                await action.Invoke();
        }
    }
}
