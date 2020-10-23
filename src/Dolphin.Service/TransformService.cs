using Dolphin.Enum;
using System;
using System.Drawing;

namespace Dolphin.Service
{
    public class TransformService : ITransformService
    {
        private readonly IHandleService handleService;

        public TransformService(IHandleService handleService)
        {
            this.handleService = handleService;
        }

        public Point TransformCoordinate(Point sourceCoordinate, RelativeCoordinatePosition coordinatePosition = RelativeCoordinatePosition.Left)
        {
            var clientRect = new Rectangle();
            WindowHelper.GetClientRect(handleService.GetHandle(), ref clientRect);

            var scaledY = clientRect.Height / 1080 * sourceCoordinate.Y;

            int scaledX;
            if (coordinatePosition == RelativeCoordinatePosition.Left)
            {
                scaledX = clientRect.Height / 1080 * sourceCoordinate.X;
            }
            else if (coordinatePosition == RelativeCoordinatePosition.Right)
            {
                scaledX = clientRect.Width - (1920 - sourceCoordinate.X) * clientRect.Height / 1080;
            }
            else
            {
                scaledX = sourceCoordinate.X * clientRect.Height / 1080 + (clientRect.Width - 1920 * clientRect.Height / 1080) / 2;
            }

            return new Point { X = scaledX, Y = scaledY };
        }

        public Rectangle TransformRectangle(Rectangle sourceRectangle, RelativeCoordinatePosition coordinatePosition = RelativeCoordinatePosition.Left)
        {
            throw new NotImplementedException();
        }

        // TODO: Untested
        public Size TransformSize(Size sourceSize, RelativeCoordinatePosition coordinatePosition = RelativeCoordinatePosition.Left)
        {
            var point = new Point { X = sourceSize.Width, Y = sourceSize.Height };

            var transformedPoint = TransformCoordinate(point);

            return new Size { Width = transformedPoint.X, Height = transformedPoint.Y };
        }
    }
}