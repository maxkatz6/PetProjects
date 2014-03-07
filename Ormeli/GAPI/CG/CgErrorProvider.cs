using System;
using System.Runtime.InteropServices;
using Ormeli.Core;

namespace Ormeli.CG
{
    public static class CgErrorProvider
    {
        public static string ToStr(this IntPtr s)
        {
            return Marshal.PtrToStringAnsi(s);
        }
        public static void CheckForCgError(CGcontext myCgContext, string situation, bool exit = true)
        {
            CGerror cGerror;
            var s = CgImports.cgGetLastErrorString(out cGerror).ToStr();

            if (string.IsNullOrEmpty(s)) return;

            Console.WriteLine(
                @"Ormeli: CG error!
Situation: {0}\n
Error: {1}\n\n
Cg compiler output...\n{2}
", situation, s, CgImports.cgGetLastListing(myCgContext));

            ErrorProvider.SendError(s, exit);
        }
    }
}