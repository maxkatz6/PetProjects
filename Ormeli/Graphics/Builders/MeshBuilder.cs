using System.Runtime.InteropServices;
using Ormeli.Core.Patterns;
using Ormeli.Graphics.Components;

namespace Ormeli.Graphics.Builders
{
    public class MeshBuilder : Builder<Mesh, MeshBuilder>
    {
        public MeshBuilder SetTexture(int index)
        {
            Template.TextureN = index;
            return this;
        }
        public MeshBuilder SetTexture(string fileName)
        {
            return SetTexture(Texture.Get(fileName));
        }

        public MeshBuilder SetShader(int index)
        {
            Template.ShaderN = index;
            return this;
        }
        public MeshBuilder SetTech(string tech)
        {
            Template.Tech = (tech);
            return this;
        }

        public MeshBuilder SetIndices(int[] ind)
        {
            Template.IndexCount = ind.Length;
            Template.Ib = App.Creator.CreateBuffer(ind, BindFlag.IndexBuffer, BufferUsage.Default, CpuAccessFlags.None);
            return this;
        }

        public MeshBuilder SetVertices<T>(T[] vert, bool isDynamic) where T : struct
        {
            Template.IsDynamic = isDynamic;
            Template.VertexSize = Marshal.SizeOf(vert[0]);
            Template.Vb = isDynamic ? App.Creator.CreateBuffer(vert, BindFlag.VertexBuffer)
                : App.Creator.CreateBuffer(vert, BindFlag.VertexBuffer, BufferUsage.Default, CpuAccessFlags.None);
            return this;
        }
    }
}
