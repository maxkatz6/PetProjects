using System.Threading.Tasks;
using Assimp;
using Assimp.Configs;
using SharpDX;
using TDF.Graphics.Data;
using TDF.Graphics.Models;

namespace TDFExample_ModelConverter.Core
{
    public static class LoadAssimpModel
    {
        public static AssimpContext AssimpImporter;

        static LoadAssimpModel()
        {
            AssimpImporter = new AssimpContext();
            var config = new NormalSmoothingAngleConfig(0);
            AssimpImporter.SetConfig(config);
        }

        public static async void AsyncLoadModel(this DxModel model, string fileName)
        {
            await Task.Run(() => model.LoadModel(fileName));
        }

        public static void LoadModel(this DxModel model,string fileName)
        {
            //Import the model - this is considered a single atomic call. All configs are set, all logstreams attached. The model
            //is imported, loaded into managed memory. Then the unmanaged memory is released, and everything is reset.
            var assimpModel = AssimpImporter.ImportFile(fileName,
               PostProcessPreset.ConvertToLeftHanded | PostProcessPreset.TargetRealTimeQuality);

            Parallel.For(0, assimpModel.MeshCount, (i, a) =>
            {
                var amesh = new TDF.Graphics.Models.Mesh();
                amesh.CreateIndexBuffer(assimpModel.Meshes[(int)i].GetUnsignedIndices());
                var aMesh = assimpModel.Meshes[(int)i];

                if (aMesh.HasTangentBasis)
                {
                    var vert = new BumpVertex[aMesh.VertexCount];

                    for (var j = 0; j < aMesh.VertexCount; j++)
                    {
                        vert[j] = new BumpVertex(
                            aMesh.Vertices[j].ToVector3(), 
                            aMesh.Normals[j].ToVector3(), 
                            aMesh.TextureCoordinateChannels[0][j].ToVector2(), 
                            aMesh.Tangents[j].ToVector3());
                    }
                    amesh.CreateVertexBuffer(vert);
                }
                else if (aMesh.HasNormals)
                {
                    var vert = new LightVertex[aMesh.VertexCount];

                    for (var j = 0; j < aMesh.VertexCount; j++)
                    {
                        vert[j] = new LightVertex(
                            aMesh.Vertices[j].ToVector3(),
                            aMesh.Normals[j].ToVector3(),
                            aMesh.TextureCoordinateChannels[0][j].ToVector2());
                    }
                    amesh.CreateVertexBuffer(vert);
                }
                else
                if (aMesh.HasTextureCoords(0))
                {
                    var vert = new TextureVertex[aMesh.VertexCount];

                    for (var j = 0; j < aMesh.VertexCount; j++)
                    {
                        vert[j] = new TextureVertex(
                            aMesh.Vertices[j].ToVector3(),
                            aMesh.TextureCoordinateChannels[0][j].ToVector2());
                    }
                    amesh.CreateVertexBuffer(vert);
                    amesh.EffectNumber =TextureVertex.VertexType;
                }
                else if (aMesh.HasVertexColors(0))
                {
                    var vert = new ColorVertex[aMesh.VertexCount];

                    for (var j = 0; j < aMesh.VertexCount; j++)
                    {
                        vert[j] = new ColorVertex(
                            aMesh.Vertices[j].ToVector3(), aMesh.VertexColorChannels[0][j].ToColor());
                    }
                    amesh.CreateVertexBuffer(vert);
                    amesh.EffectNumber = ColorVertex.VertexType;
                }
                else
                {
                    var vert = new ColorVertex[aMesh.VertexCount];

                    for (var j = 0; j < aMesh.VertexCount; j++)
                    {
                        vert[j] = new ColorVertex(
                            aMesh.Vertices[j].ToVector3(), new Color4(1, 1, 1, 1));
                    }
                    amesh.CreateVertexBuffer(vert);
                    amesh.EffectNumber = ColorVertex.VertexType;
                }

                var mater = assimpModel.Materials[assimpModel.Meshes[(int)i].MaterialIndex].ToMaterial();
                mater.DiffuseTexture = TDF.Graphics.Data.Texture.Load(
                    assimpModel.Materials[assimpModel.Meshes[(int)i].MaterialIndex].TextureDiffuse
                        .FilePath ?? "null");
                mater.NormalTexture = TDF.Graphics.Data.Texture.Load(
                    assimpModel.Materials[assimpModel.Meshes[(int)i].MaterialIndex].TextureNormal
                        .FilePath ?? "null");
                amesh.SetMaterial(mater);
                model.Meshes.Add(amesh);
            });
        }

        public static DxModel LoadModel(string s)
        {
            var dxModel = new DxModel();
            dxModel.LoadModel(s);
            return dxModel;
        }
    }
}
