using System;

namespace SoftEngine3D.Primitives
{
    public class Matrix4x4
    {
        public Matrix4x4()
        {
        }

        public Matrix4x4(float[,] values)
        {
            for(int i = 0; i < 4; ++i)
            for (int j = 0; j < 4; ++j)
            {
                _values[i, j] = values[i, j];
            }
        }

        private readonly float[,] _values = new float[4,4];

        public float this[int i, int j]
        {
            get => _values[i, j];
            set => _values[i, j] = value;
        }

        public static Vector4 operator *(Vector4 a, Matrix4x4 b) 
            => new Vector4(
                (a.X*b[0,0]) + (a.Y*b[1,0]) + (a.Z*b[2,0]) + (a.W*b[3,0]),
                (a.X*b[0,1]) + (a.Y*b[1,1]) + (a.Z*b[2,1]) + (a.W*b[3,1]),
                (a.X*b[0,2]) + (a.Y*b[1,2]) + (a.Z*b[2,2]) + (a.W*b[3,2]),
                (a.X*b[0,3]) + (a.Y*b[1,3]) + (a.Z*b[2,3]) + (a.W*b[3,3]));

        public static Matrix4x4 operator *(Matrix4x4 a, Matrix4x4 b)
            => new Matrix4x4(
                new float[4, 4]
                {
                    { a[0,0]*b[0,0]+a[0,1]*b[1,0]+a[0,2]*b[2,0]+a[0,3]*b[3,0],
                      a[0,0]*b[0,1]+a[0,1]*b[1,1]+a[0,2]*b[2,1]+a[0,3]*b[3,1],
                      a[0,0]*b[0,2]+a[0,1]*b[1,2]+a[0,2]*b[2,2]+a[0,3]*b[3,2],
                      a[0,0]*b[0,3]+a[0,1]*b[1,3]+a[0,2]*b[2,3]+a[0,3]*b[3,3],
                    },
                    { a[1,0]*b[0,0]+a[1,1]*b[1,0]+a[1,2]*b[2,0]+a[1,3]*b[3,0],
                      a[1,0]*b[0,1]+a[1,1]*b[1,1]+a[1,2]*b[2,1]+a[1,3]*b[3,1],
                      a[1,0]*b[0,2]+a[1,1]*b[1,2]+a[1,2]*b[2,2]+a[1,3]*b[3,2],
                      a[1,0]*b[0,3]+a[1,1]*b[1,3]+a[1,2]*b[2,3]+a[1,3]*b[3,3],
                    },
                    { a[2,0]*b[0,0]+a[2,1]*b[1,0]+a[2,2]*b[2,0]+a[2,3]*b[3,0],
                      a[2,0]*b[0,1]+a[2,1]*b[1,1]+a[2,2]*b[2,1]+a[2,3]*b[3,1],
                      a[2,0]*b[0,2]+a[2,1]*b[1,2]+a[2,2]*b[2,2]+a[2,3]*b[3,2],
                      a[2,0]*b[0,3]+a[2,1]*b[1,3]+a[2,2]*b[2,3]+a[2,3]*b[3,3],
                    },
                    { a[3,0]*b[0,0]+a[3,1]*b[1,0]+a[3,2]*b[2,0]+a[3,3]*b[3,0],
                      a[3,0]*b[0,1]+a[3,1]*b[1,1]+a[3,2]*b[2,1]+a[3,3]*b[3,1],
                      a[3,0]*b[0,2]+a[3,1]*b[1,2]+a[3,2]*b[2,2]+a[3,3]*b[3,2],
                      a[3,0]*b[0,3]+a[3,1]*b[1,3]+a[3,2]*b[2,3]+a[3,3]*b[3,3],
                    },
                });
    }

    public static class MatrixPrefabs
    {
        public static Matrix4x4 Identity()
        {
            var result = new Matrix4x4(
                new float[4,4]
                {
                    { 1, 0, 0, 0, },
                    { 0, 1, 0, 0, },
                    { 0, 0, 1, 0, },
                    { 0, 0, 0, 1, },
                });

            return result;
        }

