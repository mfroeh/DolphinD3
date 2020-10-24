using Dolphin.Enum;
using System;
using System.Diagnostics;
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

            var scaledY = (int)(clientRect.Height / 1080.0 * sourceCoordinate.Y);
            int scaledX;
            if (coordinatePosition == RelativeCoordinatePosition.Left)
            {
                scaledX = (int)(clientRect.Height / 1080.0 * sourceCoordinate.X);
            }
            else if (coordinatePosition == RelativeCoordinatePosition.Right)
            {
                scaledX = (int)(clientRect.Width - (1920 - sourceCoordinate.X) * clientRect.Height / 1080.0);
            }
            else
            {
                scaledX = (int)(sourceCoordinate.X * clientRect.Height / 1080.0 + (clientRect.Width - 1920 * clientRect.Height / 1080.0) / 2.0);
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