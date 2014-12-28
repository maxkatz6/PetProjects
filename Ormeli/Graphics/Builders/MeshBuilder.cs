using System.Runtime.InteropServices;
using Ormeli.Core.Patterns;

namespace Ormeli.Graphics.Builders
{
    public class MeshBuilder : Builder<Mesh, MeshBuilder>
    {
        public MeshBuilder SetTexture(int index)
        {
            template.TextureN = index;
            return this;
        }
        public MeshBuilder SetTexture(string fileName)
        {
            return SetTexture(Texture.Get(fileName));
        }

        public MeshBuilder SetShader(int index)
        {
            template.ShaderN = index;
            return this;
        }
        public MeshBuilder SetTech(string tech)
        {
            template.Tech = (tech);
            return this;
        }

        public MeshBuilder SetIndices(int[] ind)
        {
            template.IndexCount = ind.Length;
            template.Ib = App.Creator.CreateBuffer(ind, BindFlag.IndexBuffer, BufferUsage.Default, CpuAccessFlags.None);
            return this;
        }

        public MeshBuilder SetVertices<T>(T[] vert, bool isDynamic) where T : struct
        {
            template.IsDynamic = isDynamic;
            template.VertexSize = Marshal.SizeOf(vert[0]);
            template.Vb = isDynamic ? App.Creator.CreateBuffer(vert, BindFlag.VertexBuffer)
                : App.Creator.CreateBuffer(vert, BindFlag.VertexBuffer, BufferUsage.Default, CpuAccessFlags.None);
            return this;
        }
    }
}