        public static Matrix4x4 LookAtLeftHanded(Vector3 camera, Vector3 target, Vector3 up)
        {
            var zAxis = target - camera;
            zAxis = zAxis.Normalize();

            var xAxis = up.CrossProduct(zAxis);
            xAxis = xAxis.Normalize();

            var yAxis = zAxis.CrossProduct(xAxis);

            var result = MatrixPrefabs.Identity();
            result[0,0] = xAxis.X; result[1,0] = xAxis.Y; result[2,0] = xAxis.Z;
            result[0,1] = yAxis.X; result[1,1] = yAxis.Y; result[2,1] = yAxis.Z;
            result[0,2] = zAxis.X; result[1,2] = zAxis.Y; result[2,2] = zAxis.Z;

            result[3, 0] = xAxis.DotProduct(camera);
            result[3, 1] = yAxis.DotProduct(camera);
            result[3, 2] = zAxis.DotProduct(camera);

            result[3,0] = -result[3,0];
            result[3,1] = -result[3,1];
            result[3,2] = -result[3,2];

            return result;
        }

        public static Matrix4x4 PerspectiveFieldOfViewRightHanded(
            float fieldOfView,
            float aspectRatio,
            float nearClippingZ,
            float farClippingZ)
        {
            float yScale = (float)(1.0f / Math.Tan(fieldOfView * 0.5f));
            float q = farClippingZ / (nearClippingZ - farClippingZ);

            var result = new Matrix4x4(
                new float[4,4]
                {
                    { yScale / aspectRatio, 0,      0,                 0     },
                    { 0,                    yScale, 0,                 0     },
                    { 0,                    0,      q,                 -1.0f },
                    { 0,                    0,      q * nearClippingZ, 0     }
                });

            return result;
        }

        public static Matrix4x4 TranslationMatrix(Vector3 toPosition)
        {
            return MatrixPrefabs.TranslationMatrix(toPosition.X, toPosition.Y, toPosition.Z);
        }

        private static Matrix4x4 TranslationMatrix(float x, float y, float z)
        {
            // 1 0 0 0
            // 0 1 0 0
            // 0 0 1 0
            // x y z 1

            var result = MatrixPrefabs.Identity();
            result[3, 0] = x;
            result[3, 1] = y;
            result[3, 2] = z;

            return result;
        }

        public static Matrix4x4 RotationMatrixYawPitchRoll(float yaw, float pitch, float roll)
        {
            // TODO learn quaternions
            float halfRoll = roll * 0.5f;
            float halfPitch = pitch * 0.5f;
            float halfYaw = yaw * 0.5f;

            float sinRoll = (float)Math.Sin(halfRoll);
            float cosRoll = (float)Math.Cos(halfRoll);
            float sinPitch = (float)Math.Sin(halfPitch);
            float cosPitch = (float)Math.Cos(halfPitch);
            float sinYaw = (float)Math.Sin(halfYaw);
            float cosYaw = (float)Math.Cos(halfYaw);

            var vec4 = new Vector4();
            vec4.X = (cosYaw * sinPitch * cosRoll) + (sinYaw * cosPitch * sinRoll);
            vec4.Y = (sinYaw * cosPitch * cosRoll) - (cosYaw * sinPitch * sinRoll);
            vec4.Z = (cosYaw * cosPitch * sinRoll) - (sinYaw * sinPitch * cosRoll);
            vec4.W = (cosYaw * cosPitch * cosRoll) + (sinYaw * sinPitch * sinRoll);

            float xx = vec4.X * vec4.X;
            float yy = vec4.Y * vec4.Y;
            float zz = vec4.Z * vec4.Z;
            float xy = vec4.X * vec4.Y;
            float zw = vec4.Z * vec4.W;
            float zx = vec4.Z * vec4.X;
            float yw = vec4.Y * vec4.W;
            float yz = vec4.Y * vec4.Z;
            float xw = vec4.X * vec4.W;

            var result = MatrixPrefabs.Identity();
            result[0,0] = 1.0f - (2.0f * (yy + zz));
            result[0,2] = 2.0f * (xy + zw);
            result[0,2] = 2.0f * (zx - yw);
            result[1,0] = 2.0f * (xy - zw);
            result[1,1] = 1.0f - (2.0f * (zz + xx));
            result[1,2] = 2.0f * (yz + xw);
            result[2,0] = 2.0f * (zx + yw);
            result[2,1] = 2.0f * (yz - xw);
            result[2,2] = 1.0f - (2.0f * (yy + xx));

            return result;
        }
    }
}