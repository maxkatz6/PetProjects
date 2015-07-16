#if DX
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Color = SharpDX.Color;

namespace Ormeli.GAPI
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Texture
	{
		public static readonly Texture Null;

		private readonly ShaderResourceView ResourceView;

		public readonly int Height;
		public readonly int Width;

		private static readonly Dictionary<string, Texture> textures = new Dictionary<string, Texture>(100);

		public static Texture FromFile(string file)
		{
			if (string.IsNullOrEmpty(file) || file.Contains("null")) return Null;
			if (textures.ContainsKey(file)) return textures[file];

			var t = Load(file);
			textures.Add(file, t);

			return t;
		}

		static Texture()
		{
			Null = Create(new[,]
			{
				{
					new Color4(0, 0, 0, 1f), new Color4(0, 1f, 0, 1), new Color4(0, 0, 0f, 1), new Color4(0, 1f, 0, 1),
					new Color4(0, 0, 0, 1f)
				},
				{
					new Color4(0f, 1, 0, 1), new Color4(0, 0f, 0, 1), new Color4(0f, 1, 0, 1), new Color4(0, 0f, 0, 1),
					new Color4(0f, 1, 0, 1f)
				},
				{
					new Color4(0, 0, 0, 1f), new Color4(0, 1f, 0, 1), new Color4(0, 0, 0, 1f), new Color4(0, 1f, 0, 1),
					new Color4(0, 0, 0, 1f)
				},
				{
					new Color4(0f, 1, 0, 1), new Color4(0, 0f, 0, 1), new Color4(0f, 1, 0, 1), new Color4(0, 0f, 0, 1),
					new Color4(0f, 1, 0, 1f)
				},
				{
					new Color4(0, 0, 0, 1f), new Color4(0, 1f, 0, 1), new Color4(0, 0, 0f, 1), new Color4(0, 1f, 0, 1),
					new Color4(0, 0, 0, 1f)
				}
			});
		}
		
		private Texture(ShaderResourceView resourceView, int w, int h)
		{
			ResourceView = resourceView;
			Width = w;
			Height = h;
		}

		public bool IsNull => ResourceView == null;


		public static implicit operator ShaderResourceView(Texture tex)
		{
			return tex.ResourceView;
		}

		public static implicit operator IntPtr(Texture tex)
		{
			return tex.ResourceView.NativePointer;
		}

		public static implicit operator int (Texture tex)
		{
			return tex.ResourceView.NativePointer.ToInt32();
		}
		public static implicit operator Texture (string tex)
		{
			return FromFile(tex);
		}

		public static bool operator ==(Texture me, Texture other)
		{
			return me.ResourceView?.NativePointer == other.ResourceView?.NativePointer;
		}

		public static bool operator !=(Texture me, Texture other)
		{
			return !(me == other);
		}

		public bool Equals(Texture other)
		{
			return ResourceView.NativePointer.Equals(other.ResourceView.NativePointer);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is Texture && Equals((Texture) obj);
		}

		public override int GetHashCode()
		{
			return ResourceView.NativePointer.GetHashCode();
		}

		private static Texture Load(string fileName)
		{
			Bitmap bit = new Bitmap(fileName);
			
			var c = new Color4[ bit.Width, bit.Height];
			for (int y = 0; y < bit.Height; y++)
				for (int x = 0; x < bit.Width; x++)
				{
					var p = bit.GetPixel(x, y);
					c[x, y] = new Color(p.R, p.G, p.B, p.A);
				}
			return Create(Rotate(Rotate(Rotate(c))));
		}
		static T[,] Rotate<T>(T[,] oldMatrix)
		{
			T[,] newMatrix = new T[oldMatrix.GetLength(1), oldMatrix.GetLength(0)];
			int newColumn, newRow = 0;
			for (int oldColumn = oldMatrix.GetLength(1) - 1; oldColumn >= 0; oldColumn--)
			{
				newColumn = 0;
				for (int oldRow = 0; oldRow < oldMatrix.GetLength(0); oldRow++)
				{
					newMatrix[newRow, newColumn] = oldMatrix[oldRow, oldColumn];
					newColumn++;
				}
				newRow++;
			}
			return newMatrix;
		}

		public static unsafe Texture Create(Color4[,] array, Format format = Format.R32G32B32A32_Float)
		{
			var w = array.GetLength(1);
			var h = array.GetLength(0);

			DataBox[] v;
			fixed (void* p = &array[0, 0])
				v = new[]
				{
					new DataBox(new DataStream(new IntPtr(p), array.Length*16, true, true).DataPointer,
						w*FormatHelper.SizeOfInBytes(format), 0)
				};
			return new Texture(new ShaderResourceView(App.Render.Device, new Texture2D(App.Render.Device, new Texture2DDescription
			{
				ArraySize = 1,
				BindFlags = BindFlags.ShaderResource,
				CpuAccessFlags = 0,
				Format = format,
				Height = h,
				MipLevels = 1,
				OptionFlags = 0,
				Usage = ResourceUsage.Default,
				Width = w,
				SampleDescription = new SampleDescription(1, 0)
			}, v)), w, h);
		}
	}
}
#endif