using System.Runtime.InteropServices;
using SharpDX;

namespace TDF.Graphics.Data
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ColorVertex 
    {

        public Vector3 Position;
        public Color4 Color;

        public void SetToZero()
        {
            Position = Vector3.Zero;
            Color = Color4.Black;
        }

        public ColorVertex(Vector3 position, Color4 color)
        {
            Color = color;
            Position = position;
        }

        public ColorVertex(float x, float y, float z, float r, float g, float b, float a)
        {
            Color = new Color4(r, g, b, a);
            Position = new Vector3(x, y, z);
        }

        public const int VertexType = 0;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TextureVertex 
    {

        public Vector3 Position;
        public Vector2 Texture;

        public void SetToZero()
        {
            Position = Vector3.Zero;
            Texture = Vector2.Zero;
        }

        public TextureVertex(Vector3 position, Vector2 color)
        {
            Texture = color;
            Position = position;
        }

        public TextureVertex(float x, float y, float z, float u, float v)
        {
            Texture = new Vector2(u, v);
            Position = new Vector3(x, y, z);
        }

        public const int VertexType = 1;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LightVertex 
    {

        public Vector3 position;
        public Vector2 texture;
        public Vector3 normal;

        public void SetToZero()
        {
            position = Vector3.Zero;
            texture = Vector2.Zero;
            normal = Vector3.Zero;
        }

        public LightVertex(float px, float py, float pz, float nx, float ny, float nz, float u, float v)
        {
            position = new Vector3(px, py, pz);
            normal = new Vector3(nx, ny, nz);
            texture = new Vector2(u, v);
        }

        public LightVertex(Vector3 pos, Vector3 norm, Vector2 uv)
        {
            position = pos;
            texture = uv;
            normal = norm;
        }

        public const int VertexType = 2;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BumpVertex 
    {
        public Vector3 Position;
        public Vector2 Texture;
        public Vector3 Normal;
        public Vector3 Tangent;

        public void SetToZero()
        {
            Position = Vector3.Zero;
            Texture = Vector2.Zero;
            Normal = Vector3.Zero;
            Tangent = Vector3.Zero;
        }

        public BumpVertex(Vector3 position, Vector3 normal, Vector2 texC, Vector3 tangentU)
        {
            Position = position;
            Normal = normal;
            Texture = texC;
            Tangent = tangentU;
        }

        public BumpVertex(float px, float py, float pz, float nx, float ny, float nz, float tx, float ty, float tz,
            float u, float v)
        {
            Position = new Vector3(px, py, pz);
            Normal = new Vector3(nx, ny, nz);
            Texture = new Vector2(u, v);
            Tangent = new Vector3(tx, ty, tz);
        }

        public const int VertexType = 3;
    }
}