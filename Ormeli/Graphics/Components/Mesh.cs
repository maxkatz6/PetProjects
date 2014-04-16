using System.Reflection;
using System.Runtime.InteropServices;

namespace Ormeli.Graphics
{
    public abstract class Mesh
    {
        public int[] Indices;
        protected Buffer Vb, Ib;



        public abstract void Render();
    }

    public class Mesh<T> : Mesh where T : struct
    {
        public T[] Vertices;
        private int _shaderNum;
        private int _vertNum;
        private int _size;

        public void Initalize(int shader, int[] ind, T[] vert)
        {
            _shaderNum = shader;

            Indices = ind;
            Vertices = vert;

            var vertexType = typeof(T).GetField("Number", BindingFlags.Static | BindingFlags.Public);
            _vertNum = vertexType != null ? (int)vertexType.GetValue(null) : 1;
            _size = Marshal.SizeOf(vert[0]);

            Vb = App.Creator.CreateBuffer(vert, BindFlag.VertexBuffer, BufferUsage.Default, CpuAccessFlags.None);
            Ib = App.Creator.CreateBuffer(ind, BindFlag.IndexBuffer, BufferUsage.Default, CpuAccessFlags.None);
        }

        public override void Render()
        {
            App.Render.SetBuffers(Vb, Ib, _size);
            EffectManager.Effects[_shaderNum].Render(_vertNum, Indices.Length);
        }
    }
}
