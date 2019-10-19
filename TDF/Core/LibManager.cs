using System;
using System.IO;
using TDF.Properties;

namespace TDF.Core
{
    internal static class LibManager
    {
        internal const string EffectX64 = "sharpdx_direct3d11_effects_x64";
        internal const string EffectX86 = "sharpdx_direct3d11_effects_x86";

        internal static void SaveFromResource()
        {
            var dllPath = AppDomain.CurrentDomain.BaseDirectory;

            if (Environment.Is64BitProcess)
            {
                var dllByte = (byte[]) Resources.ResourceManager.GetObject(EffectX64);

                using (var stream = new FileStream(dllPath + @"\" + EffectX64 + ".dll", FileMode.OpenOrCreate))
                    stream.Write(dllByte, 0, dllByte.Length);
            }
            else
            {
                var dllByte = (byte[])Resources.ResourceManager.GetObject(EffectX86);

                using (var stream = new FileStream(dllPath + @"\" + EffectX86 + ".dll", FileMode.OpenOrCreate))
                    stream.Write(dllByte, 0, dllByte.Length);
            }
        }
    }
}
