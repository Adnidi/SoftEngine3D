using System;

namespace SoftEngine3D.Primitives
{
    public class Vector3
    {
        public Vector3()
        {
        }

        public Vector3(Vector3 other) : this(other.X, other.Y, other.Z)
        {
        }

        public Vector3(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public float Length => (float) Math.Sqrt((X * X) + (Y * Y) + (Z * Z));

        public Vector3 Normalize()
        {
            var length = Length;
            if (length != 0.0)
            {
                return new Vector3(X/length, Y/Length, Z/Length);
            }
            return new Vector3(this);
        }

        public static Vector3 operator + (Vector3 a, Vector3 b) => new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Vector3 operator - (Vector3 a, Vector3 b) => new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Vector3 operator / (Vector3 a, float b) => new Vector3(a.X / b, a.Y / b, a.Z / b);
    }

    public static class Vector3Extensions
    {
        public static Vector3 CrossProduct(this Vector3 left, Vector3 right)
        {
            return new Vector3(
                (left.Y * right.Z) - (left.Z * right.Y),
                (left.Z * right.X) - (left.X * right.Z),
                (left.X * right.Y) - (left.Y * right.X));
        }

        public static float DotProduct(this Vector3 left, Vector3 right)
        {
            return left.X * right.X +
                   left.Y * right.Y +
                   left.Z * right.Z;
        }

        public static Vector3 LeftTransformWithMatrix(this Vector3 vector, Matrix4x4 transformationMatrix)
        {
            // need 4dim Vector
            var vector4Dim = new Vector4(vector.X, vector.Y, vector.Z, 1.0f);

            var vector4Multiplied = vector4Dim * transformationMatrix;

            // bring result vector to w = 1 again
            return new Vector3(
                vector4Multiplied.X / vector4Multiplied.W,
                vector4Multiplied.Y / vector4Multiplied.W,
                vector4Multiplied.Z / vector4Multiplied.W);
        }
    }

    public static class Vector3Prefabs
    {
        public static Vector3 UnitX => new Vector3(1,0,0);
        public static Vector3 UnitY => new Vector3(0,1,0);
        public static Vector3 UnitZ => new Vector3(0,0,1);
    }
}