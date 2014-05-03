#region Using

using System;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using TDF.Core;
using TDF.Graphics.Models;
using TDF.Graphics.Render;

#endregion

namespace TDF.Graphics.Effects
{
    public abstract class Effect
    {
#if DEBUG
        protected SharpDX.Direct3D11.Effect FxEffect;
        protected InputLayout FxInputLayout;
        protected ShaderFlags FxShaderFlags = ShaderFlags.Debug;
#else
        protected ShaderFlags FxShaderFlags = ShaderFlags.None;
        protected SharpDX.Direct3D11.Effect FxEffect;
        protected InputLayout FxInputLayout;
#endif
        protected EffectTechnique FxTech;

        protected int PassCount
        {
            get { return FxTech.Description.PassCount; }
        }

        public object this[string s]
        {
            get { return FxEffect.GetVariableByName(s); }
            set { SetVariable(s, value); }
        }

        public void ChangeInputLayout(InputElement[] inputElements)
        {
            var passDesc = FxTech.GetPassByIndex(0).Description;
            FxInputLayout = new InputLayout(DirectX11.Device, passDesc.Signature, inputElements);
        }

        public virtual void ChangeTechnique(string name)
        {
            FxTech = FxEffect.GetTechniqueByName(name);
        }

        public virtual void ChangeTechnique(int index)
        {
            FxTech = FxEffect.GetTechniqueByIndex(index);
        }

        public void InitializeFromFile(string file)
        {
            try
            {
                var cr = ShaderBytecode.CompileFromFile(Config.ShadersFilePath + file, "fx_5_0",
                    FxShaderFlags);
                FxEffect = new SharpDX.Direct3D11.Effect(DirectX11.Device, cr.Bytecode.Data);
                InitializeEffect();
            }
            catch (Exception ex)
            {
                ErrorProvider.Send(ex);
                throw new Exception();
            }
        }

        public void InitializeFromMemory(string source)
        {
            try
            {
                var cr = ShaderBytecode.Compile(source, "fx_5_0", FxShaderFlags, EffectFlags.None);
                FxEffect = new SharpDX.Direct3D11.Effect(DirectX11.Device, cr.Bytecode.Data);
                InitializeEffect();
            }
            catch (Exception ex)
            {
                ErrorProvider.Send(ex);
                throw new Exception();
            }
        }

        public void Render()
        {
            DirectX11.DeviceContext.InputAssembler.InputLayout = FxInputLayout;
            for (int p = 0; p < FxTech.Description.PassCount; ++p)
            {
                FxTech.GetPassByName("P0").Apply(DirectX11.DeviceContext);
            }
        }

        public virtual void EndRender() { }

        public void SetMatrix(string name, Matrix mt)
        {
            FxEffect.GetVariableByName(name).AsMatrix().SetMatrix(mt);
        }

        public abstract void SetMesh(Mesh staticMesh, Matrix matrix);

        public void SetTexture(string name, ShaderResourceView srv)
        {
            WinAPI.MessageBox(srv.NativePointer.ToInt32().ToString());
            FxEffect.GetVariableByName(name).AsShaderResource().SetResource(srv);
        }

        public void SetTextureArray(string name, ShaderResourceView[] srv)
        {
            FxEffect.GetVariableByName(name).AsShaderResource().SetResourceArray(srv);
        }

        public void SetVariable<T>(string name, T obj)
        {
            FxEffect.GetVariableByName(name).WriteValue(obj);
        }
        protected abstract void InitializeEffect();
    }
}