using System;
using System.Collections.Generic;
using Ormeli.Core.Patterns;
using Ormeli.Graphics;
using Ormeli.Graphics.Managers;
using SharpDX;

namespace Ormeli.GAPI.Interfaces
{
    public abstract class Effect : Disposable
    {
        protected EffectBase Base { get; set; }

        private const string MATRIXName = "Matrix";
        private IntPtr _matrix;

        public static T Get<T>(int num) where T : Effect
        {
            return (T)EffectManager.Effects[num];
        }

        protected Effect()
        {
            Base = App.Creator.CreateEffect();
        }
        
        public void Render(Mesh mesh, Action act)
        {
            App.Render.SetBuffers(mesh.Vb, mesh.Ib, mesh.VertexSize);
            RenderMesh(mesh);
            Base.Render(Base.TechNum[mesh.Tech], act);
        }

        public void Render(Mesh mesh)
        {
            App.Render.SetBuffers(mesh.Vb, mesh.Ib, mesh.VertexSize);
            RenderMesh(mesh);
            Base.Render(Base.TechNum[mesh.Tech], mesh.IndexCount);
        }

        protected abstract void RenderMesh(Mesh mesh);

        public void SetMatrix(Matrix mt)
        {
            Base.SetMatrix(_matrix, mt);
        }

        public static T LoadFromFile<T>(string s) where T : Effect, new()
        {
            var ef = new T();
            ef.Base.LoadFromFile(s);
            ef._matrix = ef.Base.GetParameterByName(MATRIXName);
            ef.InitEffect();
            ef.InitAttribs();
            return ef;
        }

        public static T LoadFromMemory<T>(string s) where T : Effect, new()
        {
            var ef = new T();
            ef.Base.LoadFromMemory(s);
            ef._matrix = ef.Base.GetParameterByName(MATRIXName);
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
        protected Texture LastTexture;

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

        public void SetTexture(string name, Texture tex)
        {
            SetTexture(GetParameterByName(name), tex);
        }

        public void SetTexture(IntPtr param, Texture tex)
        {
            if (tex == LastTexture) return;
            setTexture(param, tex);
            LastTexture = tex;
        }

        protected abstract void setTexture(IntPtr param, Texture tex);

        public abstract IntPtr GetParameterByName(string name);
        public abstract IntPtr GetSignature(int techNum);
    }
}