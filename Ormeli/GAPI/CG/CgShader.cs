namespace Ormeli.CG
{
    public class CgShader
    {
        internal static void InitializeShaderEngine()
        {
            LibManager.CG_SaveFromResource();
            App.Render.InitCG();
        }
    }
}
