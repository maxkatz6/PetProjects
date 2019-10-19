using Lain.Core.Patterns;
using SharpDX;

namespace Lain.Graphics.Drawable
{
    public class Model : Disposable
    {
        public Mesh[] Meshes { get; set; }
        public int MeshCount => Meshes.Length;

        public Mesh this[int i]
        {
            get { return Meshes[i]; }
            set { Meshes[i] = value; }
        }

        public virtual void Draw(Matrix m)
        {
            for (var i = 0; i < MeshCount; i++)
                Meshes[i].Draw(m);
        }

        public static Model Create(params Mesh[] meshes)
        {
            return new Model {Meshes = meshes};
        }
    }
}