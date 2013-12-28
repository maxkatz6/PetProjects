using SharpDX;
using System;
using TDF.Core;
using TDF.Inputs;

namespace TDF.Inputs
{
    public struct MouseState
    {
        public bool LeftButton, RightButton, MiddleButton, X1BUtton, X2Button;

        public int X;

        public int Y;

        public Vector2 Vector { get { return new Vector2(X, Y); } }

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
            return LeftButton.Equals(other.LeftButton) && RightButton.Equals(other.RightButton) && MiddleButton.Equals(other.MiddleButton) && X1BUtton.Equals(other.X1BUtton) && X2Button.Equals(other.X2Button) && X == other.X && Y == other.Y;
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
                int hashCode = LeftButton.GetHashCode();
                hashCode = (hashCode * 397) ^ RightButton.GetHashCode();
                hashCode = (hashCode * 397) ^ MiddleButton.GetHashCode();
                hashCode = (hashCode * 397) ^ X1BUtton.GetHashCode();
                hashCode = (hashCode * 397) ^ X2Button.GetHashCode();
                hashCode = (hashCode * 397) ^ X;
                hashCode = (hashCode * 397) ^ Y;
                return hashCode;
            }
        }
    }

    public static class Input
    {
        public static bool[] InputKeys;

        public static MouseState MouseState;

        public static event CharEnter CharEnterEvent;

        public static event KeyDown KeyDownEvent;

        public static bool Initialize()
        {
            var keys = Enum.GetValues(typeof(Key));
            InputKeys = new bool[keys.Length*10];
            foreach (var key in keys)
            {
                InputKeys[(int)key] = false;
            }
            return true;
        }

        public static bool IsKeyDown(Key key)
        {
            return InputKeys[(byte)key];
        }

        public static void KeyDown(Key key)
        {
            InputKeys[(int)key] = true;
            switch (key)
            {
                case Key.Lbutton:
                    MouseState.LeftButton = true;
                    break;

                case Key.Mbutton:
                    MouseState.MiddleButton = true;
                    break;

                case Key.Rbutton:
                    MouseState.RightButton = true;
                    break;
            }

            if (KeyDownEvent != null)
                KeyDownEvent(key);
        }

        public static void KeyUp(Key key)
        {
            InputKeys[(int)key] = false;
            switch (key)
            {
                case Key.Lbutton:
                    MouseState.LeftButton = false;
                    break;

                case Key.Mbutton:
                    MouseState.MiddleButton = false;
                    break;

                case Key.Rbutton:
                    MouseState.RightButton = false;
                    break;
            }
        }

        public static void LeftButton(bool _true)
        {
            MouseState.LeftButton = _true;
        }

        public static void MiddleButton(bool _true)
        {
            MouseState.MiddleButton = _true;
        }

        public static void RightButton(bool _true)
        {
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

        public static void SetMouseState(MouseState mouseState)
        {
            MouseState = mouseState;
        }

        public static void CharInput(char c)
        {
            if (CharEnterEvent != null)
                CharEnterEvent(c);
        }
    }
}

public delegate void CharEnter(char ch);

public delegate void KeyDown(Key ch);