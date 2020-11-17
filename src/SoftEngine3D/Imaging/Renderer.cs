using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SoftEngine3D.Primitives;
using SoftEngine3D.Primitives.Lighting;

namespace SoftEngine3D.Imaging
{
    public class Renderer
    {
        private Bitmap _workingBitmap;
        private float[,] zBuffer;

        public void RenderLine(Camera camera, Bitmap bmp, params Mesh[] meshes)
        {
            _workingBitmap = bmp;

            zBuffer = new float[_workingBitmap.Size.Width, _workingBitmap.Size.Height];
            for (var i = 0; i < zBuffer.GetLength(0); i++)
            {
                for (var j = 0; j < zBuffer.GetLength(1); ++j)
                {
                    zBuffer[i, j] = float.MaxValue;
                }
            }

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

                foreach (var face in mesh.Faces)
                {
                    var vertexA = mesh.Vertices[face.A];
                    var vertexB = mesh.Vertices[face.B];
                    var vertexC = mesh.Vertices[face.C];

                    var vertexATransformed = Project(vertexA, transformMatrix, worldMatrix);
                    var vertexBTransformed = Project(vertexB, transformMatrix, worldMatrix);
                    var vertexCTransformed = Project(vertexC, transformMatrix, worldMatrix);

                    DrawLineBresenham(vertexATransformed.RelativePosition, vertexBTransformed.RelativePosition, vertexA.Color, vertexB.Color);
                    DrawLineBresenham(vertexBTransformed.RelativePosition, vertexCTransformed.RelativePosition, vertexB.Color, vertexC.Color);
                    DrawLineBresenham(vertexCTransformed.RelativePosition, vertexATransformed.RelativePosition, vertexC.Color, vertexA.Color);
                }
            }
        }

