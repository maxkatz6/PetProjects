namespace Ormeli.Graphics
{
    public class Mesh
    {
        public int[] Indices;
        public ColorVertex[] Vertices;
        private Buffer _vb, _ib;

        public void Initalize(int[] ind, ColorVertex[] vert)
        {
            Indices = ind;
            Vertices = vert;

            _vb = App.Render.CreateBuffer(vert, BindFlag.VertexBuffer, BufferUsage.Default, CpuAccessFlags.None);
            _ib = App.Render.CreateBuffer(ind, BindFlag.IndexBuffer);
        }

        public void Render()
        {
            App.Render.DrawBuffer(_vb, _ib, ColorVertex.SizeInBytes, Indices.Length);
        }
    }
}
