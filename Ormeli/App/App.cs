using System;
using Ormeli.CG;

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

        internal static RenderClass Render;

        public static void Initialize(RenderClass render)
        {
            Config.Initialize();
            Render = render;
            Render.CreateWindow();
            RenderType = Render.Initialize();
            CgShader.InitializeShaderEngine();
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
