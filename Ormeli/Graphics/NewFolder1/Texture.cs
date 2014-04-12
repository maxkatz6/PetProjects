using System;
using System.Runtime.InteropServices;

namespace Ormeli.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Texture
    {
        public IntPtr Handle;
    }
}
