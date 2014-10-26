﻿using System;
using System.Collections.Generic;
using Ormeli.Core.Patterns;
using Ormeli.Graphics;
using Ormeli.Math;

namespace Ormeli
{
    public abstract class Effect : Disposable
    {
        protected EffectBase Base { get; set; }

        public static T Get<T>(int num) where T : Effect
        {
            return (T)EffectManager.Effects[num];
        }

        protected Effect()
        {
            Base = App.Creator.CreateEffect();
        }

        public void Render(string tech, int indCount)
        {
            Render(Base.TechNum[tech], indCount);
        }

        public void Render(int tech, int indCount)
        {
            Base.Render(tech, indCount);
        }

        public void Render(string tech, Action act)
        {
            Render(Base.TechNum[tech], act);
        }

        public void Render(int tech, Action act)
        {
            Base.Render(tech, act);
        }

        public static T LoadFromFile<T>(string s) where T : Effect, new()
        {
            var ef = new T();
            ef.Base.LoadFromFile(s);
            ef.InitEffect();
            ef.InitAttribs();
            return ef;
        }

        public static T LoadFromMemory<T>(string s) where T : Effect, new()
        {
            var ef = new T();
            ef.Base.LoadFromMemory(s);
            ef.InitEffect();
            ef.InitAttribs();
            return ef;
        }

        protected abstract void InitEffect();
        protected abstract void InitAttribs();
    }

    public abstract class EffectBase
    {
        protected const int MaxTechCount = 10;
        public readonly Dictionary<string, int> TechNum = new Dictionary<string, int>(MaxTechCount);
        protected int LastTexture = -1;

        public abstract void LoadFromFile(string s);
        public abstract void LoadFromMemory(string s);

        public abstract void Render(int techN, int indexCount);
        public abstract void Render(int techN, Action act);


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

        public void SetTexture(IntPtr param, int tex)
        {
            if (tex == LastTexture) return;
            setTexture(param, tex);
            LastTexture = tex;
        }

        protected abstract void setTexture(IntPtr param, int tex);

        public abstract IntPtr GetParameterByName(string name);
        public abstract IntPtr GetSignature(int techNum);
    }
}