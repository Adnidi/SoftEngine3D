using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using SoftEngine3D.Imaging;
using SoftEngine3D.Primitives;
using SoftEngine3D.Utility;

namespace WpfClient.Scene
{
    public class SceneComposer
    {
        private readonly Mesh[] _meshes;
        private readonly Camera _camera;
        private readonly Renderer _renderer;
        private Mesh _coordinateAxes;

        public SceneComposer()
        {
            _renderer = new Renderer();

            _meshes = MeshImporter.LoadBabylonFile($@"..\..\..\Data\coloredSuzanne.babylon");
            _camera = new Camera
            {
                Position = new Vector3(1, 5, 10),
                Target = new Vector3(),
            };

            _coordinateAxes = new Mesh(
                new List<Vertex>
                {
                    new Vertex(new Vector3(-100, 0, 0), Color.Red),
                    new Vertex(new Vector3(100, 0, 0), Color.Red),
                    new Vertex(new Vector3(0, 0, 0), Color.Red),
                    new Vertex(new Vector3(0, -100, 0), Color.Green),
                    new Vertex(new Vector3(0, 100, 0), Color.Green),
                    new Vertex(new Vector3(0, 0, 0), Color.Green),
                    new Vertex(new Vector3(0, 0, -100), Color.Blue),
                    new Vertex(new Vector3(0, 0, 100), Color.Blue),
                    new Vertex(new Vector3(0, 0, 0), Color.Blue),
                },
                new List<Face>
                {
                    new Face { A = 0, B = 1, C = 2},
                    new Face { A = 3, B = 4, C = 5},
                    new Face { A = 6, B = 7, C = 8},
                }
            );
        }


        public Bitmap ComposeSceneForScreen(int height, int width, float secondsSinceLastRender)
        {
            Bitmap bm = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            _renderer.RenderLine(_camera, bm, _coordinateAxes);

            foreach (var mesh in _meshes)
            {
                mesh.Rotation = new Vector3(
                    mesh.Rotation.X ,
                    mesh.Rotation.Y + (0.1f),
                    mesh.Rotation.Z);
            }

            _renderer.Render(_camera, bm, _meshes);

            return bm;
        }
    }
}