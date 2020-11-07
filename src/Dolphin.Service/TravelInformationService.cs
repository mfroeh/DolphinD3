using Dolphin.Enum;
using System;
using System.Drawing;

namespace Dolphin.Service
{
    public class TravelInformationService : ITravelInformationService
    {
        private IPoolSpotService poolService;

        public TravelInformationService(IPoolSpotService poolService)
        {
            this.poolService = poolService;
        }

        public Tuple<Point, Point> GetKadalaCoordinates(ItemType itemType)
        {
            return Tuple.Create(GetKadalaTabCoordinate(itemType), GetKadalaItemCoordinate(itemType));
        }

        public Tuple<Point, Point> GetNextPoolSpot()
        {
            return poolService.GetNextPoolSpot();
        }

        public Tuple<Point, Point> GetTownCoordinates(int act)
        {
            return Tuple.Create(GetActCoordinate(act), GetTownCoordinate(act));
        }

        private Point GetActCoordinate(int act)
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
                    return default;
            }
        }

        private Point GetKadalaItemCoordinate(ItemType itemType)
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
                    return default;
            }
        }

        private Point GetKadalaTabCoordinate(ItemType itemType)
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

        private Point GetTownCoordinate(int act)
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
                    return default;
            }
        }
    }
}