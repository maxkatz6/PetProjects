using System;
using System.Collections.Generic;
using Ormeli.Core;
using Ormeli.GAPI.Interfaces;
using Ormeli.Graphics;
using SharpDX;

namespace Ormeli.Cg
{
    public sealed class CgEffectBase : EffectBase
    {
        private static readonly CG.Context Context = CG.CreateContext();

        public CG.Effect Effect;
        public CG.Technique[] Techs = new CG.Technique[MaxTechCount];
        private string _file;

        static CgEffectBase()
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

        public static void InitDirectX11(IntPtr pointer)
        {
            CG.DX11.SetDevice(Context, pointer);
            CG.DX11.RegisterStates(Context);
            CG.DX11.SetManageTextureParameters(Context, CG.True);
        }

        public static void InitOpenGl()
        {
            CG.GL.SetDebugMode(CG.True);
            CG.SetParameterSettingMode(Context, CG.Enum.DeferredParameterSetting);
            CG.GL.RegisterStates(Context);
        }

        public override void LoadFromFile(string s)
        {
            Effect = CG.CreateEffectFromFile(Context, Config.GetEffectPath(s),
                null);
            _file = s;
            LoadEff();
        }

        public override void LoadFromMemory(string s)
        {
            Effect = CG.CreateEffect(Context, s,
                null);
            _file = s;
            LoadEff();
        }

        private void LoadEff()
        {
            var myCgTechnique = CG.GetFirstTechnique(Effect);
            var list = new List<CG.Technique>();
            while (myCgTechnique)
            {
                var c = CG.GetTechniqueName(myCgTechnique).ToStr();
                if (CG.ValidateTechnique(myCgTechnique) != 1)
                    Console.WriteLine(@"Ormeli: Effect {1}: Technique {0} did not validate.  Skipping.", c, _file);
                else
                {
                    Console.WriteLine(@"Ormeli: Effect {1}: Technique {0} is valid.", c, _file);
                    list.Add(myCgTechnique);
                    TechNum.Add(c, list.Count - 1);
                }
                myCgTechnique = CG.GetNextTechnique(myCgTechnique);
            }

            if (list.Count == 0)
            {
                ErrorProvider.SendError("Ormeli: Effect " + _file + ": No valid technique.", true);
            }
            Techs = list.ToArray();
        }

        public override void Render(int techN, int indexCount)
        {
            var tech = Techs[techN];
            var pass = tech.GetFirstPass();

            while (pass)
            {
                pass.Enable();

                App.Render.Draw(indexCount);

                pass.Disable();
                pass = pass.GetNextPass();
            }
        }

        public override void Render(int techN, Action act)
        {
            var tech = Techs[techN];
            var pass = tech.GetFirstPass();

            while (pass)
            {
                pass.Enable();

                act.Invoke();

                pass.Disable();
                pass = pass.GetNextPass();
            }
        }

        public override IntPtr GetSignature(int techNum)
        {
            return Techs[techNum].GetSignatureByFirstPass();
        }

        public override void SetMatrix(IntPtr param, Matrix mt)
        {
            if (App.RenderType == RenderType.OpenGl3) CG.GL.SetMatrixParameterfr(param, mt);
            else CG.SetMatrixParameterfr(param, mt);
        }

        protected override void setTexture(IntPtr param, int tex)
        {
            if (App.RenderType == RenderType.OpenGl3) CG.GL.SetTextureParameter(param, Texture.Get(tex));
            else CG.DX11.SetTextureParameter(param, Texture.Get(tex));
            CG.SetSamplerState(param);
        }

        public override IntPtr GetParameterByName(string name)
        {
            return CG.GetNamedEffectParameter(Effect, name);
        }
    }
}