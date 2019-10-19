using Assimp;
using Lain.Core;
using Lain.GAPI;
using Lain.Graphics;
using Lain.Graphics.Drawable;
using SharpDX;
using System.Collections.Generic;

namespace Lain.Desktop
{
    public static class Assimp
    {
        private static readonly Dictionary<string, Model> Models = new Dictionary<string, Model>(1000);
        private static readonly AssimpContext Importer = new AssimpContext();
        public static Model LoadModel(string file, Effect effect)
        {
            if (!Models.ContainsKey(file))
            {
                var model = Importer.ImportFile(FileSystem.GetPath(file, "Models"),
                    PostProcessPreset.ConvertToLeftHanded | PostProcessPreset.TargetRealTimeMaximumQuality);
                var meshes = new Graphics.Drawable.Mesh[model.MeshCount];
                for (var i = 0; i < model.MeshCount; i++)
                {
                    var aMesh = model.Meshes[i];
                    var verts = new TextureVertex[aMesh.VertexCount];
                    for (var j = 0; j < aMesh.VertexCount; j++)
                    {
                        var vert = aMesh.Vertices[j];
                        var texc = aMesh.HasTextureCoords(0) ? aMesh.TextureCoordinateChannels[0][j] : new Vector3D(0, 0, 0);
                        var v = Vector3.TransformCoordinate(
                            new Vector3(vert.X, -vert.Y, vert.Z),
                            Matrix.RotationX((float)System.Math.PI));
                        verts[j] = new TextureVertex { Position = v, TexCoord = new Vector2(texc.X, texc.Y) };
                    }
                    meshes[i] = new TextureMesh(effect,
                        new GeometryInfo<TextureVertex> { Indices = aMesh.GetIndices(), Vertices = verts })
                    {
                        Texture = FileSystem.GetPath(model.Materials[aMesh.MaterialIndex].TextureDiffuse.FilePath, "Models")
                    };
                }
                Models.Add(file, Model.Create(meshes));
            }
            return Models[file];
        }
    }
}
