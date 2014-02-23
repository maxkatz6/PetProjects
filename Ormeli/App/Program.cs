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

            mesh.Initalize(new[] {1, 2, 3}, new[]
            {
                new ColorVertex(new Vector3(10, 0, -10), Color.AliceBlue),
                new ColorVertex(new Vector3(20, 10, -10), Color.WhiteSmoke),
                new ColorVertex(new Vector3(30, 0, -10), Color.Indigo)
            });
        }

        public void Run()
        {
            App.Run(Draw);
        }

        private void Draw()
        {
            App.Render.BeginDraw();
          //  mesh.Render();
            App.Render.EndDraw();
        }
    }
}
