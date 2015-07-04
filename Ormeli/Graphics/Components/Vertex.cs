using System.Runtime.InteropServices;
using Ormeli.GAPI;
using SharpDX;

namespace Ormeli.Graphics
{
	public interface IVertex
	{
		Vector3 Position { get; set; }
		Vector2 TexCoord { get; set; }
		Color4 Color { get; set; }
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct BitmapVertex : IVertex
	{
		public static readonly int SizeInBytes = Marshal.SizeOf(typeof (ColorVertex));

		public static readonly Attrib[] Attribs =
		{
			new Attrib(AttribIndex.Position, Vector2.SizeInBytes/sizeof (float), AttribType.Float, 0),
			new Attrib(AttribIndex.TexCoord, Vector2.SizeInBytes/sizeof (float), AttribType.Float,
				Vector2.SizeInBytes)
		};

		public Vector2 Location { get; set; }
		public Vector2 TexCoord { get; set; }

		public Vector3 Position
		{
			get { return new Vector3(Location, 0); }
			set { Location = new Vector2(value.X, value.Y); }
		}

		public Color4 Color
		{
			get { return Color4.Black; }
			set { }
		}
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ColorVertex : IVertex
	{
		public static readonly int SizeInBytes = Marshal.SizeOf(typeof (ColorVertex));

		public static readonly Attrib[] Attribs =
		{
			new Attrib(AttribIndex.Position, Vector3.SizeInBytes/sizeof (float), AttribType.Float, 0),
			new Attrib(AttribIndex.Color, Vector4.SizeInBytes/sizeof (float), AttribType.Float,
				Vector3.SizeInBytes)
		};

		public Vector3 Position { get; set; }
		public Color4 Color { get; set; }

		public Vector2 TexCoord
		{
			get { return Vector2.Zero; }
			set { }
		}
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct TextureVertex : IVertex
	{
		public static readonly int SizeInBytes = Marshal.SizeOf(typeof (TextureVertex));

		public static readonly Attrib[] Attribs =
		{
			new Attrib(AttribIndex.Position, Vector3.SizeInBytes/sizeof (float), AttribType.Float, 0),
			new Attrib(AttribIndex.TexCoord, Vector2.SizeInBytes/sizeof (float), AttribType.Float,
				Vector3.SizeInBytes)
		};

		public Vector3 Position { get; set; }
		public Vector2 TexCoord { get; set; }

		public Color4 Color
		{
			get { return Color4.Black; }
			set { }
		}
	}
}