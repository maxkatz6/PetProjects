using System;
using Ormeli.CG;
using Ormeli.Core;

namespace Ormeli
{
    public enum RenderType
    {
        DirectX11,
        OpneGl3
    }
    public static class App
    {
        public static RenderType RenderType;

        internal static IRenderClass Render;

        public static void Initialize(IRenderClass render)
        {
            Config.Initialize();
            Render = render;
            var t = new Timer();
            t.Initialize();
            Render.CreateWindow();
            RenderType = Render.Initialize();
            t.Frame();
            Console.WriteLine(t.FrameTime);
        }

        public static void Run(Action act)
        {
            Render.Run(act);
        }

        public static void Exit()
        {
            if (Render != null)
            {
                Render.Dispose();
                Render = null;
            }
            Environment.Exit(1);
        }
    }
}
