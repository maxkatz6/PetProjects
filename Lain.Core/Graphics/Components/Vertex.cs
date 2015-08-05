using System.Runtime.InteropServices;
using SharpDX;

namespace Lain.Graphics
{
	public interface IVertex
	{
		Vector3 Position { get; set; }
		Vector2 TexCoord { get; set; }
		Color4 Color { get; set; }
		Vector3 Normal { get; set; }
		Vector3 TangentU { get; set; }
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct BitmapVertex : IVertex
	{
		public static readonly int SizeInBytes = Marshal.SizeOf<BitmapVertex>();

		public Vector2 Location { get; set; }
		public Vector2 TexCoord { get; set; }
        public Color4 Color { get; set; }

        public Vector3 Position
		{
			get { return new Vector3(Location, 0); }
			set { Location = new Vector2(value.X, value.Y); }
		}

		public Vector3 Normal { get {return Vector3.Zero;} set {} }
		public Vector3 TangentU { get { return Vector3.Zero; } set { } }
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ColorVertex : IVertex
	{
		public static readonly int SizeInBytes = Marshal.SizeOf<ColorVertex>();

		public Vector3 Position { get; set; }
		public Color4 Color { get; set; }

		public Vector2 TexCoord
		{
			get { return Vector2.Zero; }
			set { }
		}
		public Vector3 Normal { get { return Vector3.Zero; } set { } }
		public Vector3 TangentU { get { return Vector3.Zero; } set { } }
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct TextureVertex : IVertex
	{
		public static readonly int SizeInBytes = Marshal.SizeOf<TextureVertex>();

		public Vector3 Position { get; set; }
		public Vector2 TexCoord { get; set; }

		public Color4 Color
		{
			get { return Color4.Black; }
			set { }
		}
		public Vector3 Normal { get { return Vector3.Zero; } set { } }
		public Vector3 TangentU { get { return Vector3.Zero; } set { } }
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LightVertex : IVertex
	{
		public Vector3 Position { get; set; }
		public Vector2 TexCoord { get; set; }
		public Vector3 Normal { get; set; }
		public Vector3 TangentU { get; set; }

		public Color4 Color
		{
			get { return Color4.Black; }
			set { }
		}
	}
}