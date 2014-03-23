using System.IO;

namespace Ormeli
{
    public static class Config
    {
        public static int Height { get; set; }
        public static int Width { get; set; }
        public static bool VerticalSyncEnabled { get; set; }
        public static bool FullScreen { get; set; }
        public static bool IsDebug { get; set; }
        public static string RenderVersion; 
        public static readonly string BaseDirectory;
        public static readonly string ShadersDirectory;
        static Config ()
        {
#if DEBUG  
            IsDebug = true;
#else
            IsDebug = false;
#endif
            BaseDirectory = System.AppDomain.CurrentDomain.BaseDirectory + "Resources\\";
            ShadersDirectory = BaseDirectory + "Shaders\\";
        }
        public static void Initialize()
        {
        }
    }
}
