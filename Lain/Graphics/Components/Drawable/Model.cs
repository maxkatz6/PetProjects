using System.Collections.Generic;
using Assimp;
using Lain.Core.Patterns;
using Lain.GAPI;
using SharpDX;
using Texture = Lain.GAPI.Texture;

namespace Lain.Graphics.Drawable
{
	public class Model : Disposable
	{
		private static readonly Dictionary<string, Model> Models = new Dictionary<string, Model>(1000);
		private static readonly AssimpImporter Importer = new AssimpImporter();
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


		public static Model FromFile(string file, Effect effect)
		{
            if (!Models.ContainsKey(file))
			{
				var model = Importer.ImportFile(Config.GetDataPath(file, "Models"),
					PostProcessPreset.ConvertToLeftHanded | PostProcessPreset.TargetRealTimeMaximumQuality);
				var meshes = new Mesh[model.MeshCount];
				for (var i = 0; i < model.MeshCount; i++)
				{
					var aMesh = model.Meshes[i];
					var verts = new TextureVertex[aMesh.VertexCount];
					for (var j = 0; j < aMesh.VertexCount; j++)
					{
						var vert = aMesh.Vertices[j];
						var texc = aMesh.HasTextureCoords(0) ? aMesh.GetTextureCoords(0)[j] : new Vector3D(0, 0, 0);
						var v = Vector3.TransformCoordinate(
							new Vector3(vert.X, -vert.Y, vert.Z),
							Matrix.RotationX((float) System.Math.PI));
						verts[j] = new TextureVertex {Position = v, TexCoord = new Vector2(texc.X, texc.Y)};
					}
					meshes[i] = new TextureMesh(effect,
						new GeometryInfo<TextureVertex> {Indices = aMesh.GetIntIndices(), Vertices = verts})
					{
						Texture =Config.GetDataPath(
									model.Materials[aMesh.MaterialIndex].GetTexture(TextureType.Diffuse, 0).FilePath, "Models")
					};
				}
				Models.Add(file, Create(meshes));
			}
			return Models[file];
		}
		public static Model Create(params Mesh[] meshes)
		{
			return new Model {Meshes = meshes};
		}
	}
}