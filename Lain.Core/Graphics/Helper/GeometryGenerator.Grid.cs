using Lain.Graphics.Drawable;
using SharpDX;

namespace Lain.Graphics
{
    public partial class GeometryGenerator
    {
        public static GeometryInfo<Vertex> CreateGrid<Vertex>(float width, float depth)
            where Vertex : struct, IVertex
        {
            float w2 = 0.5f*width, d2 = 0.5f*depth;
            var mV = new[]
            {
                new Vertex {Position = new Vector3(-w2, 0, -d2), TexCoord = new Vector2(1, 1)},
                new Vertex {Position = new Vector3(w2, 0, -d2), TexCoord = new Vector2(0, 1)},
                new Vertex {Position = new Vector3(w2, 0, d2), TexCoord = new Vector2(0, 0)},
                new Vertex {Position = new Vector3(-w2, 0, d2), TexCoord = new Vector2(1, 0)}
            };
            return new GeometryInfo<Vertex> {Indices = new[] {0, 1, 2, 0, 2, 3}, Vertices = mV};
        }
    }
}