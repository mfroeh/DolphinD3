using Dolphin.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Dolphin.Service
{
    public class MacroFinderService : IMacroFinderService
    {
        private static readonly IList<ActionName> cancellableMacros = new List<ActionName> { ActionName.CubeConverterDualSlot, ActionName.CubeConverterSingleSlot, ActionName.UpgradeGem };
        private readonly ILogService logService;
        private readonly ISettingsService settingsService;
        private readonly ITravelInformationService travelService;

        public MacroFinderService(ISettingsService settingsService, ILogService logService, ITravelInformationService travelService)
        {
            this.settingsService = settingsService;
            this.logService = logService;
            this.travelService = travelService;
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
            var cursorPos = InputHelper.GetCursorPos();
            InputHelper.SendClick(handle, MouseButtons.Left, cursorPos);
            Thread.Sleep(50);
        }

        private static void RightClick(IntPtr handle)
        {
            var cursorPos = InputHelper.GetCursorPos();
            InputHelper.SendClick(handle, MouseButtons.Right, cursorPos);
            Thread.Sleep(50);
        }

        private void CubeConverterDualSlot(IntPtr handle, CancellationTokenSource tokenSource)
        {
            var item = WindowHelper.TransformCoordinate(CommonCoordinate.InventoryTopLeftSpot, CoordinatePosition.Right);
            var step = WindowHelper.TransformCoordinate(CommonCoordinate.InventoryStepSize);
            var fill = WindowHelper.TransformCoordinate(CommonCoordinate.CubeFill);
            var transmute = WindowHelper.TransformCoordinate(CommonCoordinate.CubeTransmute);
            var backwards = WindowHelper.TransformCoordinate(CommonCoordinate.CubeBackwards);
            var forwards = WindowHelper.TransformCoordinate(CommonCoordinate.CubeForwards);

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
                    InputHelper.SendClick(handle, MouseButtons.Right, item.X + j * step.X, item.Y + i * step.Y * 2);
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
            var item = WindowHelper.TransformCoordinate(CommonCoordinate.InventoryTopLeftSpot, CoordinatePosition.Right);
            var step = WindowHelper.TransformCoordinate(CommonCoordinate.InventoryStepSize);
            var fill = WindowHelper.TransformCoordinate(CommonCoordinate.CubeFill);
            var transmute = WindowHelper.TransformCoordinate(CommonCoordinate.CubeTransmute);
            var backwards = WindowHelper.TransformCoordinate(CommonCoordinate.CubeBackwards);
            var forwards = WindowHelper.TransformCoordinate(CommonCoordinate.CubeForwards);

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
                    InputHelper.SendClick(handle, MouseButtons.Right, item.X + j * step.X, item.Y + i * step.Y);
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
            var item = WindowHelper.TransformCoordinate(CommonCoordinate.InventoryTopRightSpot, CoordinatePosition.Right);
            var step = WindowHelper.TransformCoordinate(CommonCoordinate.InventoryStepSize);

            var cursorPos = InputHelper.GetCursorPos();
            WindowHelper.ScreenToClient(handle, ref cursorPos); //  x, y = win32gui.ScreenToClient(handle, win32api.GetCursorPos())

            var columnIterations = 10 - settingsService.MacroSettings.SpareColumns;

            InputHelper.SendKey(handle, Keys.C);

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < columnIterations; j++)
                {
                    InputHelper.SendClick(handle, MouseButtons.Left, item.X - j * step.X, item.Y + i * step.Y);
                    InputHelper.SendClick(handle, MouseButtons.Left, cursorPos);
                }
            }
        }

        private void Gamble(IntPtr handle)
        {
            var gambleItem = settingsService.MacroSettings.SelectedGambleItem;
            var _tab = travelService.GetKadalaTabCoordinate(gambleItem);
            var _item = travelService.GetKadalaItemCoordinate(gambleItem);

            var tab = WindowHelper.TransformCoordinate(_tab);
            var item = WindowHelper.TransformCoordinate(_item);

            InputHelper.SendClick(handle, MouseButtons.Left, tab);

            for (int i = 0; i < 60; i++)
            {
                InputHelper.SendClick(handle, MouseButtons.Left, item);
            }
        }

        private void LeaveGame(IntPtr handle)
        {
            var leave = WindowHelper.TransformCoordinate(CommonCoordinate.EscapeLeave);

            InputHelper.SendKey(handle, Keys.Escape);
            InputHelper.SendClick(handle, MouseButtons.Left, leave);
        }

        private void LowerDifficulty(IntPtr handle)
        {
            var lower = WindowHelper.TransformCoordinate(CommonCoordinate.EscapeLowerDifficulty, CoordinatePosition.Right);

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
            var grift = WindowHelper.TransformCoordinate(CommonCoordinate.PortalGriftButton);
            var accept = WindowHelper.TransformCoordinate(CommonCoordinate.PortalAccept);

            InputHelper.SendClick(handle, MouseButtons.Left, grift);
            InputHelper.SendClick(handle, MouseButtons.Left, accept);
        }

        private void OpenRift(IntPtr handle)
        {
            var rift = WindowHelper.TransformCoordinate(CommonCoordinate.PortalRiftButton);
            var accept = WindowHelper.TransformCoordinate(CommonCoordinate.PortalAccept);

            InputHelper.SendClick(handle, MouseButtons.Left, rift);
            InputHelper.SendClick(handle, MouseButtons.Left, accept);
        }

        private void Reforge(IntPtr handle)
        {
            var item = WindowHelper.TransformCoordinate(CommonCoordinate.InventoryTopLeftSpot, CoordinatePosition.Right);
            var fill = WindowHelper.TransformCoordinate(CommonCoordinate.CubeFill);
            var transmute = WindowHelper.TransformCoordinate(CommonCoordinate.CubeTransmute);
            var backwards = WindowHelper.TransformCoordinate(CommonCoordinate.CubeBackwards);
            var forwards = WindowHelper.TransformCoordinate(CommonCoordinate.CubeForwards);

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
            var item = WindowHelper.TransformCoordinate(CommonCoordinate.InventoryTopRightSpot, CoordinatePosition.Right);
            var menu = WindowHelper.TransformCoordinate(CommonCoordinate.BlacksmithMenu);
            var anvil = WindowHelper.TransformCoordinate(CommonCoordinate.BlacksmithAnvil);
            var step = WindowHelper.TransformCoordinate(CommonCoordinate.InventoryStepSize);

            var columnIterations = 10 - settingsService.MacroSettings.SpareColumns;

            InputHelper.SendClick(handle, MouseButtons.Left, menu);
            InputHelper.SendClick(handle, MouseButtons.Left, anvil);

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < columnIterations; j++)
                {
                    InputHelper.SendClick(handle, MouseButtons.Left, item.X - j * step.X, item.Y + i * step.Y);
                    InputHelper.SendKey(handle, Keys.Enter);
                    InputHelper.SendKey(handle, Keys.Enter);
                }

                InputHelper.SendKey(handle, Keys.Escape);
            }
        }

        private void SwapArmour(IntPtr handle)
        {
            // Allow different spots
            var step = WindowHelper.TransformCoordinate(CommonCoordinate.InventoryStepSize, CoordinatePosition.Right);
            var item = WindowHelper.TransformCoordinate(CommonCoordinate.InventoryTopLeftSpot, CoordinatePosition.Right);

            InputHelper.SendKey(handle, Keys.C);

            for (int i = 0; i < settingsService.MacroSettings.SwapItemsAmount; i++)
            {
                InputHelper.SendClick(handle, MouseButtons.Right, item.X, item.Y + i * step.Y * 2);
            }
        }

        private void TravelPool(IntPtr handle)
        {
            var _mapAct = travelService.GetNextPoolSpotActCoordinate();
            var _mapWaypoint = travelService.GetNextPoolSpotMapCoordinate();

            var backwards = WindowHelper.TransformCoordinate(CommonCoordinate.MapBackwards, CoordinatePosition.Middle);
            var mapAct = WindowHelper.TransformCoordinate(_mapAct, CoordinatePosition.Middle);
            var mapWaypoint = WindowHelper.TransformCoordinate(_mapWaypoint, CoordinatePosition.Middle);

            InputHelper.SendClick(handle, MouseButtons.Left, backwards);
            InputHelper.SendClick(handle, MouseButtons.Left, mapAct);
            InputHelper.SendClick(handle, MouseButtons.Left, mapWaypoint);
        }

        private void TravelTown(IntPtr handle, ActionName actionName)
        {
            var _mapAct = travelService.GetActCoordinate(actionName);
            var _mapTown = travelService.GetTownCoordinate(actionName);

            var backwards = WindowHelper.TransformCoordinate(CommonCoordinate.MapBackwards, CoordinatePosition.Middle);
            var mapAct = WindowHelper.TransformCoordinate(_mapAct, CoordinatePosition.Middle);
            var mapTown = WindowHelper.TransformCoordinate(_mapTown, CoordinatePosition.Middle);

            InputHelper.SendKey(handle, Keys.M);
            InputHelper.SendClick(handle, MouseButtons.Left, backwards);
            InputHelper.SendClick(handle, MouseButtons.Left, mapAct);
            InputHelper.SendClick(handle, MouseButtons.Left, mapTown);
        }

        private void UpgradeGem(IntPtr handle, CancellationTokenSource tokenSource)
        {
            var firstGem = WindowHelper.TransformCoordinate(CommonCoordinate.UrshiFirstGem);
            var upgrade = WindowHelper.TransformCoordinate(CommonCoordinate.UrshiUpgrade);

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
                    InputHelper.SendKey(handle, Keys.T);
                }

                if (IsCancelled(tokenSource)) return;
                InputHelper.SendClick(handle, MouseButtons.Left, upgrade);
                Thread.Sleep(1800);
            }
        }
    }
}