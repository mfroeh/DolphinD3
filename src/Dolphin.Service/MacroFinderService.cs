using Dolphin.Enum;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dolphin.Service
{
    public class MacroFinderService : IMacroFinderService
    {
        private readonly ISettingsService settingsService;
        private readonly ILogService logService;
        private readonly ITravelService travelService;

        private static IList<ActionName> cancellableMacros = new List<ActionName> { ActionName.CubeConverterDualSlot, ActionName.CubeConverterSingleSlot, ActionName.UpgradeGem };

        public MacroFinderService(ISettingsService settingsService, ILogService logService, ITravelService travelService)
        {
            this.settingsService = settingsService;
            this.logService = logService;
            this.travelService = travelService;
        }

        public Action FindMacro(ActionName actionName)
        {
            if (cancellableMacros.Contains(actionName))
            {
                logService.AddEntry(this, $"Tried to recive Macro {actionName} as a non cancellable Macro.");
            }
            var macroFunction = GiveMacro(actionName);

            return () => macroFunction.Invoke(WindowHelper.GetHWND());
        }

        public Action FindCancellableMacro(ActionName actionName, CancellationTokenSource tokenSource)
        {
            if (!cancellableMacros.Contains(actionName))
            {
                logService.AddEntry(this, $"Tried to recive cancellable Macro {actionName} which is not avalible as a cancellable macro. This will still work.");

                return () => FindMacro(actionName);
            }

            switch (actionName)
            {
                case ActionName.CubeConverterDualSlot:
                    return () => CubeConverterDualSlot(WindowHelper.GetHWND(), tokenSource);
                case ActionName.CubeConverterSingleSlot:
                    return () => CubeConverterSingleSlot(WindowHelper.GetHWND(), tokenSource);
                case ActionName.UpgradeGem:
                    return () => UpgradeGem(WindowHelper.GetHWND(), tokenSource);
                default:
                    throw new NotImplementedException($"Cancellable Macro not implemented for {actionName}");
            }
        }

        private void UpgradeGem(IntPtr intPtr, CancellationTokenSource tokenSource)
        {
            var firstGem = WindowHelper.TransformCoordinate(CommonCoordinate.UrshiFirstGem);
            var upgrade = WindowHelper.TransformCoordinate(CommonCoordinate.UrshiUpgrade);

            var iterations = settingsService.MacroSettings.Empowered ? 5 : 4;


            if (!settingsService.MacroSettings.PickGemYourself)
            {
                if (IsCancelled(tokenSource)) return;
                InputHelper.MouseClick(intPtr, MouseButtons.Left, firstGem);
                Thread.Sleep(100);
            }
            g
            for (int i = 0; i < iterations; i++)
            {
                if (iterations - i == 3)
                {
                    if (IsCancelled(tokenSource)) return;
                    InputHelper.PressKey(intPtr, Keys.T);
                }

                if (IsCancelled(tokenSource)) return;
                InputHelper.MouseClick(intPtr, MouseButtons.Left, upgrade);
                Thread.Sleep(1800);
            }
        }

        private void CubeConverterDualSlot(IntPtr handle, CancellationTokenSource tokenSource)
        {
            var item = WindowHelper.TransformCoordinate(CommonCoordinate.TopLeftInventorySlot, CoordinatePosition.Right);
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
                    InputHelper.MouseClick(handle, MouseButtons.Right, item.X + j * step.X, item.Y + i * step.Y * 2);
                    Thread.Sleep(itemClickDelay);

                    if (IsCancelled(tokenSource)) return;
                    InputHelper.MouseClick(handle, MouseButtons.Left, fill);
                    Thread.Sleep(fillDelay);

                    if (IsCancelled(tokenSource)) return;
                    InputHelper.MouseClick(handle, MouseButtons.Left, transmute);
                    Thread.Sleep(transmuteDelay);

                    if (IsCancelled(tokenSource)) return;
                    InputHelper.MouseClick(handle, MouseButtons.Left, backwards);
                    Thread.Sleep(backwardsDelay);

                    if (IsCancelled(tokenSource)) return;
                    InputHelper.MouseClick(handle, MouseButtons.Left, forwards);
                }
            }
        }

        private void CubeConverterSingleSlot(IntPtr handle, CancellationTokenSource tokenSource)
        {
            var item = WindowHelper.TransformCoordinate(CommonCoordinate.TopLeftInventorySlot, CoordinatePosition.Right);
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
                    InputHelper.MouseClick(handle, MouseButtons.Right, item.X + j * step.X, item.Y + i * step.Y);
                    Thread.Sleep(itemClickDelay);

                    if (IsCancelled(tokenSource)) return;
                    InputHelper.MouseClick(handle, MouseButtons.Left, fill);
                    Thread.Sleep(fillDelay);

                    if (IsCancelled(tokenSource)) return;
                    InputHelper.MouseClick(handle, MouseButtons.Left, transmute);
                    Thread.Sleep(transmuteDelay);

                    if (IsCancelled(tokenSource)) return;
                    InputHelper.MouseClick(handle, MouseButtons.Left, backwards);
                    Thread.Sleep(backwardsDelay);

                    if (IsCancelled(tokenSource)) return;
                    InputHelper.MouseClick(handle, MouseButtons.Left, forwards);
                }
            }
        }

        private static bool IsCancelled(CancellationTokenSource tokenSource)
        {
            if (tokenSource.Token.IsCancellationRequested)
            {
                Trace.WriteLine("Cancellation requested!");
            }

            return tokenSource.Token.IsCancellationRequested;
        }

        private static void LeftClick_(IntPtr handle)
        {
            var cursorPos = InputHelper.GetCursorPos();
            InputHelper.MouseClick(handle, System.Windows.Forms.MouseButtons.Left, cursorPos);
            Thread.Sleep(50);
        }
    }

    /// <summary>
    /// Common Coordinates in 1920 x 1080
    /// </summary>
    public static class CommonCoordinate
    {
        public static Point InventoryStepSize = new Point { Height = 50, Width = 50 }; // Size
        public static Point TopLeftInventorySlot = new Point { X = 1425, Y = 580 }; // CubeTopLeftInventorySlot
        public static Point CubeForwards = new Point { X = 850, Y = 850 };
        public static Point CubeBackwards = new Point { X = 580, Y = 850 };
        public static Point CubeTransmute = new Point { X = 250, Y = 830 };
        public static Point CubeFill = new Point { X = 710, Y = 840 };
        public static Point UrshiUpgrade = new Point { X = 280, Y = 550 };
        public static Point UrshiFirstGem = new Point { X = 100, Y = 640 };
        public static Point BlacksmithMenu = new Point { X = 517, Y = 480 };
        public static Point BlacksmithAnvil = new Point { X = 165, Y = 295 };
        public static Point TopRightInventorySlot = new Point { X = 1875, Y = 580 }; // BlacksmithTopRightInventorySlot // Y = 585
        public static Point PortalGriftButton = new Point { X = 270, Y = 480 };
        public static Point PortalRiftButton = new Point { X = 0, Y = 0 };
        public static Point PortalAccept = new Point { X = 260, Y = 850 };
        public static Point EscapeLeave = new Point { X = 230, Y = 475 };
        public static Point EscapeLowerDifficulty = new Point { X = 1700, Y = 400 };

        public static Point MapBackwards = new Point { X = 895, Y = 125 };
        public static Point MapAct1 = new Point { X = 740, Y = 620 };
        public static Point MapAct2 = new Point { X = 1090, Y = 525 };
        public static Point MapAct3 = new Point { X = 710, Y = 400 };
        public static Point MapAct4 = new Point { X = 1450, Y = 370 };
        public static Point MapAct5 = new Point { X = 590, Y = 550 };
        public static Point MapAct1Town = new Point { X = 1020, Y = 490 };
        public static Point MapAct2Town = new Point { X = 1040, Y = 780 };
        public static Point MapAct3Town = new Point { X = 510, Y = 485 };
        public static Point MapAct4Town = new Point { X = 515, Y = 745 };
        public static Point MapAct5Town = new Point { X = 1170, Y = 625 };

        public static Point MapCemetryOfTheForsaken = new Point(630, 370);
        public static Point MapSouthernHighlands = new Point(810, 795);
        public static Point MapTheWeepingHollow = new Point(690, 485);
        public static Point MapLeoricsManor = new Point(580, 585);
        public static Point MapFieldsOfMisery = new Point(450, 320);
        public static Point MapDrownedTemple = new Point(580, 250);
        public static Point MapHowlingPlateau = new Point(880, 560);
        public static Point MapRoadToAlcarnus = new Point(1290, 600);
        public static Point MapTheBattlefields = new Point(630, 330);
        public static Point MapRakkisCrossing = new Point(870, 300);
        public static Point MapBrideOfKorsikk = new Point(800, 460);
        public static Point MapTowerOfTheDamnedLevel1 = new Point(1320, 340);
        public static Point MapTowerOfTheCursedLevel1 = new Point(1320, 610);
        public static Point MapLowerRealmOfInfernalFate = new Point(1480, 300);
        public static Point MapRealmOfFracturedFate = new Point(560, 510);
        public static Point MapPandemoniumFortressLevel1 = new Point(800, 325);

        public static Point KadalaTab1 = new Point { X = 515, Y = 220 };
        public static Point KadalaTab2 = new Point { X = 515, Y = 350 };
        public static Point KadalaTab3 = new Point { X = 515, Y = 480 };
        public static Point KadalaItemLeft1 = new Point { X = 70, Y = 210 };
        public static Point KadalaItemLeft2 = new Point { X = 70, Y = 310 };
        public static Point KadalaItemLeft3 = new Point { X = 70, Y = 410 };
        public static Point KadalaItemLeft4 = new Point { X = 70, Y = 510 };
        public static Point KadalaItemLeft5 = new Point { X = 70, Y = 610 };
        public static Point KadalaItemRight1 = new Point { X = 290, Y = 210 };
        public static Point KadalaItemRight2 = new Point { X = 290, Y = 310 };
        public static Point KadalaItemRight3 = new Point { X = 290, Y = 410 };
        public static Point KadalaItemRight4 = new Point { X = 290, Y = 510 };
        public static Point KadalaItemRight5 = new Point { X = 290, Y = 610 };
    }
}
