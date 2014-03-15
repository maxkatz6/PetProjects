using System;
using System.Runtime.InteropServices;
using Ormeli.Core;

namespace Ormeli.CG
{
    public static class CgErrorProvider
    {
        public static void CheckForCgError()
        {
            CGerror cGerror;
            var s = CgImports.cgGetLastErrorString(out cGerror).ToStr();

            if (string.IsNullOrEmpty(s)) return;

            Console.WriteLine(
                @"Ormeli: {2}!
Error: {0}
Cg compiler output...{1}
" ,s, CgImports.cgGetLastListing(CgShader.CGcontext),cGerror);

            ErrorProvider.SendError(s,true);
        }
    }
}