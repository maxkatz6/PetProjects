using Ormeli.GAPI;
using SharpDX;

namespace Ormeli.Graphics.Drawable
{
	public class Skydome : Model
	{
		readonly TextureMesh skyMesh =  new TextureMesh(Effect.FromFile("TexEffect.hlsl"), GeometryGenerator.CreateSphere<TextureVertex>());

		public Texture Texture { set { skyMesh.Texture = value; } }

		public override void Draw(Matrix matrix)
		{
			App.Render.ZBuffer(false);
			skyMesh.Draw(matrix);
			App.Render.ZBuffer(true);
		}
	}
}