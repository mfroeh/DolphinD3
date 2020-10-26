using Dolphin.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace Dolphin.Service
{
    public class MacroExecutionService : EventSubscriberBase
    {
        private static readonly IList<ActionName> cancellableMacros = new List<ActionName> { ActionName.CubeConverterDualSlot, ActionName.CubeConverterSingleSlot, ActionName.UpgradeGem };

        private readonly Subscription<HotkeyPressedEvent> cancelExecutionSubscriber;
        private readonly Subscription<HotkeyPressedEvent> executeMacro;
        private readonly Subscription<HotkeyPressedEvent> executeMacroCancelable;
        private readonly IHandleService handleService;
        private readonly IActionFinderService macroFinderService;

        private readonly ISettingsService settingsService;
        private CancellationTokenSource tokenSource;

        public MacroExecutionService(IEventBus eventBus, ISettingsService settingsService, IActionFinderService macroFinderService, IHandleService handleService) : base(eventBus)
        {
            this.settingsService = settingsService;
            this.macroFinderService = macroFinderService;
            this.handleService = handleService;

            Trace.WriteLine(handleService.GetHashCode());

            executeMacroCancelable = new Subscription<HotkeyPressedEvent>(ExecuteMacroCancelable);
            executeMacro = new Subscription<HotkeyPressedEvent>(ExecuteMacro);
            cancelExecutionSubscriber = new Subscription<HotkeyPressedEvent>(CancelExecution);

            SubscribeBus(executeMacro);
            SubscribeBus(cancelExecutionSubscriber);
            SubscribeBus(executeMacroCancelable);
        }

        /// <summary>
        /// Cancels the current cancellable Action
        /// TODO: Test this
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public void CancelExecution(object o, HotkeyPressedEvent e)
        {
            if (e.PressedHotkey == settingsService.Settings.Hotkeys[ActionName.CancelAction])
            {
                InputHelper.SendKey(handleService.GetHandle(), Keys.Escape);

                if (tokenSource != null)
                {
                    tokenSource.Cancel();
                }
            }
        }

        public void ExecuteMacro(object o, HotkeyPressedEvent e)
        {
            var actionName = settingsService.GetActionName(e.PressedHotkey);

            if (cancellableMacros.Contains(actionName)) return;

            var handle = handleService.GetHandle();

            if (handle == default) return;
            if (e.PressedHotkey == settingsService.Settings.Hotkeys[ActionName.Pause]
                || e.PressedHotkey == settingsService.Settings.Hotkeys[ActionName.CancelAction])
            {
                return;
            }

            var macro = macroFinderService.FindAction(actionName, handle, tokenSource);

            Execute.AndForgetAsync(macro);
        }

        // TODO: This might not need the lock / the lock is actually bad. Potentially all the delegates get staggered up.
        public void ExecuteMacroCancelable(object o, HotkeyPressedEvent e)
        {
            var actionName = settingsService.GetActionName(e.PressedHotkey);

            if (!cancellableMacros.Contains(actionName)) return;

            var handle = handleService.GetHandle();

            if (handle == default) return;
            if (e.PressedHotkey == settingsService.Settings.Hotkeys[ActionName.Pause]
                || e.PressedHotkey == settingsService.Settings.Hotkeys[ActionName.CancelAction])
            {
                return;
            }

            if (tokenSource == null)
            {
                tokenSource = new CancellationTokenSource();

                var macro = macroFinderService.FindAction(actionName, handle, tokenSource);

                Execute.AndForgetAsync(() =>
                {
                    macro.Invoke();
                    Trace.WriteLine("Nulling now!");
                    tokenSource = null;
                });
            }
        }
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
    }

    public static class CancellationTokenSourceExtensionMethods
    {
        public static bool IsDiposedOrNull(this CancellationTokenSource source)
        {
            try
            {
                bool y = source.IsCancellationRequested;
            }
            catch (Exception)
            {
                return true;
            }
            return false;
        }
    }
}