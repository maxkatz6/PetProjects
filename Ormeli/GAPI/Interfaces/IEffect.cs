using System;
using System.Collections.Generic;
using Ormeli.Graphics;
using Ormeli.Math;

namespace Ormeli
{
    public abstract class Effect
    {
        public EffectBase Base { get; set; }

        protected Effect(string file)
        {
            Base = App.Creator.CreateEffect(file);
            InitEffect();
        }

        public void Render(string tech, int indCount)
        {
            Base.Render(tech, indCount);
        }

        public void Render(int tech, int indCount)
        {
            Base.Render(tech, indCount);
        }

        protected abstract void InitEffect();
    }

    public abstract class EffectBase
    {
        protected const int MaxTechCount = 10;
        protected readonly Dictionary<string, int> TechNum = new Dictionary<string, int>(MaxTechCount);
        protected int LastTexture = -1;

        public static T Get<T>(int num) where T : Effect
        {
            return (T) EffectManager.Effects[num];
        }

        public abstract void LoadEffect(string file);

        public void Render(string tech, int indexCount)
        {
            Render(TechNum[tech], indexCount);
        }

        public abstract void Render(int techN, int indexCount);


        public void InitAttrib(string tech, int attrNum)
        {
            InitAttrib(TechNum[tech], attrNum);
        }

        public void InitAttrib(int techNum, int attrNum)
        {
            if (EffectManager.AttribsContainers[attrNum] == null)
                EffectManager.AttribsContainers[attrNum] = App.Creator.InitAttribs(EffectManager.Attribs[attrNum],
                    GetSignature(techNum));
        }

        public void SetMatrix(string name, Matrix mt)
        {
            SetMatrix(GetParameterByName(name), mt);
        }

        public abstract void SetMatrix(IntPtr param, Matrix mt);

        public void SetTexture(string name, int tex)
        {
            SetTexture(GetParameterByName(name), tex);
        }

        public abstract void SetTexture(IntPtr param, int tex);

        public abstract IntPtr GetParameterByName(string name);
        protected abstract IntPtr GetSignature(int techNum);
    }
}