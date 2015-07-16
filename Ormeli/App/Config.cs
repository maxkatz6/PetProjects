using System;
using System.IO;

namespace Ormeli
{
	public static class Config
	{
		public enum RenderType
		{
			DirectX11
		}

		public static readonly bool IsDebug;
		public static readonly bool IsMono;
		private static readonly string BaseDirectory;
		private static readonly string EffectDirectory;

		static Config()
		{
#if DEBUG  
            IsDebug = true;
#else
			IsDebug = false;
#endif
			IsMono = Type.GetType("Mono.Runtime") != null;
			BaseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
			EffectDirectory = Path.Combine(BaseDirectory, "Effects");
		}

		public static string Tittle { get; set; } = "Engine";
		public static int Height { get; set; } = 1000;
		public static int Width { get; set; } = 1920;
		public static bool VerticalSyncEnabled { get; set; } = false;
		public static bool FullScreen { get; set; } = false;
		public static bool Enable4xMSAA { get; set; } = false;
		public static float ScreenNear { get; set; } = 0.1f;
		public static float ScreenDepth { get; set; } = 20000;
		public static uint Fps { get; set; } = 100;
		public static RenderType Render { get; set; }

		public static string GetEffectPath(string fileName)
		{
			return Path.Combine(EffectDirectory, fileName);
		}

		public static string GetDataPath(string fileName, params string[] pathDirs)
		{
			return fileName == null
				? null
				: Path.Combine(BaseDirectory, Path.Combine(pathDirs), fileName.Replace('/', '\\').Trim('\\'));
		}
	}
}