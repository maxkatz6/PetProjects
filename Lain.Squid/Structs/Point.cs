using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Squid.Structs
{
    /// <summary>
    /// Struct Point
    /// </summary>
    public struct Point
    {
        public static readonly Point Zero;
        private int _x;
        private int _y;

        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> struct.
        /// </summary>
        /// <param name="pt">The pt.</param>
        public Point(Point pt)
        {
            this._x = pt.x;
            this._y = pt.y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public Point(int x, int y)
        {
            this._x = x;
            this._y = y;
        }

        /// <summary>
        /// Implements the +.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <returns>The result of the operator.</returns>
        public static Point operator +(Point p1, Point p2)
        {
            return new Point(p1.x + p2.x, p1.y + p2.y);
        }

        /// <summary>
        /// Implements the -.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <returns>The result of the operator.</returns>
        public static Point operator -(Point p1, Point p2)
        {
            return new Point(p1.x - p2.x, p1.y - p2.y);
        }

        /// <summary>
        /// Implements the ==.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Point p1, Point p2)
        {
            return ((p1.x == p2.x) && (p1.y == p2.y));
        }

        /// <summary>
        /// Implements the !=.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Point p1, Point p2)
        {
            return !(p1 == p2);
        }

        /// <summary>
        /// Implements the *.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static Point operator *(Point a, int b)
        {
            return new Point(a.x*b, a.y*b);
        }

        /// <summary>
        /// Implements the /.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static Point operator /(Point a, int b)
        {
            return new Point(a.x/b, a.y/b);
        }

        /// <summary>
        /// Implements the *.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static Point operator *(Point a, float b)
        {
            return new Point((int) ((float) a.x*b), (int) ((float) a.y*b));
        }

        /// <summary>
        /// Implements the /.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static Point operator /(Point a, float b)
        {
            return new Point((int) ((float) a.x/b), (int) ((float) a.y/b));
        }

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty
        {
            get { return ((_x == 0) && (_y == 0)); }
        }

        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        /// <value>The x.</value>
        public int x
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        /// <value>The y.</value>
        public int y
        {
            get { return _y; }
            set { _y = value; }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Point))
                return false;

            Point size = (Point) obj;
            return ((size._x == this._x) && (size._y == this._y));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return (this._x ^ this._y);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("{0}; {1}", x, y);
        }

        /// <summary>
        /// Eases to.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>Point.</returns>
        public static Point EaseTo(Point start, Point end, float divisor)
        {
            float x = ((float) end.x - (float) start.x)/divisor;
            float y = ((float) end.y - (float) start.y)/divisor;

            return start + new Point((int) x, (int) y);
        }

        static Point()
        {
            Zero = new Point();
        }
    }
}