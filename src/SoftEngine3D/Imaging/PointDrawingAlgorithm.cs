using System.Drawing;
using SoftEngine3D.Primitives;

namespace SoftEngine3D.Imaging
{
    public class PointDrawingAlgorithm
    {
        private readonly Bitmap workingBitmap;
        private readonly ZBuffer zBuffer;

        public PointDrawingAlgorithm(
            Bitmap workingBitmap,
            ZBuffer zBuffer)
        {
            this.workingBitmap = workingBitmap;
            this.zBuffer = zBuffer;
        }

        public void DrawPoint(Vector3 point, Color color)
        {
            if (point.X >= 0 && point.Y >= 0 && point.X < workingBitmap.Size.Width && point.Y < workingBitmap.Size.Height)
            {
                if (zBuffer[(int)point.X, (int)point.Y] > point.Z)
                {
                    workingBitmap.SetPixel((int)point.X, (int)point.Y, color);

                    zBuffer[(int)point.X, (int)point.Y] = point.Z;
                }
            }
        }
    }
}