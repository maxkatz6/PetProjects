using System.Runtime.InteropServices;
using Ormeli.Math;

namespace Ormeli.Graphics
{
    public enum VertexType
    {
        Color = 0,
        Texture = 1
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 16)]
    public struct BitmapVertex
    {
        public static readonly int SizeInBytes = Marshal.SizeOf(typeof(ColorVertex));
        public static readonly Attrib[] Attribs =
        {
            new Attrib(AttribIndex.Position, 2, Attrib.OrmeliType.Float, 0),
            new Attrib(AttribIndex.TexCoord, 2, Attrib.OrmeliType.Float, 8)
        };
        public const int Number = 0;

        public Vector2 Position;
        public Vector2 TexCoord;

        public BitmapVertex(Vector2 pos, Vector2 texCoord)
        {
            Position = pos;
            TexCoord = texCoord;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 28)]
    public struct ColorVertex
    {
        public static readonly int SizeInBytes = Marshal.SizeOf(typeof(ColorVertex));
        public static readonly Attrib[] Attribs =
        {
            new Attrib(AttribIndex.Position, 3, Attrib.OrmeliType.Float, 0),
            new Attrib(AttribIndex.Color, 4, Attrib.OrmeliType.Float, 12)
        };
        public const int Number = 1;

        public Vector3 Position;
        public Color Color;

        public ColorVertex(Vector3 pos, Color color)
        {
            Position = pos;
            Color = color;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 20)]
    public struct TextureVertex
    {
        public static readonly int SizeInBytes = Marshal.SizeOf(typeof(TextureVertex));
        public static readonly Attrib[] Attribs =
        {
            new Attrib(AttribIndex.Position, 3, Attrib.OrmeliType.Float, 0),
            new Attrib(AttribIndex.TexCoord, 2, Attrib.OrmeliType.Float, 12)
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
