using System.Drawing;

namespace SoftEngine3D.Primitives.Lighting
{
    public abstract class LightSource
    {

        public LightSource(Vector3 position, Color color)
        {
            Position = position;
            Color = color;
        }

        public Vector3 Position { get; set; }

        public Color Color { get; set; }
    }
}