using System;
using Ormeli.DirectX11;
using Ormeli.OpenGL;
using Timer = Ormeli.Core.Timer;

namespace Ormeli
{
    public enum RenderType
    {
        DirectX11=1,
        OpenGl3
    }

    public static class App
    {
        public static RenderType RenderType;

        internal static IRender Render;
        internal static ICreator Creator;

        public static void Initialize(RenderType render)
        {
            RenderType = render;

            if (RenderType == RenderType.DirectX11)
                Render = new DxRender();
            else Render = new GlRender();

            using (var t = new Timer())
            {
                t.Start();

                Render.CreateWindow();
                Render.Initialize();
                Creator = Render.GetCreator();

                t.Frame();
                Console.WriteLine(render + " renderer started in " + t.FrameTime + " microseconds");
            }
        }

        public static void Exit()
        {
            Environment.Exit(0);
        }
    }
}
