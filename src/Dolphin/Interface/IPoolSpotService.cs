using System;
using System.Drawing;

namespace Dolphin
{
    public interface IPoolSpotService
    {
        Tuple<Point, Point> GetNextPoolSpot();
    }
}