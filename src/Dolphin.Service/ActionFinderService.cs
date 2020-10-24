using Dolphin.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Dolphin.Service
{
    public class ActionFinderService : IActionFinderService
    {
        private static readonly IList<ActionName> cancellableMacros = new List<ActionName> { ActionName.CubeConverterDualSlot, ActionName.CubeConverterSingleSlot, ActionName.UpgradeGem };
        private readonly ILogService logService;
        private readonly ISettingsService settingsService;
        private readonly ITransformService transformService;
        private readonly ITravelInformationService travelService;

        public ActionFinderService(ISettingsService settingsService, ILogService logService, ITravelInformationService travelService, ITransformService transformService)
        {
            this.settingsService = settingsService;
            this.logService = logService;
            this.travelService = travelService;
            this.transformService = transformService;
        }

        public Action FindAction(ActionName actionName, IntPtr handle)
        {
            if (cancellableMacros.Contains(actionName))
            {
                logService.AddEntry(this, $"Tried to recive Macro {actionName} as a non cancellable Macro.");
            }

            switch (actionName)
            {
                case ActionName.RightClick:
                    return () => RightClick(handle);

                case ActionName.LeftClick:
                    return () => LeftClick(handle);

                case ActionName.DropInventory:
                    return () => DropInventory(handle);

                case ActionName.Salvage:
                    return () => Salvage(handle);

                case ActionName.OpenGrift:
                    return () => OpenGrift(handle);

                case ActionName.LeaveGame:
                    return () => LeaveGame(handle);

                case ActionName.Reforge:
                    return () => Reforge(handle);

                case ActionName.TravelAct1:
                case ActionName.TravelAct2:
                case ActionName.TravelAct34:
                case ActionName.TravelAct5:
                    return () => TravelTown(handle, actionName);

                case ActionName.TravelPool:
                    return () => TravelPool(handle);

                case ActionName.NormalizeDifficulty:
                    return () => LowerDifficulty(handle);

                case ActionName.SwapArmour:
                    return () => SwapArmour(handle);

                case ActionName.Gamble:
                    return () => Gamble(handle);
                case ActionName.AcceptGriftPopup:
                case ActionName.OpenRift:
                case ActionName.StartGame:
                    return () => logService.AddEntry(this, $"Automatic actions are not yet implemented.");
                    break;
                default:
                    throw new NotImplementedException($"Non cancellable Macro not implemented for {actionName}");
                    break;
            }
        }

        public Action FindAction(ActionName actionName, IntPtr handle, CancellationTokenSource tokenSource)
        {
            if (!cancellableMacros.Contains(actionName))
            {
                logService.AddEntry(this, $"Tried to recive cancellable Macro {actionName} which is not avalible as a cancellable macro. This will still work.");

                return FindAction(actionName, handle);
            }

            switch (actionName)
            {
                case ActionName.CubeConverterDualSlot:
                    return () => CubeConverterDualSlot(handle, tokenSource);

                case ActionName.CubeConverterSingleSlot:
                    return () => CubeConverterSingleSlot(handle, tokenSource);

                case ActionName.UpgradeGem:
                    return () => UpgradeGem(handle, tokenSource);

                default:
                    throw new NotImplementedException($"Cancellable Macro not implemented for {actionName}");
                    break;
            }
        }

        private static bool IsCancelled(CancellationTokenSource tokenSource)
        {
            try
            {
                if (tokenSource.Token.IsCancellationRequested)
                {
                    Trace.WriteLine("Cancellation requested!");
                }

                return tokenSource.Token.IsCancellationRequested;
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Exception when trying to see if TokenSource was canceled, {ex}");

                return true;
            }
        }

        private static void LeftClick(IntPtr handle)
        {
            InputHelper.SendClickAtCursorPos(handle, MouseButtons.Left);
            Thread.Sleep(10);
        }

        private static void RightClick(IntPtr handle)
        {
            InputHelper.SendClickAtCursorPos(handle, MouseButtons.Right);
            Thread.Sleep(10);
        }

        private void CubeConverterDualSlot(IntPtr handle, CancellationTokenSource tokenSource)
        {
            var item = transformService.TransformCoordinate(CommonCoordinate.InventoryTopLeftSpot, RelativeCoordinatePosition.Right);
            var step = transformService.TransformSize(CommonSize.InventoryStepSize);
            var fill = transformService.TransformCoordinate(CommonCoordinate.CubeFill);
            var transmute = transformService.TransformCoordinate(CommonCoordinate.CubeTransmute);
            var backwards = transformService.TransformCoordinate(CommonCoordinate.CubeBackwards);
            var forwards = transformService.TransformCoordinate(CommonCoordinate.CubeForwards);

            var itemClickDelay = 130;
            var transmuteDelay = 130;
            var fillDelay = 0; // Care, Thread.Sleep(0)!
            var backwardsDelay = 0; // Care, Thread.Sleep(0)!
            if (settingsService.MacroSettings.ConvertingSpeed == ConvertingSpeed.Slow)
            {
                itemClickDelay = 130;
                fillDelay = 100;
                transmuteDelay = 130;
                backwardsDelay = 100;
            }
            else if (settingsService.MacroSettings.ConvertingSpeed == ConvertingSpeed.Fast)
            {
                itemClickDelay = 60;
                transmuteDelay = 60;
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (IsCancelled(tokenSource)) return;
                    InputHelper.SendClick(handle, MouseButtons.Right, item.X + j * step.Width, item.Y + i * step.Height * 2);
                    Thread.Sleep(itemClickDelay);

                    if (IsCancelled(tokenSource)) return;
                    InputHelper.SendClick(handle, MouseButtons.Left, fill);
                    Thread.Sleep(fillDelay);

                    if (IsCancelled(tokenSource)) return;
                    InputHelper.SendClick(handle, MouseButtons.Left, transmute);
                    Thread.Sleep(transmuteDelay);

                    if (IsCancelled(tokenSource)) return;
                    InputHelper.SendClick(handle, MouseButtons.Left, backwards);
                    Thread.Sleep(backwardsDelay);

                    if (IsCancelled(tokenSource)) return;
                    InputHelper.SendClick(handle, MouseButtons.Left, forwards);
                }
            }
        }

        private void CubeConverterSingleSlot(IntPtr handle, CancellationTokenSource tokenSource)
        {
            var item = transformService.TransformCoordinate(CommonCoordinate.InventoryTopLeftSpot, RelativeCoordinatePosition.Right);
            var step = transformService.TransformSize(CommonSize.InventoryStepSize);
            var fill = transformService.TransformCoordinate(CommonCoordinate.CubeFill);
            var transmute = transformService.TransformCoordinate(CommonCoordinate.CubeTransmute);
            var backwards = transformService.TransformCoordinate(CommonCoordinate.CubeBackwards);
            var forwards = transformService.TransformCoordinate(CommonCoordinate.CubeForwards);

            var itemClickDelay = 130;
            var transmuteDelay = 130;
            var fillDelay = 0; // Care, Thread.Sleep(0)!
            var backwardsDelay = 0; // Care, Thread.Sleep(0)!
            if (settingsService.MacroSettings.ConvertingSpeed == ConvertingSpeed.Slow)
            {
                itemClickDelay = 130;
                fillDelay = 100;
                transmuteDelay = 130;
                backwardsDelay = 100;
            }
            else if (settingsService.MacroSettings.ConvertingSpeed == ConvertingSpeed.Fast)
            {
                itemClickDelay = 60;
                transmuteDelay = 60;
            }

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (IsCancelled(tokenSource)) return;
                    InputHelper.SendClick(handle, MouseButtons.Right, item.X + j * step.Width, item.Y + i * step.Height);
                    Thread.Sleep(itemClickDelay);

                    if (IsCancelled(tokenSource)) return;
                    InputHelper.SendClick(handle, MouseButtons.Left, fill);
                    Thread.Sleep(fillDelay);

                    if (IsCancelled(tokenSource)) return;
                    InputHelper.SendClick(handle, MouseButtons.Left, transmute);
                    Thread.Sleep(transmuteDelay);

                    if (IsCancelled(tokenSource)) return;
                    InputHelper.SendClick(handle, MouseButtons.Left, backwards);
                    Thread.Sleep(backwardsDelay);

                    if (IsCancelled(tokenSource)) return;
                    InputHelper.SendClick(handle, MouseButtons.Left, forwards);
                }
            }
        }

        private void DropInventory(IntPtr handle)
        {
            var item = transformService.TransformCoordinate(CommonCoordinate.InventoryTopRightSpot, RelativeCoordinatePosition.Right);
            var step = transformService.TransformSize(CommonSize.InventoryStepSize);

            var columnIterations = 10 - settingsService.MacroSettings.SpareColumns;

            InputHelper.SendKey(handle, settingsService.GetKeybinding(Command.OpenInventory));

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < columnIterations; j++)
                {
                    InputHelper.SendClick(handle, MouseButtons.Left, item.X - j * step.Width, item.Y + i * step.Height);
                    InputHelper.SendClickAtCursorPos(handle, MouseButtons.Left);
                }
            }

            InputHelper.SendKey(handle, settingsService.GetKeybinding(Command.OpenInventory));
        }

        private void Gamble(IntPtr handle)
        {
            var gambleItem = settingsService.MacroSettings.SelectedGambleItem;
            var _tab = travelService.GetKadalaTabCoordinate(gambleItem);
            var _item = travelService.GetKadalaItemCoordinate(gambleItem);

            var tab = transformService.TransformCoordinate(_tab);
            var item = transformService.TransformCoordinate(_item);

            InputHelper.SendClick(handle, MouseButtons.Left, tab);

            for (int i = 0; i < 60; i++)
            {
                InputHelper.SendClick(handle, MouseButtons.Left, item);
            }
        }

        private void LeaveGame(IntPtr handle)
        {
            var leave = transformService.TransformCoordinate(CommonCoordinate.EscapeLeave);

            InputHelper.SendKey(handle, Keys.Escape);
            InputHelper.SendClick(handle, MouseButtons.Left, leave);
        }

        private void LowerDifficulty(IntPtr handle)
        {
            var lower = transformService.TransformCoordinate(CommonCoordinate.EscapeLowerDifficulty, RelativeCoordinatePosition.Right);

            InputHelper.SendKey(handle, Keys.Escape);

            for (int i = 0; i < 19; i++)
            {
                InputHelper.SendClick(handle, MouseButtons.Left, lower);
                InputHelper.SendKey(handle, Keys.Enter);
            }

            InputHelper.SendKey(handle, Keys.Escape);
        }

        private void OpenGrift(IntPtr handle)
        {
            var grift = transformService.TransformCoordinate(CommonCoordinate.PortalGriftButton);
            var accept = transformService.TransformCoordinate(CommonCoordinate.PortalAccept);

            InputHelper.SendClick(handle, MouseButtons.Left, grift);
            InputHelper.SendClick(handle, MouseButtons.Left, accept);
        }

        private void OpenRift(IntPtr handle)
        {
            var rift = transformService.TransformCoordinate(CommonCoordinate.PortalRiftButton);
            var accept = transformService.TransformCoordinate(CommonCoordinate.PortalAccept);

            InputHelper.SendClick(handle, MouseButtons.Left, rift);
            InputHelper.SendClick(handle, MouseButtons.Left, accept);
        }

        private void Reforge(IntPtr handle)
        {
            var item = transformService.TransformCoordinate(CommonCoordinate.InventoryTopLeftSpot, RelativeCoordinatePosition.Right);
            var fill = transformService.TransformCoordinate(CommonCoordinate.CubeFill);
            var transmute = transformService.TransformCoordinate(CommonCoordinate.CubeTransmute);
            var backwards = transformService.TransformCoordinate(CommonCoordinate.CubeBackwards);
            var forwards = transformService.TransformCoordinate(CommonCoordinate.CubeForwards);

            InputHelper.SendClick(handle, MouseButtons.Right, item);
            Thread.Sleep(100);

            InputHelper.SendClick(handle, MouseButtons.Left, fill);

            InputHelper.SendClick(handle, MouseButtons.Left, transmute);
            Thread.Sleep(100);

            InputHelper.SendClick(handle, MouseButtons.Left, backwards);
            InputHelper.SendClick(handle, MouseButtons.Left, forwards);
        }

        private void Salvage(IntPtr handle)
        {
            var item = transformService.TransformCoordinate(CommonCoordinate.InventoryTopRightSpot, RelativeCoordinatePosition.Right);
            var menu = transformService.TransformCoordinate(CommonCoordinate.BlacksmithMenu);
            var anvil = transformService.TransformCoordinate(CommonCoordinate.BlacksmithAnvil);
            var step = transformService.TransformSize(CommonSize.InventoryStepSize);

            var columnIterations = 10 - settingsService.MacroSettings.SpareColumns;

            InputHelper.SendClick(handle, MouseButtons.Left, menu);
            InputHelper.SendClick(handle, MouseButtons.Left, anvil);

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < columnIterations; j++)
                {
                    InputHelper.SendClick(handle, MouseButtons.Left, item.X - j * step.Width, item.Y + i * step.Height);
                    InputHelper.SendKey(handle, Keys.Enter);
                    InputHelper.SendKey(handle, Keys.Enter);
                }
            }
        }

        private void SwapArmour(IntPtr handle)
        {
            // Allow different spots
            var step = transformService.TransformSize(CommonSize.InventoryStepSize, RelativeCoordinatePosition.Right);
            var item = transformService.TransformCoordinate(CommonCoordinate.InventoryTopLeftSpot, RelativeCoordinatePosition.Right);

            InputHelper.SendKey(handle, settingsService.GetKeybinding(Command.OpenInventory));

            for (int i = 0; i < settingsService.MacroSettings.SwapItemsAmount; i++)
            {
                InputHelper.SendClick(handle, MouseButtons.Right, item.X, item.Y + i * step.Height * 2);
            }
        }

        private void TravelPool(IntPtr handle)
        {
            var poolSpot = travelService.GetNextPoolSpot();

            if (poolSpot == default) return;

            var _mapAct = poolSpot.Item1;
            var _mapWaypoint = poolSpot.Item2;

            var backwards = transformService.TransformCoordinate(CommonCoordinate.MapBackwards, RelativeCoordinatePosition.Middle);
            var mapAct = transformService.TransformCoordinate(_mapAct, RelativeCoordinatePosition.Middle);
            var mapWaypoint = transformService.TransformCoordinate(_mapWaypoint, RelativeCoordinatePosition.Middle);

            InputHelper.SendClick(handle, MouseButtons.Left, backwards);
            InputHelper.SendClick(handle, MouseButtons.Left, mapAct);
            InputHelper.SendClick(handle, MouseButtons.Left, mapWaypoint);
        }

        private void TravelTown(IntPtr handle, ActionName actionName)
        {
            var _mapAct = travelService.GetActCoordinate(actionName);
            var _mapTown = travelService.GetTownCoordinate(actionName);

            var backwards = transformService.TransformCoordinate(CommonCoordinate.MapBackwards, RelativeCoordinatePosition.Middle);
            var mapAct = transformService.TransformCoordinate(_mapAct, RelativeCoordinatePosition.Middle);
            var mapTown = transformService.TransformCoordinate(_mapTown, RelativeCoordinatePosition.Middle);

            InputHelper.SendKey(handle, settingsService.GetKeybinding(Command.OpenMap));
            InputHelper.SendClick(handle, MouseButtons.Left, backwards);
            InputHelper.SendClick(handle, MouseButtons.Left, mapAct);
            InputHelper.SendClick(handle, MouseButtons.Left, mapTown);
        }

        private void UpgradeGem(IntPtr handle, CancellationTokenSource tokenSource)
        {
            var firstGem = transformService.TransformCoordinate(CommonCoordinate.UrshiFirstGem);
            var upgrade = transformService.TransformCoordinate(CommonCoordinate.UrshiUpgrade);

            var iterations = settingsService.MacroSettings.Empowered ? 5 : 4;

            if (!settingsService.MacroSettings.PickGemYourself)
            {
                if (IsCancelled(tokenSource)) return;
                InputHelper.SendClick(handle, MouseButtons.Left, firstGem);
                Thread.Sleep(100);
            }

            for (int i = 0; i < iterations; i++)
            {
                if (iterations - i == 3)
                {
                    if (IsCancelled(tokenSource)) return;
                    InputHelper.SendKey(handle, settingsService.GetKeybinding(Command.TeleportToTown));
                }

                if (IsCancelled(tokenSource)) return;
                InputHelper.SendClick(handle, MouseButtons.Left, upgrade);
                Thread.Sleep(1800);
            }
        }
    }
}