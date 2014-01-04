#region Using

using System.Reflection;
using System.Reflection.Emit;
using System.Resources;
using SharpDX.Direct3D11;
using System.Collections.Generic;
using System.IO;
using TDF.Core;
using TDF.Graphics.Render;
using TDF.Properties;

#endregion Using

namespace TDF.Graphics.Data
{
    public class Texture
    {
        public static Dictionary<string, Texture> Textures = new Dictionary<string, Texture>();

        public static Texture NullTexture { get; set; }

        public int Height { get; private set; }

        public ShaderResourceView TextureResource { get; private set; }

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
                using (var texture2D = resource.QueryInterface<Texture2D>())
                {
                    texture.Width = texture2D.Description.Width;
                    texture.Height = texture2D.Description.Height;
                }
            }
            Textures.Add(file, texture);
            return texture;
        }

        internal static void CreateNullTexture()
        {
            NullTexture = new Texture
            {
                TextureResource = ShaderResourceView.FromMemory(DirectX11.Device, (byte[])(Resources.ResourceManager.GetObject("nullTexture")))
            };
        }

        public Texture(){}

        public Texture(string s)
        {
            var tex = Textures.ContainsKey(s) ? Textures[s] : Load(s);
            Height = tex.Height;
            Width = tex.Width;
            TextureResource = tex.TextureResource;
        }
    }
}