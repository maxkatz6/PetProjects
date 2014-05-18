using System;
using System.IO;

namespace Ormeli
{
    public static class Config
    {
        public static readonly bool IsDebug;
        public static readonly bool IsMono;

        public static string Tittle { get; set; }
        public static int Height { get; set; }
        public static int Width { get; set; }
        public static bool VerticalSyncEnabled { get; set; }
        public static bool FullScreen { get; set; }
        public static bool Enable4xMSAA { get; set; }

        public static float ScreenNear { get; set; }
        public static float ScreenDepth { get; set; }
        public static uint Fps { get; set; }

        public static string RenderVersion;

        public static readonly string BaseDirectory; static readonly string EffectDirectory;
        static readonly string TextureDirectory;

        public static string GetEffectPath(string fileName)
        {
            return Path.Combine(EffectDirectory, fileName);
        }
        public static string GetTexturePath(string fileName)
        {
            return Path.Combine(TextureDirectory, fileName);
        }

        static Config ()
        {
#if DEBUG  
            IsDebug = true;
#else
            IsDebug = false;
#endif
            IsMono = Type.GetType ("Mono.Runtime") != null;

            Tittle = "Test";
            BaseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
            EffectDirectory = Path.Combine(BaseDirectory, "Effects");
            TextureDirectory = Path.Combine(BaseDirectory, "Textures");

            Fps = 60;
            ScreenNear = 0.1f;
            ScreenDepth = 10000;
            Height = 500;
            Width = 1000;
        }
        public static void Initialize()
        {
        }
    }
}
