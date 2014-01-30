#region Using

using System;
using Ormeli.Graphics;
using Ormeli.Math;

#endregion

namespace Ormeli
{
    public abstract class Effect
    {
/*#if DEBUG
        protected SharpDX.Direct3D11.Effect FxEffect;
        protected InputLayout FxInputLayout;
        protected ShaderFlags FxShaderFlags = ShaderFlags.Debug;
#else
        protected ShaderFlags FxShaderFlags = ShaderFlags.None;
        protected Effect FxEffect;
        protected InputLayout FxInputLayout;
#endif
        protected EffectTechnique FxTech;*/

     /*   protected int PassCount
        {
            get { return FxTech.Description.PassCount; }
        }*/

        public abstract object this[string s]
        {
            get;
            set;
        }

   /*     public void ChangeInputLayout(InputElement[] inputElements)
        {
            var passDesc = FxTech.GetPassByIndex(0).Description;
            FxInputLayout = new InputLayout(DirectX11.Device, passDesc.Signature, inputElements);
        }

        public virtual void ChangeTechnique(string name)
        {
            FxTech = FxEffect.GetTechniqueByName(name);
        }

        public virtual void ChangeTechnique(int index);
        */
        protected abstract void InitializeFromFile(string file);

        protected abstract void InitializeFromMemory(string source);

        public abstract void Render();

        public virtual void EndRender() { }

        public abstract void SetMatrix(string name, Matrix mt);

        public abstract void SetMesh(Mesh staticMesh, Matrix matrix);

        public abstract void SetTexture(string name, Texture texture);

        public abstract void SetTextureArray(string name, Texture[] textures);

        public abstract void SetVariable<T>(string name, T obj);

        protected abstract void InitializeEffect();
    }
}