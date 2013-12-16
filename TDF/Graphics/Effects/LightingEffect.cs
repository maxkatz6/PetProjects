#region Using

using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using TDF.Core;
using TDF.Graphics.Data;
using TDF.Graphics.Models;

#endregion Using

namespace TDF.Graphics.Effects
{
    public class LightingEffect : Effect
    {
        public DirectionalLight[] DirectionalLights = new DirectionalLight[DLMaxCount];
        public PointLight[] PointLights = new PointLight[PLMaxCount];
        public SpotLight[] SpotLights = new SpotLight[SLMaxCount];
        private const byte DLMaxCount = 5, PLMaxCount = 20, SLMaxCount = 5;
        private EffectShaderResourceVariable _diffuse;
        private EffectVariable _directionalLight;
        private EffectVectorVariable _evePos;
        private EffectMaterialVariable _fxMaterial;
        private EffectMatrixVariable _fxWIT;
        private EffectMatrixVariable _fxWorld;
        private Vector3 _lightsCount = new Vector3(1, 1, 1);
        private EffectShaderResourceVariable _normal;
        private EffectVectorVariable _numLights;
        private EffectVariable _pointLight;
        private EffectVariable _spotLight;
        private EffectMatrixVariable _wvp;

        public LightingEffect()
        {
            FxShaderFlags |= ShaderFlags.SkipValidation;
        }

        public void AddDirectionalLight(DirectionalLight dl)
        {
            DirectionalLights[(int)_lightsCount.X] = dl;
            _directionalLight.GetElement((int)_lightsCount.X++).WriteValue(dl);
            ;
            _numLights.Set(_lightsCount);
        }

        public void AddPointLight(PointLight pl)
        {
            PointLights[(int)_lightsCount.Y] = pl;
            _pointLight.GetElement((int)_lightsCount.Y++).AsPointLight().Set(pl);
            _numLights.Set(_lightsCount);
        }

        public void AddSpotLight(SpotLight sl)
        {
            SpotLights[(int)_lightsCount.Z] = sl;
            _spotLight.GetElement((int)_lightsCount.Z++).WriteValue(sl);
            _numLights.Set(_lightsCount);
        }

        public void SetCamPosition(Vector3 pos)
        {
            _evePos.Set(pos);
        }

        public void SetCamPosition()
        {
            _evePos.Set(Config.CurrentCamera.Position);
        }

        public void SetMaterial(Material mat)
        {
            _fxMaterial.Set(mat);
            _diffuse.SetResource(mat.DiffuseTexture.TextureResource);
            _normal.SetResource(mat.NormalTexture.TextureResource);
        }

        public override void SetModel(StaticMesh staticMesh, Matrix matrix)
        {
            SetMaterial(staticMesh.Material);
            SetWorldMatrix(matrix);
            SetWVP(matrix * Config.CurrentCamera.View * Config.CurrentCamera.Projection);
        }

        public void SetWorldMatrix(Matrix mat)
        {
            _fxWIT.SetMatrix(Matrix.Invert(Matrix.Transpose(mat)));
            _fxWorld.SetMatrix(mat);
        }

        public void SetWVP(Matrix mat)
        {
            _wvp.SetMatrix(mat);
        }

        protected override void InitializeEffect()
        {
            ChangeTechnique("Light_" + Config.FShaderVersion);

            //** http://www.gamedev.net/topic/534310-send-an-array-effectvariable-setrawvalue-d3d-10solved/ **// почитать

            _wvp = FxEffect.GetVariableByName("WorldViewProj").AsMatrix();
            _fxWorld = FxEffect.GetVariableByName("World").AsMatrix();
            _fxWIT = FxEffect.GetVariableByName("WorldInvTranspose").AsMatrix();
            _diffuse = FxEffect.GetVariableByName("shaderTexture").AsShaderResource();
            _normal = FxEffect.GetVariableByName("gNormalMap").AsShaderResource();
            _pointLight = FxEffect.GetVariableByName("gPointLight");
            _directionalLight = FxEffect.GetVariableByName("gDirLight");
            _spotLight = FxEffect.GetVariableByName("gSpotLight");
            _evePos = FxEffect.GetVariableByName("gEyePosW").AsVector();
            _numLights = FxEffect.GetVariableByName("LightCount").AsVector();
            _fxMaterial = FxEffect.GetVariableByName("gMaterial").AsMaterial();

            ChangeInputLayout(InputElements.InputElementType[BumpVertex.VertexType]);
        }
    }
}