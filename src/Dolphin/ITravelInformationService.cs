using Dolphin.Enum;
using System.Drawing;

namespace Dolphin
{
    public interface ITravelInformationService
    {
        Point GetActCoordinate(ActionName actionName);

        Point GetActCoordinate(int act);

        Point GetKadalaItemCoordinate(ItemType itemType);

        Point GetKadalaTabCoordinate(ItemType itemType);

        Point GetNextPoolSpotActCoordinate();

        Point GetNextPoolSpotMapCoordinate();

        Point GetTownCoordinate(ActionName actionName);

        Point GetTownCoordinate(int act);
    }
}