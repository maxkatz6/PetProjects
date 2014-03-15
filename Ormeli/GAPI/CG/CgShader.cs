namespace Ormeli.CG
{
    public class CgShader
    {
        public  CGprogram VertexProgram, FragmentProgram;

        public static readonly CGcontext CGcontext = CgImports.cgCreateContext();
        internal static void InitializeShaderEngine()
        {
            CgImports.cgSetErrorCallback(CgErrorProvider.CheckForCgError);
        }
    }
}
