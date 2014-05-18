using Mono.Simd;
using System;
using System.Runtime.InteropServices;

namespace Ormeli.Math
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 0, Size = 8)]
    public struct Vector2 : IEquatable<Vector2>
    {
        public static readonly int SizeInBytes = Marshal.SizeOf(typeof(Vector2));

        private float _x;
        private  float _y;

        public Vector2(float value)
        {
            _x = _y = value;
        }

        public Vector2(float x, float y)
        {
            _x = x;
            _y = y;
        }

        #region Static properties

        /// <summary>
        ///     A <see cref="SharpDX.Vector2" /> with all of its components set to zero.
        /// </summary>
        public static readonly Vector2 Zero = new Vector2();

        /// <summary>
        ///     The X unit <see cref="SharpDX.Vector2" /> (1, 0).
        /// </summary>
        public static readonly Vector2 UnitX = new Vector2(1.0f, 0.0f);

        /// <summary>
        ///     The Y unit <see cref="SharpDX.Vector2" /> (0, 1).
        /// </summary>
        public static readonly Vector2 UnitY = new Vector2(0.0f, 1.0f);

        /// <summary>
        ///     A <see cref="SharpDX.Vector2" /> with all of its components set to one.
        /// </summary>
        public static readonly Vector2 One = new Vector2(1.0f, 1.0f);

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return X;

                    case 1:
                        return Y;
                }

                throw new ArgumentOutOfRangeException("index", "Indices for Vector2 run from 0 to 1, inclusive.");
            }

            set
            {
                switch (index)
                {
                    case 0:
                        _x = value;
                        break;

                    case 1:
                        _y = value;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException("index", "Indices for Vector2 run from 0 to 1, inclusive.");
                }
            }
        }

        #endregion Static properties

        #region Arithmetic

        public static Vector2 Add(Vector2 value1, Vector2 value2)
        {
            Add(ref value1, ref value2, out value1);
            return value1;
        }

        public static void Add(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
        {
            result._x = value1._x + value2._x;
            result._y = value1._y + value2._y;
        }

        public static Vector2 Divide(Vector2 value1, float divider)
        {
            Vector4f v4 = new Vector4f(value1._x, value1._y, 0f, 0f) / new Vector4f(divider);
            return new Vector2(v4.X, v4.Y);
        }

        public static void Divide(ref Vector2 value1, float divider, out Vector2 result)
        {
            Vector4f v4 = new Vector4f(value1._x, value1._y, 0f, 0f) / new Vector4f(divider);
            result._x = v4.X;
            result._y = v4.Y;
        }

        public static Vector2 Divide(Vector2 value1, Vector2 value2)
        {
            Vector4f v4 = new Vector4f(value1._x, value1._y, 0f, 0f) / new Vector4f(value2._x, value2._y, 0f, 0f);
            return new Vector2(v4.X, v4.Y);
        }

        public static void Divide(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
        {
            Vector4f v4 = new Vector4f(value1._x, value1._y, 0f, 0f) / new Vector4f(value2._x, value2._y, 0f, 0f);
            result._x = v4.X;
            result._y = v4.Y;
        }

        public static Vector2 Multiply(Vector2 value1, float scaleFactor)
        {
            Vector4f v4 = new Vector4f(value1._x, value1._y, 0f, 0f) * new Vector4f(scaleFactor);
            return new Vector2(v4.X, v4.Y);
        }

        public static void Multiply(ref Vector2 value1, float scaleFactor, out Vector2 result)
        {
            Vector4f v4 = new Vector4f(value1._x, value1._y, 0f, 0f) * new Vector4f(scaleFactor);
            result._x = v4.X;
            result._y = v4.Y;
        }

        public static Vector2 Multiply(Vector2 value1, Vector2 value2)
        {
            Vector4f v4 = new Vector4f(value1._x, value1._y, 0f, 0f) * new Vector4f(value2._x, value2._y, 0f, 0f);
            return new Vector2(v4.X, v4.Y);
        }

        public static void Multiply(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
        {
            Vector4f v4 = new Vector4f(value1._x, value1._y, 0f, 0f) * new Vector4f(value2._x, value2._y, 0f, 0f);
            result._x = v4.X;
            result._y = v4.Y;
        }

        public static Vector2 Negate(Vector2 value)
        {
            Negate(ref value, out value);
            return value;
        }

        public static void Negate(ref Vector2 value, out Vector2 result)
        {
            result._x = -value._x;
            result._y = -value._y;
        }

        public static Vector2 Subtract(Vector2 value1, Vector2 value2)
        {
            Subtract(ref value1, ref value2, out value1);
            return value1;
        }

        public static void Subtract(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
        {
            result._x = value1._x - value2._x;
            result._y = value1._y - value2._x;
        }

        #endregion Arithmetic

        #region Operator overloads

        public static Vector2 operator -(Vector2 value1, Vector2 value2)
        {
            return new Vector2(value1._x - value2._x, value1._y - value2._y);
        }

        public static Vector2 operator -(Vector2 value)
        {
            return new Vector2(-value._x, -value._y);
        }

        public static Vector2 operator *(Vector2 value1, Vector2 value2)
        {
            Vector4f v4 = new Vector4f(value1._x, value1._y, 0f, 0f) * new Vector4f(value2._x, value2._y, 0f, 0f);
            return new Vector2(v4.X, v4.Y);
        }

        public static Vector2 operator *(Vector2 value, float scaleFactor)
        {
            Vector4f v4 = new Vector4f(value._x, value._y, 0f, 0f) * new Vector4f(scaleFactor);
            return new Vector2(v4.X, v4.Y);
        }

        public static Vector2 operator *(float scaleFactor, Vector2 value)
        {
            Vector4f v4 = new Vector4f(value._x, value._y, 0f, 0f) * new Vector4f(scaleFactor);
            return new Vector2(v4.X, v4.Y);
        }

        public static Vector2 operator /(Vector2 value, float divider)
        {
            Vector4f v4 = new Vector4f(value._x, value._y, 0f, 0f) / new Vector4f(divider);
            return new Vector2(v4.X, v4.Y);
        }

        public static Vector2 operator /(Vector2 value1, Vector2 value2)
        {
            Vector4f v4 = new Vector4f(value1._x, value1._y, 0f, 0f) / new Vector4f(value2._x, value2._y, 0f, 0f);
            return new Vector2(v4.X, v4.Y);
        }

        public static Vector2 operator +(Vector2 value1, Vector2 value2)
        {
            return new Vector2(value1._x + value2._x, value1._y + value2._y);
        }

        #endregion Operator overloads

        #region Interpolation

        public static Vector2 CatmullRom(Vector2 value1, Vector2 value2, Vector2 value3, Vector2 value4, float amount)
        {
            CatmullRom(ref value1, ref value2, ref value3, ref value4, amount, out value1);
            return value1;
        }

        public static void CatmullRom(ref Vector2 value1, ref Vector2 value2, ref Vector2 value3, ref Vector2 value4,
            float amount, out Vector2 result)
        {
            result._x = MathHelper.CatmullRom(value1._x, value2._x, value3._x, value4._x, amount);
            result._y = MathHelper.CatmullRom(value1._y, value2._y, value3._y, value4._y, amount);
        }

        public static Vector2 Hermite(Vector2 value1, Vector2 tangent1, Vector2 value2, Vector2 tangent2, float amount)
        {
            Hermite(ref value1, ref tangent1, ref value2, ref tangent2, amount, out value1);
            return value1;
        }

        public static void Hermite(ref Vector2 value1, ref Vector2 tangent1, ref Vector2 value2, ref Vector2 tangent2,
            float amount, out Vector2 result)
        {
            float s = amount;
            float s2 = s * s;
            float s3 = s2 * s;

            float h1 = 2 * s3 - 3 * s2 + 1;
            float h2 = -2 * s3 + 3 * s2;
            float h3 = s3 - 2 * s2 + s;
            float h4 = s3 - s2;

            result._x = h1 * value1._x + h2 * value2._x + h3 * tangent1._x + h4 * tangent2._x;
            result._y = h1 * value1._y + h2 * value2._y + h3 * tangent1._y + h4 * tangent2._y;
        }

        public static Vector2 Lerp(Vector2 value1, Vector2 value2, float amount)
        {
            Lerp(ref value1, ref value2, amount, out value1);
            return value1;
        }

        public static void Lerp(ref Vector2 value1, ref Vector2 value2, float amount, out Vector2 result)
        {
            Subtract(ref value2, ref value1, out result);
            Multiply(ref result, amount, out result);
            Add(ref value1, ref result, out result);
        }

        public static Vector2 SmoothStep(Vector2 value1, Vector2 value2, float amount)
        {
            SmoothStep(ref value1, ref value2, amount, out value1);
            return value1;
        }

        public static void SmoothStep(ref Vector2 value1, ref Vector2 value2, float amount, out Vector2 result)
        {
            float scale = (amount * amount * (3 - 2 * amount));
            Subtract(ref value2, ref value1, out result);
            Multiply(ref result, scale, out result);
            Add(ref value1, ref result, out result);
        }

        #endregion Interpolation

        #region Other maths

        public static Vector2 Barycentric(Vector2 value1, Vector2 value2, Vector2 value3, float amount1, float amount2)
        {
            Barycentric(ref value1, ref value2, ref value3, amount1, amount2, out value1);
            return value1;
        }

        public static void Barycentric(ref Vector2 value1, ref Vector2 value2, ref Vector2 value3, float amount1,
            float amount2, out Vector2 result)
        {
            result._x = MathHelper.Barycentric(value1._x, value2._x, value3._x, amount1, amount2);
            result._y = MathHelper.Barycentric(value1._y, value2._y, value3._y, amount1, amount2);
        }

        public static Vector2 Clamp(Vector2 value1, Vector2 min, Vector2 max)
        {
            Clamp(ref value1, ref min, ref max, out value1);
            return value1;
        }

        public static void Clamp(ref Vector2 value1, ref Vector2 min, ref Vector2 max, out Vector2 result)
        {
            result._x = MathHelper.Clamp(value1._x, min._x, max._x);
            result._y = MathHelper.Clamp(value1._y, min._y, max._y);
        }

        public static float Distance(Vector2 value1, Vector2 value2)
        {
            float result;
            Distance(ref value1, ref value2, out result);
            return result;
        }

        public static void Distance(ref Vector2 value1, ref Vector2 value2, out float result)
        {
            var r0 = new Vector4f(value2._x - value1._x, value2._y - value1._y, 0f, 0f);
            r0 = r0 * r0;
            r0 = r0 + r0.Shuffle(ShuffleSel.Swap);
            r0 = r0 + r0.Shuffle(ShuffleSel.RotateLeft);
            result = r0.Sqrt().X;
        }

        public static float DistanceSquared(Vector2 value1, Vector2 value2)
        {
            float result;
            DistanceSquared(ref value1, ref value2, out result);
            return result;
        }

        public static void DistanceSquared(ref Vector2 value1, ref Vector2 value2, out float result)
        {
            Vector2 val;
            Subtract(ref value1, ref value2, out val);
            result = val.LengthSquared();
        }

        public static float Dot(Vector2 value1, Vector2 value2)
        {
            float result;
            Dot(ref value1, ref value2, out result);
            return result;
        }

        public static void Dot(ref Vector2 value1, ref Vector2 value2, out float result)
        {
            result = value1._x * value2._x + value1._y * value2._y;
        }

        public static Vector2 Max(Vector2 value1, Vector2 value2)
        {
            Max(ref value1, ref value2, out value1);
            return value1;
        }

        public static void Max(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
        {
            result = new Vector2(System.Math.Max(value1._x, value2._x),
                System.Math.Max(value1._x, value2._x));
        }

        public static Vector2 Min(Vector2 value1, Vector2 value2)
        {
            Min(ref value1, ref value2, out value1);
            return value1;
        }

        public static void Min(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
        {
            result._x = System.Math.Min(value1._x, value2._x);
            result._y = System.Math.Min(value1._x, value2._x);
        }

        public static Vector2 Normalize(Vector2 value)
        {
            value.Normalize();
            return value;
        }

        public static void Normalize(ref Vector2 value, out Vector2 result)
        {
            float l = value.Length();
            result._x = value._x / l;
            result._y = value._y / l;
        }

        public static Vector2 Reflect(Vector2 vector, Vector2 normal)
        {
            Vector2 result;
            Reflect(ref vector, ref normal, out result);
            return result;
        }

        public static void Reflect(ref Vector2 vector, ref Vector2 normal, out Vector2 result)
        {
            //assuming normal is normalized, r = -v + 2 * n * (n . v)
            //http://mathworld.wolfram.com/Reflection.html
            //calculate the common part once
            var d2 = (float)System.Math.Sqrt(normal._x * vector._x + normal._y * vector._y);
            // add is much faster than multiply by 2
            d2 = d2 + d2;
            //subtract faster than negate and add
            result._x = d2 * normal._x - vector._x;
            result._y = d2 * normal._y - vector._y;
        }

        public float Length()
        {
            return (float)System.Math.Sqrt(LengthSquared());
        }

        public float LengthSquared()
        {
            return _x * _x + _y * _y;
        }

        public void Normalize()
        {
            Normalize(ref this, out this);
        }

        public float[] ToArray()
        {
            return new[] { X, Y };
        }

        public void Saturate()
        {
            _x = X < 0.0f ? 0.0f : X > 1.0f ? 1.0f : X;
            _y = Y < 0.0f ? 0.0f : Y > 1.0f ? 1.0f : Y;
        }

        #endregion Other maths

        #region Transform

        /// <summary>
        ///     Transforms a 2D vector by the given <see cref="SharpDX.Quaternion" /> rotation.
        /// </summary>
        /// <param name="vector">The vector to rotate.</param>
        /// <param name="rotation">The <see cref="SharpDX.Quaternion" /> rotation to apply.</param>
        /// <param name="result">When the method completes, contains the transformed <see cref="SharpDX.Vector4" />.</param>
        public static void Transform(ref Vector2 vector, ref Quaternion rotation, out Vector2 result)
        {
            float x = rotation.X + rotation.X;
            float y = rotation.Y + rotation.Y;
            float z = rotation.Z + rotation.Z;
            float wz = rotation.W * z;
            float xx = rotation.X * x;
            float xy = rotation.X * y;
            float yy = rotation.Y * y;
            float zz = rotation.Z * z;

            result = new Vector2((vector.X * (1.0f - yy - zz)) + (vector.Y * (xy - wz)),
                (vector.X * (xy + wz)) + (vector.Y * (1.0f - xx - zz)));
        }

        /// <summary>
        ///     Transforms a 2D vector by the given <see cref="SharpDX.Quaternion" /> rotation.
        /// </summary>
        /// <param name="vector">The vector to rotate.</param>
        /// <param name="rotation">The <see cref="SharpDX.Quaternion" /> rotation to apply.</param>
        /// <returns>The transformed <see cref="SharpDX.Vector4" />.</returns>
        public static Vector2 Transform(Vector2 vector, Quaternion rotation)
        {
            Vector2 result;
            Transform(ref vector, ref rotation, out result);
            return result;
        }

        /// <summary>
        ///     Transforms an array of vectors by the given <see cref="SharpDX.Quaternion" /> rotation.
        /// </summary>
        /// <param name="source">The array of vectors to transform.</param>
        /// <param name="rotation">The <see cref="SharpDX.Quaternion" /> rotation to apply.</param>
        /// <param name="destination">
        ///     The array for which the transformed vectors are stored.
        ///     This array may be the same array as <paramref name="source" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="source" /> or <paramref name="destination" /> is
        ///     <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="destination" /> is shorter in length than
        ///     <paramref name="source" />.
        /// </exception>
        public static void Transform(Vector2[] source, ref Quaternion rotation, Vector2[] destination)
        {
            CheckArrayArgs(source, destination);

            float x = rotation.X + rotation.X;
            float y = rotation.Y + rotation.Y;
            float z = rotation.Z + rotation.Z;
            float wz = rotation.W * z;
            float xx = rotation.X * x;
            float xy = rotation.X * y;
            float yy = rotation.Y * y;
            float zz = rotation.Z * z;

            float num1 = (1.0f - yy - zz);
            float num2 = (xy - wz);
            float num3 = (xy + wz);
            float num4 = (1.0f - xx - zz);

            for (int i = 0; i < source.Length; ++i)
            {
                destination[i] = new Vector2(
                    (source[i].X * num1) + (source[i].Y * num2),
                    (source[i].X * num3) + (source[i].Y * num4));
            }
        }

        /// <summary>
        ///     Performs a coordinate transformation using the given <see cref="SharpDX.Matrix" />.
        /// </summary>
        /// <param name="coordinate">The coordinate vector to transform.</param>
        /// <param name="transform">The transformation <see cref="SharpDX.Matrix" />.</param>
        /// <param name="result">When the method completes, contains the transformed coordinates.</param>
        /// <remarks>
        ///     A coordinate transform performs the transformation with the assumption that the w component
        ///     is one. The four dimensional vector obtained from the transformation operation has each
        ///     component in the vector divided by the w component. This forces the w component to be one and
        ///     therefore makes the vector homogeneous. The homogeneous vector is often preferred when working
        ///     with coordinates as the w component can safely be ignored.
        /// </remarks>
        public static void TransformCoordinate(ref Vector2 coordinate, ref Matrix transform, out Vector2 result)
        {
            var vector = new Vector4
            {
                X = (coordinate.X * transform.M11) + (coordinate.Y * transform.M21) + transform.M41,
                Y = (coordinate.X * transform.M12) + (coordinate.Y * transform.M22) + transform.M42,
                Z = (coordinate.X * transform.M13) + (coordinate.Y * transform.M23) + transform.M43,
                W = 1f / ((coordinate.X * transform.M14) + (coordinate.Y * transform.M24) + transform.M44)
            };

            result = new Vector2(vector.X * vector.W, vector.Y * vector.W);
        }

        /// <summary>
        ///     Performs a coordinate transformation using the given <see cref="SharpDX.Matrix" />.
        /// </summary>
        /// <param name="coordinate">The coordinate vector to transform.</param>
        /// <param name="transform">The transformation <see cref="SharpDX.Matrix" />.</param>
        /// <returns>The transformed coordinates.</returns>
        /// <remarks>
        ///     A coordinate transform performs the transformation with the assumption that the w component
        ///     is one. The four dimensional vector obtained from the transformation operation has each
        ///     component in the vector divided by the w component. This forces the w component to be one and
        ///     therefore makes the vector homogeneous. The homogeneous vector is often preferred when working
        ///     with coordinates as the w component can safely be ignored.
        /// </remarks>
        public static Vector2 TransformCoordinate(Vector2 coordinate, Matrix transform)
        {
            Vector2 result;
            TransformCoordinate(ref coordinate, ref transform, out result);
            return result;
        }

        /// <summary>
        ///     Performs a coordinate transformation on an array of vectors using the given <see cref="SharpDX.Matrix" />.
        /// </summary>
        /// <param name="source">The array of coordinate vectors to transform.</param>
        /// <param name="transform">The transformation <see cref="SharpDX.Matrix" />.</param>
        /// <param name="destination">
        ///     The array for which the transformed vectors are stored.
        ///     This array may be the same array as <paramref name="source" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="source" /> or <paramref name="destination" /> is
        ///     <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="destination" /> is shorter in length than
        ///     <paramref name="source" />.
        /// </exception>
        /// <remarks>
        ///     A coordinate transform performs the transformation with the assumption that the w component
        ///     is one. The four dimensional vector obtained from the transformation operation has each
        ///     component in the vector divided by the w component. This forces the w component to be one and
        ///     therefore makes the vector homogeneous. The homogeneous vector is often preferred when working
        ///     with coordinates as the w component can safely be ignored.
        /// </remarks>
        public static void TransformCoordinate(Vector2[] source, ref Matrix transform, Vector2[] destination)
        {
            CheckArrayArgs(source, destination);

            for (int i = 0; i < source.Length; ++i)
            {
                TransformCoordinate(ref source[i], ref transform, out destination[i]);
            }
        }

        /// <summary>
        ///     Performs a normal transformation using the given <see cref="SharpDX.Matrix" />.
        /// </summary>
        /// <param name="normal">The normal vector to transform.</param>
        /// <param name="transform">The transformation <see cref="SharpDX.Matrix" />.</param>
        /// <param name="result">When the method completes, contains the transformed normal.</param>
        /// <remarks>
        ///     A normal transform performs the transformation with the assumption that the w component
        ///     is zero. This causes the fourth row and fourth column of the matrix to be unused. The
        ///     end result is a vector that is not translated, but all other transformation properties
        ///     apply. This is often preferred for normal vectors as normals purely represent direction
        ///     rather than location because normal vectors should not be translated.
        /// </remarks>
        public static void TransformNormal(ref Vector2 normal, ref Matrix transform, out Vector2 result)
        {
            result = new Vector2(
                (normal.X * transform.M11) + (normal.Y * transform.M21),
                (normal.X * transform.M12) + (normal.Y * transform.M22));
        }

        /// <summary>
        ///     Performs a normal transformation using the given <see cref="SharpDX.Matrix" />.
        /// </summary>
        /// <param name="normal">The normal vector to transform.</param>
        /// <param name="transform">The transformation <see cref="SharpDX.Matrix" />.</param>
        /// <returns>The transformed normal.</returns>
        /// <remarks>
        ///     A normal transform performs the transformation with the assumption that the w component
        ///     is zero. This causes the fourth row and fourth column of the matrix to be unused. The
        ///     end result is a vector that is not translated, but all other transformation properties
        ///     apply. This is often preferred for normal vectors as normals purely represent direction
        ///     rather than location because normal vectors should not be translated.
        /// </remarks>
        public static Vector2 TransformNormal(Vector2 normal, Matrix transform)
        {
            Vector2 result;
            TransformNormal(ref normal, ref transform, out result);
            return result;
        }

        /// <summary>
        ///     Performs a normal transformation on an array of vectors using the given <see cref="SharpDX.Matrix" />.
        /// </summary>
        /// <param name="source">The array of normal vectors to transform.</param>
        /// <param name="transform">The transformation <see cref="SharpDX.Matrix" />.</param>
        /// <param name="destination">
        ///     The array for which the transformed vectors are stored.
        ///     This array may be the same array as <paramref name="source" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="source" /> or <paramref name="destination" /> is
        ///     <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="destination" /> is shorter in length than
        ///     <paramref name="source" />.
        /// </exception>
        /// <remarks>
        ///     A normal transform performs the transformation with the assumption that the w component
        ///     is zero. This causes the fourth row and fourth column of the matrix to be unused. The
        ///     end result is a vector that is not translated, but all other transformation properties
        ///     apply. This is often preferred for normal vectors as normals purely represent direction
        ///     rather than location because normal vectors should not be translated.
        /// </remarks>
        public static void TransformNormal(Vector2[] source, ref Matrix transform, Vector2[] destination)
        {
            CheckArrayArgs(source, destination);
            for (int i = 0; i < source.Length; ++i)
            {
                TransformNormal(ref source[i], ref transform, out destination[i]);
            }
        }

        private static void CheckArrayArgs(Vector2[] sourceArray, int sourceIndex, Vector2[] destinationArray,
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

        private static void CheckArrayArgs(Vector2[] sourceArray, Vector2[] destinationArray)
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

        public float X
        {
            get { return _x; }
        }

        public float Y
        {
            get { return _y; }
        }

        public static bool operator !=(Vector2 a, Vector2 b)
        {
            return System.Math.Abs(a._x - b._x) > MathHelper.ZeroTolerance || System.Math.Abs(a._y - b._y) > MathHelper.ZeroTolerance;
        }

        public static bool operator ==(Vector2 a, Vector2 b)
        {
            return System.Math.Abs(a._x - b._x) < MathHelper.ZeroTolerance && System.Math.Abs(a._y - b._y) < MathHelper.ZeroTolerance;
        }

        public bool Equals(Vector2 other)
        {
            return System.Math.Abs(_x - other._x) < MathHelper.ZeroTolerance && System.Math.Abs(_y - other._y) < MathHelper.ZeroTolerance;
        }

        public override bool Equals(object obj)
        {
            return obj is Vector2 && ((Vector2)obj) == this;
        }

        public override int GetHashCode()
        {
            return _x.GetHashCode() ^ _y.GetHashCode();
        }

        # endregion

        public override string ToString()
        {
            return string.Format("{{X:{0} Y:{1}}}", X, Y);
        }
    }
}