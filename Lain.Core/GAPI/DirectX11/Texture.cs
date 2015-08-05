#if DX
using System;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System.Collections.Generic;

namespace Lain.GAPI
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Texture
	{
		public static readonly Texture Null;
        private static readonly Dictionary<string, Texture> textures = new Dictionary<string, Texture>(100);

        public static Func<string, Texture> FromFileDel = new Func<string, Texture>(s => Null);

		private readonly ShaderResourceView ResourceView;

		public readonly int Height;
		public readonly int Width;

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
        public static implicit operator Texture(string tex)
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

        public static Texture FromFile(string file)
        {
            if (string.IsNullOrEmpty(file) || file.Contains("null")) return Null;
            if (textures.ContainsKey(file)) return textures[file];
            var t = FromFileDel(file);
            textures.Add(file, t);
            return t;
        }
        
        public static unsafe Texture Create(Color4[,] array)
        {
            var w = array.GetLength(1);
            var h = array.GetLength(0);
            
            fixed (void* p = &array[0, 0])
                return Create(new IntPtr(p), w, h);
        }
        public static Texture Create(IntPtr p, int w, int h, Format f = Format.R32G32B32A32_Float)
        {
            return
                new Texture(new ShaderResourceView(App.Render.Device, new Texture2D(App.Render.Device, new Texture2DDescription
                {
                    ArraySize = 1,
                    BindFlags = BindFlags.ShaderResource,
                    CpuAccessFlags = 0,
                    Format = f,
                    Height = h,
                    MipLevels = 1,
                    OptionFlags = 0,
                    Usage = ResourceUsage.Default,
                    Width = w,
                    SampleDescription = new SampleDescription(1, 0)
                }, new[]
                {
                    new DataBox(new DataStream(p, w*h*FormatHelper.SizeOfInBytes(f), true, true).DataPointer,
                        w*FormatHelper.SizeOfInBytes(f), 0)
                })), w, h);
        }
    }
}

#endif