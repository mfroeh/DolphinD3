using Dolphin.Enum;
using System;
using System.Threading;

namespace Dolphin.Service
{
    public class ActionFinderService : IActionFinderService
    {
        private readonly ActionService actionService;
        private readonly ILogService logService;
        private readonly ISettingsService settingsService;

        public ActionFinderService(ISettingsService settingsService, ILogService logService, ActionService actionService)
        {
            this.settingsService = settingsService;
            this.logService = logService;
            this.actionService = actionService;
        }

        public Action FindAction(ActionName actionName, IntPtr handle, params object[] @params)
        {
            if (actionName.IsCancelable())
            {
                throw new Exception($"Tried to recive Macro {actionName} as a non cancellable Macro.");
            }

            switch (actionName)
            {
                case ActionName.RightClick:
                    return () => actionService.RightClick(handle);

                case ActionName.LeftClick:
                    return () => actionService.LeftClick(handle);

                case ActionName.DropInventory:
                    var columns = settingsService.MacroSettings.SpareColumns;
                    var key = settingsService.GetKeybinding(CommandKeybinding.OpenInventory);
                    return () => actionService.DropInventory(handle, columns, key);

                case ActionName.Salvage:
                    return () => actionService.Salvage(handle, settingsService.MacroSettings.SpareColumns);

                case ActionName.OpenGrift:
                    return () => actionService.OpenGrift(handle);

                case ActionName.LeaveGame:
                    return () => actionService.LeaveGame(handle);

                case ActionName.Reforge:
                    return () => actionService.Reforge(handle, settingsService.MacroSettings.ConvertingSpeed);

                case ActionName.TravelAct1:
                case ActionName.TravelAct2:
                case ActionName.TravelAct34:
                case ActionName.TravelAct5:
                    return () => actionService.TravelTown(handle, actionName, settingsService.GetKeybinding(CommandKeybinding.OpenMap));

                case ActionName.TravelPool:
                    return () => actionService.TravelPool(handle);

                case ActionName.NormalizeDifficulty:
                    return () => actionService.LowerDifficulty(handle);

                case ActionName.SwapArmour:
                    key = settingsService.GetKeybinding(CommandKeybinding.OpenInventory);
                    var swapItemAmount = settingsService.MacroSettings.SwapItemsAmount;
                    return () => actionService.SwapArmour(handle, key, swapItemAmount);

                case ActionName.Gamble:
                    return () => actionService.Gamble(handle, settingsService.MacroSettings.SelectedGambleItem);

                case ActionName.Smart_AcceptGriftPopup:
                case ActionName.Smart_OpenRiftGrift:
                case ActionName.Smart_StartGame:
                case ActionName.Smart_Gamble:
                    return () => logService.AddEntry(this, $"Automatic actions are not yet implemented.", LogLevel.Debug);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public Action FindAction(ActionName actionName, IntPtr handle, CancellationTokenSource tokenSource, params object[] @params)
        {
            if (!actionName.IsCancelable())
            {
                throw new Exception($"Tried to recive non cancelable Macro {actionName} as a cancellable Macro.");
            }

            switch (actionName)
            {
                case ActionName.CubeConverterDualSlot:
                    var speed = settingsService.MacroSettings.ConvertingSpeed;
                    return () => actionService.CubeConverterDualSlot(handle, tokenSource, speed);

                case ActionName.CubeConverterSingleSlot:
                    speed = settingsService.MacroSettings.ConvertingSpeed;
                    return () => actionService.CubeConverterSingleSlot(handle, tokenSource, speed);

                case ActionName.UpgradeGem:
                    var key = settingsService.GetKeybinding(CommandKeybinding.TeleportToTown);
                    var isEmpowered = settingsService.MacroSettings.Empowered;
                    var pickYourself = settingsService.MacroSettings.PickGemYourself;
                    return () => actionService.UpgradeGem(handle, tokenSource, isEmpowered, pickYourself, key);

                case ActionName.Smart_UpgradeGem:
                    key = settingsService.GetKeybinding(CommandKeybinding.TeleportToTown);
                    return () => actionService.UpgradeGem(handle, tokenSource, (int)@params[0], key);

                default:
                    throw new NotImplementedException($"Cancellable Macro not implemented for {actionName}");
                    break;
            }
        }
    }
}