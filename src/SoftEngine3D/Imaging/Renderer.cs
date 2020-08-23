using System;
using System.Drawing;
using SoftEngine3D.Primitives;

namespace SoftEngine3D.Imaging
{
    public class Renderer
    {
        private Bitmap _workingBitmap;

        public void Render(Camera camera, Bitmap bmp, params Mesh[] meshes)
        {
            _workingBitmap = bmp;

            var viewMatrix = MatrixPrefabs.LookAtLeftHanded(camera.Position, camera.Target, Vector3Prefabs.UnitY);
            var projectionMatrix = MatrixPrefabs.PerspectiveFieldOfViewRightHanded(
                0.78f,
                (float)_workingBitmap.Size.Width / _workingBitmap.Size.Height,
                0.01f,
                1.0f);

            foreach (Mesh mesh in meshes)
            {
                var worldMatrix = MatrixPrefabs.RotationMatrixYawPitchRoll(
                                      mesh.Rotation.Y,
                                      mesh.Rotation.X,
                                      mesh.Rotation.Z) *
                                  MatrixPrefabs.TranslationMatrix(mesh.Position);

                var transformMatrix = worldMatrix * viewMatrix * projectionMatrix;

                foreach (var vertex in mesh.Vertices)
                {
                    var point = Project(vertex.Position, transformMatrix);
                    DrawPoint(point, vertex.Color);
                }

                foreach (var face in mesh.Faces)
                {
                    var vertexA = mesh.Vertices[face.A];
                    var vertexB = mesh.Vertices[face.B];
                    var vertexC = mesh.Vertices[face.C];

                    var pixelA = Project(vertexA.Position, transformMatrix);
                    var pixelB = Project(vertexB.Position, transformMatrix);
                    var pixelC = Project(vertexC.Position, transformMatrix);

                    DrawLine(pixelA, pixelB, vertexA.Color, vertexB.Color);
                    DrawLine(pixelB, pixelC, vertexB.Color, vertexC.Color);
                    DrawLine(pixelC, pixelA, vertexC.Color, vertexA.Color);

                    //DrawLineBresenham(pixelA, pixelB);
                    //DrawLineBresenham(pixelB, pixelC);
                    //DrawLineBresenham(pixelC, pixelA);
                }
            }
        }

        private Vector2 Project(Vector3 coordinate, Matrix4x4 transformationMatrix)
        {
            var point = coordinate.LeftTransformWithMatrix(transformationMatrix);

            var x = point.X * _workingBitmap.Size.Width + _workingBitmap.Size.Width / 2.0f;
            var y = -point.Y * _workingBitmap.Size.Height + _workingBitmap.Size.Height / 2.0f;
            return (new Vector2(x, y));
        }

        private void DrawPoint(Vector2 point, Color color)
        {
            if (point.X >= 0 && point.Y >= 0 && point.X < _workingBitmap.Size.Width && point.Y < _workingBitmap.Size.Height)
            {
                _workingBitmap.SetPixel((int)point.X, (int)point.Y, color);
            }
        }

        private void DrawLine(Vector2 point0, Vector2 point1, Color color0, Color color1)
        {
            var dist = (point1 - point0).Length;

            if (dist < 2)
                return;

            Vector2 middlePoint = point0 + (point1 - point0) / 2;
            var midColor = InterpolateColor(color0, color1);

            DrawPoint(middlePoint, midColor);

            DrawLine(point0, middlePoint, color0, midColor);
            DrawLine(middlePoint, point1, midColor, color1);
        }

        private Color InterpolateColor(Color c0, Color c1)
        {
            return Color.FromArgb(
                InterpolateBytes(c0.A, c1.A),
                InterpolateBytes(c0.R, c1.R),
                InterpolateBytes(c0.G, c1.G),
                InterpolateBytes(c0.B, c1.B));
        }

        private byte InterpolateBytes(byte b0, byte b1)
        {
            var newValue = ((int) b0 + (int) b1) / 2;

            return (byte) newValue;
        }

        public void DrawLineBresenham(Vector2 point0, Vector2 point1)
        {
            //TODO: color interpolation
            int x0 = (int)point0.X;
            int y0 = (int)point0.Y;
            int x1 = (int)point1.X;
            int y1 = (int)point1.Y;

            var dx = Math.Abs(x1 - x0);
            var dy = Math.Abs(y1 - y0);
            var sx = (x0 < x1) ? 1 : -1;
            var sy = (y0 < y1) ? 1 : -1;
            var err = dx - dy;

            while (true)
            {
                DrawPoint(new Vector2(x0, y0), Color.Yellow);

                if ((x0 == x1) && (y0 == y1)) break;
                var e2 = 2 * err;
                if (e2 > -dy) { err -= dy; x0 += sx; }
                if (e2 < dx) { err += dx; y0 += sy; }
            }
        }
    }
}