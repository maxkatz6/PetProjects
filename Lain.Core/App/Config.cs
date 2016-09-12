using System;

namespace Lain
{
    public static class Config
    {
        public enum RenderType
        {
            DirectX11,
            DirectX12
        }

        public static readonly bool IsDebug;
        public static readonly bool IsMono;

        static Config()
        {
#if DEBUG
            IsDebug = true;
#else
			IsDebug = false;
#endif
            IsMono = Type.GetType("Mono.Runtime") != null;
        }

        public static string Tittle { get; set; } = "Lain";
        public static int Height { get; set; } = 1000;
        public static int Width { get; set; } = 1920;
        public static bool VerticalSyncEnabled { get; set; } = false;
        public static bool FullScreen { get; set; } = false;
        public static bool Enable4xMSAA { get; set; } = false;
        public static float ScreenNear { get; set; } = 0.1f;
        public static float ScreenDepth { get; set; } = 20000;
        public static uint Fps { get; set; } = 100;
        public static RenderType Render { get; set; }
    }
}