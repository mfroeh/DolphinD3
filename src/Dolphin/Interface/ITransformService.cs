using Dolphin.Enum;
using System.Drawing;

namespace Dolphin
{
    public interface ITransformService
    {
        Point TransformCoordinate(Point sourceCoordinate, RelativeCoordinatePosition coordinatePosition = RelativeCoordinatePosition.Left, int originalWidth = 1920, int originalHeight = 1080);

        Rectangle TransformRectangle(Rectangle sourceRectangle, RelativeCoordinatePosition coordinatePosition = RelativeCoordinatePosition.Left, int originalWidth = 1920, int originalHeight = 1080);

        Size TransformSize(Size sourceSize, RelativeCoordinatePosition coordinatePosition = RelativeCoordinatePosition.Left, int originalWidth = 1920, int originalHeight = 1080);
    }
}