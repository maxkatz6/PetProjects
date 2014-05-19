using System.Collections.Generic;
using Ormeli.Math;
using System;
using System.Runtime.InteropServices;

namespace Ormeli.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Texture
    {
        private readonly IntPtr Handle;

        public static List<Texture> Textures = new List<Texture>(100);
        private static readonly Dictionary<string, int> textures = new Dictionary<string, int>(100); 

        public static int GetNumber(string file)
        {
            if (file == null || file.Contains("null")) return 0;
            if (textures.ContainsKey(file)) return textures[file];

            Textures.Add(App.Creator.LoadTexture(file));
            textures.Add(file, Textures.Count - 1);

            return Textures.Count - 1;
        }

        static Texture()
        {
            Textures.Add(App.Creator.CreateTexture(new[,]
                {
                    {new Color(0, 0, 0, 1f), new Color(0, 1f, 0, 1),new Color(0, 0, 0f, 1), new Color(0, 1f, 0, 1),new Color(0, 0, 0, 1f)},
                    {new Color(0f, 1, 0, 1), new Color(0, 0f, 0,1),new Color(0f, 1, 0, 1), new Color(0, 0f, 0,1),new Color(0f, 1, 0, 1f)},
                    {new Color(0, 0, 0, 1f), new Color(0, 1f, 0, 1),new Color(0, 0, 0, 1f), new Color(0, 1f, 0, 1),new Color(0, 0, 0, 1f)},
                    {new Color(0f, 1, 0, 1), new Color(0, 0f, 0,1),new Color(0f, 1, 0, 1), new Color(0, 0f, 0,1),new Color(0f, 1, 0, 1f)},
                    {new Color(0, 0, 0, 1f), new Color(0, 1f, 0, 1),new Color(0, 0, 0f, 1), new Color(0, 1f, 0, 1),new Color(0, 0, 0, 1f)}
                })); // number 0 texture//
                     // null texture    //
            NullTexture = Textures.Count - 1;
        }

        public Texture(IntPtr handle)
        {
            Handle = handle;
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