using System.Drawing;
using SoftEngine3D.Primitives;

namespace SoftEngine3D.Imaging
{
    public class SimpleLineDrawingAlgorithm : LineDrawingAlgorithm
    {
        public SimpleLineDrawingAlgorithm(PointDrawingAlgorithm pointDrawingAlgorithm)
            : base(pointDrawingAlgorithm)
        {
        }

        public override void DrawLine(Vector3 point0, Vector3 point1, Color color0, Color color1)
        {
            var dist = (point1 - point0).Length;

            if (dist < 2)
                return;

            Vector3 middlePoint = point0 + (point1 - point0) / 2;
            var midColor = InterpolationHelpers.InterpolateColor(color0, color1);

            pointDrawingAlgorithm.DrawPoint(middlePoint, midColor);

            DrawLine(point0, middlePoint, color0, midColor);
            DrawLine(middlePoint, point1, midColor, color1);
        }
    }
}