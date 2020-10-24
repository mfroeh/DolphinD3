using Dolphin.Enum;
using System;
using System.Drawing;

namespace Dolphin
{
    public interface ITravelInformationService
    {
        Point GetActCoordinate(ActionName actionName);

        Point GetActCoordinate(int act);

        Point GetKadalaItemCoordinate(ItemType itemType);

        Point GetKadalaTabCoordinate(ItemType itemType);

        Tuple<Point, Point> GetNextPoolSpot();

        Point GetTownCoordinate(ActionName actionName);

        Point GetTownCoordinate(int act);
    }
}