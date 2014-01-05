#region Using

using System;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TDF.Graphics.Data;
using TDF.Graphics.Effects;
using TDF.Graphics.Render;
using Buffer = SharpDX.Direct3D11.Buffer;
using MapFlags = SharpDX.Direct3D11.MapFlags;

#endregion Using

namespace TDF.Graphics.Models
{
    public sealed class DxModel
    {
        public List<Material> Materials;
        public List<Mesh> Meshes;

        public Matrix? Matrix { get; set; }

        public DxModel()
        {
            Materials = new List<Material>(20);
            Meshes = new List<Mesh>(20);
        }

        public int MeshCount
        {
            get { return Meshes.Count; }
        }

        public void Render()
        {
            for (int i = 0; i < Meshes.Count; i++)
            {
                Meshes[i].Render((Matrix ?? SharpDX.Matrix.Identity) *DirectX11.ViewMatrix * DirectX11.ProjectionMatrix);
            }
        }

        public void SetMeshes(params Mesh[] meshes)
        {
            Meshes = new List<Mesh>(meshes);
        }
    }

    public class Mesh
    {
        public int EffectNumber;
        internal bool HasMaterial;

        public Material Material { get; protected set; }

        public void SetMaterial(Material mater)
        {
            Material = mater;
            HasMaterial = true;
        }

        public virtual void Render(Matrix m)
        {
            EffectManager.Effects[EffectNumber].SetMesh(this, m);
            EffectManager.Effects[EffectNumber].Render();
            Render();
            EffectManager.Effects[EffectNumber].EndRender();
        }

        public int IndexCount;
        public int VertexCount;
        protected int VertexStride;

        private bool _isInitedVb, _isInitedIb;

        protected Buffer IndexBuffer { get; set; }

        protected Buffer VertexBuffer { get; set; }

        protected virtual void Render()
        {
            DirectX11.DeviceContext.InputAssembler.SetVertexBuffers(0,
                new VertexBufferBinding(VertexBuffer,
                    VertexStride,
                    0));
            DirectX11.DeviceContext.InputAssembler.SetIndexBuffer(IndexBuffer, Format.R32_UInt, 0);
            DirectX11.DeviceContext.DrawIndexed(IndexCount, 0, 0);
        }

        public virtual void CreateIndexBuffer(uint[] indices)
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
            _isInitedIb = true;
        }

        public void CreateIndexBuffer(List<uint> indices)
        {
            CreateIndexBuffer(indices.ToArray());
        }

        public virtual void CreateVertexBuffer<T>(T[] vert) where T : struct
        {
            VertexCount = vert.Length;
            VertexStride = Marshal.SizeOf(typeof(T));

            var vbd = new BufferDescription(
                VertexStride * VertexCount,
                ResourceUsage.Dynamic,
                BindFlags.VertexBuffer,
                CpuAccessFlags.Write,
                ResourceOptionFlags.None,
                0
                );
            VertexBuffer = Buffer.Create(DirectX11.Device, vert, vbd);
            _isInitedVb = true;
        }

        public void CreateVertexBuffer<T>(List<T> vertices) where T : struct
        {
            CreateVertexBuffer(vertices.ToArray());
        }

        
        public virtual void SetIndices(uint[] indices)
        {
            if (!_isInitedIb) {CreateIndexBuffer(indices);
                return;
            }
            DataStream mappedResource;

            // Lock the vertex buffer so it can be written to.
            DirectX11.DeviceContext.MapSubresource(IndexBuffer, MapMode.WriteDiscard, MapFlags.None,
                                         out mappedResource);

            // Copy the data into the vertex buffer.
            mappedResource.WriteRange(indices);

            // Unlock the vertex buffer.
            DirectX11.DeviceContext.UnmapSubresource(IndexBuffer, 0);
        }

        public void SetIndices(List<uint> indices)
        {
            SetIndices(indices.ToArray());
        }

        public virtual void SetVertices<T>(T[] vert) where T : struct
        {
            if (!_isInitedVb)
            {
                CreateVertexBuffer(vert);
                return;
            }
            if (VertexStride !=  Marshal.SizeOf(typeof (T))) throw new Exception("\tCannotSetVertices\r\n\tNot valid vertex type");
            DataStream mappedResource;

            // Lock the vertex buffer so it can be written to.
            DirectX11.DeviceContext.MapSubresource(VertexBuffer, MapMode.WriteDiscard, MapFlags.None,
                                         out mappedResource);

            // Copy the data into the vertex buffer.
            mappedResource.WriteRange(vert);

            // Unlock the vertex buffer.
            DirectX11.DeviceContext.UnmapSubresource(VertexBuffer, 0);
        }

        public void SetVertices<T>(List<T> vertices) where T : struct
        {
            SetVertices(vertices.ToArray());
        }
    }
}