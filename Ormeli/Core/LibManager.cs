using System;
using Ormeli.Properties;

namespace Ormeli.Core
{
    public static class LibManager
    {
        private static readonly string DllPath = AppDomain.CurrentDomain.BaseDirectory;

        internal const string cg = "cg";
        internal const string cgDx = "cgD3D11";
        internal const string cgGL = "cgGL";
        internal const string cgGlut = "glut32";

        internal static void CG_SaveFromResource()
        {
            var e = Environment.Is64BitProcess ? "_64" : "_32";

            Save(cg + e, cg);
            if (App.RenderType == RenderType.DirectX11)
                Save(cgDx + e, cgDx);
            else
            {
                Save(cgGL + e, cgGL);
                Save(cgGlut + e, cgGlut);
            }
        }

        private static void Save(string res, string file)
        {
            FileManager.WriteByteArray(file, (byte[])Resources.ResourceManager.GetObject(res));
        }
    }
}
