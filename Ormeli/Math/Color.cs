using System.Runtime.InteropServices;

namespace Ormeli.Math
{
    public struct Color
    {
        public static readonly int SizeInBytes = Marshal.SizeOf(typeof(Color));

        public float R;
        public float G;
        public float B;
        public float A;

        public Color(byte r, byte g, byte b, byte a)
        {
            R = (float)r/255;
            G = (float)g/255;
            B = (float)b/255;
            A = (float)a/255;
        }
    }
}
