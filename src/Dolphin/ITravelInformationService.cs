using Dolphin.Enum;
using System.Drawing;

namespace Dolphin
{
    public interface ITravelInformationService
    {
        Point GetActCoordinate(ActionName actionName);
        Point GetActCoordinate(int act);

        Point GetTownCoordinate(ActionName actionName);

        Point GetTownCoordinate(int act);

        Point GetNextPoolSpotActCoordinate();

        Point GetNextPoolSpotMapCoordinate();

        Point GetKadalaTabCoordinate(ItemType itemType);

        Point GetKadalaItemCoordinate(ItemType itemType);
    }
}
