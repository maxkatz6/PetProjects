using Ormeli.Core.Patterns;
using Ormeli.Graphics.Cameras;
using SharpDX;

namespace Ormeli.Graphics
{
    public class Model : Disposable, IDrawable
    {
        public Mesh[] Meshes { get; set; }
        public int MeshCount { get { return Meshes.Length; } }

        public Mesh GetMesh(int id)
        {
            return Meshes[id];
        }
        
        public void Draw(Matrix m)
        {
            var v = m * Camera.Current.ViewProjection;
            for (int i = 0; i < MeshCount; i++)
                Meshes[i].Draw(v);
        }
    }
}
