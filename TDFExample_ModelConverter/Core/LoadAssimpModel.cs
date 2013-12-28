using Assimp;
using Assimp.Configs;
using SharpDX;
using TDF.Graphics.Data;
using TDF.Graphics.Effects;
using TDF.Graphics.Models;
using Material = TDF.Graphics.Data.Material;

namespace TDFExample_ModelConverter.Core
{
    public static class LoadAssimpModel
    {
        public static AssimpImporter AssimpImporter;

        static LoadAssimpModel()
        {
            AssimpImporter = new AssimpImporter();
            var config = new NormalSmoothingAngleConfig(0);
            AssimpImporter.SetConfig(config);
        }

        public static DxModel LoadModel(string fileName)
        {
            var model = new DxModel();
            //Import the model - this is considered a single atomic call. All configs are set, all logstreams attached. The model
            //is imported, loaded into managed memory. Then the unmanaged memory is released, and everything is reset.
            var assimpModel = AssimpImporter.ImportFile(fileName,
                PostProcessPreset.TargetRealTimeMaximumQuality | PostProcessPreset.ConvertToLeftHanded);
            
            model.Meshes = new StaticMesh[assimpModel.MeshCount];

            Material mater;
            for (var i = 0; i < assimpModel.MeshCount; i++)
            {
                model.Meshes[i] = new StaticMesh();
                model.Meshes[i].SetIndices(assimpModel.Meshes[i].GetIndices());
                var aMesh = assimpModel.Meshes[i];

               /* if (aMesh.HasTangentBasis)
                {
                    var vert = new BumpVertex[aMesh.VertexCount];

                    for (var j = 0; j < aMesh.VertexCount; j++)
                    {
                        vert[j] = new BumpVertex(
                            aMesh.Vertices[j].ToVector3(), 
                            aMesh.Normals[j].ToVector3(), 
                            aMesh.GetTextureCoords(0)[j].ToVector2(), 
                            aMesh.Tangents[j].ToVector3());
                    }
                    model.Meshes[i].SetVertices(vert);
                }
                else if (aMesh.HasNormals)
                {
                    var vert = new LightVertex[aMesh.VertexCount];

                    for (var j = 0; j < aMesh.VertexCount; j++)
                    {
                        vert[j] = new LightVertex(
                            aMesh.Vertices[j].ToVector3(),
                            aMesh.Normals[j].ToVector3(),
                            aMesh.GetTextureCoords(0)[j].ToVector2());
                    }
                    model.Meshes[i].SetVertices(vert);
                }
                else */if (aMesh.HasTextureCoords(0))
                {
                    var vert = new TextureVertex[aMesh.VertexCount];

                    for (var j = 0; j < aMesh.VertexCount; j++)
                    {
                        vert[j] = new TextureVertex(
                            aMesh.Vertices[j].ToVector3(),
                            aMesh.GetTextureCoords(0)[j].ToVector2());
                    }
                    model.Meshes[i].SetVertices(vert);
                    model.Meshes[i].Effect = EffectManager.Effects[TextureVertex.VertexType];
                }
                else if (aMesh.HasVertexColors(0))
                {
                    var vert = new ColorVertex[aMesh.VertexCount];

                    for (var j = 0; j < aMesh.VertexCount; j++)
                    {
                        vert[j] = new ColorVertex(
                            aMesh.Vertices[j].ToVector3(), aMesh.GetVertexColors(0)[j].ToColor());
                    }
                    model.Meshes[i].SetVertices(vert);
                    model.Meshes[i].Effect = EffectManager.Effects[ColorVertex.VertexType];
                }
                else
                {
                    var vert = new ColorVertex[aMesh.VertexCount];

                    for (var j = 0; j < aMesh.VertexCount; j++)
                    {
                        vert[j] = new ColorVertex(
                            aMesh.Vertices[j].ToVector3(),new Color4(1,1,1,1));
                    }
                    model.Meshes[i].SetVertices(vert);
                    model.Meshes[i].Effect = EffectManager.Effects[ColorVertex.VertexType];
                }

                mater = assimpModel.Materials[assimpModel.Meshes[i].MaterialIndex].ToMaterial();
                mater.DiffuseTexture = TDF.Graphics.Data.Texture.Load(
                    assimpModel.Materials[assimpModel.Meshes[i].MaterialIndex].GetTexture(TextureType.Diffuse, 0).FilePath ?? "null");
                mater.NormalTexture = TDF.Graphics.Data.Texture.Load(
                    assimpModel.Materials[assimpModel.Meshes[i].MaterialIndex].GetTexture(TextureType.Normals, 0).FilePath ?? "null");
                model.Meshes[i].SetMaterial(mater);
            }
            return model;
        }
    }
}
