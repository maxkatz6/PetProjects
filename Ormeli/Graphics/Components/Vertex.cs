using System.Runtime.InteropServices;
using Ormeli.GAPI.Interfaces;
using SharpDX;

namespace Ormeli.Graphics.Components
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BitmapVertex
    {
        public static readonly int SizeInBytes = Marshal.SizeOf(typeof(ColorVertex));

        public static readonly Attrib[] Attribs =
        {
            new Attrib(AttribIndex.Position, Vector2.SizeInBytes/sizeof (float), Attrib.OrmeliType.Float, 0),
            new Attrib(AttribIndex.TexCoord, Vector2.SizeInBytes/sizeof (float), Attrib.OrmeliType.Float,
                Vector2.SizeInBytes)
        };

        public const int Number = 0;

        public Vector2 Position;
        public Vector2 TexCoord;

        public BitmapVertex(Vector2 pos, Vector2 texCoord)
        {
            Position = pos;
            TexCoord = texCoord;
        }

        public BitmapVertex(float x, float y, float u, float v)
        {
            Position = new Vector2(x, y);
            TexCoord = new Vector2(u, v);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ColorVertex
    {
        public static readonly int SizeInBytes = Marshal.SizeOf(typeof(ColorVertex));

        public static readonly Attrib[] Attribs =
        {
            new Attrib(AttribIndex.Position, Vector3.SizeInBytes/sizeof (float), Attrib.OrmeliType.Float, 0),
            new Attrib(AttribIndex.Color, Vector4.SizeInBytes/sizeof (float), Attrib.OrmeliType.Float,
                Vector3.SizeInBytes)
        };

        public const int Number = 1;

        public Vector3 Position;
        public Color4 Color;

        public ColorVertex(Vector3 pos, Color4 color)
        {
            Position = pos;
            Color = color;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TextureVertex
    {
        public static readonly int SizeInBytes = Marshal.SizeOf(typeof(TextureVertex));

        public static readonly Attrib[] Attribs =
        {
            new Attrib(AttribIndex.Position, Vector3.SizeInBytes/sizeof (float), Attrib.OrmeliType.Float, 0),
            new Attrib(AttribIndex.TexCoord, Vector2.SizeInBytes/sizeof (float), Attrib.OrmeliType.Float,
                Vector3.SizeInBytes)
        };

        public const int Number = 2;

        public Vector3 Position;
        public Vector2 TexCoord;

        public TextureVertex(Vector3 pos, Vector2 texCoord)
        {
            Position = pos;
            TexCoord = texCoord;
        }

        public TextureVertex(float x, float y, float z, float u, float v)
        {
            Position = new Vector3(x, y, z);
            TexCoord = new Vector2(u, v);
        }
    }
}