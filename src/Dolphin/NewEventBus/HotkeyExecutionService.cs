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
        private readonly Subscription<HotkeyPressedEvent> executeMacroLong;
        private readonly Subscription<HotkeyPressedEvent> executeHotkeyShort;
        private readonly Subscription<HotkeyPressedEvent> cancelExecutionSubscriber;

        private object longMacroLock;

        private readonly IEventBus eventBus;

        public HotkeyExecutionService(IEventBus eventBus)
        {
            this.eventBus = eventBus;

            executeMacroLong = new Subscription<HotkeyPressedEvent>(ExecuteMacroLong);
            executeHotkeyShort = new Subscription<HotkeyPressedEvent>(ExecuteMacroShort);
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

        public void ExecuteMacroShort(object o, HotkeyPressedEvent e)
        {
            // Execute macro
        }

        // TODO: This might not need the lock / the lock is actually bad. Potentially all the delegates get staggered up.
        public void ExecuteMacroLong(HotkeyPressedEvent e, CancellationToken token)
        {
            if (longMacroLock == null)
            {
                lock (longMacroLock)
                {
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

        public void CancelExecution(object o, HotkeyPressedEvent e)
        {
            // cancel all 
            executeMacroLong.CancelReaction();
        }
    }
}
