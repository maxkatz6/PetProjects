namespace Ormeli.CG
{
    public class CgEffect
    {
        public static readonly CGcontext CGcontext = CgImports.cgCreateContext();
        public CGeffect CGeffect;
        public CGtechnique CGtechnique;

        static CgEffect()
        {
            CgImports.cgSetErrorCallback(CgErrorProvider.CheckForCgError);
        }

        public CgEffect()
        {
        }

        public CgEffect(string file)
        {
            CgEffect cg = App.Render.InitCgShader(file);
            CGeffect = cg.CGeffect;
            CGtechnique = cg.CGtechnique;
        }
    }
}