#region Using

using SharpDX;
using SharpDX.Direct3D11;
using TDF.Core;
using TDF.Graphics.Data;
using TDF.Graphics.Models;

#endregion Using

namespace TDF.Graphics.Effects
{
    public class ColorEffect : Effect
    {
        private static EffectMatrixVariable _fxWVP;

        public override void SetModel(StaticMesh staticMesh, Matrix matrix)
        {
            SetWVP(matrix * Config.CurrentCamera.View * Config.CurrentCamera.Projection);
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