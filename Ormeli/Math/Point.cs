using System.Runtime.InteropServices;

namespace Ormeli.Math
{
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct Point
    {
        private int _x, _y;
        public int X { get { return _x; } set { _x = value; } }
        public int Y { get { return _y; } set { _y = value; } }

        public Point(int x, int y)
        {
            _x = x;
            _y = y;
        }
    }
}
