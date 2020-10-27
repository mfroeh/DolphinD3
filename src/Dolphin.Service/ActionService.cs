using Dolphin.Enum;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Dolphin.Service
{
    public class ActionService
    {
        public readonly ITransformService transformService;
        public readonly ITravelInformationService travelService;

        public ActionService(ITransformService transformService, ITravelInformationService travelService)
        {
            this.transformService = transformService;
            this.travelService = travelService;
        }

        public static bool IsCancelled(CancellationTokenSource tokenSource)
        {
            if (tokenSource == null) return true;

            return tokenSource.Token.IsCancellationRequested;
        }

        public static void LeftClick(IntPtr handle)
        {
            InputHelper.SendClickAtCursorPos(handle, MouseButtons.Left);
            Thread.Sleep(10);
        }

        public static void RightClick(IntPtr handle)
        {
            InputHelper.SendClickAtCursorPos(handle, MouseButtons.Right);
            Thread.Sleep(10);
        }

        public void CubeConverterDualSlot(IntPtr handle, CancellationTokenSource tokenSource, ConvertingSpeed speed)
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
            if (speed == ConvertingSpeed.Slow)
            {
                fillDelay = 100;
                backwardsDelay = 100;
            }
            else if (speed == ConvertingSpeed.Fast)
            {
                itemClickDelay = 60;
                transmuteDelay = 60;
            }
            else if (speed == ConvertingSpeed.VerySlow)
            {
                fillDelay = 150;
                backwardsDelay = 150;
                itemClickDelay = 250;
                transmuteDelay = 250;
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

        public void CubeConverterSingleSlot(IntPtr handle, CancellationTokenSource tokenSource, ConvertingSpeed speed)
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
            if (speed == ConvertingSpeed.Slow)
            {
                fillDelay = 100;
                backwardsDelay = 100;
            }
            else if (speed == ConvertingSpeed.Fast)
            {
                itemClickDelay = 60;
                transmuteDelay = 60;
            }
            else if (speed == ConvertingSpeed.VerySlow)
            {
                fillDelay = 150;
                backwardsDelay = 150;
                itemClickDelay = 250;
                transmuteDelay = 250;
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

        public void DropInventory(IntPtr handle, uint spareColumns, Keys openInventoryKey)
        {
            var item = transformService.TransformCoordinate(CommonCoordinate.InventoryTopRightSpot, RelativeCoordinatePosition.Right);
            var step = transformService.TransformSize(CommonSize.InventoryStepSize);

            var columnIterations = 10 - spareColumns;

            InputHelper.SendKey(handle, openInventoryKey);

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < columnIterations; j++)
                {
                    InputHelper.SendClick(handle, MouseButtons.Left, item.X - j * step.Width, item.Y + i * step.Height);
                    InputHelper.SendClickAtCursorPos(handle, MouseButtons.Left);
                }
            }

            InputHelper.SendKey(handle, openInventoryKey);
        }

        public void Gamble(IntPtr handle, ItemType gambleItem)
        {
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

        public void LeaveGame(IntPtr handle)
        {
            var leave = transformService.TransformCoordinate(CommonCoordinate.EscapeLeave);

            InputHelper.SendKey(handle, Keys.Escape);
            InputHelper.SendClick(handle, MouseButtons.Left, leave);
        }

        public void LowerDifficulty(IntPtr handle)
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

        public void OpenGrift(IntPtr handle)
        {
            var grift = transformService.TransformCoordinate(CommonCoordinate.PortalGriftButton);
            var accept = transformService.TransformCoordinate(CommonCoordinate.PortalAccept);

            InputHelper.SendClick(handle, MouseButtons.Left, grift);
            InputHelper.SendClick(handle, MouseButtons.Left, accept);
        }

        public void OpenRift(IntPtr handle)
        {
            var rift = transformService.TransformCoordinate(CommonCoordinate.PortalRiftButton);
            var accept = transformService.TransformCoordinate(CommonCoordinate.PortalAccept);

            InputHelper.SendClick(handle, MouseButtons.Left, rift);
            InputHelper.SendClick(handle, MouseButtons.Left, accept);
        }

        public void Reforge(IntPtr handle, ConvertingSpeed speed)
        {
            var item = transformService.TransformCoordinate(CommonCoordinate.InventoryTopLeftSpot, RelativeCoordinatePosition.Right);
            var fill = transformService.TransformCoordinate(CommonCoordinate.CubeFill);
            var transmute = transformService.TransformCoordinate(CommonCoordinate.CubeTransmute);
            var backwards = transformService.TransformCoordinate(CommonCoordinate.CubeBackwards);
            var forwards = transformService.TransformCoordinate(CommonCoordinate.CubeForwards);

            var itemClickDelay = 130;
            var transmuteDelay = 130;
            var fillDelay = 0; // Care, Thread.Sleep(0)!
            var backwardsDelay = 0; // Care, Thread.Sleep(0)!
            if (speed == ConvertingSpeed.Slow)
            {
                fillDelay = 100;
                backwardsDelay = 100;
            }
            else if (speed == ConvertingSpeed.Fast)
            {
                itemClickDelay = 60;
                transmuteDelay = 60;
            }
            else if (speed == ConvertingSpeed.VerySlow)
            {
                fillDelay = 150;
                backwardsDelay = 150;
                itemClickDelay = 250;
                transmuteDelay = 250;
            }

            InputHelper.SendClick(handle, MouseButtons.Right, item);
            Thread.Sleep(itemClickDelay);

            InputHelper.SendClick(handle, MouseButtons.Left, fill);
            Thread.Sleep(fillDelay);

            InputHelper.SendClick(handle, MouseButtons.Left, transmute);
            Thread.Sleep(transmuteDelay);

            InputHelper.SendClick(handle, MouseButtons.Left, backwards);
            Thread.Sleep(backwardsDelay);

            InputHelper.SendClick(handle, MouseButtons.Left, forwards);
        }

        public void Salvage(IntPtr handle, uint spareColumns)
        {
            var item = transformService.TransformCoordinate(CommonCoordinate.InventoryTopRightSpot, RelativeCoordinatePosition.Right);
            var menu = transformService.TransformCoordinate(CommonCoordinate.BlacksmithMenu);
            var anvil = transformService.TransformCoordinate(CommonCoordinate.BlacksmithAnvil);
            var step = transformService.TransformSize(CommonSize.InventoryStepSize);

            var columnIterations = 10 - spareColumns;

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

        public void SwapArmour(IntPtr handle, Keys openInventoryKey, uint swapItemAmount)
        {
            // Allow different spots
            var step = transformService.TransformSize(CommonSize.InventoryStepSize, RelativeCoordinatePosition.Right);
            var item = transformService.TransformCoordinate(CommonCoordinate.InventoryTopLeftSpot, RelativeCoordinatePosition.Right);

            InputHelper.SendKey(handle, openInventoryKey);

            for (int i = 0; i < swapItemAmount; i++)
            {
                InputHelper.SendClick(handle, MouseButtons.Right, item.X, item.Y + i * step.Height * 2);
            }
        }

        public void TravelPool(IntPtr handle)
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

        public void TravelTown(IntPtr handle, ActionName actionName, Keys openMapKey)
        {
            var _mapAct = travelService.GetActCoordinate(actionName);
            var _mapTown = travelService.GetTownCoordinate(actionName);

            var backwards = transformService.TransformCoordinate(CommonCoordinate.MapBackwards, RelativeCoordinatePosition.Middle);
            var mapAct = transformService.TransformCoordinate(_mapAct, RelativeCoordinatePosition.Middle);
            var mapTown = transformService.TransformCoordinate(_mapTown, RelativeCoordinatePosition.Middle);

            InputHelper.SendKey(handle, openMapKey);
            InputHelper.SendClick(handle, MouseButtons.Left, backwards);
            InputHelper.SendClick(handle, MouseButtons.Left, mapAct);
            InputHelper.SendClick(handle, MouseButtons.Left, mapTown);
        }

        public void UpgradeGem(IntPtr handle, CancellationTokenSource tokenSource, bool isEmpowered, bool pickGemYourself, Keys teleporToTownKey)
        {
            var firstGem = transformService.TransformCoordinate(CommonCoordinate.UrshiFirstGem);

            var iterations = isEmpowered ? 5 : 4;

            if (!pickGemYourself)
            {
                if (IsCancelled(tokenSource)) return;
                InputHelper.SendClick(handle, MouseButtons.Left, firstGem);
                Thread.Sleep(100);
            }

            UpgradeGem(handle, tokenSource, iterations, teleporToTownKey);
        }

        public void UpgradeGem(IntPtr handle, CancellationTokenSource tokenSource, int gemUpCount, Keys teleporToTownKey)
        {
            var upgrade = transformService.TransformCoordinate(CommonCoordinate.UrshiUpgrade);

            for (int i = 0; i < gemUpCount; i++)
            {
                if (gemUpCount - i <= 3)
                {
                    if (IsCancelled(tokenSource)) return;
                    InputHelper.SendKey(handle, teleporToTownKey);
                }

                if (IsCancelled(tokenSource)) return;
                InputHelper.SendClick(handle, MouseButtons.Left, upgrade);

                for (int _ = 0; _ < 18; _++)
                {
                    if (IsCancelled(tokenSource)) return;
                    Thread.Sleep(100);
                }
            }
        }
    }
}