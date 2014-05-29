using Mono.Simd;
using System;
using System.Runtime.InteropServices;

namespace Ormeli.Math
{
    [StructLayout(LayoutKind.Explicit, Pack = 0)]
    public struct Matrix
    {
        public static readonly int SizeInBytes = Marshal.SizeOf(typeof(Matrix));
        [FieldOffset(0)]
        internal Vector4f R0;

        [FieldOffset(16)]
        internal Vector4f R1;

        [FieldOffset(32)]
        internal Vector4f R2;

        [FieldOffset(48)]
        internal Vector4f R3;

        public float this[int a, int b]
        {
            get
            {
                switch (a)
                {
                    case 0:
                        return R0[b];

                    case 1:
                        return R1[b];

                    case 2:
                        return R2[b];

                    case 3:
                        return R3[b];

                    default:
                        return 0;
                }
            }
            set
            {
                switch (a)
                {
                    case 0:
                        R0[b] = value;
                        break;

                    case 1:
                        R1[b] = value;
                        break;

                    case 2:
                        R2[b] = value;
                        break;

                    case 3:
                        R3[b] = value;
                        break;
                }
            }
        }

        public Matrix(
            float m11, float m12, float m13, float m14,
            float m21, float m22, float m23, float m24,
            float m31, float m32, float m33, float m34,
            float m41, float m42, float m43, float m44)
        {
            R0 = new Vector4f(m11, m12, m13, m14);
            R1 = new Vector4f(m21, m22, m23, m24);
            R2 = new Vector4f(m31, m32, m33, m34);
            R3 = new Vector4f(m41, m42, m43, m44);
        }

        public Matrix(Vector4f r0, Vector4f r1, Vector4f r2, Vector4f r3)
        {
            R0 = r0;
            R1 = r1;
            R2 = r2;
            R3 = r3;
        }
        public Matrix(Vector4 r0, Vector4 r1, Vector4 r2, Vector4 r3)
        {
            R0 = r0._v4;
            R1 = r1._v4;
            R2 = r2._v4;
            R3 = r3._v4;
        }
        public Matrix(float[] values)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            if (values.Length != 16)
                throw new ArgumentOutOfRangeException("values",
                    "There must be sixteen and only sixteen input values for Matrix.");

            R0 = new Vector4f(values[0], values[1], values[2], values[3]);
            R1 = new Vector4f(values[4], values[5], values[6], values[7]);
            R2 = new Vector4f(values[7], values[9], values[10], values[11]);
            R3 = new Vector4f(values[12], values[13], values[14], values[15]);
        }

        public Matrix(float v)
        {
            R0 = new Vector4f(v);
            R1 = new Vector4f(v);
            R2 = new Vector4f(v);
            R3 = new Vector4f(v);
        }

        public Matrix(Matrix m)
        {
            R0 = m.R0;
            R1 = m.R1;
            R2 = m.R2;
            R3 = m.R3;
        }


        public float[] GetFloatArray()
        {
            return new[]
            {
                R0.X, R0.Y, R0.Z, R0.W,
                R1.X, R1.Y, R1.Z, R1.W,
                R2.X, R2.Y, R2.Z, R2.W,
                R3.X, R3.Y, R3.Z, R3.W
            };
        }

        #region Vector Properties

        public Vector3 Backward
        {
            get { return new Vector3(M31, M32, M33); }
            set
            {
                M31 = value.X;
                M32 = value.Y;
                M33 = value.Z;
            }
        }

        public Vector3 Down
        {
            get { return new Vector3(R1 ^ new Vector4f(-0.0f)); }
            set
            {
                Vector4f minus = value.v4 ^ new Vector4f(-0.0f);
                minus.W = M24;
                R1 = minus;
            }
        }

        public Vector3 Forward
        {
            get { return new Vector3(R2 ^ new Vector4f(-0.0f)); }
            set
            {
                Vector4f minus = value.v4 ^ new Vector4f(-0.0f);
                minus.W = M34;
                R2 = minus;
            }
        }

        public Vector3 Left
        {
            get { return new Vector3(R0 ^ new Vector4f(-0.0f)); }
            set
            {
                Vector4f minus = value.v4 ^ new Vector4f(-0.0f);
                minus.W = M14;
                R0 = minus;
            }
        }

        //See http://stevehazen.wordpress.com/2010/02/15/
        //matrix-basics-how-to-step-away-from-storing-an-orientation-as-3-angles/
        public Vector3 Right
        {
            get { return new Vector3(M11, M12, M13); }
            set
            {
                M11 = value.X;
                M12 = value.Y;
                M13 = value.Z;
            }
        }

        public Vector3 Up
        {
            get { return new Vector3(M21, M22, M23); }
            set
            {
                M21 = value.X;
                M22 = value.Y;
                M23 = value.Z;
            }
        }

        #endregion Vector Properties

        #region Static properties

        public static Matrix Identity
        {
            get
            {
                return new Matrix(
                    1f, 0f, 0f, 0f,
                    0f, 1f, 0f, 0f,
                    0f, 0f, 1f, 0f,
                    0f, 0f, 0f, 1f);
            }
        }

        #endregion Static properties

        #region Creation

        public static Matrix FromAxisAngle(Vector3 axis, float angle)
        {
            Matrix result;
            FromAxisAngle(ref axis, angle, out result);
            return result;
        }

        public static void FromAxisAngle(ref Vector3 axis, float angle, out Matrix result)
        {
            float x = axis.X;
            float y = axis.Y;
            float z = axis.Z;
            var sin = MathHelper.FastSin(angle);
            var cos = MathHelper.FastCos(angle);
            float xx = x * x;
            float yy = y * y;
            float zz = z * z;
            float xy = x * y;
            float xz = x * z;
            float yz = y * z;

            result = new Matrix(new Vector4f
            {
                X = xx + (cos * (1f - xx)),
                Y = (xy - (cos * xy)) + (sin * z),
                Z = (xz - (cos * xz)) - (sin * y),
                W = 0f
            }, new Vector4f
            {
                X = (xy - (cos * xy)) - (sin * z),
                Y = yy + (cos * (1f - yy)),
                Z = (yz - (cos * yz)) + (sin * x),
                W = 0f
            }, new Vector4f
            {
                X = (xz - (cos * xz)) + (sin * y),
                Y = (yz - (cos * yz)) - (sin * x),
                Z = zz + (cos * (1f - zz)),
                W = 0f
            }, new Vector4f(0, 0, 0, 1));
        }

