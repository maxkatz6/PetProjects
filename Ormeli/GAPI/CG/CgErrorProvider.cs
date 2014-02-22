using System;

namespace Ormeli.CG
{
    public static class CgErrorProvider
    {
        public static void CheckForCgError(IntPtr myCgContext, string situation, bool exit = true)
        {
            var s = CgImports.cgGetLastErrorString(new IntPtr());

            if (s.String == null) return;

                Console.WriteLine(
                    "Program: {0}\n" +
                    "Situation: {1}\n" +
                    "Error: {2}\n\n" +
                    "Cg compiler output...\n{3}",
                    "Ormeli", situation, s,
                    CgImports.cgGetLastListing(myCgContext));

            if (exit) App.Exit();
        }
    }
}