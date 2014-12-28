using System;
using System.IO;

namespace Ormeli
{
    public static class Config
    {
        public static readonly bool IsDebug;
        public static readonly bool IsMono;

        public static string Tittle { get; set; } = "OrmeliEngine";
        public static int Height { get; set; } = 750;
        public static int Width { get; set; } = 1020;
        public static bool VerticalSyncEnabled { get; set; } = false;
        public static bool FullScreen { get; set; } = false;
        public static bool Enable4xMSAA { get; set; } = false;

        public static float ScreenNear { get; set; } = 0.1f;
        public static float ScreenDepth { get; set; } = 20000;
        public static uint Fps { get; set; } = 60;

        static readonly string BaseDirectory; 
        static readonly string EffectDirectory;

        public static string GetEffectPath(string fileName)
        {
            return Path.Combine(EffectDirectory, fileName);
        }

        public static string GetDataPath(string fileName, params string[] pathDirs)
        {
            return fileName == null ? null : Path.Combine(BaseDirectory, Path.Combine(pathDirs), fileName.Replace('/', '\\').Trim('\\'));
        }

        static Config ()
        {
#if DEBUG  
            IsDebug = true;
#else
            IsDebug = false;
#endif
            IsMono = Type.GetType ("Mono.Runtime") != null;
            BaseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
            EffectDirectory = Path.Combine(BaseDirectory, "Effects");
        }
    }
}
