using SharpDX;

namespace Ormeli.Input
{
    public static class Input
    {
        private readonly static bool[] InputKeys = new bool[256];

        public static MouseState MouseState;

        public static event CharEnter CharEnterEvent;

        public static event KeyDown KeyDownEvent;

        public static event MouseDown MouseDownEvent;

        public static bool IsKeyDown(Key key)
        {
            return InputKeys[(byte)key];
        }

        public static void KeyDown(Key key)
        {
            InputKeys[(int)key] = true;
            KeyDownEvent?.Invoke(key);
        }

        public static void KeyUp(Key key)
        {
            InputKeys[(int)key] = false;
        }

        public static void LeftButton(bool _true)
        {
            if (_true) MouseDownEvent?.Invoke(MouseButton.Left);
            MouseState.LeftButton = _true;
        }

        public static void MiddleButton(bool _true)
        {
            if (_true) MouseDownEvent?.Invoke(MouseButton.Middle);
            MouseState.MiddleButton = _true;
        }

        public static void RightButton(bool _true)
        {
            if (_true) MouseDownEvent?.Invoke(MouseButton.Right);
            MouseState.RightButton = _true;
        }

        public static void SetMouseCoord(int x, int y)
        {
            MouseState.X = x;
            MouseState.Y = y;
        }

        public static void SetMouseCoord(Point p)
        {
            SetMouseCoord(p.X, p.Y);
        }

        public static void CharInput(char c)
        {
            CharEnterEvent?.Invoke(c);
        }
    }

    public delegate void CharEnter(char ch);

    public delegate void KeyDown(Key ch);

    public delegate void MouseDown(MouseButton mb);

    public struct MouseState
    {
        public bool LeftButton, RightButton, MiddleButton;

        public int X;

        public int Y;

        public MouseState(bool left, bool right, bool middle, int x, int y)
        {
            LeftButton = left;
            RightButton = right;
            MiddleButton = middle;
            X = x;
            Y = y;
        }

        public Vector2 Vector
        {
            get { return new Vector2(X, Y); }
        }

        public Point Point
        {
            get { return new Point(X, Y); }
        }

        public static bool operator !=(MouseState a, MouseState b)
        {
            return !(a == b);
        }

        public static bool operator ==(MouseState a, MouseState b)
        {
            return (a.X == b.X && a.Y == b.Y);
        }

        public bool Equals(MouseState other)
        {
            return LeftButton.Equals(other.LeftButton) && RightButton.Equals(other.RightButton) &&
                   MiddleButton.Equals(other.MiddleButton) && X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is MouseState && Equals((MouseState)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = LeftButton.GetHashCode();
                hashCode = (hashCode * 397) ^ RightButton.GetHashCode();
                hashCode = (hashCode * 397) ^ MiddleButton.GetHashCode();
                hashCode = (hashCode * 397) ^ X;
                hashCode = (hashCode * 397) ^ Y;
                return hashCode;
            }
        }

        public override string ToString()
        {
            return "[ " + X + " | " + Y + " ]";
        }
    }

}