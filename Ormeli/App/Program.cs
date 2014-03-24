﻿using System;
using OpenTK.Graphics.OpenGL;
using Ormeli.CG;
using Ormeli.Core.Patterns;
using Ormeli.DirectX11;
using Ormeli.Graphics;
using Ormeli.Math;
using Ormeli.OpenGL;
using SharpDX.DXGI;

namespace Ormeli
{
    public class Program : Disposable
    {
        static void Main(string[] args)
        {
            new Program().Run();
        }
        private readonly Mesh mesh = new Mesh();
        public Program()
        {
            
             Config.Height = 500;
            Config.Width = 500;
            App.Initialize(Console.ReadLine() == "1" ? (IRenderClass) new DXRender() : new OGRender());
            HardwareDescription.VideoCardMemory = 0;
            App.Render.ChangeBackColor(Color.Indigo);

            var effect = new CgEffect("effect.cgfx", 0);
            effect.SetAttribNum(0,0);

            EffectManager.Effects.Add(effect);
            
            mesh.Initalize(new[] {0,1,2,0,2,3}, new[]
            {
                new ColorVertex(new Vector3(-0.8f,  -0.8f,0), Color.Red),
                new ColorVertex(new Vector3( -0.8f,  0.8f, 0), Color.White),
                new ColorVertex(new Vector3(0.8f, 0.8f, 0), Color.Blue),
                new ColorVertex(new Vector3(0.8f,  -0.8f,0), Color.Green),
            });

        }

        public void Run()
        {
            App.Run(Draw);
        }

        private void Draw()
        {
            App.Render.BeginDraw();
            mesh.Render();
            App.Render.EndDraw();
        }
    }
}
