using Dolphin.Enum;
using System;
using System.Drawing;

namespace Dolphin.Service
{
    public class TravelInformationService : ITravelInformationService
    {
        public Point GetActCoordinate(ActionName actionName)
        {
            switch (actionName)
            {
                case ActionName.TravelAct1:
                    return CommonCoordinate.MapAct1;

                case ActionName.TravelAct2:
                    return CommonCoordinate.MapAct2;

                case ActionName.TravelAct34:
                    return CommonCoordinate.MapAct3;

                case ActionName.TravelAct5:
                    return CommonCoordinate.MapAct5;

                default:
                    return new Point(0, 0);
            }
        }

        public Point GetActCoordinate(int act)
        {
            switch (act)
            {
                case 1:
                    return CommonCoordinate.MapAct1;

                case 2:
                    return CommonCoordinate.MapAct2;

                case 3:
                    return CommonCoordinate.MapAct3;

                case 4:
                    return CommonCoordinate.MapAct4;

                case 5:
                    return CommonCoordinate.MapAct5;

                default:
                    return new Point(0, 0);
            }
        }

        public Point GetKadalaItemCoordinate(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.OneHandedWeapon:
                case ItemType.Helm:
                case ItemType.Ring:
                    return CommonCoordinate.KadalaItemLeft1;

                case ItemType.Quiver:
                case ItemType.Boots:
                    return CommonCoordinate.KadalaItemLeft2;

                case ItemType.Mojo:
                case ItemType.Belt:
                    return CommonCoordinate.KadalaItemLeft3;

                case ItemType.Pants:
                    return CommonCoordinate.KadalaItemLeft4;

                case ItemType.Shield:
                    return CommonCoordinate.KadalaItemLeft5;

                case ItemType.TwoHandedWeapon:
                case ItemType.Gloves:
                case ItemType.Amulet:
                    return CommonCoordinate.KadalaItemRight1;

                case ItemType.Orb:
                case ItemType.ChestArmour:
                    return CommonCoordinate.KadalaItemRight2;

                case ItemType.Phylacetery:
                case ItemType.Shoulders:
                    return CommonCoordinate.KadalaItemRight3;

                case ItemType.Bracers:
                    return CommonCoordinate.KadalaItemRight4;

                default:
                    return new Point(0, 0);
            }
        }

        public Point GetKadalaTabCoordinate(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.Ring:
                case ItemType.Amulet:
                    return CommonCoordinate.KadalaTab3;

                case ItemType.OneHandedWeapon:
                case ItemType.TwoHandedWeapon:
                case ItemType.Quiver:
                case ItemType.Mojo:
                case ItemType.Orb:
                case ItemType.Phylacetery:
                    return CommonCoordinate.KadalaTab1;

                default:
                    return CommonCoordinate.KadalaTab2;
            }
        }

        public Point GetNextPoolSpotActCoordinate()
        {
            throw new NotImplementedException();
        }

        public Point GetNextPoolSpotMapCoordinate()
        {
            throw new NotImplementedException();
        }

        public Point GetTownCoordinate(ActionName actionName)
        {
            switch (actionName)
            {
                case ActionName.TravelAct1:
                    return CommonCoordinate.MapAct1Town;

                case ActionName.TravelAct2:
                    return CommonCoordinate.MapAct2Town;

                case ActionName.TravelAct34:
                    return CommonCoordinate.MapAct3Town;

                case ActionName.TravelAct5:
                    return CommonCoordinate.MapAct5Town;

                default:
                    return new Point(0, 0);
            }
        }

        public Point GetTownCoordinate(int act)
        {
            switch (act)
            {
                case 1:
                    return CommonCoordinate.MapAct1Town;

                case 2:
                    return CommonCoordinate.MapAct2Town;

                case 3:
                    return CommonCoordinate.MapAct3Town;

                case 4:
                    return CommonCoordinate.MapAct4Town;

                case 5:
                    return CommonCoordinate.MapAct5Town;

                default:
                    return new Point(0, 0);
            }
        }
    }
}