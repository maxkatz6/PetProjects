using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using SharpDX;

namespace Ormeli.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Texture
    {
        private readonly IntPtr Handle;

        public readonly int Height;
        public readonly int Width;

        private static readonly List<Texture> Textures = new List<Texture>(100);
        private static readonly Dictionary<string, int> textures = new Dictionary<string, int>(100); 

        public static int Get(string file)
        {
            if (string.IsNullOrEmpty(file) || file.Contains("null")) return 0;
            if (textures.ContainsKey(file)) return textures[file];

            Textures.Add(App.Creator.LoadTexture(file));
            textures.Add(file, Textures.Count - 1);

            return Textures.Count - 1;
        }
        public static Texture Get(int t)
        {
            if (t > Textures.Count - 1) throw new Exception("Invalid texture number");
            return Textures[t];
        }

        static Texture()
        {
            Textures.Add(App.Creator.CreateTexture(new[,]
                {
                    {new Color4(0, 0, 0, 1f), new Color4(0, 1f, 0, 1),new Color4(0, 0, 0f, 1), new Color4(0, 1f, 0, 1),new Color4(0, 0, 0, 1f)},
                    {new Color4(0f, 1, 0, 1), new Color4(0, 0f, 0,1),new Color4(0f, 1, 0, 1), new Color4(0, 0f, 0,1),new Color4(0f, 1, 0, 1f)},
                    {new Color4(0, 0, 0, 1f), new Color4(0, 1f, 0, 1),new Color4(0, 0, 0, 1f), new Color4(0, 1f, 0, 1),new Color4(0, 0, 0, 1f)},
                    {new Color4(0f, 1, 0, 1), new Color4(0, 0f, 0,1),new Color4(0f, 1, 0, 1), new Color4(0, 0f, 0,1),new Color4(0f, 1, 0, 1f)},
                    {new Color4(0, 0, 0, 1f), new Color4(0, 1f, 0, 1),new Color4(0, 0, 0f, 1), new Color4(0, 1f, 0, 1),new Color4(0, 0, 0, 1f)}
                })); // number 0 texture//
                     // null texture    //
            NullTexture = Textures.Count - 1;
        }

        public Texture(IntPtr handle, int w, int h)
        {
            Handle = handle;
            Width = w;
            Height = h;
        }

        public static implicit operator IntPtr(Texture tex)
        {
            return tex.Handle;
        }

        public static implicit operator int(Texture tex)
        {
            return tex.Handle.ToInt32();
        }

        public static readonly int NullTexture;
    }
}