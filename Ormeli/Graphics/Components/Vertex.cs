using System.Runtime.InteropServices;
using Ormeli.Math;

namespace Ormeli.Graphics
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ColorVertex
    {
        public static readonly int SizeInBytes = Marshal.SizeOf(typeof(ColorVertex));
        public static readonly Attrib[] Attribs =
        {
            new Attrib(AttribIndex.Position, 3, Attrib.OrmeliType.Float, 0),
            new Attrib(AttribIndex.Color, 4, Attrib.OrmeliType.Float, 12)
        };
        public const int Number = 0;

        public Vector3 Position;
        public Color Color;

        public ColorVertex(Vector3 pos, Color color)
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
            new Attrib(AttribIndex.Position, 3, Attrib.OrmeliType.Float, 0),
            new Attrib(AttribIndex.TexCoord, 2, Attrib.OrmeliType.Float, 12)
        };
        public const int Number = 1;

        public Vector3 Position;
        public Vector2 TexCoord;

        public TextureVertex(Vector3 pos, Vector2 texCoord)
        {
            Position = pos;
            TexCoord = texCoord;
        }

        public TextureVertex(float pos, float texCoord, float f, float f1, float f2)
        {
            Position = new Vector3(pos,texCoord, f);
            TexCoord = new Vector2(f1, f2);
        }
    }
}
