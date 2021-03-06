﻿using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfClient.Scene;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SceneComposer sceneComposer = new SceneComposer();

        private DateTime previousRenderTime = DateTime.Now;

        public MainWindow()
        {
            InitializeComponent();

            this.Title = "3D Softengine";

            this.Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ImageControl.Source = new BitmapImage();

            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        void CompositionTarget_Rendering(object sender, object e)
        {
            var time = DateTime.Now;
            this.Title = $"3D Softengine - {1000.0 / (time - previousRenderTime).TotalMilliseconds:0.00} fps";
            previousRenderTime = time;

            var args = e as RenderingEventArgs;

            var bitMap = sceneComposer
                .ComposeSceneForScreen(
                    (int)this.ImageControl.Height,
                    (int)this.ImageControl.Width,
                    (float)args.RenderingTime.TotalSeconds);

            ImageControl.Source = BitmapToImageSource(bitMap);
        }

        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
    }
}
