using System.Runtime.InteropServices;

namespace Ormeli.Math
{
    /// <summary>
    ///     Define a RectangleF. This structure is slightly different from System.Drawing.RectangleF as It is
    ///     internally storing Left,Top,Right,Bottom instead of Left,Top,Width,Height.
    ///     Although automatic casting from a to System.Drawing.Rectangle is provided by this class.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RectangleF
    {
        private float _left;
        private float _top;
        private float _right;
        private float _bottom;

        /// <summary>
        ///     An empty rectangle
        /// </summary>
        public static readonly RectangleF Empty;

        static RectangleF()
        {
            Empty = new RectangleF();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RectangleF" /> struct.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="top">The top.</param>
        /// <param name="right">The right.</param>
        /// <param name="bottom">The bottom.</param>
        public RectangleF(float left, float top, float right, float bottom)
        {
            _left = left;
            _top = top;
            _right = right;
            _bottom = bottom;
        }

        /// <summary>
        ///     Gets or sets the left.
        /// </summary>
        /// <value>The left.</value>
        public float Left
        {
            get { return _left; }
            set { _left = value; }
        }

        /// <summary>
        ///     Gets or sets the top.
        /// </summary>
        /// <value>The top.</value>
        public float Top
        {
            get { return _top; }
            set { _top = value; }
        }

        /// <summary>
        ///     Gets or sets the right.
        /// </summary>
        /// <value>The right.</value>
        public float Right
        {
            get { return _right; }
            set { _right = value; }
        }

        /// <summary>
        ///     Gets or sets the bottom.
        /// </summary>
        /// <value>The bottom.</value>
        public float Bottom
        {
            get { return _bottom; }
            set { _bottom = value; }
        }

        /// <summary>
        ///     Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public float Width
        {
            get { return Right - Left; }
            set { Right = Left + value; }
        }

        /// <summary>
        ///     Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public float Height
        {
            get { return Bottom - Top; }
            set { Top = Bottom + value; }
        }

        public float X { get { return Left; } set { Left = value; } }
        public float Y { get { return Top; } set { Top = value; } }

        /// <summary>
        ///     Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///     <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is RectangleF)
                return this == (RectangleF) obj;
            return false;
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return (((((int) Left) ^ (((int) Top) << 13) | (((int) Top) >> 0x13))) ^
                    ((((int) Bottom) << 0x1a) | (((int) Bottom) >> 6))) ^
                   ((((int) Right) << 7) | (((int) Right) >> 0x19));
        }


        /// <summary>
        ///     Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(RectangleF left, RectangleF right)
        {
            return ((((System.Math.Abs(left.Left - right.Left) < MathHelper.ZeroTolerance) &&
                      (System.Math.Abs(left.Right - right.Right) < MathHelper.ZeroTolerance)) &&
                     (System.Math.Abs(left.Top - right.Top) < MathHelper.ZeroTolerance)) &&
                    (System.Math.Abs(left.Bottom - right.Bottom) < MathHelper.ZeroTolerance));
        }

        /// <summary>
        ///     Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(RectangleF left, RectangleF right)
        {
            return !(left == right);
        }
    }
}