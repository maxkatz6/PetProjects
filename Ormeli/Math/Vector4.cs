using Mono.Simd;
using System;
using System.Runtime.InteropServices;

namespace Ormeli.Math
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct Vector4 : IEquatable<Vector4>
    {
        public static readonly int SizeInBytes = Marshal.SizeOf(typeof(Vector4));

        internal Vector4f _v4;

        private Vector4(Vector4f v4)
        {
            _v4 = v4;
        }

        #region Constructors

        public Vector4(float value)
        {
            _v4 = new Vector4f(value);
        }

        public Vector4(Vector2 value, float z, float w)
            : this(value.X, value.Y, z, w)
        {
        }

        public Vector4(Vector3 value, float w)
            : this(value.X, value.Y, value.Z, w)
        {
        }

        public Vector4(float x, float y, float z, float w)
        {
            _v4 = new Vector4f(x, y, z, w);
        }
        public Vector4(double x, double y, double z, double w)
        {
            _v4 = new Vector4f((float)x, (float)y, (float)z, (float)w);
        }

        #endregion Constructors

        #region Static properties

        /// <summary>
        ///     A <see cref="SharpDX.Vector4" /> with all of its components set to zero.
        /// </summary>
        public static readonly Vector4 Zero = new Vector4();

        /// <summary>
        ///     The X unit <see cref="SharpDX.Vector4" /> (1, 0, 0, 0).
        /// </summary>
        public static readonly Vector4 UnitX = new Vector4(1.0f, 0.0f, 0.0f, 0.0f);

        /// <summary>
        ///     The Y unit <see cref="SharpDX.Vector4" /> (0, 1, 0, 0).
        /// </summary>
        public static readonly Vector4 UnitY = new Vector4(0.0f, 1.0f, 0.0f, 0.0f);

        /// <summary>
        ///     The Z unit <see cref="SharpDX.Vector4" /> (0, 0, 1, 0).
        /// </summary>
        public static readonly Vector4 UnitZ = new Vector4(0.0f, 0.0f, 1.0f, 0.0f);

        /// <summary>
        ///     The W unit <see cref="SharpDX.Vector4" /> (0, 0, 0, 1).
        /// </summary>
        public static readonly Vector4 UnitW = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);

        /// <summary>
        ///     A <see cref="SharpDX.Vector4" /> with all of its components set to one.
        /// </summary>
        public static readonly Vector4 One = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);

        #endregion Static properties

        #region Arithmetic

        public bool IsNormalized
        {
            get { return System.Math.Abs((X * X) + (Y * Y) + (Z * Z) + (W * W) - 1f) < MathHelper.ZeroTolerance; }
        }

        public static Vector4 Add(Vector4 value1, Vector4 value2)
        {
            return new Vector4(value1._v4 + value2._v4);
        }

        public static void Add(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
        {
            result._v4 = value1._v4 + value2._v4;
        }

        public static Vector4 Divide(Vector4 value1, float value2)
        {
            return new Vector4(value1._v4 / new Vector4f(value2));
        }

        public static void Divide(ref Vector4 value1, float divider, out Vector4 result)
        {
            result._v4 = value1._v4 / new Vector4f(divider);
        }

        public static Vector4 Divide(Vector4 value1, Vector4 value2)
        {
            return new Vector4(value1._v4 / value2._v4);
        }

        public static void Divide(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
        {
            result._v4 = value1._v4 / value2._v4;
        }

        public static Vector4 Multiply(Vector4 value1, float scaleFactor)
        {
            return new Vector4(value1._v4 * new Vector4f(scaleFactor));
        }

        public static void Multiply(ref Vector4 value1, float scaleFactor, out Vector4 result)
        {
            result._v4 = value1._v4 * new Vector4f(scaleFactor);
        }

        public static Vector4 Multiply(Vector4 value1, Vector4 value2)
        {
            return new Vector4(value1._v4 * value2._v4);
        }

        public static void Multiply(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
        {
            result._v4 = value1._v4 * value2._v4;
        }

        public static Vector4 Negate(Vector4 value)
        {
            return new Vector4(value._v4 ^ new Vector4f(-0.0f));
        }

        public static void Negate(ref Vector4 value, out Vector4 result)
        {
            result._v4 = value._v4 ^ new Vector4f(-0.0f);
        }

        public static Vector4 Subtract(Vector4 value1, Vector4 value2)
        {
            return new Vector4(value1._v4 - value2._v4);
        }

        public static void Subtract(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
        {
            result._v4 = value1._v4 - value2._v4;
        }

        #endregion Arithmetic

        #region Operator overloads

        public static Vector4 operator -(Vector4 value1, Vector4 value2)
        {
            return new Vector4(value1._v4 - value2._v4);
        }

        public static Vector4 operator -(Vector4 value)
        {
            return new Vector4(value._v4 ^ new Vector4f(-0.0f));
        }

        public static Vector4 operator *(Vector4 value1, Vector4 value2)
        {
            return new Vector4(value1._v4 * value2._v4);
        }

        public static Vector4 operator *(Vector4 value1, float scaleFactor)
        {
            return new Vector4(value1._v4 * scaleFactor);
        }

        public static Vector4 operator *(float scaleFactor, Vector4 value1)
        {
            return new Vector4(value1._v4 * scaleFactor);
        }

        public static Vector4 operator /(Vector4 value, float divider)
        {
            return new Vector4(value._v4 / new Vector4f(divider));
        }

        public static Vector4 operator /(Vector4 value1, Vector4 value2)
        {
            return new Vector4(value1._v4 / value2._v4);
        }

        public static Vector4 operator +(Vector4 value1, Vector4 value2)
        {
            return new Vector4(value1._v4 + value2._v4);
        }

        #endregion Operator overloads

        #region Interpolation

        public static Vector4 CatmullRom(Vector4 value1, Vector4 value2, Vector4 value3, Vector4 value4, float amount)
        {
            CatmullRom(ref value1, ref value2, ref value3, ref value4, amount, out value1);
            return value1;
        }

        public static void CatmullRom(ref Vector4 value1, ref Vector4 value2, ref Vector4 value3, ref Vector4 value4,
            float amount, out Vector4 result)
        {
            result = new Vector4(new Vector4f(
                MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount),
                MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount),
                MathHelper.CatmullRom(value1.Z, value2.Z, value3.Z, value4.Z, amount),
                MathHelper.CatmullRom(value1.W, value2.W, value3.W, value4.W, amount)));
        }

        public static Vector4 Hermite(Vector4 value1, Vector4 tangent1, Vector4 value2, Vector4 tangent2, float amount)
        {
            Hermite(ref value1, ref tangent1, ref value2, ref tangent2, amount, out value1);
            return value1;
        }

        public static void Hermite(ref Vector4 value1, ref Vector4 tangent1, ref Vector4 value2, ref Vector4 tangent2,
            float amount, out Vector4 result)
        {
            var s = new Vector4f(amount);
            Vector4f s2 = s * s;
            Vector4f s3 = s2 * s;
            var c1 = new Vector4f(1f);
            var c2 = new Vector4f(2f);
            var m2 = new Vector4f(-2f);
            var c3 = new Vector4f(3f);

            Vector4f h1 = c2 * s3 - c3 * s2 + c1;
            Vector4f h2 = m2 * s3 + c3 * s2;
            Vector4f h3 = s3 - 2 * s2 + s;
            Vector4f h4 = s3 - s2;

            result._v4 = h1 * value1._v4 + h2 * value2._v4 + h3 * tangent1._v4 + h4 * tangent2._v4;
        }

        public static Vector4 Lerp(Vector4 value1, Vector4 value2, float amount)
        {
            Lerp(ref value1, ref value2, amount, out value1);
            return value1;
        }

        public static void Lerp(ref Vector4 value1, ref Vector4 value2, float amount, out Vector4 result)
        {
            result._v4 = value1._v4 + (value2._v4 - value1._v4) * amount;
        }

        public static Vector4 SmoothStep(Vector4 value1, Vector4 value2, float amount)
        {
            SmoothStep(ref value1, ref value2, amount, out value1);
            return value1;
        }

        public static void SmoothStep(ref Vector4 value1, ref Vector4 value2, float amount, out Vector4 result)
        {
            float scale = (amount * amount * (3 - 2 * amount));
            result._v4 = value1._v4 + (value2._v4 - value1._v4) * scale;
        }

        #endregion Interpolation

        #region Other maths

        public static Vector4 Barycentric(Vector4 value1, Vector4 value2, Vector4 value3, float amount1, float amount2)
        {
            Barycentric(ref value1, ref value2, ref value3, amount1, amount2, out value1);
            return value1;
        }

        public static void Barycentric(ref Vector4 value1, ref Vector4 value2, ref Vector4 value3, float amount1,
            float amount2, out Vector4 result)
        {
            result = new Vector4(
                MathHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2),
                MathHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2),
                MathHelper.Barycentric(value1.Z, value2.Z, value3.Z, amount1, amount2),
                MathHelper.Barycentric(value1.W, value2.W, value3.Z, amount1, amount2));
        }

        public static Vector4 Clamp(Vector4 value1, Vector4 min, Vector4 max)
        {
            Clamp(ref value1, ref min, ref max, out value1);
            return value1;
        }

        public static void Clamp(ref Vector4 value1, ref Vector4 min, ref Vector4 max, out Vector4 result)
        {
            result._v4 = value1._v4.Max(min._v4).Min(max._v4);
        }

        public static float Distance(Vector4 value1, Vector4 value2)
        {
            float result;
            Distance(ref value1, ref value2, out result);
            return result;
        }

        public static void Distance(ref Vector4 value1, ref Vector4 value2, out float result)
        {
            Vector4f r0 = value2._v4 - value1._v4;
            r0 = r0 * r0;
            r0 = r0 + r0.Shuffle(ShuffleSel.Swap);
            r0 = r0 + r0.Shuffle(ShuffleSel.RotateLeft);
            result = r0.Sqrt().X;
        }

        public static float DistanceSquared(Vector4 value1, Vector4 value2)
        {
            float result;
            DistanceSquared(ref value1, ref value2, out result);
            return result;
        }

        public static void DistanceSquared(ref Vector4 value1, ref Vector4 value2, out float result)
        {
            Vector4f r0 = value2._v4 - value1._v4;
            r0 = r0 * r0;
            r0 = r0 + r0.Shuffle(ShuffleSel.Swap);
            r0 = r0 + r0.Shuffle(ShuffleSel.RotateLeft);
            result = r0.X;
        }

        public static float Dot(Vector4 vector1, Vector4 vector2)
        {
            float result;
            Dot(ref vector1, ref vector2, out result);
            return result;
        }

        public static void Dot(ref Vector4 vector1, ref Vector4 vector2, out float result)
        {
            //NOTE: shuffle->add->shuffle->add is faster than horizontal-add->horizontal-add
            Vector4f r0 = vector2._v4 * vector1._v4;
            // r0 = xX yY zZ wW
            Vector4f r1 = r0.Shuffle(ShuffleSel.Swap);
            //R0 = zZ wW xX yY
            r0 = r0 + r1;
            //r0 = xX+zZ yY+wW xX+zZ yY+wW
            r1 = r0.Shuffle(ShuffleSel.RotateLeft);
            //R0 = yY+wW xX+zZ yY+wW xX+zZ
            r0 = r0 + r1;
            //r0 = xX+yY+zZ+wW xX+yY+zZ+wW xX+yY+zZ+wW xX+yY+zZ+wW
            result = r0.Sqrt().X;
        }

        public static Vector4 Max(Vector4 value1, Vector4 value2)
        {
            Max(ref value1, ref value2, out value1);
            return value1;
        }

        public static void Max(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
        {
            result._v4 = value1._v4.Max(value2._v4);
        }

        public static Vector4 Min(Vector4 value1, Vector4 value2)
        {
            Min(ref value1, ref value2, out value1);
            return value1;
        }

        public static void Min(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
        {
            result._v4 = value1._v4.Min(value2._v4);
        }

        public static Vector4 Normalize(Vector4 value)
        {
            value.Normalize();
            return value;
        }

        public static void Normalize(ref Vector4 value, out Vector4 result)
        {
            Vector4f r0 = value._v4;
            r0 = r0 * r0;
            r0 = r0 + r0.Shuffle(ShuffleSel.Swap);
            r0 = r0 + r0.Shuffle(ShuffleSel.RotateLeft);
            result._v4 = value._v4 / r0.Sqrt();
        }

        public float Length()
        {
            Vector4f r0 = _v4;
            r0 = r0 * r0;
            r0 = r0 + r0.Shuffle(ShuffleSel.Swap);
            r0 = r0 + r0.Shuffle(ShuffleSel.RotateLeft);
            return r0.Sqrt().X;
        }

        public float LengthSquared()
        {
            Vector4f r0 = _v4;
            r0 = r0 * r0;
            r0 = r0 + r0.Shuffle(ShuffleSel.Swap);
            r0 = r0 + r0.Shuffle(ShuffleSel.RotateLeft);
            return r0.X;
        }

        public void Normalize()
        {
            Normalize(ref this, out this);
        }

        #endregion Other maths

        #region Transform

        /// <summary>
        ///     Transforms a 2D vector by the given <see cref="SharpDX.Matrix" />.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="transform">The transformation <see cref="SharpDX.Matrix" />.</param>
        /// <param name="result">When the method completes, contains the transformed <see cref="SharpDX.Vector4" />.</param>
        public static void Transform(ref Vector2 vector, ref Matrix transform, out Vector4 result)
        {
            result = new Vector4(
                (vector.X * transform.M11) + (vector.Y * transform.M21) + transform.M41,
                (vector.X * transform.M12) + (vector.Y * transform.M22) + transform.M42,
                (vector.X * transform.M13) + (vector.Y * transform.M23) + transform.M43,
                (vector.X * transform.M14) + (vector.Y * transform.M24) + transform.M44);
        }

        /// <summary>
        ///     Transforms a 2D vector by the given <see cref="SharpDX.Matrix" />.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="transform">The transformation <see cref="SharpDX.Matrix" />.</param>
        /// <returns>The transformed <see cref="SharpDX.Vector4" />.</returns>
        public static Vector4 Transform(Vector2 vector, Matrix transform)
        {
            Vector4 result;
            Transform(ref vector, ref transform, out result);
            return result;
        }

        /// <summary>
        ///     Transforms an array of 2D vectors by the given <see cref="SharpDX.Matrix" />.
        /// </summary>
        /// <param name="source">The array of vectors to transform.</param>
        /// <param name="transform">The transformation <see cref="SharpDX.Matrix" />.</param>
        /// <param name="destination">The array for which the transformed vectors are stored.</param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="source" /> or <paramref name="destination" /> is
        ///     <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="destination" /> is shorter in length than
        ///     <paramref name="source" />.
        /// </exception>
        public static void Transform(Vector2[] source, ref Matrix transform, Vector4[] destination)
        {
            CheckArrayArgs(source, destination);

            for (int i = 0; i < source.Length; ++i)
            {
                Transform(ref source[i], ref transform, out destination[i]);
            }
        }

        public static Vector4 Transform(Vector3 position, Matrix matrix)
        {
            Vector4 result;
            Transform(ref position, ref matrix, out result);
            return result;
        }

        public static void Transform(ref Vector3 vector, ref Matrix transform, out Vector4 result)
        {
            result = new Vector4(
                (vector.X * transform.M11) + (vector.Y * transform.M21) + (vector.Z * transform.M31) + transform.M41,
                (vector.X * transform.M12) + (vector.Y * transform.M22) + (vector.Z * transform.M32) + transform.M42,
                (vector.X * transform.M13) + (vector.Y * transform.M23) + (vector.Z * transform.M33) + transform.M43,
                (vector.X * transform.M14) + (vector.Y * transform.M24) + (vector.Z * transform.M34) + transform.M44);
        }

        public static void Transform(Vector3[] source, ref Matrix transform, Vector4[] destination)
        {
            CheckArrayArgs(source, destination);

            for (int i = 0; i < source.Length; ++i)
            {
                Transform(ref source[i], ref transform, out destination[i]);
            }
        }


        public static Vector4 Transform(Vector3 value, Quaternion rotation)
        {
            Vector4 result;
            Transform(ref value, ref rotation, out result);
            return result;
        }

        public static void Transform(ref Vector3 value, ref Quaternion rotation, out Vector4 result)
        {
            float x = rotation.X + rotation.X;
            float y = rotation.Y + rotation.Y;
            float z = rotation.Z + rotation.Z;
            float wx = rotation.W * x;
            float wy = rotation.W * y;
            float wz = rotation.W * z;
            float xx = rotation.X * x;
            float xy = rotation.X * y;
            float xz = rotation.X * z;
            float yy = rotation.Y * y;
            float yz = rotation.Y * z;
            float zz = rotation.Z * z;

            result = new Vector4(
                ((value.X * ((1.0f - yy) - zz)) + (value.Y * (xy - wz))) + (value.Z * (xz + wy)),
                ((value.X * (xy + wz)) + (value.Y * ((1.0f - xx) - zz))) + (value.Z * (yz - wx)),
                ((value.X * (xz - wy)) + (value.Y * (yz + wx))) + (value.Z * ((1.0f - xx) - yy)),
                1);
        }

        public static Vector4 Transform(Vector4 position, Matrix matrix)
        {
            Vector4 result;
            Transform(ref position, ref matrix, out result);
            return result;
        }

        public static void Transform(ref Vector4 vector, ref Matrix transform, out Vector4 result)
        {
            result = new Vector4(
                (vector.X * transform.M11) + (vector.Y * transform.M21) + (vector.Z * transform.M31) +
                (vector.W * transform.M41),
                (vector.X * transform.M12) + (vector.Y * transform.M22) + (vector.Z * transform.M32) +
                (vector.W * transform.M42),
                (vector.X * transform.M13) + (vector.Y * transform.M23) + (vector.Z * transform.M33) +
                (vector.W * transform.M43),
                (vector.X * transform.M14) + (vector.Y * transform.M24) + (vector.Z * transform.M34) +
                (vector.W * transform.M44));
        }

        public static Vector4 Transform(Vector4 value, Quaternion rotation)
        {
            Vector4 result;
            Transform(ref value, ref rotation, out result);
            return result;
        }

        public static void Transform(ref Vector4 value, ref Quaternion rotation, out Vector4 result)
        {
            float x = rotation.X + rotation.X;
            float y = rotation.Y + rotation.Y;
            float z = rotation.Z + rotation.Z;
            float wx = rotation.W * x;
            float wy = rotation.W * y;
            float wz = rotation.W * z;
            float xx = rotation.X * x;
            float xy = rotation.X * y;
            float xz = rotation.X * z;
            float yy = rotation.Y * y;
            float yz = rotation.Y * z;
            float zz = rotation.Z * z;

            result = new Vector4(
                ((value.X * ((1.0f - yy) - zz)) + (value.Y * (xy - wz))) + (value.Z * (xz + wy)),
                ((value.X * (xy + wz)) + (value.Y * ((1.0f - xx) - zz))) + (value.Z * (yz - wx)),
                ((value.X * (xz - wy)) + (value.Y * (yz + wx))) + (value.Z * ((1.0f - xx) - yy)),
                value.W);
        }

        public static void Transform(Vector4[] sourceArray, int sourceIndex, ref Matrix matrix,
            Vector4[] destinationArray, int destinationIndex, int length)
        {
            CheckArrayArgs(sourceArray, sourceIndex, destinationArray, destinationIndex, length);

            int smax = sourceIndex + length;
            for (int s = sourceIndex, d = destinationIndex; s < smax; s++, d++)
                Transform(ref sourceArray[s], ref matrix, out destinationArray[d]);
        }

        public static void Transform(Vector4[] sourceArray, int sourceIndex, ref Quaternion rotation,
            Vector4[] destinationArray, int destinationIndex, int length)
        {
            CheckArrayArgs(sourceArray, sourceIndex, destinationArray, destinationIndex, length);

            int smax = sourceIndex + length;
            for (int s = sourceIndex, d = destinationIndex; s < smax; s++, d++)
                Transform(ref sourceArray[s], ref rotation, out destinationArray[d]);
        }

        public static void Transform(Vector4[] sourceArray, ref Matrix matrix, Vector4[] destinationArray)
        {
            CheckArrayArgs(sourceArray, destinationArray);

            for (int i = 0; i < sourceArray.Length; i++)
                Transform(ref sourceArray[i], ref matrix, out destinationArray[i]);
        }

        public static void Transform(Vector4[] source, ref Quaternion rotation, Vector4[] destination)
        {
            CheckArrayArgs(source, destination);

            float x = rotation.X + rotation.X;
            float y = rotation.Y + rotation.Y;
            float z = rotation.Z + rotation.Z;
            float wx = rotation.W * x;
            float wy = rotation.W * y;
            float wz = rotation.W * z;
            float xx = rotation.X * x;
            float xy = rotation.X * y;
            float xz = rotation.X * z;
            float yy = rotation.Y * y;
            float yz = rotation.Y * z;
            float zz = rotation.Z * z;

            float num1 = ((1.0f - yy) - zz);
            float num2 = (xy - wz);
            float num3 = (xz + wy);
            float num4 = (xy + wz);
            float num5 = ((1.0f - xx) - zz);
            float num6 = (yz - wx);
            float num7 = (xz - wy);
            float num8 = (yz + wx);
            float num9 = ((1.0f - xx) - yy);

            for (int i = 0; i < source.Length; ++i)
            {
                destination[i] = new Vector4(
                    ((source[i].X * num1) + (source[i].Y * num2)) + (source[i].Z * num3),
                    ((source[i].X * num4) + (source[i].Y * num5)) + (source[i].Z * num6),
                    ((source[i].X * num7) + (source[i].Y * num8)) + (source[i].Z * num9),
                    source[i].W);
            }
        }

        private static void CheckArrayArgs(Vector4[] sourceArray, int sourceIndex, Vector4[] destinationArray,
            int destinationIndex, int length)
        {
            if (sourceArray == null)
                throw new ArgumentNullException("sourceArray");
            if (destinationArray == null)
                throw new ArgumentNullException("destinationArray");
            if (sourceIndex + length > sourceArray.Length)
                throw new ArgumentException("Source is smaller than specified length and index");
            if (destinationIndex + length > destinationArray.Length)
                throw new ArgumentException("Destination is smaller than specified length and index");
        }

        private static void CheckArrayArgs(Vector4[] sourceArray, Vector4[] destinationArray)
        {
            if (sourceArray == null)
                throw new ArgumentNullException("sourceArray");
            if (destinationArray == null)
                throw new ArgumentNullException("destinationArray");
            if (destinationArray.Length < sourceArray.Length)
                throw new ArgumentException("Destination is smaller than source", "destinationArray");
        }
        private static void CheckArrayArgs(Vector2[] sourceArray, Vector4[] destinationArray)
        {
            if (sourceArray == null)
                throw new ArgumentNullException("sourceArray");
            if (destinationArray == null)
                throw new ArgumentNullException("destinationArray");
            if (destinationArray.Length < sourceArray.Length)
                throw new ArgumentException("Destination is smaller than source", "destinationArray");
        }

        private static void CheckArrayArgs(Vector3[] sourceArray, Vector4[] destinationArray)
        {
            if (sourceArray == null)
                throw new ArgumentNullException("sourceArray");
            if (destinationArray == null)
                throw new ArgumentNullException("destinationArray");
            if (destinationArray.Length < sourceArray.Length)
                throw new ArgumentException("Destination is smaller than source", "destinationArray");
        }

        #endregion Transform

        #region Equality

        public float W
        {
            get { return _v4.W; }
            set { _v4.W = value; }
        }

        public float X
        {
            get { return _v4.X; }
            set { _v4.X = value; }
        }

        public float Y
        {
            get { return _v4.Y; }
            set { _v4.Y = value; }
        }

        public float Z
        {
            get { return _v4.Z; }
            set { _v4.Z = value; }
        }

        public static bool operator !=(Vector4 a, Vector4 b)
        {
            return a._v4 != b._v4;
        }

        public static bool operator ==(Vector4 a, Vector4 b)
        {
            return a._v4 == b._v4;
        }

        public bool Equals(Vector4 other)
        {
            return _v4 == other._v4;
        }

        public override bool Equals(object obj)
        {
            return obj is Vector4 && ((Vector4)obj) == this;
        }

        public override int GetHashCode()
        {
#if UNSAFE
			unsafe {
				Vector4f f = v4;
				Vector4i i = *((Vector4i*)&f);
				i = i ^ i.Shuffle (ShuffleSel.Swap);
				i = i ^ i.Shuffle (ShuffleSel.RotateLeft);
				return i.X;
			}
#else
            return _v4.X.GetHashCode() ^ _v4.Y.GetHashCode() ^ _v4.Z.GetHashCode() ^ _v4.W.GetHashCode();
#endif
        }

        # endregion

        public override string ToString()
        {
            return String.Format("{{X:{0} Y:{1} Z:{2} W:{3}}}", X, Y, Z, W);
        }
    }
}