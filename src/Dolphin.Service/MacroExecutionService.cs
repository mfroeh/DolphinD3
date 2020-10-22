using Dolphin.Enum;
using System;
using System.Threading;

namespace Dolphin.Service
{
    public class MacroExecutionService : EventSubscriberBase
    {
        private readonly Subscription<HotkeyPressedEvent> cancelExecutionSubscriber;
        private readonly Subscription<HotkeyPressedEvent> executeMacro;
        private readonly Subscription<HotkeyPressedEvent> executeMacroCancelable;
        private bool executing;
        private CancellationTokenSource tokenSource;
        private readonly ISettingsService settingsService;
        private readonly IMacroFinderService macroFinderService;

        public MacroExecutionService(IEventBus eventBus, ISettingsService settingsService, IMacroFinderService macroFinderService) : base(eventBus)
        {
            this.settingsService = settingsService;
            this.macroFinderService = macroFinderService;

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
            if (e.PressedHotkey != settingsService.Settings.Hotkeys[ActionName.Pause]) return;

            var actionName = settingsService.GetActionName(e.PressedHotkey);

            var macro = MacroDonor.GiveMacro(actionName, settingsService.Settings);

            if (macro != null)
            {
                var handle = WindowHelper.GetHWND();

                Execute.AndForgetAsync(() => macro.Invoke(handle));
            }
        }

        // TODO: This might not need the lock / the lock is actually bad. Potentially all the delegates get staggered up.
        public void ExecuteMacroCancelable(HotkeyPressedEvent e, CancellationToken token)
        {
            if (e.PressedHotkey != settingsService.Settings.Hotkeys[ActionName.Pause]) return;

            var isExecuting = tokenSource != null;
            if (!executing)
            {
                var actionName = settingsService.GetActionName(e.PressedHotkey);
                var macro = MacroDonor.GiveCancellableMacro(actionName, settingsService.Settings);

                if (macro != null)
                {
                    var handle = WindowHelper.GetHWND();
                    tokenSource = new CancellationTokenSource();

                    Action action = () =>
                    {
                        executing = true;
                        macro.Invoke(handle, tokenSource);
                        tokenSource = null;
                        tokenSource.Dispose();
                        executing = false;
                    };

                    Execute.AndForgetAsync(action);
                }
            }
        }
    }
}