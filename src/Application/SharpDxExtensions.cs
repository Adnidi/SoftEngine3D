using SoftEngine3D.Primitives;

namespace SoftEngineClient
{
    public static class SharpDxExtensions
    {
        public static Matrix4x4 ToOwnMatrix(this SharpDX.Matrix m)
        {
            return new Matrix4x4(
                new float[4, 4]
                {
                    { m.M11, m.M12, m.M13, m.M14 },
                    { m.M21, m.M22, m.M23, m.M24 },
                    { m.M31, m.M32, m.M33, m.M34 },
                    { m.M41, m.M42, m.M43, m.M44 },
                });
        }
    }
}