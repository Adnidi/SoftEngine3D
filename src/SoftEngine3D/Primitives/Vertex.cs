using System.Drawing;

namespace SoftEngine3D.Primitives
{
    public class Vertex
    {
        public Vertex() : this(new Vector3(), Color.Transparent)
        {
        }

        public Vertex(Vector3 relativePosition, Color color)
        {
            this.RelativePosition = relativePosition;
            this.Color = color;

            this.NormalVector = new Vector3();
            this.WorldPosition = new Vector3();
        }

        public Vector3 RelativePosition { get; set; }
        public Vector3 NormalVector { get; set; }
        public Vector3 WorldPosition { get; set; }

        public Color Color { get; set; }
    }
}