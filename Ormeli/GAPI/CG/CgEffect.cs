﻿using System;
using System.Collections.Generic;
using Ormeli.Core;
using Ormeli.Graphics;
using Ormeli.Math;

namespace Ormeli.Cg
{
    public class CgEffect
    {
        public static readonly CG.Context Context = CG.CreateContext();
        protected CG.Effect Effect;
        public CG.Technique[] Techs;
        public Dictionary<string, int> TechNum = new Dictionary<string, int>();


        static CgEffect()
        {
            CG.SetErrorCallback(() =>
            {
                CG.Error cGerror;
                var s = CG.GetLastErrorString(out cGerror).ToStr();

                if (string.IsNullOrEmpty(s)) return;

                Console.WriteLine(
                    @"Ormeli: {2}!
Error: {0}
Cg compiler output...{1}
", s, CG.GetLastListing(Context), cGerror);

                ErrorProvider.SendError(s, true);
            });
        }

        protected CgEffect(string file, int defaultAttrContainerNum = 0)
        {
            Effect = CG.CreateEffectFromFile(Context, Config.EffectDirectory + file,
                null);
            var myCgTechnique = CG.GetFirstTechnique(Effect);
            var list = new List<CG.Technique>();
            while (myCgTechnique)
            {
                var c = CG.GetTechniqueName(myCgTechnique).ToStr();
                if (CG.ValidateTechnique(myCgTechnique) != 1)
                    Console.WriteLine(@"Ormeli: Technique {0} did not validate.  Skipping.", c);
                else
                {
                    Console.WriteLine(@"Ormeli: Technique {0} is valid.", c);
                    list.Add(myCgTechnique);
                    TechNum.Add(c.Substring(5), list.Count-1);
                }
                myCgTechnique = CG.GetNextTechnique(myCgTechnique);
            }

            if (list.Count == 0)
            {
                ErrorProvider.SendError("Ormeli: No valid technique.", true);
            }
            Techs = list.ToArray();
        }

        protected void InitAttrib(int techNum, int attrNum)
        {
            if (EffectManager.AttribsContainers[attrNum] == null)
                EffectManager.AttribsContainers[attrNum] = App.Creator.InitAttribs(EffectManager.Attribs[attrNum],
                    CG.DX11.GetIASignatureByPass(CG.GetFirstPass(Techs[techNum])));
        }
        protected void InitAttrib(string tech, int attrNum)
        {
            InitAttrib(TechNum[tech], attrNum);
        }

        protected void SetMatrix(string name, Matrix mt)
        {
            SetMatrix(CG.GetNamedEffectParameter(Effect, name), mt);
        }
        protected void SetMatrix(CG.Parameter param, Matrix mt)
        {
            if (App.RenderType == RenderType.OpenGl3) CG.GL.SetMatrixParameterfr(param, mt.GetFloatArray());
            else CG.SetMatrixParameterfr(param, mt.GetFloatArray());
        }

        protected void SetTexture(string name, Texture tex)
        {
            SetTexture(CG.GetNamedEffectParameter(Effect, name), tex);
        }
        protected void SetTexture(CG.Parameter param, Texture tex)
        {
            if (App.RenderType == RenderType.OpenGl3) CG.GL.SetTextureParameter(param, tex.Handle.ToInt32());
            else CG.DX11.SetTextureParameter(param, tex.Handle);
            CG.SetSamplerState(param);
        }

        private int _lastAttrNum = -1;
        public void Render(int techN, int indexCount)
        {
            var tech = Techs[techN];
            var pass = CG.GetFirstPass(tech);
            if (_lastAttrNum != techN)
            {
                EffectManager.AttribsContainers[techN].Accept();
                _lastAttrNum = techN;
            }

            while (pass)
            {
                CG.SetPassState(pass);

                App.Render.Draw(indexCount);

                CG.ResetPassState(pass);
                pass = CG.GetNextPass(pass);
            }
        }
    }
}