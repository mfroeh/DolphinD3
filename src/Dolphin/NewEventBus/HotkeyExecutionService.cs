using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dolphin.NewEventBus
{
    public class HotkeyExecutionService : IEventSubscriber
    {
        private readonly Subscription<HotkeyPressedEvent> executeMacroCancelable;
        private readonly Subscription<HotkeyPressedEvent> executeMacro;
        private readonly Subscription<HotkeyPressedEvent> cancelExecutionSubscriber;

        private object longMacroLock;
        private bool executing;

        private CancellationTokenSource tokenSource;

        private readonly IEventBus eventBus;

        public HotkeyExecutionService(IEventBus eventBus)
        {
            this.eventBus = eventBus;

            executeMacroCancelable = new Subscription<HotkeyPressedEvent>(ExecuteMacroCancelable);
            executeMacro = new Subscription<HotkeyPressedEvent>(ExecuteMacro);
            cancelExecutionSubscriber = new Subscription<HotkeyPressedEvent>(CancelExecution);
        }

        public void Subscribe<T>(Subscription<T> subscription) where T : IEvent
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe<T>(Subscription<T> subscription) where T : IEvent
        {
            throw new NotImplementedException();
        }

        public void ExecuteMacro(object o, HotkeyPressedEvent e)
        {
            // Execute macro
        }

        // TODO: This might not need the lock / the lock is actually bad. Potentially all the delegates get staggered up.
        public void ExecuteMacroCancelable(HotkeyPressedEvent e, CancellationToken token)
        {
            var isExecuting = tokenSource != null;
            if (!executing)
            {
                executing = true;
                tokenSource = new CancellationTokenSource();
                // Do stuff x.GetMacro(tokenSource);
                // action GetMacro(TokenSOurce tokenSource) {
                //
                //  checkCanceled(tokenSource.Token)
                //      if (token.isCanceld)
                //          tokenSource.dispose()
                //          tokenSource = null
                //          isExecuting = false;
                //          return
                //      
                //  tokenSource.dispose()
                //  tokenSource = null
                //  isExecuting = false;
                //}
                //
                longMacroLock = new object();
                Action a = () =>
                {
                    Console.WriteLine("Executing the macro.");    // Execute the macro
                    longMacroLock = null;
                };
                a.Invoke();
            }
        }

        public void CancelExecution(object o, HotkeyPressedEvent e)
        {
            tokenSource.Cancel();
        }
    }
}