        public static Matrix FromQuaternion(Quaternion quaternion)
        {
            Matrix result;
            FromQuaternion(ref quaternion, out result);
            return result;
        }

        public static void FromQuaternion(ref Quaternion quaternion, out Matrix result)
        {
            var m1 = new Matrix(
                quaternion.W, quaternion.Z, -quaternion.Y, quaternion.X,
                -quaternion.Z, quaternion.W, quaternion.X, quaternion.Y,
                quaternion.Y, -quaternion.X, quaternion.W, quaternion.Z,
                -quaternion.X, -quaternion.Y, -quaternion.Z, quaternion.W);

            var m2 = new Matrix(
                quaternion.W, quaternion.Z, -quaternion.Y, -quaternion.X,
                -quaternion.Z, quaternion.W, quaternion.X, -quaternion.Y,
                quaternion.Y, -quaternion.X, quaternion.W, -quaternion.Z,
                quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

            result = m1 * m2;
        }

        public static Matrix FromYawPitchRoll(float yaw, float pitch, float roll)
        {
            Matrix result;
            FromYawPitchRoll(yaw, pitch, roll, out result);
            return result;
        }

        public static void FromYawPitchRoll(float yaw, float pitch, float roll, out Matrix matrix)
        {
            matrix = new Matrix();
            // Get the cosine and sin of the yaw, pitch, and roll.
            var cYaw = MathHelper.FastCos(yaw);
            var cPitch = MathHelper.FastCos(pitch);
            var cRoll = MathHelper.FastCos(roll);

            var sYaw = MathHelper.FastSin(yaw);
            var sPitch = MathHelper.FastSin(pitch);
            var sRoll = MathHelper.FastSin(roll);

            // Calculate the yaw, pitch, roll rotation matrix.
            matrix[0, 0] = (cRoll * cYaw) + (sRoll * sPitch * sYaw);
            matrix[0, 1] = (sRoll * cPitch);
            matrix[0, 2] = (cRoll * -sYaw) + (sRoll * sPitch * cYaw);

            matrix[1, 0] = (-sRoll * cYaw) + (cRoll * sPitch * sYaw);
            matrix[1, 1] = (cRoll * cPitch);
            matrix[1, 2] = (sRoll * sYaw) + (cRoll * sPitch * cYaw);

            matrix[2, 0] = (cPitch * sYaw);
            matrix[2, 1] = -sPitch;
            matrix[2, 2] = (cPitch * cYaw);

            matrix[3, 3] = 1;
        }

        public static Matrix FromYawPitch(float yaw, float pitch)
        {
            Matrix result;
            FromYawPitchRoll(yaw, pitch, out result);
            return result;
        }

        public static void FromYawPitchRoll(float yaw, float pitch, out Matrix matrix)
        {
            matrix = new Matrix();
            // Get the cosine and sin of the yaw, pitch, and roll.
            float cPitch = MathHelper.FastCos(pitch),
                sPitch = MathHelper.FastSin(pitch),
                cYaw = MathHelper.FastCos(yaw),
                sYaw = MathHelper.FastSin(yaw);

            // Calculate the yaw, pitch, roll rotation matrix.
            matrix[0, 0] = cYaw;
            matrix[0, 2] = -sYaw;

            matrix[1, 0] = sPitch * sYaw;
            matrix[1, 1] = cPitch;
            matrix[1, 2] = sPitch * cYaw;

            matrix[2, 0] = cPitch * sYaw;
            matrix[2, 1] = -sPitch;
            matrix[2, 2] = cPitch * cYaw;

            matrix[3, 3] = 1;
        }

        public static void Billboard(ref Vector3 objectPosition, ref Vector3 cameraPosition,
            ref Vector3 cameraUpVector, ref Vector3 cameraForwardVector, out Matrix result)
        {
            Vector3 crossed;
            Vector3 final;
            Vector3 difference = cameraPosition - objectPosition;

            float lengthSq = difference.LengthSquared();
            if (lengthSq == 0)
                difference = -cameraForwardVector;
            else
                difference *= (float)(1.0 / System.Math.Sqrt(lengthSq));

            Vector3.Cross(ref cameraUpVector, ref difference, out crossed);
            crossed.Normalize();
            Vector3.Cross(ref difference, ref crossed, out final);

            result = new Matrix
            {
                M11 = crossed.X,
                M12 = crossed.Y,
                M13 = crossed.Z,
                M14 = 0.0f,
                M21 = final.X,
                M22 = final.Y,
                M23 = final.Z,
                M24 = 0.0f,
                M31 = difference.X,
                M32 = difference.Y,
                M33 = difference.Z,
                M34 = 0.0f,
                M41 = objectPosition.X,
                M42 = objectPosition.Y,
                M43 = objectPosition.Z,
                M44 = 1.0f
            };
        }

        /// <summary>
        ///     Creates a right-handed spherical billboard that rotates around a specified object position.
        /// </summary>
        /// <param name="objectPosition">The position of the object around which the billboard will rotate.</param>
        /// <param name="cameraPosition">The position of the camera.</param>
        /// <param name="cameraUpVector">The up vector of the camera.</param>
        /// <param name="cameraForwardVector">The forward vector of the camera.</param>
        /// <returns>The created billboard matrix.</returns>
        public static Matrix Billboard(Vector3 objectPosition, Vector3 cameraPosition, Vector3 cameraUpVector,
            Vector3 cameraForwardVector)
        {
            Matrix result;
            Billboard(ref objectPosition, ref cameraPosition, ref cameraUpVector, ref cameraForwardVector, out result);
            return result;
        }
        public static void LookAt(ref Vector3 eye, ref Vector3 target, ref Vector3 up, out Matrix result)
        {
            Vector3 xaxis, yaxis, zaxis;
            Vector3.Subtract(ref eye, ref target, out zaxis); zaxis.Normalize();
            Vector3.Cross(ref up, ref zaxis, out xaxis); xaxis.Normalize();
            Vector3.Cross(ref zaxis, ref xaxis, out yaxis);

            result = Identity;
            result.M11 = xaxis.X; result.M21 = xaxis.Y; result.M31 = xaxis.Z;
            result.M12 = yaxis.X; result.M22 = yaxis.Y; result.M32 = yaxis.Z;
            result.M13 = zaxis.X; result.M23 = zaxis.Y; result.M33 = zaxis.Z;

            result.M41 = -Vector3.Dot(xaxis, eye);
            result.M42 = -Vector3.Dot(yaxis, eye);
            result.M43 = -Vector3.Dot(zaxis, eye);
        }
        public static Matrix LookAt(Vector3 eye, Vector3 target, Vector3 up)
        {
            Matrix result;
            LookAt(ref eye, ref target, ref up, out result);
            return result;
        }

        public static void LookAt(ref Vector3 eye, ref Vector3 target, out Matrix result)
        {           
            var zaxis = Vector3.Normalize(Vector3.Subtract(eye, target));
            result = new Matrix {M13 = zaxis.X, M23 = zaxis.Y, M33 = zaxis.Z, M43 = -Vector3.Dot(zaxis, eye), M44 = 1};
        }
        public static Matrix LookAt(Vector3 eye, Vector3 target)
        {
            Matrix result;
            LookAt(ref eye, ref target, out result);
            return result;
        }

        /// <summary>
        /// Creates a right-handed, orthographic projection matrix.
        /// </summary>
        /// <param name="width">Width of the viewing volume.</param>
        /// <param name="height">Height of the viewing volume.</param>
        /// <param name="znear">Minimum z-value of the viewing volume.</param>
        /// <param name="zfar">Maximum z-value of the viewing volume.</param>
        /// <param name="result">When the method completes, contains the created projection matrix.</param>
        public static void Ortho(float width, float height, float znear, float zfar, out Matrix result)
        {
            var zRange = 1.0f / (zfar - znear);

            result = new Matrix
            {
                M11 = 2.0f / width,
                M22 = 2.0f/ height,
                M33 = -zRange,
                M43 = -znear*zRange,
                M44 = 1
            };
        }

        /// <summary>
        /// Creates a right-handed, orthographic projection matrix.
        /// </summary>
        /// <param name="width">Width of the viewing volume.</param>
        /// <param name="height">Height of the viewing volume.</param>
        /// <param name="znear">Minimum z-value of the viewing volume.</param>
        /// <param name="zfar">Maximum z-value of the viewing volume.</param>
        /// <returns>The created projection matrix.</returns>
        public static Matrix Ortho(float width, float height, float znear, float zfar)
        {
            Matrix result;
            Ortho(width, height, znear, zfar, out result);
            return result;
        }

        /// <summary>
        /// Creates a right-handed, customized orthographic projection matrix.
        /// </summary>
        /// <param name="left">Minimum x-value of the viewing volume.</param>
        /// <param name="right">Maximum x-value of the viewing volume.</param>
        /// <param name="bottom">Minimum y-value of the viewing volume.</param>
        /// <param name="top">Maximum y-value of the viewing volume.</param>
        /// <param name="znear">Minimum z-value of the viewing volume.</param>
        /// <param name="zfar">Maximum z-value of the viewing volume.</param>
        /// <param name="result">When the method completes, contains the created projection matrix.</param>
        public static void OrthoOffCenter(float left, float right, float bottom, float top, float znear, float zfar, out Matrix result)
        {
            float zRange = 1.0f / (zfar - znear);

            result = Identity;
            result.M11 = 2.0f / (right - left);
            result.M22 = 2.0f / (top - bottom);
            result.M33 = -zRange;
            result.M41 = (left + right) / (left - right);
            result.M42 = (top + bottom) / (bottom - top);
            result.M43 = -znear * zRange;
        }

        /// <summary>
        /// Creates a right-handed, customized orthographic projection matrix.
        /// </summary>
        /// <param name="left">Minimum x-value of the viewing volume.</param>
        /// <param name="right">Maximum x-value of the viewing volume.</param>
        /// <param name="bottom">Minimum y-value of the viewing volume.</param>
        /// <param name="top">Maximum y-value of the viewing volume.</param>
        /// <param name="znear">Minimum z-value of the viewing volume.</param>
        /// <param name="zfar">Maximum z-value of the viewing volume.</param>
        /// <returns>The created projection matrix.</returns>
        public static Matrix OrthoOffCenter(float left, float right, float bottom, float top, float znear, float zfar)
        {
            Matrix result;
            OrthoOffCenter(left, right, bottom, top, znear, zfar, out result);
            return result;
        }
        /// <summary>
        /// Creates a right-handed, customized perspective projection matrix.
        /// </summary>
        /// <param name="left">Minimum x-value of the viewing volume.</param>
        /// <param name="right">Maximum x-value of the viewing volume.</param>
        /// <param name="bottom">Minimum y-value of the viewing volume.</param>
        /// <param name="top">Maximum y-value of the viewing volume.</param>
        /// <param name="znear">Minimum z-value of the viewing volume.</param>
        /// <param name="zfar">Maximum z-value of the viewing volume.</param>
        /// <param name="result">Matrix</param>
        public static void PerspectiveOffCenter(float left, float right, float bottom, float top, float znear, float zfar, out Matrix result)
        {
            var zRange = zfar / (zfar - znear);
            result = new Matrix
            {
                M11 = 2.0f*znear/(right - left),
                M22 = 2.0f*znear/(top - bottom),
                M31 = -(left + right)/(left - right),
                M32 = -(top + bottom)/(bottom - top),
                M33 = -zRange,
                M34 = -1.0f,
                M43 = -znear*zRange,
                M44 = 1
            };
        }

        /// <summary>
        /// Creates a right-handed, customized perspective projection matrix.
        /// </summary>
        /// <param name="left">Minimum x-value of the viewing volume.</param>
        /// <param name="right">Maximum x-value of the viewing volume.</param>
        /// <param name="bottom">Minimum y-value of the viewing volume.</param>
        /// <param name="top">Maximum y-value of the viewing volume.</param>
        /// <param name="znear">Minimum z-value of the viewing volume.</param>
        /// <param name="zfar">Maximum z-value of the viewing volume.</param>
        /// <returns>The created projection matrix.</returns>
        public static Matrix PerspectiveOffCenter(float left, float right, float bottom, float top, float znear, float zfar)
        {
            Matrix result;
            PerspectiveOffCenter(left, right, bottom, top, znear, zfar, out result);
            return result;
        }

        /// <summary>
        /// Creates a right-handed, perspective projection matrix based on a field of view.
        /// </summary>
        /// <param name="fov">Field of view in the y direction, in radians.</param>
        /// <param name="aspect">Aspect ratio, defined as view space width divided by height.</param>
        /// <param name="znear">Minimum z-value of the viewing volume.</param>
        /// <param name="zfar">Maximum z-value of the viewing volume.</param>
        /// <param name="result">Matrix</param>
        public static void PerspectiveFov(float fov, float aspect, float znear, float zfar, out Matrix result)
        {
            var yScale = (float)(1.0 / System.Math.Tan(fov * 0.5f));
            var zRange = zfar / (zfar - znear);
            result = new Matrix
            {
                M11 = yScale / aspect,
                M22 = yScale,
                M33 = -zRange,
                M34 = -1.0f,
                M43 = -znear * zRange,
                M44 = 1
            };
        }

        /// <summary>
        /// Creates a right-handed, perspective projection matrix based on a field of view.
        /// </summary>
        /// <param name="fov">Field of view in the y direction, in radians.</param>
        /// <param name="aspect">Aspect ratio, defined as view space width divided by height.</param>
        /// <param name="znear">Minimum z-value of the viewing volume.</param>
        /// <param name="zfar">Maximum z-value of the viewing volume.</param>
        /// <returns>The created projection matrix.</returns>
        public static Matrix PerspectiveFov(float fov, float aspect, float znear, float zfar)
        {
            Matrix result;
            PerspectiveFov(fov, aspect, znear, zfar, out result);
            return result;
        }
        /// <summary>
        ///     Builds a matrix that can be used to reflect vectors about a plane.
        /// </summary>
        /// <param name="plane">The plane for which the reflection occurs. This parameter is assumed to be normalized.</param>
        /// <param name="result">When the method completes, contains the reflection matrix.</param>
        public static void Reflection(ref Plane plane, out Matrix result)
        {
            float x = plane.Normal.X;
            float y = plane.Normal.Y;
            float z = plane.Normal.Z;
            float x2 = -2.0f * x;
            float y2 = -2.0f * y;
            float z2 = -2.0f * z;

            result = new Matrix
            {
                M11 = (x2 * x) + 1.0f,
                M12 = y2 * x,
                M13 = z2 * x,
                M14 = 0.0f,
                M21 = x2 * y,
                M22 = (y2 * y) + 1.0f,
                M23 = z2 * y,
                M24 = 0.0f,
                M31 = x2 * z,
                M32 = y2 * z,
                M33 = (z2 * z) + 1.0f,
                M34 = 0.0f,
                M41 = x2 * plane.D,
                M42 = y2 * plane.D,
                M43 = z2 * plane.D,
                M44 = 1.0f
            };
        }

        /// <summary>
        ///     Builds a matrix that can be used to reflect vectors about a plane.
        /// </summary>
        /// <param name="plane">The plane for which the reflection occurs. This parameter is assumed to be normalized.</param>
        /// <returns>The reflection matrix.</returns>
        public static Matrix Reflection(Plane plane)
        {
            Matrix result;
            Reflection(ref plane, out result);
            return result;
        }

        /// <summary>
        ///     Creates a matrix that flattens geometry into a shadow.
        /// </summary>
        /// <param name="light">
        ///     The light direction. If the W component is 0, the light is directional light; if the
        ///     W component is 1, the light is a point light.
        /// </param>
        /// <param name="plane">
        ///     The plane onto which to project the geometry as a shadow. This parameter is assumed to be
        ///     normalized.
        /// </param>
        /// <param name="result">When the method completes, contains the shadow matrix.</param>
        public static void Shadow(ref Vector4 light, ref Plane plane, out Matrix result)
        {
            float dot = (plane.Normal.X * light.X) + (plane.Normal.Y * light.Y) + (plane.Normal.Z * light.Z) +
                        (plane.D * light.W);
            float x = -plane.Normal.X;
            float y = -plane.Normal.Y;
            float z = -plane.Normal.Z;
            float d = -plane.D;

            result = new Matrix
            {
                M11 = (x * light.X) + dot,
                M21 = y * light.X,
                M31 = z * light.X,
                M41 = d * light.X,
                M12 = x * light.Y,
                M22 = (y * light.Y) + dot,
                M32 = z * light.Y,
                M42 = d * light.Y,
                M13 = x * light.Z,
                M23 = y * light.Z,
                M33 = (z * light.Z) + dot,
                M43 = d * light.Z,
                M14 = x * light.W,
                M24 = y * light.W,
                M34 = z * light.W,
                M44 = (d * light.W) + dot
            };
        }

        /// <summary>
        ///     Creates a matrix that flattens geometry into a shadow.
        /// </summary>
        /// <param name="light">
        ///     The light direction. If the W component is 0, the light is directional light; if the
        ///     W component is 1, the light is a point light.
        /// </param>
        /// <param name="plane">
        ///     The plane onto which to project the geometry as a shadow. This parameter is assumed to be
        ///     normalized.
        /// </param>
        /// <returns>The shadow matrix.</returns>
        public static Matrix Shadow(Vector4 light, Plane plane)
        {
            Matrix result;
            Shadow(ref light, ref plane, out result);
            return result;
        }

        public static Matrix RotationX(float radians)
        {
            Matrix result;
            RotationX(radians, out result);
            return result;
        }

        public static void RotationX(float radians, out Matrix result)
        {
            var cos = MathHelper.FastCos(radians);
            var sin = MathHelper.FastSin(radians);

            result = new Matrix { M11 = 1.0f, M22 = cos, M23 = sin, M32 = -sin, M33 = cos, M44 = 1.0f };
        }

        public static Matrix RotationY(float radians)
        {
            Matrix result;
            RotationY(radians, out result);
            return result;
        }

        public static void RotationY(float radians, out Matrix result)
        {
            var cos = MathHelper.FastCos(radians);
            var sin = MathHelper.FastSin(radians);

            result = new Matrix { M11 = cos, M13 = -sin, M22 = 1.0f, M31 = sin, M33 = cos, M44 = 1.0f };
        }

        public static Matrix RotationZ(float radians)
        {
            Matrix result;
            RotationZ(radians, out result);
            return result;
        }

        public static void RotationZ(float radians, out Matrix result)
        {
            var cos = MathHelper.FastCos(radians);
            var sin = MathHelper.FastSin(radians);

            result = new Matrix { M11 = cos, M12 = sin, M21 = -sin, M22 = cos, M33 = 1.0f, M44 = 1.0f };
        }

        public static Matrix Scale(float scale)
        {
            return Scale(scale, scale, scale);
        }

        public static void Scale(float scale, out Matrix result)
        {
            Scale(scale, scale, scale, out result);
        }

        public static Matrix Scale(float xScale, float yScale, float zScale)
        {
            Matrix result;
            Scale(xScale, yScale, zScale, out result);
            return result;
        }

        public static void Scale(float xScale, float yScale, float zScale, out Matrix result)
        {
            var scale = new Vector3(xScale, yScale, zScale);
            Scale(ref scale, out result);
        }

        public static Matrix Scale(Vector3 scales)
        {
            Matrix result;
            Scale(ref scales, out result);
            return result;
        }

        public static void Scale(ref Vector3 scales, out Matrix result)
        {
            result = new Matrix { M11 = scales.X, M22 = scales.Y, M33 = scales.Z, M44 = 1.0f };
        }

        public static Matrix Translation(float xPosition, float yPosition, float zPosition)
        {
            Matrix result;
            Translation(xPosition, yPosition, zPosition, out result);
            return result;
        }

        public static void Translation(float xPosition, float yPosition, float zPosition, out Matrix result)
        {
            var position = new Vector3(xPosition, yPosition, zPosition);
            Translation(ref position, out result);
        }

        public static Matrix Translation(Vector3 position)
        {
            Matrix result;
            Translation(ref position, out result);
            return result;
        }

        public static void Translation(ref Vector3 position, out Matrix result)
        {
            result = new Matrix(0)
            {
                M11 = 1.0f,
                M22 = 1.0f,
                M33 = 1.0f,
                M41 = position.X,
                M42 = position.Y,
                M43 = position.Z,
                M44 = 1.0f
            };
        }

        public static Matrix World(Vector3 position, Vector3 forward, Vector3 up)
        {
            Matrix result;
            World(ref position, ref forward, ref up, out result);
            return result;
        }

        public static void World(ref Vector3 position, ref Vector3 forward, ref Vector3 up, out Matrix result)
        {
            Vector3 x, y, z;

            Vector3.Cross(ref forward, ref up, out x);
            Vector3.Cross(ref x, ref forward, out y);
            Vector3.Normalize(ref forward, out z);

            x.Normalize();
            y.Normalize();

            result = new Matrix(Translation(position)) { Right = x, Up = y, Forward = z, M44 = 1.0f };
        }

        #endregion Creation

        #region Arithmetic

        public static Matrix Add(Matrix matrix1, Matrix matrix2)
        {
            Add(ref matrix1, ref matrix2, out matrix1);
            return matrix1;
        }

        public static void Add(ref Matrix matrix1, ref Matrix matrix2, out Matrix result)
        {
            result.R0 = matrix1.R0 + matrix2.R0;
            result.R1 = matrix1.R1 + matrix2.R1;
            result.R2 = matrix1.R2 + matrix2.R2;
            result.R3 = matrix1.R3 + matrix2.R3;
        }

        public static Matrix Divide(Matrix matrix1, Matrix matrix2)
        {
            Divide(ref matrix1, ref matrix2, out matrix1);
            return matrix1;
        }

        public static void Divide(ref Matrix matrix1, ref Matrix matrix2, out Matrix result)
        {
            result.R0 = matrix1.R0 * matrix2.R0;
            result.R1 = matrix1.R1 * matrix2.R1;
            result.R2 = matrix1.R2 * matrix2.R2;
            result.R3 = matrix1.R3 * matrix2.R3;
        }

        public static Matrix Divide(Matrix matrix1, float divider)
        {
            Divide(ref matrix1, divider, out matrix1);
            return matrix1;
        }

        public static void Divide(ref Matrix matrix1, float divider, out Matrix result)
        {
            var divisor = new Vector4f(divider);
            result.R0 = matrix1.R0 / divisor;
            result.R1 = matrix1.R1 / divisor;
            result.R2 = matrix1.R2 / divisor;
            result.R3 = matrix1.R3 / divisor;
        }

        public static Matrix Multiply(Matrix matrix1, Matrix matrix2)
        {
            //sse version only sets the result when the calculation is complete
            Multiply(ref matrix1, ref matrix2, out matrix1);
            return matrix1;
        }

        public static void Multiply(ref Matrix left, ref Matrix right, out Matrix result)
        {
           var t1 = (left.R0.Shuffle(ShuffleSel.ExpandX) * right.R0) + (left.R0.Shuffle(ShuffleSel.ExpandY) * right.R1);
            var t2 = (left.R0.Shuffle(ShuffleSel.ExpandZ) * right.R2) + (left.R0.Shuffle(ShuffleSel.ExpandW) * right.R3);
            var out0 = t1 + t2;

            t1 = (left.R1.Shuffle(ShuffleSel.ExpandX) * right.R0) + (left.R1.Shuffle(ShuffleSel.ExpandY) * right.R1);
            t2 = (left.R1.Shuffle(ShuffleSel.ExpandZ) * right.R2) + (left.R1.Shuffle(ShuffleSel.ExpandW) * right.R3);
            var out1 = t1 + t2;

            t1 = (left.R2.Shuffle(ShuffleSel.ExpandX) * right.R0) + (left.R2.Shuffle(ShuffleSel.ExpandY) * right.R1);
            t2 = (left.R2.Shuffle(ShuffleSel.ExpandZ) * right.R2) + (left.R2.Shuffle(ShuffleSel.ExpandW) * right.R3);
            var out2 = t1 + t2;

            t1 = (left.R3.Shuffle(ShuffleSel.ExpandX) * right.R0) + (left.R3.Shuffle(ShuffleSel.ExpandY) * right.R1);
            t2 = (left.R3.Shuffle(ShuffleSel.ExpandZ) * right.R2) + (left.R3.Shuffle(ShuffleSel.ExpandW) * right.R3);
            var out3 = t1 + t2;

            result =  new Matrix(out0, out1, out2, out3);
        }

        public static Matrix Multiply(Matrix matrix1, float scaleFactor)
        {
            Multiply(ref matrix1, scaleFactor, out matrix1);
            return matrix1;
        }

        public static void Multiply(ref Matrix matrix1, float scaleFactor, out Matrix result)
        {
            var scale = new Vector4f(scaleFactor);
            result.R0 = matrix1.R0 * scale;
            result.R1 = matrix1.R1 * scale;
            result.R2 = matrix1.R2 * scale;
            result.R3 = matrix1.R3 * scale;
        }

        public static Matrix Negate(Matrix matrix)
        {
            matrix.Negate();
            return matrix;
        }

        public static void Negate(ref Matrix matrix, out Matrix result)
        {
            matrix.Negate();
            result = matrix;
        }

        public void Negate()
        {
            R0 *= Vector4f.MinusOne;
            R1 *= Vector4f.MinusOne;
            R2 *= Vector4f.MinusOne;
            R3 *= Vector4f.MinusOne;
        }

        public static Matrix Subtract(Matrix matrix1, Matrix matrix2)
        {
            Subtract(ref matrix1, ref matrix2, out matrix1);
            return matrix1;
        }

        public static void Subtract(ref Matrix matrix1, ref Matrix matrix2, out Matrix result)
        {
            result.R0 = matrix1.R0 - matrix2.R0;
            result.R1 = matrix1.R1 - matrix2.R1;
            result.R2 = matrix1.R2 - matrix2.R2;
            result.R3 = matrix1.R3 - matrix2.R3;
        }

        #endregion Arithmetic

        #region Operator overloads

        public static Matrix operator -(Matrix matrix1, Matrix matrix2)
        {
            Subtract(ref matrix1, ref matrix2, out matrix1);
            return matrix1;
        }

        public static Matrix operator -(Matrix matrix)
        {
            Negate(ref matrix, out matrix);
            return matrix;
        }

        public static Matrix operator *(Matrix matrix1, Matrix matrix2)
        {
            //sse version only sets the result when the calculation is complete
            Multiply(ref matrix1, ref matrix2, out matrix1);
            return matrix1;
        }

        public static Matrix operator *(Matrix matrix, float scaleFactor)
        {
            Multiply(ref matrix, scaleFactor, out matrix);
            return matrix;
        }

        public static Matrix operator *(float scaleFactor, Matrix matrix)
        {
            Multiply(ref matrix, scaleFactor, out matrix);
            return matrix;
        }

        public static Matrix operator /(Matrix matrix, float divider)
        {
            Divide(ref matrix, divider, out matrix);
            return matrix;
        }

        public static Matrix operator /(Matrix matrix1, Matrix matrix2)
        {
            Divide(ref matrix1, ref matrix2, out matrix1);
            return matrix1;
        }

        public static Matrix operator +(Matrix matrix1, Matrix matrix2)
        {
            Add(ref matrix1, ref matrix2, out matrix1);
            return matrix1;
        }

        #endregion Operator overloads

        #region Other maths

        public static Matrix Invert(Matrix matrix)
        {
            Matrix result;
            Invert(ref matrix, out result);
            return result;
        }

        public static void Invert(ref Matrix value, out Matrix result)
        {
            float b0 = (value.M31 * value.M42) - (value.M32 * value.M41);
            float b1 = (value.M31 * value.M43) - (value.M33 * value.M41);
            float b2 = (value.M34 * value.M41) - (value.M31 * value.M44);
            float b3 = (value.M32 * value.M43) - (value.M33 * value.M42);
            float b4 = (value.M34 * value.M42) - (value.M32 * value.M44);
            float b5 = (value.M33 * value.M44) - (value.M34 * value.M43);

            float d11 = value.M22 * b5 + value.M23 * b4 + value.M24 * b3;
            float d12 = value.M21 * b5 + value.M23 * b2 + value.M24 * b1;
            float d13 = value.M21 * -b4 + value.M22 * b2 + value.M24 * b0;
            float d14 = value.M21 * b3 + value.M22 * -b1 + value.M23 * b0;

            float det = value.M11 * d11 - value.M12 * d12 + value.M13 * d13 - value.M14 * d14;
            if (det == 0.0f)
            {
                result = new Matrix(0);
                return;
            }

            det = 1f / det;

            float a0 = (value.M11 * value.M22) - (value.M12 * value.M21);
            float a1 = (value.M11 * value.M23) - (value.M13 * value.M21);
            float a2 = (value.M14 * value.M21) - (value.M11 * value.M24);
            float a3 = (value.M12 * value.M23) - (value.M13 * value.M22);
            float a4 = (value.M14 * value.M22) - (value.M12 * value.M24);
            float a5 = (value.M13 * value.M24) - (value.M14 * value.M23);

            float d21 = value.M12 * b5 + value.M13 * b4 + value.M14 * b3;
            float d22 = value.M11 * b5 + value.M13 * b2 + value.M14 * b1;
            float d23 = value.M11 * -b4 + value.M12 * b2 + value.M14 * b0;
            float d24 = value.M11 * b3 + value.M12 * -b1 + value.M13 * b0;

            float d31 = value.M42 * a5 + value.M43 * a4 + value.M44 * a3;
            float d32 = value.M41 * a5 + value.M43 * a2 + value.M44 * a1;
            float d33 = value.M41 * -a4 + value.M42 * a2 + value.M44 * a0;
            float d34 = value.M41 * a3 + value.M42 * -a1 + value.M43 * a0;

            float d41 = value.M32 * a5 + value.M33 * a4 + value.M34 * a3;
            float d42 = value.M31 * a5 + value.M33 * a2 + value.M34 * a1;
            float d43 = value.M31 * -a4 + value.M32 * a2 + value.M34 * a0;
            float d44 = value.M31 * a3 + value.M32 * -a1 + value.M33 * a0;

            result = new Matrix
            {
                M11 = +d11 * det,
                M12 = -d21 * det,
                M13 = +d31 * det,
                M14 = -d41 * det,
                M21 = -d12 * det,
                M22 = +d22 * det,
                M23 = -d32 * det,
                M24 = +d42 * det,
                M31 = +d13 * det,
                M32 = -d23 * det,
                M33 = +d33 * det,
                M34 = -d43 * det,
                M41 = -d14 * det,
                M42 = +d24 * det,
                M43 = -d34 * det,
                M44 = +d44 * det
            };
        }

        public static Matrix Lerp(Matrix matrix1, Matrix matrix2, float amount)
        {
            Lerp(ref matrix1, ref matrix2, amount, out matrix1);
            return matrix1;
        }

        public static void Lerp(ref Matrix matrix1, ref Matrix matrix2, float amount, out Matrix result)
        {
            result.R0 = matrix1.R0 + amount * (matrix2.R0 - matrix1.R0);
            result.R1 = matrix1.R1 + amount * (matrix2.R1 - matrix1.R1);
            result.R2 = matrix1.R2 + amount * (matrix2.R2 - matrix1.R2);
            result.R3 = matrix1.R3 + amount * (matrix2.R3 - matrix1.R3);
        }

        public static Matrix Transform(Matrix value, Quaternion rotation)
        {
            Matrix result;
            Transform(ref value, ref rotation, out result);
            return result;
        }

        public static void Transform(ref Matrix value, ref Quaternion rotation, out Matrix result)
        {
            float x2 = rotation.X + rotation.X;
            float y2 = rotation.Y + rotation.Y;
            float z2 = rotation.Z + rotation.Z;

            float a = (1f - rotation.Y * y2) - rotation.Z * z2;
            float b = rotation.X * y2 - rotation.W * z2;
            float c = rotation.X * z2 + rotation.W * y2;
            float d = rotation.X * y2 + rotation.W * z2;
            float e = (1f - rotation.X * x2) - rotation.Z * z2;
            float f = rotation.Y * z2 - rotation.W * x2;
            float g = rotation.X * z2 - rotation.W * y2;
            float h = rotation.Y * z2 + rotation.W * x2;
            float i = (1f - rotation.X * x2) - rotation.Y * y2;

            result = new Matrix(
                new Vector4f(
                    ((value.R0.X * a) + (value.R0.Y * b)) + (value.R0.Z * c),
                    ((value.R0.X * d) + (value.R0.Y * e)) + (value.R0.Z * f),
                    ((value.R0.X * g) + (value.R0.Y * h)) + (value.R0.Z * i),
                    value.R0.W),
                new Vector4f(
                    ((value.R1.X * a) + (value.R1.Y * b)) + (value.R1.Z * c),
                    ((value.R1.X * d) + (value.R1.Y * e)) + (value.R1.Z * f),
                    ((value.R1.X * g) + (value.R1.Y * h)) + (value.R1.Z * i),
                    value.R1.W),
                new Vector4f(
                    ((value.R2.X * a) + (value.R2.Y * b)) + (value.R2.Z * c),
                    ((value.R2.X * d) + (value.R2.Y * e)) + (value.R2.Z * f),
                    ((value.R2.X * g) + (value.R2.Y * h)) + (value.R2.Z * i),
                    value.R2.W),
                new Vector4f(
                    ((value.R3.X * a) + (value.R3.Y * b)) + (value.R3.Z * c),
                    ((value.R3.X * d) + (value.R3.Y * e)) + (value.R3.Z * f),
                    ((value.R3.X * g) + (value.R3.Y * h)) + (value.R3.Z * i),
                    value.R3.W));
        }

        public Matrix Transform(Quaternion rotation)
        {
            Matrix res;
            Transform(ref this, ref rotation, out res);
            return res;
        }

        public static Matrix Transpose(Matrix matrix)
        {
            Vector4f xmm0 = matrix.R0, xmm1 = matrix.R1, xmm2 = matrix.R2, xmm3 = matrix.R3;
            Vector4f xmm4 = xmm0;
            xmm0 = xmm0.InterleaveLow(xmm2);
            xmm4 = xmm4.InterleaveHigh(xmm2);
            xmm2 = xmm1;
            xmm1 = xmm1.InterleaveLow(xmm3);
            xmm2 = xmm2.InterleaveHigh(xmm3);
            xmm3 = xmm0;
            xmm0 = xmm0.InterleaveLow(xmm1);
            xmm3 = xmm3.InterleaveHigh(xmm1);
            xmm1 = xmm4;
            xmm1 = xmm1.InterleaveLow(xmm2);
            xmm4 = xmm4.InterleaveHigh(xmm2);

            return new Matrix(xmm0, xmm3, xmm1, xmm4);
        }

        public static void Transpose(ref Matrix matrix, out Matrix result)
        {
            result = Transpose(matrix);
        }

        public bool Decompose(out Vector3 scale, out Quaternion rotation, out Vector3 translation)
        {
            //Source: Unknown
            //References: http://www.gamedev.net/community/forums/topic.asp?topic_id=441695

            //Get the translation.
            translation = new Vector3 { X = M41, Y = M42, Z = this.M43 };

            //Scaling is the length of the rows.
            scale = new Vector3
            {
                X = MathHelper.FastSqrt((M11 * M11) + (M12 * M12) + (M13 * M13)),
                Y = MathHelper.FastSqrt((M21 * M21) + (M22 * M22) + (M23 * M23)),
                Z = MathHelper.FastSqrt((M31 * M31) + (M32 * M32) + (M33 * M33))
            };

            //If any of the scaling factors are zero, than the rotation matrix can not exist.
            if (scale.X == 0 ||
                scale.Y == 0 ||
                scale.Z == 0)
            {
                rotation = Quaternion.Identity;
                return false;
            }

            //The rotation is the left over matrix after dividing out the scaling.
            var rotationmatrix = new Matrix
            {
                M11 = M11 / scale.X,
                M12 = M12 / scale.X,
                M13 = M13 / scale.X,
                M21 = M21 / scale.Y,
                M22 = M22 / scale.Y,
                M23 = M23 / scale.Y,
                M31 = M31 / scale.Z,
                M32 = M32 / scale.Z,
                M33 = M33 / scale.Z,
                M44 = 1f
            };

            Quaternion.CreateFromRotationMatrix(ref rotationmatrix, out rotation);
            return true;
        }

        public float Determinant()
        {
            return
                M11 * M22 * M33 * M44 - M11 * M22 * M34 * M43 + M11 * M23 * M34 * M42 - M11 * M23 * M32 * M44 +
                M11 * M24 * M32 * M43 - M11 * M24 * M33 * M42 - M12 * M23 * M34 * M41 + M12 * M23 * M31 * M44 -
                M12 * M24 * M31 * M43 + M12 * M24 * M33 * M41 - M12 * M21 * M33 * M44 + M12 * M21 * M34 * M43 +
                M13 * M24 * M31 * M42 - M13 * M24 * M32 * M41 + M13 * M21 * M32 * M44 - M13 * M21 * M34 * M42 +
                M13 * M22 * M34 * M41 - M13 * M22 * M31 * M44 - M14 * M21 * M32 * M43 + M14 * M21 * M33 * M42 -
                M14 * M22 * M33 * M41 + M14 * M22 * M31 * M43 - M14 * M23 * M31 * M42 + M14 * M23 * M32 * M41;
        }

        #endregion Other maths

        #region Equality

        public float M11
        {
            get { return R0.X; }
            set { R0.X = value; }
        }

        public float M12
        {
            get { return R0.Y; }
            set { R0.Y = value; }
        }

        public float M13
        {
            get { return R0.Z; }
            set { R0.Z = value; }
        }

        public float M14
        {
            get { return R0.W; }
            set { R0.W = value; }
        }

        public float M21
        {
            get { return R1.X; }
            set { R1.X = value; }
        }

        public float M22
        {
            get { return R1.Y; }
            set { R1.Y = value; }
        }

        public float M23
        {
            get { return R1.Z; }
            set { R1.Z = value; }
        }

        public float M24
        {
            get { return R1.W; }
            set { R1.W = value; }
        }

        public float M31
        {
            get { return R2.X; }
            set { R2.X = value; }
        }

        public float M32
        {
            get { return R2.Y; }
            set { R2.Y = value; }
        }

        public float M33
        {
            get { return R2.Z; }
            set { R2.Z = value; }
        }

        public float M34
        {
            get { return R2.W; }
            set { R2.W = value; }
        }

        public float M41
        {
            get { return R3.X; }
            set { R3.X = value; }
        }

        public float M42
        {
            get { return R3.Y; }
            set { R3.Y = value; }
        }

        public float M43
        {
            get { return R3.Z; }
            set { R3.Z = value; }
        }

        public float M44
        {
            get { return R3.W; }
            set { R3.W = value; }
        }

        public static bool operator !=(Matrix a, Matrix b)
        {
            return a.R0 != b.R0 || a.R1 != b.R1 || a.R2 != b.R2 || a.R3 != b.R3;
        }

        public static bool operator ==(Matrix a, Matrix b)
        {
            return a.R0 == b.R0 && a.R1 == b.R1 && a.R2 == b.R2 && a.R3 == b.R3;
        }

        public bool Equals(Matrix other)
        {
            return R0 == other.R0 && R1 == other.R1 && R2 == other.R2 && R3 == other.R3;
        }

        public override bool Equals(object obj)
        {
            return obj is Matrix && ((Matrix)obj) == this;
        }

        public override unsafe int GetHashCode()
        {
            var f = R0;
            var i = *((Vector4i*) &f);
            i = i ^ i.Shuffle(ShuffleSel.Swap);
            i = i ^ i.Shuffle(ShuffleSel.RotateLeft);
            f = R1;
            var j = *((Vector4i*) &f);
            j = j ^ j.Shuffle(ShuffleSel.Swap);
            j = j ^ j.Shuffle(ShuffleSel.RotateLeft);
            f = R2;
            var k = *((Vector4i*) &f);
            k = k ^ k.Shuffle(ShuffleSel.Swap);
            k = k ^ k.Shuffle(ShuffleSel.RotateLeft);
            f = R3;
            var l = *((Vector4i*) &f);
            l = l ^ l.Shuffle(ShuffleSel.Swap);
            l = l ^ l.Shuffle(ShuffleSel.RotateLeft);
            return (i ^ j ^ k ^ l).X;
        }

        # endregion

        public override string ToString()
        {
            return string.Format(
                "[ {0} | {1} | {2} | {3} ]  " +
                "[ {4} | {5} | {6} | {7} ]  " +
                "[ {8} | {9} | {10} | {11} ]  " +
                "[ {12} | {13} | {14} | {15} ]",
                M11, M12, M13, M14,
                M21, M22, M23, M24,
                M31, M32, M33, M34,
                M41, M42, M43, M44);
        }

        public float Trace()
        {
            return R0.X + R1.Y + R2.Z + R3.W;
        }

        public Quaternion GetQuaternion()
        {
            var quat = new Quaternion();
            float trace = Trace() + 1f;

            if (trace > Single.Epsilon)
            {
                float s = 0.5f / MathHelper.FastSqrt(trace);

                quat.X = (R2.Y - R1.Z) * s;
                quat.Y = (R0.Z - R2.X) * s;
                quat.Z = (R1.X - R0.Y) * s;
                quat.W = 0.25f / s;
            }
            else
            {
                if (R0.X > R1.Y && R0.X > R2.Z)
                {
                    float s = 2.0f * MathHelper.FastSqrt(1.0f + R0.X - R1.Y - R2.Z);

                    quat.X = 0.25f * s;
                    quat.Y = (R0.Y + R1.X) / s;
                    quat.Z = (R0.Z + R2.X) / s;
                    quat.W = (R1.Z - R2.Y) / s;
                }
                else if (R1.Y > R2.Z)
                {
                    float s = 2.0f * MathHelper.FastSqrt(1.0f + R1.Y - R0.X - R2.Z);

                    quat.X = (R0.Y + R1.X) / s;
                    quat.Y = 0.25f * s;
                    quat.Z = (R1.Z + R2.Y) / s;
                    quat.W = (R0.Z - R2.X) / s;
                }
                else
                {
                    float s = 2.0f * MathHelper.FastSqrt(1.0f + R2.Z - R0.X - R1.Y);

                    quat.X = (R0.Z + R2.X) / s;
                    quat.Y = (R1.Z + R2.Y) / s;
                    quat.Z = 0.25f * s;
                    quat.W = (R0.Y - R1.X) / s;
                }
            }

            return quat;
        }

        public void GetEulerAngles(out float roll, out float pitch, out float yaw)
        {
            double angleX, angleY, angleZ;
            double cx, cy, cz; // cosines
            double sx, sz; // sines

            angleY = System.Math.Asin(MathHelper.Clamp(R0.Z, -1f, 1f));
            cy = System.Math.Cos(angleY);

            if (System.Math.Abs(cy) > 0.005f)
            {
                // No gimbal lock
                cx = R2.Z / cy;
                sx = (-R1.Z) / cy;

                angleX = System.Math.Atan2(sx, cx);

                cz = R0.X / cy;
                sz = (-R0.Y) / cy;

                angleZ = System.Math.Atan2(sz, cz);
            }
            else
            {
                // Gimbal lock
                angleX = 0;

                cz = R1.Y;
                sz = R1.X;

                angleZ = System.Math.Atan2(sz, cz);
            }

            // Return only positive angles in [0,360]
            if (angleX < 0) angleX += 360d;
            if (angleY < 0) angleY += 360d;
            if (angleZ < 0) angleZ += 360d;

            roll = (float)angleX;
            pitch = (float)angleY;
            yaw = (float)angleZ;
        }

        public Vector3 GetEulerAngles()
        {
            float r, p, y;
            GetEulerAngles(out r, out p, out y);
            return new Vector3(r, p, y);
        }

        public void Invert()
        {
            Invert(ref this, out this);
        }
    }
}