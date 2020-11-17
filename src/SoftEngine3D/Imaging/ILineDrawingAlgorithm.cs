using System.Drawing;
using SoftEngine3D.Primitives;

namespace SoftEngine3D.Imaging
{
    public interface ILineDrawingAlgorithm
    {
        void DrawLine(Vector3 point0, Vector3 point1, Color color0, Color color1);
    }
}