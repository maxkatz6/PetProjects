using Ormeli.GAPI;
using SharpDX;

namespace Ormeli.Graphics.GUI
{
	public class BitmapGS
	{
		struct BitmapInfo
		{
			public Vector2 Location;
			public Vector2 Size;
		}
		private static readonly Effect Effect;
		private Vector2 size;
		private Vector2 loc;
		private Buffer Vb;

		static BitmapGS()
		{
			Effect = Effect.FromFile("BitmapEffectGS.hlsl");
		}

		public BitmapGS()
		{
			Vb = Buffer.Create(new BitmapInfo[1], BindFlag.VertexBuffer);
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
			Effect.Render(1);
		}

		private unsafe void UpdateVertexBuffer()
		{
			*(BitmapInfo*) Vb.MapBuffer() = new BitmapInfo
			{
				Location = new Vector2(Location.X/Config.Width*2 - 1, -(Location.Y/Config.Height*2 - 1)),
				Size = new Vector2(Size.X/Config.Width*2, (Size.Y/Config.Height*2))
			};
			Vb.UnmapBuffer();
		}
	}
}