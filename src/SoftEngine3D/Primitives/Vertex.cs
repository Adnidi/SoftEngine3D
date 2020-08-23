using System.Drawing;

namespace SoftEngine3D.Primitives
{
    public class Vertex
    {
        public Vertex() : this(new Vector3(), Color.Transparent)
        {
        }

        public Vertex(Vector3 position, Color color)
        {
            this.Position = position;
            this.Color = color;
        }

        public Vector3 Position { get; set; }
        public Color Color { get; set; }
    }
}