using System;
using System.Collections.Generic;
using Ormeli.Core;
using Ormeli.Graphics;
using Ormeli.Math;

namespace Ormeli.Cg
{
    public sealed class CgEffectBase : EffectBase
    {
        private static readonly CG.Context Context = CG.CreateContext();

        public CG.Effect Effect;
        public CG.Technique[] Techs = new CG.Technique[MaxTechCount];

        static CgEffectBase()
        {
            CG.SetErrorCallback(() =>
            {
                CG.Error cGerror;
                string s = CG.GetLastErrorString(out cGerror).ToStr();

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

        public override void Render(int techN, int indexCount)
        {
            CG.Technique tech = Techs[techN];
            CG.Pass pass = tech.GetFirstPass();

            while (pass)
            {
                pass.Enable();

                App.Render.Draw(indexCount);

                pass.Disable();
                pass = pass.GetNextPass();
            }
        }

        public override void LoadEffect(string file)
        {
            Effect = CG.CreateEffectFromFile(Context, Config.GetEffectPath(file),
                null);

            var myCgTechnique = CG.GetFirstTechnique(Effect);
            var list = new List<CG.Technique>();
            while (myCgTechnique)
            {
                string c = CG.GetTechniqueName(myCgTechnique).ToStr();
                if (CG.ValidateTechnique(myCgTechnique) != 1)
                    Console.WriteLine(@"Ormeli: Effect {1}: Technique {0} did not validate.  Skipping.", c, file);
                else
                {
                    Console.WriteLine(@"Ormeli: Effect {1}: Technique {0} is valid.", c, file);
                    list.Add(myCgTechnique);
                    TechNum.Add(c, list.Count - 1);
                }
                myCgTechnique = CG.GetNextTechnique(myCgTechnique);
            }

            if (list.Count == 0)
            {
                ErrorProvider.SendError("Ormeli: Effect " + file + ": No valid technique.", true);
            }
            Techs = list.ToArray();
        }

        protected override IntPtr GetSignature(int techNum)
        {
            return Techs[techNum].GetSignatureByFirstPass();
        }

        public override void SetMatrix(IntPtr param, Matrix mt)
        {
            if (App.RenderType == RenderType.OpenGl3) CG.GL.SetMatrixParameterfr(param, mt);
            else CG.SetMatrixParameterfr(param, mt);
        }

        public override void SetTexture(IntPtr param, int tex)
        {
            if (LastTexture == tex) return;
            if (App.RenderType == RenderType.OpenGl3) CG.GL.SetTextureParameter(param, Texture.Textures[tex]);
            else CG.DX11.SetTextureParameter(param, Texture.Textures[tex]);
            CG.SetSamplerState(param);
            LastTexture = tex;
        }

        public override IntPtr GetParameterByName(string name)
        {
            return CG.GetNamedEffectParameter(Effect, name);
        }
    }
}