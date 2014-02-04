using System.Windows.Forms;
using Ormeli.Core.Patterns;
using Ormeli.DirectX11;
using Ormeli.Graphics;
using Ormeli.Math;
using SharpDX.Direct3D11;

namespace Ormeli.App
{
    public class Program : Disposable
    {
        public Form Form;

        static void Main(string[] args)
        {
            using (var prog = new Program())
               Application.Run(prog.Form);
        }

        public Program()
        {
            Config.Height = 1080;
            Config.Width = 1920;
            Form = new Form();
            App.Initialize(new DXRender(), Form.Handle);

            App.Render.ChangeBackColor(new Color(100, 0, 255, 100));
            Form.Paint += (sender, args) =>
            {
                App.Render.BeginDraw();
                App.Render.EndDraw();
            };
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
