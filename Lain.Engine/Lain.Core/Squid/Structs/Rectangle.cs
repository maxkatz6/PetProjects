using System;

namespace Squid.Structs
{
    /// <summary>
    /// Struct Rectangle
    /// </summary>
    public struct Rectangle
    {
        private int _left;
        private int _right;
        private int _top;
        private int _bottom;

        public Point Size => new Point(Width, Height);

        /// <summary>
        /// Gets or sets the left edge.
        /// </summary>
        /// <value>The left.</value>
        public int Left
        {
            get { return _left; }
            set { _left = value; }
        }

        /// <summary>
        /// Gets or sets the top edge.
        /// </summary>
        /// <value>The top.</value>
        public int Top
        {
            get { return _top; }
            set { _top = value; }
        }

        /// <summary>
        /// Gets or sets the right edge.
        /// </summary>
        /// <value>The right.</value>
        public int Right
        {
            get { return _right; }
            set { _right = value; }
        }

        /// <summary>
        /// Gets or sets the bottom edge.
        /// </summary>
        /// <value>The bottom.</value>
        public int Bottom
        {
            get { return _bottom; }
            set { _bottom = value; }
        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width
        {
            get { return Right - Left; }
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height
        {
            get { return Bottom - Top; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Rectangle(int x, int y, int width, int height)
        {
            _left = x;
            _top = y;
            _right = x + width;
            _bottom = y + height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> struct.
        /// </summary>
        /// <param name="pos">The pos.</param>
        /// <param name="size">The size.</param>
        public Rectangle(Point pos, Point size)
        {
            _left = pos.x;
            _top = pos.y;

            _right = pos.x + size.x;
            _bottom = pos.y + size.y;
        }

        /// <summary>
        /// Intersects the specified rect.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public bool Intersects(Rectangle rect)
        {
            return ((((rect.Left < (Left + Width)) && (Left < (rect.Left + rect.Width))) && (rect.Top < (Top + Height))) &&
                    (Top < (rect.Top + rect.Height)));
        }

        /// <summary>
        /// Intersects the specified rect.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public bool Contains(Rectangle rect)
        {
            return rect.Left >= Left && Right >= rect.Right && rect.Top >= Top && Bottom >= rect.Bottom;
        }

        /// <summary>
        /// Clips the specified rect.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <returns>Rectangle.</returns>
        public Rectangle Clip(Rectangle rect)
        {
            Rectangle result = new Rectangle();
            result.Left = Math.Max(Left, rect.Left);
            result.Top = Math.Max(Top, rect.Top);

            result.Right = Math.Min(Right, rect.Right);
            result.Bottom = Math.Min(Bottom, rect.Bottom);

            return result;
        }

        /// <summary>
        /// Determines whether [contains] [the specified point].
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns><c>true</c> if [contains] [the specified point]; otherwise, <c>false</c>.</returns>
        public bool Contains(Point point)
        {
            return point.x > Left && point.x < Right && point.y > Top && point.y < Bottom;
        }

        /// <summary>
        /// Determines whether this instance is empty.
        /// </summary>
        /// <returns><c>true</c> if this instance is empty; otherwise, <c>false</c>.</returns>
        public bool IsEmpty()
        {
            return Left == Right && Top == Bottom;
        }
    }
}