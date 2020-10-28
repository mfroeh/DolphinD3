using Dolphin.Enum;
using System.Drawing;

namespace Dolphin
{
    public interface ITransformService
    {
        Point TransformCoordinate(Point sourceCoordinate, RelativeCoordinatePosition coordinatePosition = RelativeCoordinatePosition.Left);

        Rectangle TransformRectangle(Rectangle sourceRectangle, RelativeCoordinatePosition coordinatePosition = RelativeCoordinatePosition.Left);

        Size TransformSize(Size sourceSize, RelativeCoordinatePosition coordinatePosition = RelativeCoordinatePosition.Left);
    }
}