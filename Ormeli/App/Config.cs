namespace Ormeli
{
    public static class Config
    {
        public static int Height { get; set; }
        public static int Width { get; set; }
        public static bool VerticalSyncEnabled { get; set; }
        public static bool FullScreen { get; set; }
        public static bool IsDebug { get; set; }

        public static void Initialize()
        {
#if DEBUG
            IsDebug = true;
#else
            IsDebug = false;
#endif
        }
    }
}
