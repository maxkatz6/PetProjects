namespace Ormeli.Graphics
{
    public class Mesh
    {
        public int[] Indices;
        public ColorVertex[] Vertices;
        private Buffer vb, ib;

        public void Initalize(int[] ind, ColorVertex[] vert)
        {
            Indices = ind;
            Vertices = vert;

            vb = App.Render.CreateBuffer(vert, BindFlag.VertexBuffer);
            ib = App.Render.CreateBuffer(ind, BindFlag.IndexBuffer);
        }

        public void Render()
        {
            App.Render.DrawBuffer(vb, ib, ColorVertex.SizeInBytes, Indices.Length);
        }
    }
}
