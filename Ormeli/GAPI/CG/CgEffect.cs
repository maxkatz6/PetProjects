using System;
using System.Collections.Generic;
using Ormeli.Core;
using Ormeli.Math;

namespace Ormeli.CG
{
    public class CgEffect
    {
        public static readonly CGcontext CGcontext = CgImports.cgCreateContext();
        public CGeffect CGeffect;
        public TechInfo[] Techs;
        public Dictionary<string, int> TechNum = new Dictionary<string, int>();
 
        public struct TechInfo
        {
            public CGtechnique CGtechnique;
            public int AttribsContainerNumber;
        }

        static CgEffect()
        {
            CgImports.cgSetErrorCallback(CgErrorProvider.CheckForCgError);
        }

        public CgEffect(string file, int defaultAttrContainerNum)
        {
            CGeffect = CgImports.cgCreateEffectFromFile(CGcontext, Config.ShadersDirectory + file,
                null);
            var myCgTechnique = CgImports.cgGetFirstTechnique(CGeffect);
            var list = new List<TechInfo>();
            while (myCgTechnique)
            {
                var c = CgImports.cgGetTechniqueName(myCgTechnique).ToStr();
                if (CgImports.cgValidateTechnique(myCgTechnique) != 1)
                    Console.WriteLine(@"Ormeli: Technique {0} did not validate.  Skipping.", c);
                else
                {
                    Console.WriteLine(@"Ormeli: Technique {0} is valid.", c);
                    list.Add(new TechInfo
                    {
                        CGtechnique = myCgTechnique,
                        AttribsContainerNumber = defaultAttrContainerNum
                    });
                    TechNum.Add(c, list.Count-1);
                }
                myCgTechnique = CgImports.cgGetNextTechnique(myCgTechnique);
            }

            if (list.Count == 0)
            {
                ErrorProvider.SendError("Ormeli: No valid technique.", true);
            }
            Techs = list.ToArray();
        }

        public object this[string s]
        {
            get { return GetVariable(s); }
            set { SetVariable(s, value); }
        }

        public void SetAttribNum(int techNum, int attrNum)
        {
            Techs[techNum].AttribsContainerNumber = attrNum;
            if (EffectManager.AttribsContainers[attrNum] == null)
                EffectManager.AttribsContainers[attrNum] = App.Render.InitAttribs(EffectManager.Attribs[attrNum],
                    CgImports.cgD3D11GetIASignatureByPass(CgImports.cgGetFirstPass(Techs[techNum].CGtechnique)));
        }
        public void SetAttribNum(string tech, int attrNum)
        {
            SetAttribNum(TechNum[tech], attrNum);
        }

        public void SetMatrix(string name, Matrix mt)
        {
        //    CgImports.cgGLSetMatrixParameterfr(CgImports.cgGetNamedEffectParameter(CGeffect,name),mt);
        }

        public void SetTexture(string name, ITexture tex)
        {
       //     FxEffect.GetVariableByName(name).AsShaderResource().SetResource(srv);
        }

        public void SetTextureArray(string name, ITexture[] textures)
        {
         //   FxEffect.GetVariableByName(name).AsShaderResource().SetResourceArray(srv);
        }

        public void SetVariable<T>(string name, T obj)
        {
           // FxEffect.GetVariableByName(name).WriteValue(obj);
        }

        public object GetVariable(string name)
        {
            return null;
            // FxEffect.GetVariableByName(name).WriteValue(obj);
        }
    }
}