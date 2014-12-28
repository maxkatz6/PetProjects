using System;
using System.Globalization;
using System.Threading;
using Ormeli.GAPI.DirectX11;
using Ormeli.GAPI.Interfaces;
using Ormeli.GAPI.OpenGL;
using Ormeli.Graphics;
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
        public static RenderType RenderType;
        public static EffectLanguage EffectLanguage;

        internal static IRender Render;
        internal static ICreator Creator;
        internal static readonly Loop Loop = new Loop();

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

        public static void Exit()
        {
            Environment.Exit(0);
        }
    }
}
