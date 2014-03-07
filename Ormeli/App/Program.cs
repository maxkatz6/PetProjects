using Ormeli.Core.Patterns;
using Ormeli.DirectX11;
using Ormeli.Graphics;
using Ormeli.Math;

namespace Ormeli
{
    public class Program : Disposable
    {
        static void Main(string[] args)
        {
            using (var prog = new Program())
            {
                prog.Run();
            }
        }

        private readonly Mesh mesh = new Mesh();
        public Program()
        {

             Config.Height = 500;
            Config.Width = 500;
            App.Initialize(new DXRender());
            App.Render.ChangeBackColor(Color.Indigo);

            mesh.Initalize(new[] {0,1,2}, new[]
            {
                new ColorVertex(new Vector3(-0.8f,  0.8f, 0.0f), Color.Red),
                new ColorVertex(new Vector3( 0.8f,  0.8f, 0.0f), Color.Green),
                new ColorVertex(new Vector3(0.0f, -0.8f, 0.0f), Color.Blue)
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
