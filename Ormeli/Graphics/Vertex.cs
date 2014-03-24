using System.Runtime.InteropServices;
using Ormeli.Math;

namespace Ormeli.Graphics
{
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
}
