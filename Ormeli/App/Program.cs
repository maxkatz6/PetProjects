using System.Windows.Forms;
using Ormeli.Core.Patterns;
using Ormeli.DirectX11;
using Ormeli.Graphics;
using Ormeli.Math;

namespace Ormeli
{
    public class Program : Disposable
    {
        public Form Form;

        static void Main(string[] args)
        {
            using (var prog = new Program())
               Application.Run(prog.Form);
        }

        private readonly Mesh mesh = new Mesh();
        public Program()
        {
            Config.Height = 1080;
            Config.Width = 1920;
            Form = new Form();
            App.Initialize(new DXRender(), Form.Handle);
            App.Render.ChangeBackColor(new Color(100, 0, 255, 100));

            mesh.Initalize(new[] {1, 2, 3}, new[]
            {
                new ColorVertex(new Vector3(10, 0, -10), new Color(100, 0, 255, 1)),
                new ColorVertex(new Vector3(20, 10, -10), new Color(100, 0, 255, 1)),
                new ColorVertex(new Vector3(30, 0, -10), new Color(100, 0, 255, 1))
            });

            Form.Paint += Draw;
        }

        private void Draw(object sender, PaintEventArgs args)
        {
            App.Render.BeginDraw();
            mesh.Render();
            App.Render.EndDraw();
        }

        protected override void OnDispose()
        {
            if (Form != null)
            {
                Form.Dispose();
                Form = null;
            }
        }
    }
}
