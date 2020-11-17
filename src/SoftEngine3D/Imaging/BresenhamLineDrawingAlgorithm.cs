using System;
using System.Drawing;
using SoftEngine3D.Primitives;

namespace SoftEngine3D.Imaging
{
    public class BresenhamLineDrawingAlgorithm : LineDrawingAlgorithm
    {
        public BresenhamLineDrawingAlgorithm(PointDrawingAlgorithm pointDrawingAlgorithm)
            : base(pointDrawingAlgorithm)
        {
        }

        public override void DrawLine(Vector3 point0, Vector3 point1, Color c0, Color c1)
        {
            int x0 = (int)point0.X;
            int y0 = (int)point0.Y;
            int x1 = (int)point1.X;
            int y1 = (int)point1.Y;

            var dx = Math.Abs(x1 - x0);
            var dy = Math.Abs(y1 - y0);
            var sx = (x0 < x1) ? 1 : -1;
            var sy = (y0 < y1) ? 1 : -1;
            var err = dx - dy;

            var totalDistance = (point0 - point1).Length;

            while (true)
            {
                var pointToDraw = new Vector3(x0, y0, 0);
                var distanceToP0 = (pointToDraw - point0).Length;
                var ratioCovered = distanceToP0 / totalDistance;

                if (ratioCovered < 0) ratioCovered = 0;
                if (ratioCovered > 1) ratioCovered = 1;

                var colorToDraw = Color
                    .FromArgb(
                        (int)(c0.A * (1 - ratioCovered) + c1.A * ratioCovered),
                        (int)(c0.R * (1 - ratioCovered) + c1.R * ratioCovered),
                        (int)(c0.G * (1 - ratioCovered) + c1.G * ratioCovered),
                        (int)(c0.B * (1 - ratioCovered) + c1.B * ratioCovered));

                pointDrawingAlgorithm.DrawPoint(pointToDraw, colorToDraw);

                if ((x0 == x1) && (y0 == y1)) break;
                var e2 = 2 * err;
                if (e2 > -dy) { err -= dy; x0 += sx; }
                if (e2 < dx) { err += dx; y0 += sy; }
            }
        }
    }
}