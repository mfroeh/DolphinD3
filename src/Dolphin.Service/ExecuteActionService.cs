using Dolphin.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Dolphin.Service
{
    public class ExecuteActionService : EventSubscriberBase
    {
        private static readonly IList<ActionName> cancellableMacros = new List<ActionName>
        {
            ActionName.CubeConverterDualSlot,
            ActionName.CubeConverterSingleSlot,
            ActionName.UpgradeGem
        };

        private readonly IActionFinderService actionFinderService;
        private readonly Subscription<HotkeyPressedEvent> cancelExecutionSubscriber;
        private readonly Subscription<HotkeyPressedEvent> executeMacro;
        private readonly Subscription<HotkeyPressedEvent> executeMacroCancelable;
        private readonly Subscription<WorldInformationChangedEvent> executeSmartAction;

        private readonly IHandleService handleService;
        private readonly ISettingsService settingsService;
        private CancellationTokenSource tokenSource;

        public ExecuteActionService(IEventBus eventBus, ISettingsService settingsService, IActionFinderService actionFinderService, IHandleService handleService) : base(eventBus)
        {
            this.settingsService = settingsService;
            this.actionFinderService = actionFinderService;
            this.handleService = handleService;

            executeMacroCancelable = new Subscription<HotkeyPressedEvent>(ExecuteMacroCancelable);
            executeMacro = new Subscription<HotkeyPressedEvent>(ExecuteMacro);
            cancelExecutionSubscriber = new Subscription<HotkeyPressedEvent>(CancelExecution);
            executeSmartAction = new Subscription<WorldInformationChangedEvent>(OnWorldInformationChanged);

            SubscribeBus(executeMacro);
            SubscribeBus(cancelExecutionSubscriber);
            SubscribeBus(executeMacroCancelable);
            SubscribeBus(executeSmartAction);
        }

        public void CancelExecution(object o, HotkeyPressedEvent e)
        {
            if (e.PressedHotkey == settingsService.Settings.Hotkeys[ActionName.CancelAction])
            {
                InputHelper.SendKey(handleService.GetHandle("Diablo III64"), Keys.Escape);

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

            var handle = handleService.GetHandle("Diablo III64");
            if (handle == default ||
                e.PressedHotkey == settingsService.Settings.Hotkeys[ActionName.Pause] ||
                e.PressedHotkey == settingsService.Settings.Hotkeys[ActionName.CancelAction]) return;

            var macro = actionFinderService.FindAction(actionName, handle, tokenSource);

            Execute.AndForgetAsync(macro);
        }

        public void ExecuteMacroCancelable(object o, HotkeyPressedEvent e)
        {
            var actionName = settingsService.GetActionName(e.PressedHotkey);
            if (!cancellableMacros.Contains(actionName)) return;

            var handle = handleService.GetHandle("Diablo III64");
            if (handle == default ||
                e.PressedHotkey == settingsService.Settings.Hotkeys[ActionName.Pause] ||
                e.PressedHotkey == settingsService.Settings.Hotkeys[ActionName.CancelAction]) return;

            if (tokenSource == null)
            {
                tokenSource = new CancellationTokenSource();

                var macro = actionFinderService.FindAction(actionName, handle, tokenSource);

                ExecuteAndResetTokenSource(macro);
            }
        }

        private void ExecuteAndResetTokenSource(Action action)
        {
            Execute.AndForgetAsync(() =>
            {
                action.Invoke();
                Trace.WriteLine("Nulling now!");
                tokenSource = null;
            });
        }

        private void OnWorldInformationChanged(object o, WorldInformationChangedEvent @event)
        {
            var handle = handleService.GetHandle("Diablo III64");

            if (@event.NewOpenWindow == default || handle == default) return;

            if (@event.NewOpenWindow == Window.Urshi && settingsService.IsSmartActionEnabled(ActionName.Smart_UpgradeGem) && tokenSource == null)
            {
                tokenSource = new CancellationTokenSource();

                var macro = actionFinderService.FindAction(ActionName.Smart_UpgradeGem, handle, (int)@event.WindowExtraInformation[0]);

                ExecuteAndResetTokenSource(macro);
            }
            else if (@event.NewOpenWindow == Window.Kadala && settingsService.IsSmartActionEnabled(ActionName.Smart_Gamble) && tokenSource == null)
            {
                tokenSource = new CancellationTokenSource();

                var action = actionFinderService.FindAction(ActionName.Smart_Gamble, handle);

                ExecuteAndResetTokenSource(() =>
                {
                    action.Invoke();
                    Thread.Sleep(200);
                });
            }

            switch (@event.NewOpenWindow)
            {
                case Window.Obelisk:
                    if (settingsService.IsSmartActionEnabled(ActionName.Smart_OpenGrift))
                    {
                        Execute.AndForgetAsync(actionFinderService.FindAction(ActionName.Smart_OpenGrift, handle));
                    }
                    else if (settingsService.IsSmartActionEnabled(ActionName.Smart_OpenRift))
                    {
                        Execute.AndForgetAsync(actionFinderService.FindAction(ActionName.Smart_OpenRift, handle));
                    }
                    break;

                case Window.StartGame:
                    if (settingsService.IsSmartActionEnabled(ActionName.Smart_StartGame))
                    {
                        Execute.AndForgetAsync(actionFinderService.FindAction(ActionName.Smart_StartGame, handle));
                    }
                    break;

                case Window.AcceptGrift:
                    if (settingsService.IsSmartActionEnabled(ActionName.Smart_AcceptGriftPopup))
                    {
                        Execute.AndForgetAsync(actionFinderService.FindAction(ActionName.Smart_AcceptGriftPopup, handle));
                    }
                    break;
            }
        }
    }
}