using Dolphin.Enum;
using System;
using System.Threading;
using System.Windows.Forms;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin.Service
{
    public class MacroExecutionService : EventSubscriberBase
    {
        private readonly Subscription<HotkeyPressedEvent> cancelExecutionSubscriber;
        private readonly Subscription<HotkeyPressedEvent> executeMacro;
        // private readonly Subscription<HotkeyPressedEvent> executeMacroCancelable;
        private readonly IMacroFinderService macroFinderService;
        private readonly ISettingsService settingsService;
        private bool executing;
        private CancellationTokenSource tokenSource;

        public MacroExecutionService(IEventBus eventBus, ISettingsService settingsService, IMacroFinderService macroFinderService) : base(eventBus)
        {
            this.settingsService = settingsService;
            this.macroFinderService = macroFinderService;

            // executeMacroCancelable = new Subscription<HotkeyPressedEvent>(ExecuteMacroCancelable);
            executeMacro = new Subscription<HotkeyPressedEvent>(ExecuteMacro);
            cancelExecutionSubscriber = new Subscription<HotkeyPressedEvent>(CancelExecution);
        }

        /// <summary>
        /// Cancels the current cancellable Action
        /// TODO: Test this
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public void CancelExecution(object o, HotkeyPressedEvent e)
        {
            if (e.PressedHotkey == new Hotkey(Keys.Escape))
            {
                tokenSource.Cancel();
            }
        }

        public void ExecuteMacro(object o, HotkeyPressedEvent e)
        {
            if (e.PressedHotkey != settingsService.Settings.Hotkeys[ActionName.Pause]) return;

            var actionName = settingsService.GetActionName(e.PressedHotkey);
            var handle = WindowHelper.GetHWND();

            if (handle != IntPtr.Zero)
            {
                var macro = macroFinderService.FindAction(actionName, handle, tokenSource);
                Execute.AndForgetAsync(macro);
            }
        }

        // TODO: This might not need the lock / the lock is actually bad. Potentially all the delegates get staggered up.
        public void ExecuteMacroCancelable(HotkeyPressedEvent e, CancellationTokenSource tokenSource)
        {
            //if (e.PressedHotkey != settingsService.Settings.Hotkeys[ActionName.Pause]) return;

            //var isExecuting = tokenSource != null;
            //if (!executing)
            //{
            //    var actionName = settingsService.GetActionName(e.PressedHotkey);
            //    var macro = MacroDonor.GiveCancellableMacro(actionName, settingsService.Settings);

            //    if (macro != null)
            //    {
            //        var handle = WindowHelper.GetHWND();
            //        tokenSource = new CancellationTokenSource();

            //        Action action = () =>
            //        {
            //            executing = true;
            //            macro.Invoke(handle, tokenSource);
            //            tokenSource = null;
            //            tokenSource.Dispose();
            //            executing = false;
            //        };

            //        Execute.AndForgetAsync(action);
            //    }
            //}
        }
    }
}