using System.Collections.Generic;
using Ormeli.Core.Patterns;
using Ormeli.Graphics.Cameras;
using Ormeli.Math;

namespace Ormeli.Graphics
{
    public class Model : Disposable
    {
        public Mesh[] Meshes { get; private set; }
        public int MeshCount { get { return Meshes.Length; } }
        private Matrix _worldMatrix = Matrix.Identity;

        public T GetMesh<T>(int num) where T : Mesh
        {
            return (T) Meshes[num];
        }

        public void SetMeshes(params Mesh[] meshes)
        {
            Meshes = meshes;
        }

        public void SetMeshes(List<Mesh> meshes)
        {
            SetMeshes(meshes.ToArray());
        }

        public void SetWorldMatrix(Matrix matrix)
        {
            _worldMatrix = matrix;
        }


        public void Render()
        {
            var v = _worldMatrix * Camera.Current.ViewProjection;
            for (int i = 0; i < MeshCount; i++)
                Meshes[i].Render(v);
        }
    }
}
