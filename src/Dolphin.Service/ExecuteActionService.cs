using Dolphin.Enum;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dolphin.Service
{
    public class ExecuteActionService : EventSubscriberBase
    {
        private readonly IActionFinderService actionFinderService;

        private readonly IHandleService handleService;
        private readonly ILogService logService;
        private readonly ISettingsService settingsService;
        private CancellationTokenSource tokenSource;

        public ExecuteActionService(IEventBus eventBus, ISettingsService settingsService, ILogService logService, IActionFinderService actionFinderService, IHandleService handleService) : base(eventBus)
        {
            this.settingsService = settingsService;
            this.actionFinderService = actionFinderService;
            this.handleService = handleService;
            this.logService = logService;

            var executeAction = new Subscription<HotkeyPressedEvent>(OnHotkeyPressed);
            var executeSmartAction = new Subscription<WorldInformationChangedEvent>(OnWorldInformationChanged);

            SubscribeBus(executeAction);
            SubscribeBus(executeSmartAction);
        }

        private Task ExecuteAndResetTokenSourceAsync(Action action)
        {
            return Execute.AndForgetAsync(() =>
            {
                action.Invoke();
                Trace.WriteLine("Nulling now!");
                tokenSource = null;
            });
        }

        private void OnHotkeyPressed(object o, HotkeyPressedEvent e)
        {
            var handle = handleService.GetHandle("Diablo III64");
            if (handle.IsDefault()) return;

            var actionName = settingsService.GetActionName(e.PressedHotkey);
            if (actionName == ActionName.CancelAction || actionName == ActionName.Pause)
            {
                if (e.PressedHotkey == settingsService.Settings.Hotkeys[ActionName.CancelAction])
                {
                    InputHelper.SendKey(handle.Handle, Keys.Escape);
                }
                if (tokenSource != null)
                {
                    tokenSource.Cancel();
                    logService.AddEntry(this, $"Cancelling current action... [{actionName}][{e.PressedHotkey}]");
                }
            }
            else if (actionName.IsCancelable())
            {
                if (tokenSource == null)
                {
                    tokenSource = new CancellationTokenSource();
                    var macro = actionFinderService.FindAction(actionName, handle.Handle, tokenSource);

                    ExecuteAndResetTokenSourceAsync(macro);
                    logService.AddEntry(this, $"Beginning to execute... [{actionName}][{e.PressedHotkey}]");
                }
                else
                {
                    tokenSource.Cancel();
                    logService.AddEntry(this, $"Cancelling current action... [{actionName}][{e.PressedHotkey}]");
                }
            }
            else if (!actionName.IsSuspensionAction())
            {
                var macro = actionFinderService.FindAction(actionName, handle.Handle);

                Execute.AndForgetAsync(macro);
                logService.AddEntry(this, $"Beginning to execute... [{actionName}][{e.PressedHotkey}]");
            }
        }

        private void OnWorldInformationChanged(object o, WorldInformationChangedEvent @event)
        {
            var handle = handleService.GetHandle("Diablo III64");
            if (handle.IsDefault() || @event.NewOpenWindow == default) return;

            var actionName = settingsService.GetSmartActionName(@event.NewOpenWindow);
            if (actionName == SmartActionName.UpgradeGem && tokenSource == null)
            {
                tokenSource = new CancellationTokenSource();
                var macro = actionFinderService.FindSmartAction(actionName, handle.Handle, tokenSource, (int)@event.WindowExtraInformation[0]);

                ExecuteAndResetTokenSourceAsync(macro);
                logService.AddEntry(this, $"Beginning to execute action... [{actionName}][{@event.NewOpenWindow}]");
            }
            else if (actionName == SmartActionName.Gamble && tokenSource == null)
            {
                tokenSource = new CancellationTokenSource();
                var action = actionFinderService.FindSmartAction(actionName, handle.Handle);

                ExecuteAndResetTokenSourceAsync(() =>
                {
                    action.Invoke();
                    Thread.Sleep(200);
                });
                logService.AddEntry(this, $"Beginning to execute action... [{actionName}][{@event.NewOpenWindow}]");
            }
            else if (actionName != default)
            {
                var action = actionFinderService.FindSmartAction(actionName, handle.Handle);

                Execute.AndForgetAsync(action);
                logService.AddEntry(this, $"Beginning to execute action... [{actionName}][{@event.NewOpenWindow}]");
            }
        }
    }
}