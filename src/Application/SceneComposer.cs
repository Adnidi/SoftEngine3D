using System.Drawing;
using System.Drawing.Imaging;
using SoftEngine3D.Imaging;
using SoftEngine3D.Primitives;
using SoftEngine3D.Utility;

namespace SoftEngineClient
{
    public class SceneComposer
    {
        private readonly Mesh[] _meshes;
        private readonly Camera _camera;
        private readonly Renderer _renderer;

        public SceneComposer()
        {
            _renderer = new Renderer();

            _meshes = MeshImporter.LoadBabylonFile($@"..\..\..\Data\untitled.babylon");
            _camera = new Camera
            {
                Position = new Vector3(0, 0, 10),
                Target = new Vector3(),
            };
        }


        public Bitmap ComposeSceneForScreen(int height, int width, float secondsSinceLastRender)
        {
            Bitmap bm = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            foreach (var mesh in _meshes)
            {
                mesh.Rotation = new Vector3(
                    mesh.Rotation.X + (0.01f),
                    mesh.Rotation.Y + (0.01f),
                    mesh.Rotation.Z);
            }

            _renderer.Render(_camera, bm, _meshes);

            return bm;
        }
    }
}