using System.Drawing;

namespace SoftEngine3D.Primitives.Lighting
{
    public class PointLightSource : LightSource
    {
        public PointLightSource(Vector3 position, Color color)
            : base(position, color)
        {
        }
    }
}