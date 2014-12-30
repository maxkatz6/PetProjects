using System;
using System.Collections.Generic;
using System.Diagnostics;
using Assimp;
using Assimp.Configs;
using Ormeli.Core.Patterns;
using Ormeli.Graphics.Components;
using Ormeli.Graphics.Effects;
using SharpDX;

namespace Ormeli.Graphics.Builders
{
    public class ModelBuilder : Builder<Model, ModelBuilder>
    {
        private static readonly Dictionary<string, Model> Models = new Dictionary<string, Model>(1000);
        private static readonly AssimpImporter Importer = new AssimpImporter();

        static ModelBuilder()
        {
            //This is how we add a configuration (each config is its own class)
            var config = new NormalSmoothingAngleConfig(66.0f);
            Importer.SetConfig(config);

            //This is how we add a logging callback 
            var logstream = new LogStream((msg, ud) => Console.WriteLine(msg));
            Importer.AttachLogStream(logstream);
        }

        public ModelBuilder LoadFromFile(string file)
        {
            if (Models.ContainsKey(file))
                Template = Models[file];
            else
            {
                Stopwatch st = new Stopwatch();
                st.Start();
                var model = Importer.ImportFile(Config.GetDataPath(file, "Models"), PostProcessPreset.ConvertToLeftHanded | PostProcessPreset.TargetRealTimeMaximumQuality);
                var meshes = new Mesh[model.MeshCount];
                for (int i=0; i < model.MeshCount; i++)
                {
                    var aMesh = model.Meshes[i];
                    var verts = new TextureVertex[aMesh.VertexCount];
                    for (int j = 0; j < aMesh.VertexCount; j++)
                    {
                        var vert = aMesh.Vertices[j];
                        var texc = aMesh.HasTextureCoords(0) ? aMesh.GetTextureCoords(0)[j] : new Vector3D(0,0,0);
                        var v = Vector3.TransformCoordinate(
                            new Vector3(vert.X, -vert.Y, vert.Z),
                            Matrix.RotationX((float)System.Math.PI));
                        verts[j] = new TextureVertex(v, new Vector2(texc.X, texc.Y));
                    }
                    meshes[i] = MeshBuilder.Create().
                        SetIndices(aMesh.GetIntIndices()).
                        SetVertices(verts, false).
                        SetTexture(Config.GetDataPath(model.Materials[aMesh.MaterialIndex].GetTexture(TextureType.Diffuse, 0).FilePath, "Models")).
                        SetEffect(ColTexEffect.Default).
                        SetTech("Texture");
                }
                SetMeshes(meshes);
                Models.Add(file, Template);
                st.Stop();
                Console.WriteLine(st.ElapsedMilliseconds);
            }
            return this;
        }
        public ModelBuilder SetMeshes(params Mesh[] meshes)
        {
            Template.Meshes = meshes;
            return this;
        }

        public ModelBuilder SetMeshes(List<Mesh> meshes)
        {
            return SetMeshes(meshes.ToArray());
        }
    }
}
