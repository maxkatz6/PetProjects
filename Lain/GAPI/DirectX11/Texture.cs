#if DX
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Lain.Core;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace Lain.GAPI
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

		public static implicit operator int(Texture tex)
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

        private static Texture Load(string fileName)
        {
            int id = 0;
            DevIL.GenImages(1, ref id);
            DevIL.BindImage(id);
            if (!(DevIL.LoadImage(fileName) && DevIL.ConvertImage(DevIL.RGBA, DevIL.FLOAT)))
            {
                ErrorProvider.SendError("Dx11.Texture.Load() : "+DevIL.GetError()+" on load " + fileName + ".");
                return Null;
            }
            int w = DevIL.GetInteger(DevIL.IMAGE_WIDTH);
            int h = DevIL.GetInteger(DevIL.IMAGE_HEIGHT);

            return new Texture(new ShaderResourceView(App.Render.Device, new Texture2D(App.Render.Device, new Texture2DDescription
                {
                    ArraySize = 1,
                    BindFlags = BindFlags.ShaderResource,
                    CpuAccessFlags = 0,
                    Format = Format.R32G32B32A32_Float,
                    Height = h,
                    MipLevels = 1,
                    OptionFlags = 0,
                    Usage = ResourceUsage.Default,
                    Width = w,
                    SampleDescription = new SampleDescription(1, 0)
                }, new[]
                {
                    new DataBox(new DataStream(DevIL.GetData(), w*h*16, true, true).DataPointer,w*16, 0)
                })), w, h);
        }

        public static unsafe Texture Create(Color4[,] array)
		{
			var w = array.GetLength(1);
			var h = array.GetLength(0);

			DataBox[] v;
			fixed (void* p = &array[0, 0])
				v = new[]
				{
					new DataBox(new DataStream(new IntPtr(p), array.Length*16, true, true).DataPointer,
						w*FormatHelper.SizeOfInBytes(Format.R32G32B32A32_Float), 0)
				};
			return
				new Texture(new ShaderResourceView(App.Render.Device, new Texture2D(App.Render.Device, new Texture2DDescription
				{
					ArraySize = 1,
					BindFlags = BindFlags.ShaderResource,
					CpuAccessFlags = 0,
					Format = Format.R32G32B32A32_Float,
					Height = h,
					MipLevels = 1,
					OptionFlags = 0,
					Usage = ResourceUsage.Default,
					Width = w,
					SampleDescription = new SampleDescription(1, 0)
				}, v)), w, h);
		}

        private static class DevIL
        {
            public const uint IMAGE_WIDTH = 0x0DE4;
            public const uint IMAGE_HEIGHT = 0x0DE5;
            public const int RGBA = 0x1908;
            public const int FLOAT = 0x1406;

            static DevIL()
            {
                Init();
            }

            [DllImport("DevIL.dll", EntryPoint = "ilInit")]
            public static extern void Init();
            [DllImport("DevIL.dll", EntryPoint = "ilGenImages")]
            public static extern void GenImages(int d, ref int id);
            [DllImport("DevIL.dll", EntryPoint = "ilBindImage")]
            public static extern void BindImage(int id);
            [DllImport("DevIL.dll", EntryPoint = "ilDeleteImage")]
            public static extern void DeleteImage(int id);
            [DllImport("DevIL.dll", EntryPoint = "ilLoadImage", CharSet = CharSet.Ansi)]
            public static extern bool LoadImage(string filename);
            [DllImport("DevIL.dll", EntryPoint = "ilGetInteger")]
            public static extern int GetInteger(uint enumValue);
            [DllImport("DevIL.dll", EntryPoint = "ilConvertImage")]
            public static extern bool ConvertImage(int formatEnum, int typeEnum);
            [DllImport("DevIL.dll")]
            public static extern void ilDeleteImages(uint num, ref uint handle);
            [DllImport("DevIL.dll", EntryPoint = "ilGetError", CallingConvention = CallingConvention.StdCall)]
            public static extern ErrorType GetError();
            [DllImport("DevIL.dll", EntryPoint = "ilGetData", CallingConvention = CallingConvention.StdCall)]
            public static unsafe extern IntPtr GetData();
            public enum ErrorType
            {
                NoError = 0x0000,
                InvalidEnum = 0x0501,
                OutOfMemory = 0x0502,
                FormatNotSupported = 0x0503,
                InternalError = 0x0504,
                InvalidValue = 0x0505,
                IllegalOperation = 0x0506,
                IllegalFileValue = 0x0507,
                IllegalFileHeader = 0x0508,
                InvalidParameter = 0x0509,
                CouldNotOpenFile = 0x050A,
                InvalidExtension = 0x050B,
                FileAlreadyExists = 0x050C,
                OutFormatSame = 0x050D,
                StackOverflow = 0x050E,
                StackUnderflow = 0x050F,
                InvalidConversion = 0x0510,
                BadDimensions = 0x0511,
                FileReadError = 0x0512,
                FileWriteError = 0x0512,
                GifError = 0x05E1,
                JpegError = 0x05E2,
                PngError = 0x05E3,
                TiffError = 0x05E4,
                MngError = 0x05E5,
                Jp2Error = 0x05E6,              
                ExrError = 0x05E7,
                UnknownError = 0x05FF
            }

        }
    }
}

#endif