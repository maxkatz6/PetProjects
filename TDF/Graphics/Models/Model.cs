#region Using

using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TDF.Graphics.Data;
using TDF.Graphics.Effects;
using TDF.Graphics.Render;

#endregion Using

namespace TDF.Graphics.Models
{
    public sealed class DxModel
    {
        public List<Material> Materials;
        public List<StaticMesh> Meshes;

        public Matrix? Matrix { get; set; }

        public DxModel()
        {
            Materials = new List<Material>(20);
            Meshes = new List<StaticMesh>(20);
        }

        public int MeshCount
        {
            get { return Meshes.Count; }
        }

        public void Render()
        {
            for (int i = 0; i < Meshes.Count; i++)
            {
                Meshes[i].Render(Matrix ?? SharpDX.Matrix.Identity);
            }
        }

        public void SetMeshes(params StaticMesh[] meshes)
        {
            Meshes = new List<StaticMesh>(meshes);
        }
    }

    public class Mesh
    {
        public int IndexCount;
        public int VertexCount;
        protected int VertexStride;

        public Buffer IndexBuffer { get; set; }

        public Buffer VertexBuffer { get; set; }

        public virtual void Render()
        {
            DirectX11.DeviceContext.InputAssembler.SetVertexBuffers(0,
                new VertexBufferBinding(VertexBuffer,
                    VertexStride,
                    0));
            DirectX11.DeviceContext.InputAssembler.SetIndexBuffer(IndexBuffer, Format.R32_UInt, 0);
            DirectX11.DeviceContext.DrawIndexed(IndexCount, 0, 0);
        }

        public virtual void SetIndices(uint[] indices)
        {
            IndexCount = indices.Length;
            var ibd = new BufferDescription(
                sizeof(uint) * IndexCount,
                ResourceUsage.Immutable,
                BindFlags.IndexBuffer,
                CpuAccessFlags.None,
                ResourceOptionFlags.None,
                0
                );
            IndexBuffer = Buffer.Create(DirectX11.Device, indices, ibd);
        }

        public void SetIndices(List<uint> indices)
        {
            SetIndices(indices.ToArray());
        }

        public virtual void SetVertices<T>(T[] vert) where T : struct
        {
            VertexCount = vert.Length;
            VertexStride = Marshal.SizeOf(typeof(T));

            var vbd = new BufferDescription(
                VertexStride * VertexCount,
                ResourceUsage.Immutable,
                BindFlags.VertexBuffer,
                CpuAccessFlags.None,
                ResourceOptionFlags.None,
                0
                );
            VertexBuffer = Buffer.Create(DirectX11.Device, vert, vbd);
        }

        public void SetVertices<T>(List<T> vertices) where T : struct
        {
            SetVertices(vertices.ToArray());
        }
    }

    public sealed class StaticMesh : Mesh
    {
        public int Effect;
        internal bool HasMaterial;

        public Material Material { get; private set; }

        public void SetMaterial(Material mater)
        {
            Material = mater;
            HasMaterial = true;
        }

        public void Render(Matrix m)
        {
            EffectManager.Effects[Effect].SetModel(this, m);
            EffectManager.Effects[Effect].Render();
            Render();
            EffectManager.Effects[Effect].EndRender();
        }
    }
}