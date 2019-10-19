using SharpDX;
using System.Runtime.InteropServices;

namespace TDF.Graphics.Data
{
    /// <summary>
    /// Свет
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DirectionalLight
    {
        public Color4 Ambient;
        public Color4 Diffuse;
        public Color4 Specular;
        public Vector3 Direction;
        public float Pad;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Material
    {
        public Color4 Ambient;
        public Color4 Diffuse;
        public Color4 Specular;
        public Color4 Reflect;

        public Texture DiffuseTexture;
        public Texture NormalTexture;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PointLight
    {
        public Color4 Ambient;
        public Color4 Diffuse;
        public Color4 Specular;

        public Vector3 Position;
        public float Range;

        public Vector3 Attenuation;
        public float Pad;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SpotLight
    {
        public Color4 Ambient;
        public Color4 Diffuse;
        public Color4 Specular;
        public Vector3 Position;
        public float Range;
        public Vector3 Direction;
        public float Spot;
        public Vector3 Attenuation;
        public float Pad;
    }
}