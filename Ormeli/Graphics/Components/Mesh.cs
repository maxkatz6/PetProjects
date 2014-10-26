using System.Runtime.InteropServices;
using Ormeli.Graphics.Effects;
using Ormeli.Math;

namespace Ormeli.Graphics
{
    public abstract class Mesh
    {
        protected Buffer Vb, Ib;
        public abstract void Render(Matrix matrix);
        public abstract void Render(Effect ef, Matrix matrix);
    }

    public abstract class StaticMesh : Mesh
    {
        protected int ShaderNum;
        protected string _tech;
        protected int _size;
        protected int _indCount;

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
            Effect.Get<ColTexEffect>(ShaderNum).SetMatrix(matrix);
            App.Render.SetBuffers(Vb, Ib, _size);
            EffectManager.Effects[ShaderNum].Render(_tech, _indCount);
        }

        public override void Render(Effect effect, Matrix matrix)
        {
            ((ColTexEffect)effect).SetMatrix(matrix);
            App.Render.SetBuffers(Vb, Ib, _size);
            effect.Render(_tech, _indCount);
        }
    }

    public abstract class DynamicMesh : StaticMesh
    {
        protected new void Initalize<T>(int[] ind, T[] vert, int shader, string tech) where T : struct
        {
            ShaderNum = shader;

            _indCount = ind.Length;
            _tech = tech;
            _size = Marshal.SizeOf(vert[0]);

            Vb = App.Creator.CreateBuffer(vert, BindFlag.VertexBuffer);
            Ib = App.Creator.CreateBuffer(ind, BindFlag.IndexBuffer, BufferUsage.Default, CpuAccessFlags.None);
        }
    }

    public class TextureMesh : StaticMesh
    {
        public int TextureN;

        public void Initialize(int[] ind, TextureVertex[] vert)
        {
            Initalize(ind, vert, 0, "Texture");
        }

        public override void Render(Matrix matrix)
        {
            Effect.Get<ColTexEffect>(ShaderNum).SetTexture(TextureN);
            base.Render(matrix);                                                            
        }

        public void SetTexture(int index)
        {
            TextureN = index;
        }

        public void SetTexture(string fileName)
        {
            SetTexture(Texture.Get(fileName));
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
