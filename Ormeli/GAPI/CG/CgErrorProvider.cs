using System;
using Ormeli.Core;

namespace Ormeli.CG
{
    public static class CgErrorProvider
    {
        public static void CheckForCgError(IntPtr myCgContext, string situation, bool exit = true)
        {
            var s = CgImports.cgGetLastErrorString(new IntPtr());

            if (s.String == null) return;

            Console.WriteLine(
                @"Ormeli: CG error!
Situation: {0}\n
Error: {1}\n\n
Cg compiler output...\n{2}
", situation, s, CgImports.cgGetLastListing(myCgContext));

            ErrorProvider.SendError(s.String, exit);
        }
    }
}