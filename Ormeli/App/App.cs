using System;
using System.Globalization;
using System.Threading;
using Ormeli.DirectX11;
using Ormeli.Graphics;
using Ormeli.OpenGL;
using Timer = Ormeli.Core.Timer;

namespace Ormeli
{
    public enum RenderType
    {
        DirectX11=1,
        OpenGl3
    }

    public enum EffectLanguage
    {
        CG,
        HLSL,
        GLSL
    }

    public static class App
    {
        public static readonly Loop Loop = new Loop();
        public static RenderType RenderType;
        public static EffectLanguage EffectLanguage;

        internal static IRender Render;
        internal static ICreator Creator;

        public static void Initialize(RenderType render, EffectLanguage effectLanguage = EffectLanguage.CG)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            RenderType = render;
            EffectLanguage = effectLanguage;

            if (RenderType == RenderType.DirectX11)
                Render = new DxRender();
            else Render = new GlRender();

            using (var t = new Timer())
            {
                t.Start();

                Render.CreateWindow();
                Render.Initialize();
                Render.ZBuffer(true);
                Render.AlphaBlending(true);

                Creator = Render.GetCreator();

                t.Frame();
                Console.WriteLine(render + " renderer started in " + t.FrameTime + " microseconds");
                Console.WriteLine(HardwareDescription.VideoCardDescription);
                Console.WriteLine("VideoCard Memory: " + HardwareDescription.VideoCardMemory);
                Console.WriteLine("----------------------------------------------");
            }
        }

        public static void Run(Action drawAction, Action updateAction)
        {
            Loop.Run(drawAction, updateAction);
        }

        public static void Exit()
        {
            Environment.Exit(0);
        }
    }
}
