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

        public Point TransformCoordinate(Point sourceCoordinate, RelativeCoordinatePosition coordinatePosition = RelativeCoordinatePosition.Left, int originalWidth = 1920, int originalHeight = 1080)
        {
            var handle = handleService.GetHandle("Diablo III64");

            var rect = WindowHelper.GetClientRect(handle);

            var originalWidthF = (float)originalWidth;
            var originalHeightF = (float)originalHeight;

            var scaledY = (int)(rect.Height / originalHeightF * sourceCoordinate.Y);
            
            int scaledX;
            if (coordinatePosition == RelativeCoordinatePosition.Left)
            {
                scaledX = (int)(rect.Height / originalHeightF * sourceCoordinate.X);
            }
            else if (coordinatePosition == RelativeCoordinatePosition.Right)
            {
                scaledX = (int)(rect.Width - (originalWidthF - sourceCoordinate.X) * rect.Height / originalHeightF);
            }
            else
            {
                scaledX = (int)(sourceCoordinate.X * rect.Height / originalHeightF + (rect.Width - originalWidthF * rect.Height / originalHeightF) / 2.0);
            }

            return new Point { X = scaledX, Y = scaledY };
        }

        // TODO: Untested
        public Rectangle TransformRectangle(Rectangle sourceRectangle, RelativeCoordinatePosition coordinatePosition = RelativeCoordinatePosition.Left, int originalWidth = 1920, int originalHeight = 1080)
        {
            var newSize = TransformSize(sourceRectangle.Size, coordinatePosition, originalWidth, originalHeight);
            var newPoint = TransformCoordinate(new Point(sourceRectangle.X, sourceRectangle.Y), coordinatePosition, originalWidth, originalHeight);

            return new Rectangle(newPoint, newSize);
        }

        // TODO: Untested
        public Size TransformSize(Size sourceSize, RelativeCoordinatePosition coordinatePosition = RelativeCoordinatePosition.Left, int originalWidth = 1920, int originalHeight = 1080)
        {
            var point = new Point { X = sourceSize.Width, Y = sourceSize.Height };

            var transformedPoint = TransformCoordinate(point, coordinatePosition, originalWidth, originalHeight);

            return new Size { Width = transformedPoint.X, Height = transformedPoint.Y };
        }
    }
}