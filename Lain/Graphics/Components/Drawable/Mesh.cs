using System.Runtime.InteropServices;
using SharpDX;
using Lain.GAPI;

namespace Lain.Graphics.Drawable
{
	public struct GeometryInfo<Vertex> where Vertex : struct, IVertex
	{
		public Vertex[] Vertices { get; set; }
		public int[] Indices { get; set; }

		public GeometryInfo<Vertex> Transform(Matrix m)
		{
			for (int i = 0; i < Vertices.Length; i++)
			{
				Vertices[i].Position = Vector3.TransformCoordinate(Vertices[i].Position, m);
				//Vertices[i].Normal = Vector3.TransformNormal(Vertices[i].Normal, m);
			}
			return this;
		}
	}
	public class Mesh
	{

		public Mesh(Effect ef, int[] ind)
		{
			Effect = ef;
			IndexCount = ind.Length;
			Ib = Buffer.Create(ind, BindFlag.IndexBuffer, BufferUsage.Default, CpuAccessFlags.None);
		}

		public Buffer Vb { get; private set; }
		public Buffer Ib { get; }
		public bool IsDynamic { get; private set; }
		public int VertexSize { get; private set; }
		public Effect Effect { get; }
		public int IndexCount { get; }
		public string Tech { get; set; }

		public virtual void Draw(Matrix matrix)
		{
			Effect.SetMatrix(matrix);
			App.Render.SetVertexBuffer(Vb, VertexSize);
			App.Render.SetIndexBuffer(Ib);
			Effect.Render(IndexCount);
		}

		protected void SetVertices<T>(T[] vert, bool isDynamic) where T : struct
		{
			IsDynamic = isDynamic;
			Vb = IsDynamic
				? Buffer.Create(vert, BindFlag.VertexBuffer)
				: Buffer.Create(vert, BindFlag.VertexBuffer, BufferUsage.Default, CpuAccessFlags.None);
			VertexSize = Marshal.SizeOf(vert[0]);
		}
	}


	public class TextureMesh : Mesh
	{
		public TextureMesh(Effect effect, GeometryInfo<TextureVertex> geometry, bool isDynamic = false)
			: base(effect, geometry.Indices)
		{
			SetVertices(geometry.Vertices, isDynamic);
		}

		public Texture Texture { get; set; } = Texture.Null;

		public override void Draw(Matrix matrix)
		{
			Effect.SetTexture(Texture);
			base.Draw(matrix);
		}
	}
}