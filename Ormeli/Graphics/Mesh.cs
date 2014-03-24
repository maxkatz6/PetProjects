namespace Ormeli.Graphics
{
    public class Mesh//TODO now-only for ColorVertex - change
    {
        public int[] Indices;
        public ColorVertex[] Vertices;
        private Buffer _vb, _ib;
        private int ShaderNum = 0;

        public void Initalize(int[] ind, ColorVertex[] vert)
        {
            Indices = ind;
            Vertices = vert;

            _vb = App.Render.CreateBuffer(vert, BindFlag.VertexBuffer, BufferUsage.Default, CpuAccessFlags.None);
            _ib = App.Render.CreateBuffer(ind, BindFlag.IndexBuffer, BufferUsage.Default, CpuAccessFlags.None);
        }

        public void Render()
        {
            App.Render.Draw(EffectManager.Effects[ShaderNum].Techs[0], _vb, _ib, ColorVertex.SizeInBytes, Indices.Length);
        }
    }
}
