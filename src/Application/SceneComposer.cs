using System.Drawing;
using System.Drawing.Imaging;
using SoftEngine3D.Imaging;
using SoftEngine3D.Primitives;

namespace SoftEngineClient
{
    public class SceneComposer
    {
        private readonly Mesh _mesh;
        private readonly Camera _camera;
        private readonly Renderer _renderer;

        public SceneComposer()
        {
            _renderer = new Renderer();

            _mesh = MeshPrefabs.Cube();
            _camera = new Camera
            {
                Position = new Vector3(0, 0, 10),
                Target = new Vector3(),
            };
        }


        public Bitmap ComposeSceneForScreen(int height, int width, float secondsSinceLastRender)
        {
            Bitmap bm = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            _mesh.Rotation = new Vector3(
                _mesh.Rotation.X + (0.01f),
                _mesh.Rotation.Y + (0.01f),
                _mesh.Rotation.Z);

            _renderer.Render(_camera, bm, _mesh);

            return bm;
        }
    }
}