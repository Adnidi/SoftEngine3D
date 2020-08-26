﻿using System;
using System.Drawing;

namespace SoftEngineClient
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var scene = new SceneComposer();

            var screen = new SimulatedScreen(
                new Size(
                    1920,
                    1080));

            screen.Run(scene.ComposeSceneForScreen);
        }
    }
}
