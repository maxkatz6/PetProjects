using System.Collections.Generic;
using System.Runtime.InteropServices;
using Ormeli.Core.Patterns;
using Ormeli.GAPI;
using Ormeli.Graphics.Builders;
using SharpDX;

namespace Ormeli.Graphics.Drawable
{
	public class Mesh : IDrawable
	{
		private static readonly Dictionary<string, Effect> effects = new Dictionary<string, Effect>();

		public Mesh(string ef, Attrib[] attribs, int[] ind)
		{
			if (effects.ContainsKey(ef)) Effect = effects[ef];
			else
			{
				Effect = new Effect(ef, attribs);
				effects.Add(ef, Effect);
			}
			IndexCount = ind.Length;
			Ib = App.Creator.CreateBuffer(ind, BindFlag.IndexBuffer, BufferUsage.Default, CpuAccessFlags.None);
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
			Effect.Render(this);
		}

		protected void SetVertices<T>(T[] vert, bool isDynamic) where T : struct
		{
			IsDynamic = isDynamic;
			Vb = IsDynamic
				? App.Creator.CreateBuffer(vert, BindFlag.VertexBuffer)
				: App.Creator.CreateBuffer(vert, BindFlag.VertexBuffer, BufferUsage.Default, CpuAccessFlags.None);
			VertexSize = Marshal.SizeOf(vert[0]);
		}
	}


	public class TextureMesh : Mesh
	{
		public TextureMesh(string effect, GeometryInfo<TextureVertex> geometry, bool isDynamic = false)
			: base(effect, TextureVertex.Attribs, geometry.Indices)
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