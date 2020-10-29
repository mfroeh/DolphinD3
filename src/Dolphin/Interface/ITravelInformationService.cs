using Dolphin.Enum;
using System;
using System.Drawing;

namespace Dolphin
{
    public interface ITravelInformationService
    {
        Tuple<Point, Point> GetKadalaCoordinates(ItemType itemType);

        Tuple<Point, Point> GetNextPoolSpot();

        Tuple<Point, Point> GetTownCoordinates(ActionName actionName);
    }
}