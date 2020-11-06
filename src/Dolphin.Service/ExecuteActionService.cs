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
        private readonly ISettingsService settingsService;
        private CancellationTokenSource tokenSource;

        public ExecuteActionService(IEventBus eventBus, ISettingsService settingsService, IActionFinderService actionFinderService, IHandleService handleService) : base(eventBus)
        {
            this.settingsService = settingsService;
            this.actionFinderService = actionFinderService;
            this.handleService = handleService;

            var executeAction = new Subscription<HotkeyPressedEvent>(OnHotkeyPressed);
            var executeSmartAction = new Subscription<WorldInformationChangedEvent>(OnWorldInformationChanged);

            SubscribeBus(executeAction);
            SubscribeBus(executeSmartAction);
        }

        private void CancelAction(IntPtr handle)
        {
            InputHelper.SendKey(handle, Keys.Escape);

            if (tokenSource != null)
            {
                tokenSource.Cancel();
            }
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
            if (handle?.Handle == default) return;

            var actionName = settingsService.GetActionName(e.PressedHotkey);
            if (actionName.IsSuspensionAction()) return;

            if (actionName == ActionName.CancelAction || actionName == ActionName.Pause)
            {
                CancelAction(handle.Handle);
            }
            else if (actionName.IsCancelable() && tokenSource == null)
            {
                tokenSource = new CancellationTokenSource();
                var macro = actionFinderService.FindAction(actionName, handle.Handle, tokenSource);

                ExecuteAndResetTokenSourceAsync(macro);
            }
            else if (!actionName.IsSmartAction())
            {
                var macro = actionFinderService.FindAction(actionName, handle.Handle);

                Execute.AndForgetAsync(macro);
            }
        }

        private void OnWorldInformationChanged(object o, WorldInformationChangedEvent @event)
        {
            var handle = handleService.GetHandle("Diablo III64");
            if (handle?.Handle == default || @event.NewOpenWindow == default) return;

            var actionName = settingsService.GetSmartActionName(@event.NewOpenWindow);
            if (actionName == ActionName.Smart_UpgradeGem && tokenSource == null)
            {
                tokenSource = new CancellationTokenSource();
                var macro = actionFinderService.FindAction(actionName, handle.Handle, tokenSource, (int)@event.WindowExtraInformation[0]);

                ExecuteAndResetTokenSourceAsync(macro);
            }
            else if (actionName == ActionName.Smart_Gamble && tokenSource == null)
            {
                tokenSource = new CancellationTokenSource();
                var action = actionFinderService.FindAction(ActionName.Smart_Gamble, handle.Handle);

                ExecuteAndResetTokenSourceAsync(() =>
                {
                    action.Invoke();
                    Thread.Sleep(200);
                });
            }
            else if (actionName != default)
            {
                var action = actionFinderService.FindAction(actionName, handle.Handle);

                Execute.AndForgetAsync(action);
            }
        }
    }
}