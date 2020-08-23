using System;

namespace SoftEngine3D.Primitives
{
    public class Vector2
    {
        public Vector2()
        {
        }

        public Vector2(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public float X { get; set; }
        public float Y { get; set; }

        public float Length => (float)Math.Sqrt((X * X) + (Y * Y));

        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.X + b.X, a.Y + b.Y);
        public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.X - b.X, a.Y - b.Y);
        public static Vector2 operator /(Vector2 a, float b) => new Vector2(a.X / b, a.Y / b);
    }
}