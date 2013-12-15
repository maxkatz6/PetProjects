#region Using

using SharpDX;
using SharpDX.Direct3D11;
using TDF.Core;
using TDF.Graphics.Data;
using TDF.Graphics.Models;
using TDF.Graphics.Render;

#endregion

namespace TDF.Graphics.Effects
{
    public class TextureEffect : Effect
    {
        private static EffectShaderResourceVariable _fxTexture;
        private static EffectMatrixVariable _fxWVP;

        public void SetWVP(Matrix matrix)
        {
            _fxWVP.SetMatrix(matrix);
        }

        public void SetDiffuseTexture(Texture texture)
        {
            _fxTexture.SetResource(texture.TextureResource);
        }

        protected override void InitializeEffect()
        {
            ChangeTechnique("Texture_" + Config.FShaderVersion);
           
            _fxWVP = FxEffect.GetVariableByName("WorldViewProj").AsMatrix();
            _fxTexture = FxEffect.GetVariableByName("shaderTexture").AsShaderResource();
            ChangeInputLayout(InputElements.InputElementType[TextureVertex.VertexType]);
        }

        public override void SetModel(Mesh mesh, Matrix matrix)
        {
            SetWVP(matrix * Config.CurrentCamera.View * Config.CurrentCamera.Projection);
            SetDiffuseTexture(mesh.Material.DiffuseTexture);
        }
    }
}