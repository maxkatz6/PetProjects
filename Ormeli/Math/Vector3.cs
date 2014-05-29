using Mono.Simd;
using System;
using System.Runtime.InteropServices;

namespace Ormeli.Math
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct Vector3 : IEquatable<Vector3>
    {
        public static readonly int SizeInBytes = Marshal.SizeOf(typeof(Vector3));

        public Vector3(float value)
            : this(value, value, value)
        {
        }

        public Vector3(Vector2 value, float z)
            : this(value.X, value.Y, z)
        {
        }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3(Vector4f value)
        {
            X = value.X;
            Y = value.Y;
            Z = value.Z;
        }

        public float X;

        public float Y;

        public float Z;

        public Vector4f v4
        {
            get { return new Vector4f(X, Y, Z, 0); }
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }

        #region Static properties

        public static Vector3 Backward
        {
            get { return new Vector3(0f, 0f, 1f); }
        }

        public static Vector3 Down
        {
            get { return new Vector3(0f, -1f, 0f); }
        }

        public static Vector3 Forward
        {
            get { return new Vector3(0f, 0f, -1f); }
        }

        public static Vector3 Left
        {
            get { return new Vector3(-1f, 0f, 0f); }
        }

        public static Vector3 One
        {
            get { return new Vector3(1f); }
        }

        public static Vector3 Right
        {
            get { return new Vector3(1f, 0f, 0f); }
        }

        public static Vector3 UnitX
        {
            get { return new Vector3(1f, 0f, 0f); }
        }

        public static Vector3 UnitY
        {
            get { return new Vector3(0f, 1f, 0f); }
        }

        public static Vector3 UnitZ
        {
            get { return new Vector3(0f, 0f, 1f); }
        }

        public static Vector3 Up
        {
            get { return new Vector3(0f, 1f, 0f); }
        }

        public static Vector3 Zero
        {
            get { return new Vector3(0f); }
        }

        #endregion Static properties

        #region Arithmetic

        public static Vector3 Add(Vector3 value1, Vector3 value2)
        {
            return new Vector3(value1.v4 + value2.v4);
        }

        public static void Add(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result = new Vector3(value1.v4 + value2.v4);
        }

        public static Vector3 Divide(Vector3 value1, float value2)
        {
            return new Vector3(value1.v4 / new Vector4f(value2));
        }

        public static void Divide(ref Vector3 value1, float value2, out Vector3 result)
        {
            result = new Vector3(value1.v4 / new Vector4f(value2));
        }

        public static Vector3 Divide(Vector3 value1, Vector3 value2)
        {
            return new Vector3(value1.v4 / value2.v4);
        }

        public static void Divide(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result = new Vector3(value1.v4 / value2.v4);
        }

        public static Vector3 Multiply(Vector3 value1, float scaleFactor)
        {
            return new Vector3(value1.v4 * new Vector4f(scaleFactor));
        }

        public static void Multiply(ref Vector3 value1, float scaleFactor, out Vector3 result)
        {
            result = new Vector3(value1.v4 * new Vector4f(scaleFactor));
        }

        public static Vector3 Multiply(Vector3 value1, Vector3 value2)
        {
            return new Vector3(value1.v4 * value2.v4);
        }

        public static void Multiply(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result = new Vector3(value1.v4 * value2.v4);
        }

        public static Vector3 Negate(Vector3 value)
        {
            return new Vector3(value.v4 ^ new Vector4f(-0.0f));
        }

        public static void Negate(ref Vector3 value, out Vector3 result)
        {
            result = new Vector3(value.v4 ^ new Vector4f(-0.0f));
        }

        public static Vector3 Subtract(Vector3 value1, Vector3 value2)
        {
            return new Vector3(value1.v4 - value2.v4);
        }

        public static void Subtract(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result = new Vector3(value1.v4 - value2.v4);
        }

        #endregion Arithmetic

        #region Operator overloads

        public static Vector3 operator -(Vector3 value1, Vector3 value2)
        {
            return new Vector3(value1.v4 - value2.v4);
        }

        public static Vector3 operator -(Vector3 value)
        {
            return new Vector3(value.v4 ^ new Vector4f(-0.0f));
        }

        public static Vector3 operator *(Vector3 value1, Vector3 value2)
        {
            return new Vector3(value1.v4 * value2.v4);
        }

        public static Vector3 operator *(Vector3 value, float scaleFactor)
        {
            return new Vector3(value.v4 * scaleFactor);
        }

        public static Vector3 operator *(float scaleFactor, Vector3 value)
        {
            return new Vector3(scaleFactor * value.v4);
        }

        public static Vector3 operator /(Vector3 value, float divider)
        {
            return new Vector3(value.v4 / new Vector4f(divider));
        }

        public static Vector3 operator /(Vector3 value1, Vector3 value2)
        {
            return new Vector3(value1.v4 / value2.v4);
        }

        public static Vector3 operator +(Vector3 value1, Vector3 value2)
        {
            return new Vector3(value1.v4 + value2.v4);
        }

        #endregion Operator overloads

        #region Interpolation

        public static Vector3 CatmullRom(Vector3 value1, Vector3 value2, Vector3 value3, Vector3 value4, float amount)
        {
            CatmullRom(ref value1, ref value2, ref value3, ref value4, amount, out value1);
            return value1;
        }

        public static void CatmullRom(ref Vector3 value1, ref Vector3 value2, ref Vector3 value3, ref Vector3 value4,
            float amount, out Vector3 result)
        {
            result = new Vector3(
                MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount),
                MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount),
                MathHelper.CatmullRom(value1.Z, value2.Z, value3.Z, value4.Z, amount)
                );
        }

        public static Vector3 Hermite(Vector3 value1, Vector3 tangent1, Vector3 value2, Vector3 tangent2, float amount)
        {
            Hermite(ref value1, ref tangent1, ref value2, ref tangent2, amount, out value1);
            return value1;
        }

        public static void Hermite(ref Vector3 value1, ref Vector3 tangent1, ref Vector3 value2, ref Vector3 tangent2,
            float amount, out Vector3 result)
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

            result = new Vector3(h1 * value1.v4 + h2 * value2.v4 + h3 * tangent1.v4 + h4 * tangent2.v4);
        }

        public static Vector3 Lerp(Vector3 value1, Vector3 value2, float amount)
        {
            Lerp(ref value1, ref value2, amount, out value1);
            return value1;
        }

        public static void Lerp(ref Vector3 value1, ref Vector3 value2, float amount, out Vector3 result)
        {
            result = new Vector3(value1.v4 + (value2.v4 - value1.v4) * amount);
        }

        public static Vector3 SmoothStep(Vector3 value1, Vector3 value2, float amount)
        {
            SmoothStep(ref value1, ref value2, amount, out value1);
            return value1;
        }

        public static void SmoothStep(ref Vector3 value1, ref Vector3 value2, float amount, out Vector3 result)
        {
            float scale = (amount * amount * (3 - 2 * amount));

            result = new Vector3(value1.v4 + (value2.v4 - value1.v4) * scale);
        }

        #endregion Interpolation

        #region Other maths

        public static Vector3 Barycentric(Vector3 value1, Vector3 value2, Vector3 value3, float amount1, float amount2)
        {
            Barycentric(ref value1, ref value2, ref value3, amount1, amount2, out value1);
            return value1;
        }

        public static void Barycentric(ref Vector3 value1, ref Vector3 value2, ref Vector3 value3, float amount1,
            float amount2, out Vector3 result)
        {
            result = new Vector3(
                MathHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2),
                MathHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2),
                MathHelper.Barycentric(value1.Z, value2.Z, value3.Z, amount1, amount2)
                );
        }

        public static Vector3 Clamp(Vector3 value1, Vector3 min, Vector3 max)
        {
            Clamp(ref value1, ref min, ref max, out value1);
            return value1;
        }

        public static void Clamp(ref Vector3 value1, ref Vector3 min, ref Vector3 max, out Vector3 result)
        {
            result = new Vector3(value1.v4.Max(min.v4).Min(max.v4));
        }

        public static Vector3 Cross(Vector3 vector1, Vector3 vector2)
        {
            Vector3 result;
            Cross(ref vector1, ref vector2, out result);
            return result;
        }

        public static void Cross(ref Vector3 left, ref Vector3 right, out Vector3 result)
        {
    /*        Vector4f r1 = left.v4;
            Vector4f r2 = right.v4;
            result = new Vector3(
                r1.Shuffle((ShuffleSel) 201) *
                r2.Shuffle((ShuffleSel) 210) -
                r1.Shuffle((ShuffleSel) 210) *
                r2.Shuffle((ShuffleSel) 201));*/
            result = new Vector3(left.Y*right.Z - left.Z*right.Y,
                left.Z*right.X - left.X*right.Z,
                left.X*right.Y - left.Y*right.X);
        }

        public static float Distance(Vector3 value1, Vector3 value2)
        {
            float result;
            Distance(ref value1, ref value2, out result);
            return result;
        }

        public static void Distance(ref Vector3 value1, ref Vector3 value2, out float result)
        {
            Vector4f r0 = value2.v4 - value1.v4;
            r0 = r0 * r0;
            r0 = r0 + r0.Shuffle(ShuffleSel.Swap);
            r0 = r0 + r0.Shuffle(ShuffleSel.RotateLeft);
            result = r0.Sqrt().X;
        }

        public static float DistanceSquared(Vector3 value1, Vector3 value2)
        {
            float result;
            DistanceSquared(ref value1, ref value2, out result);
            return result;
        }

        public static void DistanceSquared(ref Vector3 value1, ref Vector3 value2, out float result)
        {
            Vector4f r0 = value2.v4 - value1.v4;
            r0 = r0 * r0;
            r0 = r0 + r0.Shuffle(ShuffleSel.Swap);
            r0 = r0 + r0.Shuffle(ShuffleSel.RotateLeft);
            result = r0.X;
        }

        public static float Dot(Vector3 vector1, Vector3 vector2)
        {
            float result;
            Dot(ref vector1, ref vector2, out result);
            return result;
        }

        public static void Dot(ref Vector3 left, ref Vector3 right, out float result)
        {
            result = (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
        }

        public static Vector3 Max(Vector3 value1, Vector3 value2)
        {
            Max(ref value1, ref value2, out value1);
            return value1;
        }

        public static void Max(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result = new Vector3(value1.v4.Max(value2.v4));
        }

        public static Vector3 Min(Vector3 value1, Vector3 value2)
        {
            Min(ref value1, ref value2, out value1);
            return value1;
        }

        public static void Min(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result = new Vector3(value1.v4.Min(value2.v4));
        }

        public static Vector3 Normalize(Vector3 value)
        {
            value.Normalize();
            return value;
        }

        public static void Normalize(ref Vector3 value, out Vector3 result)
        {
            /*Vector4f r0 = value.v4;
           r0 = r0 * r0;
            r0 = r0 + r0.Shuffle(ShuffleSel.Swap);
            r0 = r0 + r0.Shuffle(ShuffleSel.RotateLeft);
            result = new Vector3(value.v4 / r0.Sqrt());*/
            float scale = 1.0f / value.Length();
            result = (float.IsInfinity(scale) ? Zero : new Vector3(value.X * scale, value.Y * scale, value.Z * scale));
        }

        public static Vector3 Reflect(Vector3 vector, Vector3 normal)
        {
            Vector3 result;
            Reflect(ref vector, ref normal, out result);
            return result;
        }

        public static void Reflect(ref Vector3 vector, ref Vector3 normal, out Vector3 result)
        {
            Vector4f v = vector.v4, n = normal.v4;
            Vector4f r0 = v * n;
            r0 = r0 + r0.Shuffle(ShuffleSel.Swap);
            r0 = r0 + r0.Shuffle(ShuffleSel.RotateLeft);
            r0 = r0.Sqrt();
            result = new Vector3((r0 + r0) * n - v);
        }

        public float Length()
        {
         /*   Vector4f r0 = v4;
            r0 = r0 * r0;
            r0 = r0 + r0.Shuffle(ShuffleSel.Swap);
            r0 = r0 + r0.Shuffle(ShuffleSel.RotateLeft);
            return r0.Sqrt().X;*/

            return (float)System.Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public float LengthSquared()
        {
            Vector4f r0 = v4;
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

        public static Vector3 Transform(Vector3 position, Matrix matrix)
        {
            Vector3 result;
            Transform(ref position, ref matrix, out result);
            return result;
        }

        public static void Transform(ref Vector3 vector, ref Matrix transform, out Vector3 result)
        {
            //maybe
            result = new Vector3(
                (vector.X * transform.M11) + (vector.Y * transform.M21) + (vector.Z * transform.M31) + transform.M41,
                (vector.X * transform.M12) + (vector.Y * transform.M22) + (vector.Z * transform.M32) + transform.M42,
                (vector.X * transform.M13) + (vector.Y * transform.M23) + (vector.Z * transform.M33) + transform.M43);
        }

        public static Vector3 Transform(Vector3 value, Quaternion rotation)
        {
            Vector3 result;
            Transform(ref value, ref rotation, out result);
            return result;
        }

        public static void Transform(ref Vector3 vector, ref Quaternion rotation, out Vector3 result)
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

            result = new Vector3(
                ((vector.X * ((1.0f - yy) - zz)) + (vector.Y * (xy - wz))) + (vector.Z * (xz + wy)),
                ((vector.X * (xy + wz)) + (vector.Y * ((1.0f - xx) - zz))) + (vector.Z * (yz - wx)),
                ((vector.X * (xz - wy)) + (vector.Y * (yz + wx))) + (vector.Z * ((1.0f - xx) - yy)));
        }

        public static void Transform(Vector3[] sourceArray, int sourceIndex, ref Matrix matrix,
            Vector3[] destinationArray, int destinationIndex, int length)
        {
            CheckArrayArgs(sourceArray, sourceIndex, destinationArray, destinationIndex, length);

            int smax = sourceIndex + length;
            for (int s = sourceIndex, d = destinationIndex; s < smax; s++, d++)
                Transform(ref sourceArray[s], ref matrix, out destinationArray[d]);
        }

        public static void Transform(Vector3[] source, int sourceIndex, ref Quaternion rotation,
            Vector3[] destination, int destinationIndex, int length)
        {
            CheckArrayArgs(source, sourceIndex, destination, destinationIndex, length);

            int smax = sourceIndex + length;

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

            for (int i = sourceIndex, d = destinationIndex; i < smax; i++, d++)
            {
                destination[d] = new Vector3(
                    ((source[i].X * num1) + (source[i].Y * num2)) + (source[i].Z * num3),
                    ((source[i].X * num4) + (source[i].Y * num5)) + (source[i].Z * num6),
                    ((source[i].X * num7) + (source[i].Y * num8)) + (source[i].Z * num9));
            }
        }

        public static void Transform(Vector3[] source, ref Matrix matrix, Vector3[] destination)
        {
            Transform(source, 0, ref matrix, destination, 0, source.Length);
        }

        public static void Transform(Vector3[] source, ref Quaternion rotation, Vector3[] destination)
        {
            Transform(source, 0, ref rotation, destination, 0, source.Length);
        }

        private static void CheckArrayArgs(Vector3[] sourceArray, int sourceIndex, Vector3[] destinationArray,
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

        private static void CheckArrayArgs(Vector3[] sourceArray, Vector3[] destinationArray)
        {
            if (sourceArray == null)
                throw new ArgumentNullException("sourceArray");
            if (destinationArray == null)
                throw new ArgumentNullException("destinationArray");
            if (destinationArray.Length < sourceArray.Length)
                throw new ArgumentException("Destination is smaller than source", "destinationArray");
        }

        #endregion Transform

        #region TransformNormal

        public static Vector3 TransformNormal(Vector3 normal, Matrix matrix)
        {
            Vector3 result;
            TransformNormal(ref normal, ref matrix, out result);
            return result;
        }

        public static void TransformNormal(ref Vector3 normal, ref Matrix transform, out Vector3 result)
        {
            result = new Vector3(
                (normal.X * transform.M11) + (normal.Y * transform.M21) + (normal.Z * transform.M31),
                (normal.X * transform.M12) + (normal.Y * transform.M22) + (normal.Z * transform.M32),
                (normal.X * transform.M13) + (normal.Y * transform.M23) + (normal.Z * transform.M33));
        }

        public static void TransformNormal(Vector3[] sourceArray, int sourceIndex, ref Matrix matrix,
            Vector3[] destinationArray, int destinationIndex, int length)
        {
            CheckArrayArgs(sourceArray, sourceIndex, destinationArray, destinationIndex, length);

            int smax = sourceIndex + length;
            for (int s = sourceIndex, d = destinationIndex; s < smax; s++, d++)
                TransformNormal(ref sourceArray[s], ref matrix, out destinationArray[d]);
        }

        public static void TransformNormal(Vector3[] sourceArray, ref Matrix matrix, Vector3[] destinationArray)
        {
            CheckArrayArgs(sourceArray, destinationArray);

            for (int i = 0; i < sourceArray.Length; i++)
                TransformNormal(ref sourceArray[i], ref matrix, out destinationArray[i]);
        }

        #endregion TransformNormal

        #region Equality

        public bool Equals(Vector3 other)
        {
            return v4 == other.v4;
        }

        public static bool operator !=(Vector3 a, Vector3 b)
        {
            return a.v4 != b.v4;
        }

        public static bool operator ==(Vector3 a, Vector3 b)
        {
            return a.v4 == b.v4;
        }

        public override bool Equals(object obj)
        {
            return obj is Vector3 && ((Vector3)obj) == this;
        }

        public unsafe override int GetHashCode()
        {
                Vector4f f = v4;
                Vector4i i = *((Vector4i*)&f);
                i = i ^ i.Shuffle(ShuffleSel.Swap);
                i = i ^ i.Shuffle(ShuffleSel.RotateLeft);
                return i.X;
        }

        # endregion

        public override string ToString()
        {
            return string.Format("{{X:{0} Y:{1} Z:{2}}}", X, Y, Z);
        }
    }
}