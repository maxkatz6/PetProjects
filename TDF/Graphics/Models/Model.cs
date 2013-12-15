#region Using

using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TDF.Graphics.Data;
using TDF.Graphics.Effects;
using TDF.Graphics.Render;
using Effect = TDF.Graphics.Effects.Effect;
using Material = TDF.Graphics.Data.Material;

#endregion Using

namespace TDF.Graphics.Models
{
    public sealed class DxModel
    {
        public List<Mesh> Meshes;
        public Material[] Materials;
        public Matrix Matrix { get; set; }

        public int MeshCount
        {
            get { return Meshes.Count; }
        }

        public void Render()
        {
            foreach (var mesh in Meshes)
            {
                mesh.Render(Matrix);
            }
        }
    }

    public sealed class Mesh : MeshM
    {
        public readonly int VertexType;

        public Material Material;

        public bool HasMaterial;

        public Effect Effect;

        public void Render(Matrix m)
        {
            Effect.SetModel(this, m);
            Effect.Render();
            Render();
        }
    }

    public class MeshM
    {
        public int IndexCount;
        public int VertexCount;
        protected int _vertexStride;

        public Buffer IndexBuffer { get; set; }

        public Buffer VertexBuffer { get; set; }

        public virtual void SetVertices<T>(List<T> vertices) where T : struct
        {
            VertexCount = vertices.Count;
            _vertexStride = Marshal.SizeOf(typeof(T));

            var vbd = new BufferDescription(
                _vertexStride * VertexCount,
                ResourceUsage.Immutable,
                BindFlags.VertexBuffer,
                CpuAccessFlags.None,
                ResourceOptionFlags.None,
                0
                );
            VertexBuffer = Buffer.Create(DirectX11.Device, vertices.ToArray(), vbd);
        }

        public virtual void SetIndices(List<uint> indices)
        {
            IndexCount = indices.Count;
            var ibd = new BufferDescription(
                sizeof(uint) * IndexCount,
                ResourceUsage.Immutable,
                BindFlags.IndexBuffer,
                CpuAccessFlags.None,
                ResourceOptionFlags.None,
                0
                );
            IndexBuffer = Buffer.Create(DirectX11.Device, indices.ToArray(), ibd);
        }

        public virtual void Render()
        {
            DirectX11.DeviceContext.InputAssembler.SetVertexBuffers(0,
                new VertexBufferBinding(VertexBuffer,
                    _vertexStride,
                    0));
            DirectX11.DeviceContext.InputAssembler.SetIndexBuffer(IndexBuffer, Format.R32_UInt, 0);
            DirectX11.DeviceContext.DrawIndexed(IndexCount, 0, 0);
        }
    }
}