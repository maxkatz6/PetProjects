using Lain.GAPI;
using SharpDX;

namespace Lain.Graphics.Drawable
{
	public class Skydome : Model
	{
		private readonly TextureMesh skyMesh = new TextureMesh(Effect.FromFile("TexEffect.hlsl"),
			GeometryGenerator.CreateHemiSphere<TextureVertex>());

		public Texture Texture { set { skyMesh.Texture = value; } }

		public override void Draw(Matrix matrix)
		{
			App.Render.ZBuffer(false);
			skyMesh.Draw(matrix);
			App.Render.ZBuffer(true);
		}
	}
}