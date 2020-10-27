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
        private static readonly IList<ActionName> cancellableMacros = new List<ActionName> { ActionName.CubeConverterDualSlot, ActionName.CubeConverterSingleSlot, ActionName.UpgradeGem };

        private readonly IActionFinderService actionFinderService;
        private readonly Subscription<HotkeyPressedEvent> cancelExecutionSubscriber;
        private readonly Subscription<HotkeyPressedEvent> executeMacro;
        private readonly Subscription<HotkeyPressedEvent> executeMacroCancelable;
        private readonly Subscription<WorldInformationChangedEvent> executeSmartAction;

        private readonly IHandleService handleService;
        private readonly ISettingsService settingsService;
        private CancellationTokenSource tokenSource;

        public ExecuteActionService(IEventBus eventBus, ISettingsService settingsService, IActionFinderService macroFinderService, IHandleService handleService) : base(eventBus)
        {
            this.settingsService = settingsService;
            this.actionFinderService = macroFinderService;
            this.handleService = handleService;

            Trace.WriteLine(handleService.GetHashCode());

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

            var macro = actionFinderService.FindAction(actionName, handle, tokenSource);

            Execute.AndForgetAsync(macro);
        }

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

                var macro = actionFinderService.FindAction(actionName, handle, tokenSource);

                Execute.AndForgetAsync(() =>
                {
                    macro.Invoke();
                    Trace.WriteLine("Nulling now!");
                    tokenSource = null;
                });
            }
        }

        private void OnWorldInformationChanged(object o, WorldInformationChangedEvent @event)
        {
            var handle = handleService.GetHandle();

            if (@event.NewOpenWindow == Window.Urshi && tokenSource == null)
            {
                tokenSource = new CancellationTokenSource();

                var macro = actionFinderService.FindAction(ActionName.Smart_UpgradeGem, handle, (int)@event.WindowExtraInformation[0]);

                Execute.AndForgetAsync(() =>
                {
                    macro.Invoke();
                    Trace.WriteLine("Nulling Smart now!");
                    tokenSource = null;
                });
            }
            else if (@event.NewOpenWindow != default)
            {
                Action action;

                switch (@event.NewOpenWindow)
                {
                    case Window.Kadala when settingsService.SmartActionSettings.GambleEnabled:
                        action = actionFinderService.FindAction(ActionName.Smart_Gamble, handle);
                        break;

                    case Window.Obelisk when settingsService.SmartActionSettings.StartRiftEnabled:
                        if (settingsService.SmartActionSettings.UseRift)
                        {
                            action = actionFinderService.FindAction(ActionName.Smart_OpenRift, handle);
                        }
                        else
                        {
                            action = actionFinderService.FindAction(ActionName.Smart_OpenGrift, handle);
                        }
                        break;

                    case Window.StartGame when settingsService.SmartActionSettings.StartGameEnabled:
                        action = actionFinderService.FindAction(ActionName.Smart_StartGame, handle);
                        break;

                    case Window.AcceptGrift when settingsService.SmartActionSettings.AcceptGriftEnabled:
                        action = actionFinderService.FindAction(ActionName.Smart_AcceptGriftPopup, handle);
                        break;

                    default:
                        throw new NotImplementedException();
                }

                Execute.AndForgetAsync(action);
            }
        }
    }
}