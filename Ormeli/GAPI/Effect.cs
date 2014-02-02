#region Using

using System;
using Ormeli.Graphics;
using Ormeli.Math;

#endregion

namespace Ormeli
{
    public abstract class Effect
    {
        public struct InputElement
        {
            public string Name;
            public int Index;
            public int Offset;
            public Type Type;
        }

        public virtual object this[string s]
        {
            get { return GetValue(s); }
            set { SetVariable(s, value); }
        }

        protected int PassCount;

        public abstract void ChangeInputLayout(InputElement[] inputElements);

        public abstract void ChangeTechnique(string name);

        public abstract void ChangeTechnique(int index);

        public abstract void InitializeFromFile(string file);

        public abstract void InitializeFromMemory(string source);

        public abstract void Render();

        public virtual void EndRender() { }

        public abstract void SetMatrix(string name, Matrix mt);

        public abstract void SetMesh(Mesh staticMesh, Matrix matrix);

        public abstract void SetTexture(string name, Texture texture);

        public abstract void SetTextureArray(string name, Texture[] textures);

        public abstract void SetVariable<T>(string name, T obj);

        public abstract object GetValue(string name);

        protected abstract void InitializeEffect();
    }
}