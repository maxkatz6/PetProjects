using System;
using System.Runtime.InteropServices;
using Ormeli.Cg;
using Ormeli.Graphics.Effects;
using Ormeli.Math;

namespace Ormeli.Graphics
{
    public abstract class Mesh
    {
        protected Buffer Vb, Ib;
        public abstract void Render(Matrix matrix);
    }

    public abstract class StaticMesh : Mesh
    {
        protected int ShaderNum;
        private string _tech;
        private int _size;
        private int _indCount;

        protected void Initalize<T>( int[] ind, T[] vert, int shader, string tech) where T:struct 
        {
            ShaderNum = shader;

            _indCount = ind.Length;
            _tech = tech;
            _size = Marshal.SizeOf(vert[0]);

            Vb = App.Creator.CreateBuffer(vert, BindFlag.VertexBuffer, BufferUsage.Default, CpuAccessFlags.None);
            Ib = App.Creator.CreateBuffer(ind, BindFlag.IndexBuffer, BufferUsage.Default, CpuAccessFlags.None);
        }

        public override void Render(Matrix matrix)
        {
            CgEffect.Get<ColTexEffect>(ShaderNum).SetMatrix(matrix);
            App.Render.SetBuffers(Vb, Ib, _size);
            EffectManager.Effects[ShaderNum].Render(_tech, _indCount);
        }
    }

    public class TextureMesh : StaticMesh
    {
        public int TextureNum { get; set; }

        public static int LastTexture;

        public void Initialize(int[] ind, TextureVertex[] vert)
        {
            Initalize(ind, vert, 0, "Texture");
        }

        public override void Render(Matrix matrix)
        {
            if (LastTexture != TextureNum)
            {
                CgEffect.Get<ColTexEffect>(ShaderNum).SetTexture(Texture.Textures[TextureNum]);
                LastTexture = TextureNum;
            }
            base.Render(matrix);                                                            
        }
    }
    public class ColorMesh : StaticMesh
    {
        public void Initialize(int[] ind, ColorVertex[] vert)
        {
            Initalize(ind, vert, 0, "Color");
        }
    }
}
