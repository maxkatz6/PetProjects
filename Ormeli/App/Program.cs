using System;
using System.Windows.Forms;
using Ormeli.Core.Patterns;
using Ormeli.DirectX11;
using Color = Ormeli.Math.Color;

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
            var render = new DXRender();
            render.Initialize(Form.Handle);
            App.Initialize(render);
            Form.Paint += (sender, args) =>
            {
                App.Render.BeginDraw(new Color(100,0,255,100));
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
