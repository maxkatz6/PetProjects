using System;
using Ormeli.Core;

namespace Ormeli
{
    public enum RenderType
    {
        DirectX11,
        OpenGl3
    }
    public static class App
    {
        public static RenderType RenderType;

        internal static IRender Render;
        internal static ICreator Creator;

        public static void Initialize(IRender render)
        {
            Config.Initialize();
            Render = render;

            using (var t = new Timer())
            {
                t.Start();

                Render.CreateWindow();
                RenderType = Render.Initialize();
                Creator = Render.GetCreator();

                t.Frame();
                Console.WriteLine(t.FrameTime);
            }
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
