#region Using

using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Resources;
using SharpDX;
using SharpDX.Direct3D11;
using System.Collections.Generic;
using System.IO;
using SharpDX.DXGI;
using TDF.Core;
using TDF.Graphics.Render;
using TDF.Properties;

#endregion Using

namespace TDF.Graphics.Data
{
    public class Texture : IShutdown
    {
        public static Dictionary<string, Texture> Textures = new Dictionary<string, Texture>();

        public static Texture NullTexture { get; set; }

        public int Height { get; private set; }

        public ShaderResourceView TextureResource;//{ get; private set; }

        public int Width { get; private set; }

        public static Texture Load(string file)
        {
            if (file == null || file.Contains("null")) return NullTexture;
            if (Textures.ContainsKey(file)) return Textures[file];

            var texture = new Texture();
            var filePath = Config.TextureFilePath + file.TrimStart('/', '\\');
            if (!File.Exists(filePath)) return NullTexture;
            texture.TextureResource = ShaderResourceView.FromFile(DirectX11.Device, filePath);

            using (var resource = texture.TextureResource.Resource)
            {
                WinAPI.MessageBox(resource.NativePointer.ToInt32().ToString());
                using (var texture2D = resource.QueryInterface<Texture2D>())
                {
                    texture.Width = texture2D.Description.Width;
                    texture.Height = texture2D.Description.Height;
                }
            }

            Textures.Add(file, texture);
            return texture;
        }
        public static unsafe Texture CreateTexture(Color[,] array)
        {

            var boxTexDesc = new Texture2DDescription
            {
                ArraySize = 1,
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = 0,
                Format = Format.R32G32B32A32_Float,
                Height = array.GetLength(0),
                MipLevels = 1,
                OptionFlags = 0,
                Usage = ResourceUsage.Default,
                Width = array.GetLength(1),
                SampleDescription = new SampleDescription(1, 0),
            };

            DataBox[] v;
            fixed (void* p = &array[0, 0])
                v = new[]
                {
                    new DataBox(
                        new DataStream(new IntPtr(p), array.Length*16, true, true).DataPointer,
                        boxTexDesc.Width*(int) FormatHelper.SizeOfInBytes(boxTexDesc.Format), 0)
                };

            return new Texture
            {
                TextureResource =
                    new ShaderResourceView(DirectX11.Device, new Texture2D(DirectX11.Device, boxTexDesc, v)),
                Height = array.GetLength(0),
                Width = array.GetLength(1)
            };
        }
        internal static void CreateNullTexture()
        {
            NullTexture = new Texture
            {
                TextureResource = ShaderResourceView.FromMemory(DirectX11.Device, (byte[])(Resources.ResourceManager.GetObject("nullTexture"))),
                Height = 200,
                Width = 200
            };
        }

        public Texture(){}

        public Texture(string s)
        {
            var tex = s == null || !Textures.ContainsKey(s) ? Load(s) : Textures[s];
            Height = tex.Height;
            Width = tex.Width;
            TextureResource = tex.TextureResource;
        }

        public void Shutdown()
        {
            TextureResource.Dispose();
        }
    }
}