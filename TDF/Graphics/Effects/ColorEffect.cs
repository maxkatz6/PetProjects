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
    public class ColorEffect : Effect
    {
        private static EffectMatrixVariable _fxWVP;

        public override void SetModel(StaticMesh staticMesh, Matrix matrix)
        {
            SetWVP(matrix *DirectX11.ViewMatrix * DirectX11.ProjectionMatrix);
        }

        public void SetWVP(Matrix matrix)
        {
            _fxWVP.SetMatrix(matrix);
        }

        protected override void InitializeEffect()
        {
            ChangeTechnique("Color_" + Config.FShaderVersion);
            _fxWVP = FxEffect.GetVariableByName("WorldViewProj").AsMatrix();
            ChangeInputLayout(InputElements.InputElementType[ColorVertex.VertexType]);
        }
    }
}