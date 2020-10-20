using System;
using System.Threading;

namespace Dolphin.Service
{
    public class HotkeyExecutionService : EventSubscriberBase
    {
        private readonly Subscription<HotkeyPressedEvent> cancelExecutionSubscriber;
        private readonly Subscription<HotkeyPressedEvent> executeMacro;
        private readonly Subscription<HotkeyPressedEvent> executeMacroCancelable;
        private bool executing;
        private object longMacroLock;
        private CancellationTokenSource tokenSource;

        public HotkeyExecutionService(IEventBus eventBus) : base(eventBus)
        {
            executeMacroCancelable = new Subscription<HotkeyPressedEvent>(ExecuteMacroCancelable);
            executeMacro = new Subscription<HotkeyPressedEvent>(ExecuteMacro);
            cancelExecutionSubscriber = new Subscription<HotkeyPressedEvent>(CancelExecution);
        }

        public void CancelExecution(object o, HotkeyPressedEvent e)
        {
            tokenSource.Cancel();
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
    }
}