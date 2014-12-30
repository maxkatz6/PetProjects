using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using SharpDX;
using Ormeli.Core.Patterns;

namespace Ormeli.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Texture : IDrawable
    {
        public static readonly Texture Null;

        private readonly IntPtr Handle;

        public readonly int Height;
        public readonly int Width;

        private static readonly Dictionary<string, Texture> textures = new Dictionary<string, Texture>(100); 

        public static Texture Get(string file)
        {
            if (string.IsNullOrEmpty(file) || file.Contains("null")) return Null;
            if (textures.ContainsKey(file)) return textures[file];

            var t = App.Creator.LoadTexture(file);
            textures.Add(file, t);

            return t;
        }

        public void Draw(Matrix m)
        {
            throw new NotImplementedException();
        }

        static Texture()
        {
            Null = App.Creator.CreateTexture(new[,]
                {
                    {new Color4(0, 0, 0, 1f), new Color4(0, 1f, 0, 1),new Color4(0, 0, 0f, 1), new Color4(0, 1f, 0, 1),new Color4(0, 0, 0, 1f)},
                    {new Color4(0f, 1, 0, 1), new Color4(0, 0f, 0,1),new Color4(0f, 1, 0, 1), new Color4(0, 0f, 0,1),new Color4(0f, 1, 0, 1f)},
                    {new Color4(0, 0, 0, 1f), new Color4(0, 1f, 0, 1),new Color4(0, 0, 0, 1f), new Color4(0, 1f, 0, 1),new Color4(0, 0, 0, 1f)},
                    {new Color4(0f, 1, 0, 1), new Color4(0, 0f, 0,1),new Color4(0f, 1, 0, 1), new Color4(0, 0f, 0,1),new Color4(0f, 1, 0, 1f)},
                    {new Color4(0, 0, 0, 1f), new Color4(0, 1f, 0, 1),new Color4(0, 0, 0f, 1), new Color4(0, 1f, 0, 1),new Color4(0, 0, 0, 1f)}
                });
        }

        public Texture()
        {
            Handle = Null.Handle;
            Width = Null.Width;
            Height = Null.Height;
        }


        public Texture(IntPtr handle, int w, int h)
        {
            Handle = handle;
            Width = w;
            Height = h;
        }

        public bool IsNull => Handle == IntPtr.Zero;


        public static implicit operator IntPtr(Texture tex)
        {
            return tex.Handle;
        }

        public static implicit operator int(Texture tex)
        {
            return tex.Handle.ToInt32();
        }
        public static bool operator ==(Texture me, Texture other)
        {
            return me.Handle == other.Handle;
        }

        public static bool operator !=(Texture me, Texture other)
        {
            return !(me == other);
        }
        public bool Equals(Texture other)
        {
            return Handle.Equals(other.Handle);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Texture && Equals((Texture)obj);
        }

        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }

    }
}