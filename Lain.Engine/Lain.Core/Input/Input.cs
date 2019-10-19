using SharpDX;

namespace Lain
{
    public static class Input
    {
        public delegate void CharEnterDelegate(char ch);

        public delegate void KeyDownDelegate(Key ch);

        public delegate void MouseDownDelegate(MouseButton mb);

        public enum Key
        {
            Cancel = 3, // Control-break processing
            Back = 0x08, // BACKSPACE key
            Tab = 0x09, // TAB key
            Clear = 0x0C, // CLEAR key
            Enter = 13, // ENTER key
            Shift = 0x10, // SHIFT key
            Control = 0x11, // CTRL key
            Alt = 0x12, // ALT key
            Pause = 0x13, // PAUSE key
            CapsLock = 0x14, // CAPS LOCK key

            Escape = 0x1B,
            Space = 0x20,
            Pageup = 0x21,
            Pagedown = 0x22,
            End = 0x23,
            Home = 0x24,

            Left = 0x25, // left arrow key
            Up = 0x26, // up arrow key
            Right = 0x27, // right arrow key
            Down = 0x28, // down arrow key

            Select = 0x29,
            Exe = 0x2B, // execute key
            PrintScreen = 0x2C,
            Insert = 0x2D,
            Delete = 0x2E,

            D0 = 0x30,
            D1 = 0x31,
            D2 = 0x32,
            D3 = 0x33,
            D4 = 0x34,
            D5 = 0x35,
            D6 = 0x36,
            D7 = 0x37,
            D8 = 0x38,
            D9 = 0x39,

            A = 0x41,
            B = 0x42,
            C = 0x43,
            D = 0x44,
            E = 0x45,
            F = 0x46,
            G = 0x47,
            H = 0x48,
            I = 0x49,
            J = 0x4A,
            K = 0x4B,
            L = 0x4C,
            M = 0x4D,
            N = 0x4E,
            O = 0x4F,
            P = 0x50,
            Q = 0x51,
            R = 0x52,
            S = 0x53,
            T = 0x54,
            U = 0x55,
            V = 0x56,
            W = 0x57,
            X = 0x58,
            Y = 0x59,
            Z = 0x5A,

            Winleft = 0x5B,
            Winright = 0x5C,
            Menu = 0x5D,

            N0 = 0x60,
            N1 = 0x61,
            N2 = 0x62,
            N3 = 0x63,
            N4 = 0x64,
            N5 = 0x65,
            N6 = 0x66,
            N7 = 0x67,
            N8 = 0x68,
            N9 = 0x69,

            Multiply = 0x6A,
            Add = 0x6B,
            Separator = 0x6C,
            Subtract = 0x6D,
            Decimal = 0x6E,
            Divide = 0x6F,

            F1 = 112,
            F2 = 0x71,
            F3 = 0x72,
            F4 = 0x73,
            F5 = 0x74,
            F6 = 0x75,
            F7 = 0x76,
            F8 = 0x77,
            F9 = 0x78,
            F10 = 0x79,
            F11 = 0x7A,
            F12 = 123,

            NumLock = 0x90,
            Scroll = 0x91,

            Plus = 0xBB, // '+'
            Comma = 188, // ','
            Minus = 0xBD, // '-'
            Period = 190, // '.'

            BackSlash = 220,
            Question = 191,

            Oem1 = 186,
            Oem3 = 192,
            Oem4 = 219,
            Oem6 = 221,
            Oem7 = 222,

            Max = 0xff
        }

        public enum MouseButton
        {
            Left = 1,
            Right = 2,
            Middle = 4,
            X1 = 5,
            X2 = 6
        }

        private static readonly bool[] InputKeys = new bool[256];
        public static MouseState Mouse;
        public static event CharEnterDelegate CharEnterEvent;
        public static event KeyDownDelegate KeyDownEvent;
        public static event MouseDownDelegate MouseDownEvent;

        public static bool IsKeyDown(Key key)
        {
            return InputKeys[(byte) key];
        }

        public static void KeyDown(Key key)
        {
            InputKeys[(int) key] = true;
            KeyDownEvent?.Invoke(key);
        }

        public static void KeyUp(Key key)
        {
            InputKeys[(int) key] = false;
        }

        public static void PressLButton(bool _true)
        {
            if (_true) MouseDownEvent?.Invoke(MouseButton.Left);
            Mouse.LeftButton = _true;
        }

        public static void PressMButton(bool _true)
        {
            if (_true) MouseDownEvent?.Invoke(MouseButton.Middle);
            Mouse.MiddleButton = _true;
        }

        public static void PressRButton(bool _true)
        {
            if (_true) MouseDownEvent?.Invoke(MouseButton.Right);
            Mouse.RightButton = _true;
        }

        public static void SetMouseCoord(int x, int y)
        {
            Mouse.X = x;
            Mouse.Y = y;
        }

        public static void CharInput(char c)
        {
            CharEnterEvent?.Invoke(c);
        }

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

            public Point Point => new Point(X, Y);

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
                return obj is MouseState && Equals((MouseState) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = LeftButton.GetHashCode();
                    hashCode = (hashCode*397) ^ RightButton.GetHashCode();
                    hashCode = (hashCode*397) ^ MiddleButton.GetHashCode();
                    hashCode = (hashCode*397) ^ X;
                    hashCode = (hashCode*397) ^ Y;
                    return hashCode;
                }
            }

            public override string ToString()
            {
                return "[ " + X + " | " + Y + " ] (" + (LeftButton ? "▌" : "_") + "|" + (MiddleButton ? "•" : "_") + "|" +
                       (RightButton ? "▌" : "_") + ")";
            }
        }
    }
}