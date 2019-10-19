using System;
using System.Collections;
using System.Globalization;

namespace Squid.Structs
{
    /// <summary>
    /// Struct Margin
    /// </summary>
    public struct Margin
    {
        private bool _all;
        private int _top;
        private int _left;
        private int _right;
        private int _bottom;
        public static readonly Margin Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="Margin"/> struct.
        /// </summary>
        /// <param name="all">All.</param>
        public Margin(int all)
        {
            _all = true;
            _top = _left = _right = _bottom = all;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Margin"/> struct.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="top">The top.</param>
        /// <param name="right">The right.</param>
        /// <param name="bottom">The bottom.</param>
        public Margin(int left, int top, int right, int bottom)
        {
            _top = top;
            _left = left;
            _right = right;
            _bottom = bottom;
            _all = ((_top == _left) && (_top == _right)) && (_top == _bottom);
        }

        /// <summary>
        /// Gets or sets all.
        /// </summary>
        /// <value>All.</value>
        public int All
        {
            get
            {
                if (!_all)
                    return -1;

                return _top;
            }
            set
            {
                if (!_all || (_top != value))
                {
                    _all = true;
                    _top = _left = _right = _bottom = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the bottom.
        /// </summary>
        /// <value>The bottom.</value>
        
        public int Bottom
        {
            get
            {
                if (_all)
                    return _top;

                return _bottom;
            }
            set
            {
                if (_all || (_bottom != value))
                {
                    _all = false;
                    _bottom = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the left.
        /// </summary>
        /// <value>The left.</value>
        
        public int Left
        {
            get
            {
                if (_all)
                    return _top;

                return _left;
            }
            set
            {
                if (_all || (_left != value))
                {
                    _all = false;
                    _left = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the right.
        /// </summary>
        /// <value>The right.</value>
        
        public int Right
        {
            get
            {
                if (_all)
                    return _top;

                return _right;
            }
            set
            {
                if (_all || (_right != value))
                {
                    _all = false;
                    _right = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the top.
        /// </summary>
        /// <value>The top.</value>
        
        public int Top
        {
            get
            {
                return _top;
            }
            set
            {
                if (_all || (_top != value))
                {
                    _all = false;
                    _top = value;
                }
            }
        }

        /// <summary>
        /// Gets the horizontal.
        /// </summary>
        /// <value>The horizontal.</value>
        
        public int Horizontal
        {
            get
            {
                return (Left + Right);
            }
        }

        /// <summary>
        /// Gets the vertical.
        /// </summary>
        /// <value>The vertical.</value>
        
        public int Vertical
        {
            get
            {
                return (Top + Bottom);
            }
        }
        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        
        public Point Size
        {
            get
            {
                return new Point(Horizontal, Vertical);
            }
        }

        /// <summary>
        /// Adds the specified p1.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <returns>Margin.</returns>
        public static Margin Add(Margin p1, Margin p2)
        {
            return p1 + p2;
        }

        /// <summary>
        /// Subtracts the specified p1.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <returns>Margin.</returns>
        public static Margin Subtract(Margin p1, Margin p2)
        {
            return p1 - p2;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object other)
        {
            return (other is Margin) && ((Margin)other) == this;
        }

        /// <summary>
        /// Implements the +.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <returns>The result of the operator.</returns>
        public static Margin operator +(Margin p1, Margin p2)
        {
            return new Margin(p1.Left + p2.Left, p1.Top + p2.Top, p1.Right + p2.Right, p1.Bottom + p2.Bottom);
        }

        /// <summary>
        /// Implements the -.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <returns>The result of the operator.</returns>
        public static Margin operator -(Margin p1, Margin p2)
        {
            return new Margin(p1.Left - p2.Left, p1.Top - p2.Top, p1.Right - p2.Right, p1.Bottom - p2.Bottom);
        }

        /// <summary>
        /// Implements the ==.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Margin p1, Margin p2)
        {
            return (((p1.Left == p2.Left) && (p1.Top == p2.Top)) && (p1.Right == p2.Right)) && (p1.Bottom == p2.Bottom);
        }

        /// <summary>
        /// Implements the !=.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Margin p1, Margin p2)
        {
            return !(p1 == p2);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("{0}; {1}; {2}; {3}", Left, Top, Right, Bottom);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        internal bool ShouldSerializeAll()
        {
            return _all;
        }

        static Margin()
        {
            Empty = new Margin(0);
        }
    }
}
