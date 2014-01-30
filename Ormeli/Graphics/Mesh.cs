using Ormeli.Math;

namespace Ormeli.Graphics
{
    public struct ColorVertex
    {
        public Vector3 Position;
        public Color Color;
    }
    public class Mesh
    {
        public int[] Indices;
        public ColorVertex[] Vertices;
    }
}
