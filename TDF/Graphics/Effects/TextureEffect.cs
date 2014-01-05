#region Using

using SharpDX;
using SharpDX.Direct3D11;
using TDF.Core;
using TDF.Graphics.Data;
using TDF.Graphics.Models;
using TDF.Graphics.Render;

#endregion Using

namespace TDF.Graphics.Effects
{
    public class TextureEffect : Effect
    {
        private EffectShaderResourceVariable _fxTexture;
        private EffectMatrixVariable _fxWVP;

        public void SetDiffuseTexture(Texture texture)
        {
            _fxTexture.SetResource(texture.TextureResource);
        }

        public override void SetMesh(Mesh staticMesh, Matrix matrix)
        {
            SetWVP(matrix);
            SetDiffuseTexture(staticMesh.Material.DiffuseTexture);
        }

        public void SetWVP(Matrix matrix)
        {
            _fxWVP.SetMatrix(matrix);
        }

        protected override void InitializeEffect()
        {
            ChangeTechnique("Texture_" + Config.FShaderVersion);

            _fxWVP = FxEffect.GetVariableByName("WorldViewProj").AsMatrix();
            _fxTexture = FxEffect.GetVariableByName("shaderTexture").AsShaderResource();
            ChangeInputLayout(InputElements.InputElementType[TextureVertex.VertexType]);
        }
    }
}