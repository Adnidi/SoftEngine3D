using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace SoftEngineClient
{
    public class SimulatedScreen
    {
        private readonly Size _initialSize;

        private Form _form;
        
        private Stopwatch _stopWatch;
        private long _millisecondsSinceLastRender;

        private Func<int, int, float, Bitmap> _sceneComposer;



        public SimulatedScreen(Size initialSize)
        {
            this._initialSize = initialSize;
        }

        public void Run(Func<int, int, float, Bitmap> sceneCompositionHandler)
        {
            _sceneComposer = sceneCompositionHandler;
            

            _form = new Form
            {
                BackColor = Color.Black,
                Bounds = Screen.PrimaryScreen.Bounds,
                Size = _initialSize
            };

            _stopWatch = Stopwatch.StartNew();

            _form.Paint += RenderHandler;

            Application.EnableVisualStyles();
            Application.Run(_form);
        }

        private void RenderHandler(object sender, PaintEventArgs e)
        {
            _millisecondsSinceLastRender = _stopWatch.ElapsedMilliseconds - _millisecondsSinceLastRender;

            var height = e.ClipRectangle.Size.Height;
            var width = e.ClipRectangle.Size.Width;

            var bm = _sceneComposer(height, width, (float)_millisecondsSinceLastRender / 1000.0f);

            Present(bm, e);

            ((Form)sender).Invalidate();
        }

        

        private void Present(Bitmap bm, PaintEventArgs e)
        {
            Graphics graphicsObj = e.Graphics;
            graphicsObj.DrawImage(bm, 0, 0);
            graphicsObj.Dispose();
        }
    }
}