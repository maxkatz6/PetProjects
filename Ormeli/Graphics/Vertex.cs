using System;
using System.Runtime.InteropServices;
using Ormeli.Math;

namespace Ormeli.Graphics
{
    public struct ColorVertex
    {
        public static readonly int SizeInBytes = Marshal.SizeOf(typeof(ColorVertex));

        public Vector3 Position;
        public Color Color;

        public ColorVertex(Vector3 pos, Color color)
        {
            Position = pos;
            Color = color;
        }
    }
}
