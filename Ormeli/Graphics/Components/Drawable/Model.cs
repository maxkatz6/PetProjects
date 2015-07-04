using Ormeli.Core.Patterns;
using SharpDX;

namespace Ormeli.Graphics.Drawable
{
	public class Model : Disposable, IDrawable
	{
		public Mesh[] Meshes { get; set; }
		public int MeshCount => Meshes.Length;

		public Mesh this[int i]
		{
			get { return Meshes[i]; }
			set { Meshes[i] = value; }
		}

		public void Draw(Matrix m)
		{
			for (var i = 0; i < MeshCount; i++)
				Meshes[i].Draw(m);
		}
	}
}