        public void Render(Camera camera, Bitmap bmp, IEnumerable<LightSource> lightSources, params Mesh[] meshes)
        {
            _workingBitmap = bmp;

            zBuffer = new float[_workingBitmap.Size.Width, _workingBitmap.Size.Height];
            for (var i = 0; i < zBuffer.GetLength(0); i++)
            {
                for (var j = 0; j < zBuffer.GetLength(1); ++j)
                {
                    zBuffer[i, j] = float.MaxValue;
                }
            }

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

                //foreach (var vertex in mesh.Vertices)
                //{
                //    var point = Project(vertex.RelativePosition, transformMatrix);
                //    DrawPoint(point, vertex.Color);
                //}

                foreach (var face in mesh.Faces)
                {
                    var vertexA = mesh.Vertices[face.A];
                    var vertexB = mesh.Vertices[face.B];
                    var vertexC = mesh.Vertices[face.C];

                    var vertexATransformed = Project(vertexA, transformMatrix, worldMatrix);
                    var vertexBTransformed = Project(vertexB, transformMatrix, worldMatrix);
                    var vertexCTransformed = Project(vertexC, transformMatrix, worldMatrix);

                    //DrawLine(pixelA, pixelB, vertexA.Color, vertexB.Color);
                    //DrawLine(pixelB, pixelC, vertexB.Color, vertexC.Color);
                    //DrawLine(pixelC, pixelA, vertexC.Color, vertexA.Color);

                    //DrawLineBresenham(pixelA, pixelB, vertexA.Color, vertexB.Color);
                    //DrawLineBresenham(pixelB, pixelC, vertexB.Color, vertexC.Color);
                    //DrawLineBresenham(pixelC, pixelA, vertexC.Color, vertexA.Color);

                    DrawTriangle(vertexATransformed, vertexBTransformed, vertexCTransformed, lightSources);
                }
            }
        }

        private Vertex Project(Vertex vertex, Matrix4x4 transformationMatrix, Matrix4x4 worldMatrix)
        {
            var point = vertex.RelativePosition.LeftTransformWithMatrix(transformationMatrix);

            var pointInWorld = vertex.RelativePosition.LeftTransformWithMatrix(worldMatrix);
            var normalInWorld = vertex.NormalVector.LeftTransformWithMatrix(transformationMatrix);

            var x = point.X * _workingBitmap.Size.Width + _workingBitmap.Size.Width / 2.0f;
            var y = -point.Y * _workingBitmap.Size.Height + _workingBitmap.Size.Height / 2.0f;
            return new Vertex
            {
                Color = vertex.Color,
                NormalVector = normalInWorld,
                WorldPosition = pointInWorld,
                RelativePosition = new Vector3(x, y, point.Z)
            };
        }

        private void DrawPoint(Vector3 point, Color color)
        {
            

            if (point.X >= 0 && point.Y >= 0 && point.X < _workingBitmap.Size.Width && point.Y < _workingBitmap.Size.Height)
            {
                if (zBuffer[(int) point.X, (int) point.Y] > point.Z)
                {
                    _workingBitmap.SetPixel((int)point.X, (int)point.Y, color);

                    zBuffer[(int) point.X, (int) point.Y] = point.Z;
                }
            }
        }

        private void DrawLine(Vector3 point0, Vector3 point1, Color color0, Color color1)
        {
            var dist = (point1 - point0).Length;

            if (dist < 2)
                return;

            Vector3 middlePoint = point0 + (point1 - point0) / 2;
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

        public void DrawLineBresenham(Vector3 point0, Vector3 point1, Color c0, Color c1)
        {
            int x0 = (int)point0.X;
            int y0 = (int)point0.Y;
            int x1 = (int)point1.X;
            int y1 = (int)point1.Y;

            var dx = Math.Abs(x1 - x0);
            var dy = Math.Abs(y1 - y0);
            var sx = (x0 < x1) ? 1 : -1;
            var sy = (y0 < y1) ? 1 : -1;
            var err = dx - dy;

            var totalDistance = (point0 - point1).Length;

            while (true)
            {
                var pointToDraw = new Vector3(x0, y0, 0);
                var distanceToP0 = (pointToDraw - point0).Length;
                var ratioCovered = distanceToP0 / totalDistance;

                if (ratioCovered < 0) ratioCovered = 0;
                if (ratioCovered > 1) ratioCovered = 1;

                var colorToDraw = Color
                    .FromArgb(
                        (int) (c0.A * (1-ratioCovered) + c1.A * ratioCovered),
                        (int) (c0.R * (1-ratioCovered) + c1.R * ratioCovered),
                        (int) (c0.G * (1-ratioCovered) + c1.G * ratioCovered),
                        (int) (c0.B * (1-ratioCovered) + c1.B * ratioCovered));

                DrawPoint(pointToDraw, colorToDraw);

                if ((x0 == x1) && (y0 == y1)) break;
                var e2 = 2 * err;
                if (e2 > -dy) { err -= dy; x0 += sx; }
                if (e2 < dx) { err += dx; y0 += sy; }
            }
        }

        public void DrawTriangle(Vertex v1, Vertex v2, Vertex v3, IEnumerable<LightSource> lights)
        {
            // sort by y coordinate
            if (v1.RelativePosition.Y > v2.RelativePosition.Y)
            {
                var temp = v2;
                v2 = v1;
                v1 = temp;
            }

            if (v2.RelativePosition.Y > v3.RelativePosition.Y)
            {
                var temp = v2;
                v2 = v3;
                v3 = temp;
            }

            if (v1.RelativePosition.Y > v2.RelativePosition.Y)
            {
                var temp = v2;
                v2 = v1;
                v1 = temp;
            }

            // normal face's vector is the average normal between each vertex's normal
            // computing also the center point of the face
            Vector3 vnFace = (v1.NormalVector + v2.NormalVector + v3.NormalVector) / 3;
            Vector3 centerPoint = (v1.WorldPosition + v2.WorldPosition + v3.WorldPosition) / 3;
            // Light position 
            Vector3 lightPos = lights.FirstOrDefault().Position ?? new Vector3();
            // computing the cos of the angle between the light vector and the normal vector
            // it will return a value between 0 and 1 that will be used as the intensity of the color
            float ndotl = ComputeNDotL(centerPoint, vnFace, lightPos);

            // inverse slopes
            float dP1P2, dP1P3;

            // http://en.wikipedia.org/wiki/Slope
            // Computing inverse slopes
            if (v2.RelativePosition.Y - v1.RelativePosition.Y > 0)
                dP1P2 = (v2.RelativePosition.X - v1.RelativePosition.X) / (v2.RelativePosition.Y - v1.RelativePosition.Y);
            else
                dP1P2 = 0;

            if (v3.RelativePosition.Y - v1.RelativePosition.Y > 0)
                dP1P3 = (v3.RelativePosition.X - v1.RelativePosition.X) / (v3.RelativePosition.Y - v1.RelativePosition.Y);
            else
                dP1P3 = 0;

            // First case where triangles are like that:
            // P1
            // -
            // -- 
            // - -
            // -  -
            // -   - P2
            // -  -
            // - -
            // -
            // P3
            if (dP1P2 > dP1P3)
            {
                for (var y = (int)v1.RelativePosition.Y; y <= (int)v3.RelativePosition.Y; y++)
                {
                    if (y < v2.RelativePosition.Y)
                    {
                        ProcessScanLine(y, v1.RelativePosition, v3.RelativePosition, v1.RelativePosition, v2.RelativePosition, v1, v2, v3, ndotl);
                    }
                    else
                    {
                        ProcessScanLine(y, v1.RelativePosition, v3.RelativePosition, v2.RelativePosition, v3.RelativePosition, v1, v2, v3, ndotl);
                    }
                }
            }
            // First case where triangles are like that:
            //       P1
            //        -
            //       -- 
            //      - -
            //     -  -
            // P2 -   - 
            //     -  -
            //      - -
            //        -
            //       P3
            else
            {
                for (var y = (int)v1.RelativePosition.Y; y <= (int)v3.RelativePosition.Y; y++)
                {
                    if (y < v2.RelativePosition.Y)
                    {
                        ProcessScanLine(y, v1.RelativePosition, v2.RelativePosition, v1.RelativePosition, v3.RelativePosition, v1, v2, v3, ndotl);
                    }
                    else
                    {
                        ProcessScanLine(y, v2.RelativePosition, v3.RelativePosition, v1.RelativePosition, v3.RelativePosition, v1, v2, v3, ndotl);
                    }
                }
            }
        }

        // Compute the cosine of the angle between the light vector and the normal vector
        // Returns a value between 0 and 1
        float ComputeNDotL(Vector3 vertex, Vector3 normal, Vector3 lightPosition)
        {
            var lightDirection = lightPosition - vertex;

            var normalizedNormal = normal.Normalize();
            var normalizedLightDirection = lightDirection.Normalize();

            return Math.Max(0, normalizedNormal.DotProduct(normalizedLightDirection));
        }

        // drawing line between 2 points from left to right
        // papb -> pcpd
        // pa, pb, pc, pd must then be sorted before
        void ProcessScanLine(int y, Vector3 pa, Vector3 pb, Vector3 pc, Vector3 pd, Vertex a, Vertex b, Vertex c, float lightAngle)
        {
            // Thanks to current Y, we can compute the gradient to compute others values like
            // the starting X (sx) and ending X (ex) to draw between
            // if pa.Y == pb.Y or pc.Y == pd.Y, gradient is forced to 1
            var gradient1 = pa.Y != pb.Y ? (y - pa.Y) / (pb.Y - pa.Y) : 1;
            var gradient2 = pc.Y != pd.Y ? (y - pc.Y) / (pd.Y - pc.Y) : 1;

            int sx = (int)Interpolate(pa.X, pb.X, gradient1);
            int ex = (int)Interpolate(pc.X, pd.X, gradient2);

            float zStart = Interpolate(pa.Z, pb.Z, gradient1);
            float zEnd = Interpolate(pc.Z, pd.Z, gradient2);

            // drawing a line from left (sx) to right (ex) 
            for (var x = sx; x < ex; x++)
            {
                var z = Interpolate(zStart, zEnd, (x - sx) / (float) (ex - sx));

                var pointToDraw = new Vector3(x, y, z);

                var distanceToA = (pointToDraw - a.RelativePosition).Length;
                var distanceToB = (pointToDraw - b.RelativePosition).Length;
                var distanceToC = (pointToDraw - c.RelativePosition).Length;

                var totalDistance = distanceToA + distanceToB + distanceToC;

                var partColorA = 1 - ((distanceToB + distanceToC) / totalDistance);
                var partColorB = 1 - ((distanceToC + distanceToA) / totalDistance);
                var partColorC = 1 - ((distanceToA + distanceToB) / totalDistance);

                var color = Color.FromArgb(
                    (int) ((a.Color.A * partColorA + b.Color.A * partColorB + c.Color.A * partColorC) * lightAngle),
                    (int) ((a.Color.R * partColorA + b.Color.R * partColorB + c.Color.R * partColorC) * lightAngle),
                    (int) ((a.Color.G * partColorA + b.Color.G * partColorB + c.Color.G * partColorC) * lightAngle),
                    (int) ((a.Color.B * partColorA + b.Color.B * partColorB + c.Color.B * partColorC) * lightAngle));

                DrawPoint(pointToDraw, color);
            }
        }

        public float Interpolate(float start, float end, float gradient)
        {
            return start + (end - start) * gradient;
        }
    }
}