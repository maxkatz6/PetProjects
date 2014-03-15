using System;
using Ormeli.CG;
using Ormeli.Core.Patterns;
using Ormeli.DirectX11;
using Ormeli.Graphics;
using Ormeli.Math;
using Ormeli.OpenGL3;

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
            App.Render.ChangeBackColor(Color.Indigo);

            ShaderManager.Shaders.Add(App.Render.InitCgShader("vertex.cg", "pixel.cg"));

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
