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

        public Action FindAction(ActionName actionName, IntPtr handle)
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
                    return () => actionService.TravelTown(handle, 1, settingsService.GetKeybinding(CommandKeybinding.OpenMap));

                case ActionName.TravelAct2:
                    return () => actionService.TravelTown(handle, 2, settingsService.GetKeybinding(CommandKeybinding.OpenMap));

                case ActionName.TravelAct34:
                    return () => actionService.TravelTown(handle, 3, settingsService.GetKeybinding(CommandKeybinding.OpenMap));

                case ActionName.TravelAct5:
                    return () => actionService.TravelTown(handle, 5, settingsService.GetKeybinding(CommandKeybinding.OpenMap));

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

                default:
                    throw new NotImplementedException();
            }
        }

        public Action FindAction(ActionName actionName, IntPtr handle, CancellationTokenSource tokenSource)
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

                case ActionName.SkillCastLoop:
                    var configuration = settingsService.SkillCastSettings.SelectedSkillCastConfiguration;
                    var keybindings = settingsService.Settings.SkillKeybindings;
                    return () => actionService.SkillCastLoop(handle, tokenSource, configuration, keybindings);

                default:
                    throw new NotImplementedException($"Cancellable Macro not implemented for {actionName}");
                    break;
            }
        }

        public Action FindSmartAction(SmartActionName actionName, IntPtr handle, params object[] @params)
        {
            if (actionName.IsCancelable())
            {
                throw new Exception($"Tried to recive cancelable smart action {actionName} as a non cancellable Macro.");
            }

            switch (actionName)
            {
                case SmartActionName.AcceptGriftPopup:
                case SmartActionName.OpenRiftGrift:
                case SmartActionName.StartGame:
                case SmartActionName.Gamble:
                    return () => logService.AddEntry(this, $"Automatic actions are not yet implemented.", LogLevel.Debug);

                default:
                    throw new NotImplementedException();
            }
        }

        public Action FindSmartAction(SmartActionName actionName, IntPtr handle, CancellationTokenSource tokenSource, params object[] @params)
        {
            if (!actionName.IsCancelable())
            {
                throw new Exception($"Tried to recive non cancelable smart action {actionName} as a cancellable Macro.");
            }

            switch (actionName)
            {
                case SmartActionName.UpgradeGem:
                    var key = settingsService.GetKeybinding(CommandKeybinding.TeleportToTown);
                    return () => actionService.UpgradeGem(handle, tokenSource, (int)@params[0], key);

                default:
                    throw new NotImplementedException();
            }
        }
    }
}