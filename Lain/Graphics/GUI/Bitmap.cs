using Lain.GAPI;
using SharpDX;

namespace Lain.Graphics.GUI
{
	public class Bitmap
	{
		private static readonly Buffer Ib;
		private static readonly Effect Effect;
		private Vector2 size;
		private Vector2 loc;
		private Buffer Vb;

		static Bitmap()
		{
			Effect = Effect.FromFile("BitmapEffect.hlsl");
			Ib = Buffer.Create(new[] {0, 1, 2, 0, 3, 1}, BindFlag.IndexBuffer, BufferUsage.Default,
				CpuAccessFlags.None);
		}

		public Bitmap()
		{
			Vb = Buffer.Create(new BitmapVertex[4], BindFlag.VertexBuffer, BufferUsage.Default, CpuAccessFlags.None);
		}
		public Texture Texture { get; set; } = Texture.Null;

		public Vector2 Size
		{
			get { return size; }
			set
			{
				size = value;
				UpdateVertexBuffer();
			}
		}
		public Vector2 Location
		{
			get { return loc; }
			set
			{
				loc = value;
				UpdateVertexBuffer();
			}
		}
		

		public void Draw()
		{
			Effect.SetTexture(Texture);
			App.Render.SetVertexBuffer(Vb, BitmapVertex.SizeInBytes);
			App.Render.SetIndexBuffer(Ib);
			Effect.Render(6);
		}

		private void UpdateVertexBuffer()
		{
			var left = Location.X/Config.Width*2-1;
			var right = left + Size.X/Config.Width*2;
			var top = -(Location.Y/Config.Height*2-1);
			var bottom = top - (Size.Y/Config.Height*2); //є[-1;1]
			Vb = Buffer.Create(new[]
			{
				new BitmapVertex
				{
					Location = new Vector2(left, top),
					TexCoord = new Vector2(0, 0)
				},
				new BitmapVertex
				{
					Location = new Vector2(right, bottom),
					TexCoord = new Vector2(1, 1)
				},
				new BitmapVertex
				{
					Location = new Vector2(left, bottom),
					TexCoord = new Vector2(0, 1)
				},
				new BitmapVertex
				{
					Location = new Vector2(right, top),
					TexCoord = new Vector2(1, 0)
				}
			}, BindFlag.VertexBuffer, BufferUsage.Default, CpuAccessFlags.None);
		}
	}
}