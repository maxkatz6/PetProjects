using Lain.Graphics.Drawable;
using SharpDX;

namespace Lain.Graphics
{
    public partial class GeometryGenerator
    {
        public static GeometryInfo<Vertex> CreateBox<Vertex>(float width, float height, float depth)
            where Vertex : struct, IVertex
        {
            float w2 = 0.5f*width, h2 = 0.5f*height, d2 = 0.5f*depth;

            return new GeometryInfo<Vertex>
            {
                Indices = new[]
                {
                    0, 1, 2, 0, 2, 3,
                    4, 5, 6, 4, 6, 7,
                    8, 9, 10, 8, 10, 11,
                    12, 13, 14, 12, 14, 15,
                    16, 17, 18, 16, 18, 19,
                    20, 21, 22, 20, 22, 23
                },
                Vertices = new[]
                {
                    new Vertex
                    {
                        Position = new Vector3(-w2, -h2, -d2),
                        TexCoord = new Vector2(0.0f, 1.0f),
                        Normal = new Vector3(0.0f, 0.0f, -1.0f),
                        TangentU = new Vector3(1.0f, 0.0f, 0.0f)
                    },
                    new Vertex
                    {
                        Position = new Vector3(-w2, +h2, -d2),
                        TexCoord = new Vector2(0.0f, 0.0f),
                        Normal = new Vector3(0.0f, 0.0f, -1.0f),
                        TangentU = new Vector3(1.0f, 0.0f, 0.0f)
                    },
                    new Vertex
                    {
                        Position = new Vector3(+w2, +h2, -d2),
                        TexCoord = new Vector2(1.0f, 0.0f),
                        Normal = new Vector3(0.0f, 0.0f, -1.0f),
                        TangentU = new Vector3(1.0f, 0.0f, 0.0f)
                    },
                    new Vertex
                    {
                        Position = new Vector3(+w2, -h2, -d2),
                        TexCoord = new Vector2(1.0f, 1.0f),
                        Normal = new Vector3(0.0f, 0.0f, -1.0f),
                        TangentU = new Vector3(1.0f, 0.0f, 0.0f)
                    },

                    // Fill in the back face vertex data.
                    new Vertex
                    {
                        Position = new Vector3(-w2, -h2, +d2),
                        TexCoord = new Vector2(1.0f, 1.0f),
                        Normal = new Vector3(0.0f, 0.0f, 1.0f),
                        TangentU = new Vector3(-1.0f, 0.0f, 0.0f)
                    },
                    new Vertex
                    {
                        Position = new Vector3(+w2, -h2, +d2),
                        TexCoord = new Vector2(0.0f, 1.0f),
                        Normal = new Vector3(0.0f, 0.0f, 1.0f),
                        TangentU = new Vector3(-1.0f, 0.0f, 0.0f)
                    },
                    new Vertex
                    {
                        Position = new Vector3(+w2, +h2, +d2),
                        TexCoord = new Vector2(0.0f, 0.0f),
                        Normal = new Vector3(0.0f, 0.0f, 1.0f),
                        TangentU = new Vector3(-1.0f, 0.0f, 0.0f)
                    },
                    new Vertex
                    {
                        Position = new Vector3(-w2, +h2, +d2),
                        TexCoord = new Vector2(1.0f, 0.0f),
                        Normal = new Vector3(0.0f, 0.0f, 1.0f),
                        TangentU = new Vector3(-1.0f, 0.0f, 0.0f)
                    },

                    // Fill in the top face vertex data.
                    new Vertex
                    {
                        Position = new Vector3(-w2, +h2, -d2),
                        TexCoord = new Vector2(0.0f, 1.0f),
                        Normal = new Vector3(0.0f, 1.0f, 0.0f),
                        TangentU = new Vector3(1.0f, 0.0f, 0.0f)
                    },
                    new Vertex
                    {
                        Position = new Vector3(-w2, +h2, +d2),
                        TexCoord = new Vector2(0.0f, 0.0f),
                        Normal = new Vector3(0.0f, 1.0f, 0.0f),
                        TangentU = new Vector3(1.0f, 0.0f, 0.0f)
                    },
                    new Vertex
                    {
                        Position = new Vector3(+w2, +h2, +d2),
                        TexCoord = new Vector2(1.0f, 0.0f),
                        Normal = new Vector3(0.0f, 1.0f, 0.0f),
                        TangentU = new Vector3(1.0f, 0.0f, 0.0f)
                    },
                    new Vertex
                    {
                        Position = new Vector3(+w2, +h2, -d2),
                        TexCoord = new Vector2(1.0f, 1.0f),
                        Normal = new Vector3(0.0f, 1.0f, 0.0f),
                        TangentU = new Vector3(1.0f, 0.0f, 0.0f)
                    },

                    // Fill in the bottom face vertex data.
                    new Vertex
                    {
                        Position = new Vector3(-w2, -h2, -d2),
                        TexCoord = new Vector2(1.0f, 1.0f),
                        Normal = new Vector3(0.0f, -1.0f, 0.0f),
                        TangentU = new Vector3(-1.0f, 0.0f, 0.0f)
                    },
                    new Vertex
                    {
                        Position = new Vector3(+w2, -h2, -d2),
                        TexCoord = new Vector2(0.0f, 1.0f),
                        Normal = new Vector3(0.0f, -1.0f, 0.0f),
                        TangentU = new Vector3(-1.0f, 0.0f, 0.0f)
                    },
                    new Vertex
                    {
                        Position = new Vector3(+w2, -h2, +d2),
                        TexCoord = new Vector2(0.0f, 0.0f),
                        Normal = new Vector3(0.0f, -1.0f, 0.0f),
                        TangentU = new Vector3(-1.0f, 0.0f, 0.0f)
                    },
                    new Vertex
                    {
                        Position = new Vector3(-w2, -h2, +d2),
                        TexCoord = new Vector2(1.0f, 0.0f),
                        Normal = new Vector3(0.0f, -1.0f, 0.0f),
                        TangentU = new Vector3(-1.0f, 0.0f, 0.0f)
                    },

                    // Fill in the left face vertex data.
                    new Vertex
                    {
                        Position = new Vector3(-w2, -h2, +d2),
                        TexCoord = new Vector2(0.0f, 1.0f),
                        Normal = new Vector3(-1.0f, 0.0f, 0.0f),
                        TangentU = new Vector3(0.0f, 0.0f, -1.0f)
                    },
                    new Vertex
                    {
                        Position = new Vector3(-w2, +h2, +d2),
                        TexCoord = new Vector2(0.0f, 0.0f),
                        Normal = new Vector3(-1.0f, 0.0f, 0.0f),
                        TangentU = new Vector3(0.0f, 0.0f, -1.0f)
                    },
                    new Vertex
                    {
                        Position = new Vector3(-w2, +h2, -d2),
                        TexCoord = new Vector2(1.0f, 0.0f),
                        Normal = new Vector3(-1.0f, 0.0f, 0.0f),
                        TangentU = new Vector3(0.0f, 0.0f, -1.0f)
                    },
                    new Vertex
                    {
                        Position = new Vector3(-w2, -h2, -d2),
                        TexCoord = new Vector2(1.0f, 1.0f),
                        Normal = new Vector3(-1.0f, 0.0f, 0.0f),
                        TangentU = new Vector3(0.0f, 0.0f, -1.0f)
                    },

                    // Fill in the right face vertex data.
                    new Vertex
                    {
                        Position = new Vector3(+w2, -h2, -d2),
                        TexCoord = new Vector2(0.0f, 1.0f),
                        Normal = new Vector3(1.0f, 0.0f, 0.0f),
                        TangentU = new Vector3(0.0f, 0.0f, 1.0f)
                    },
                    new Vertex
                    {
                        Position = new Vector3(+w2, +h2, -d2),
                        TexCoord = new Vector2(0.0f, 0.0f),
                        Normal = new Vector3(1.0f, 0.0f, 0.0f),
                        TangentU = new Vector3(0.0f, 0.0f, 1.0f)
                    },
                    new Vertex
                    {
                        Position = new Vector3(+w2, +h2, +d2),
                        TexCoord = new Vector2(1.0f, 0.0f),
                        Normal = new Vector3(1.0f, 0.0f, 0.0f),
                        TangentU = new Vector3(0.0f, 0.0f, 1.0f)
                    },
                    new Vertex
                    {
                        Position = new Vector3(+w2, -h2, +d2),
                        TexCoord = new Vector2(1.0f, 1.0f),
                        Normal = new Vector3(1.0f, 0.0f, 0.0f),
                        TangentU = new Vector3(0.0f, 0.0f, 1.0f)
                    }
                }
            };
        }
    }
}