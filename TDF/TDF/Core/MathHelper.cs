using SharpDX;

namespace TDF.Core
{

    /// <summary>
    /// Math and convert helper class
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// Create Color4 from byte color
        /// </summary>
        /// <param name="r">Red.</param>
        /// <param name="g">Green.</param>
        /// <param name="b">Blue.</param>
        /// <param name="a">Alpha.</param>
        /// <returns></returns>
        public static Color4 ToColor4(byte r, byte g, byte b, byte a)
        {
            return new Color4(r / 255F, g / 255F, b / 255F, a / 255F);
        }

        /// <summary>
        /// Convert Vector4 to Vector3 with losing W coord.
        /// </summary>
        /// <param name="vector">Vector4.</param>
        /// <returns></returns>
        public static Vector3 ToVector3(this Vector4 vector)
        {
            return new Vector3(vector.X, vector.Y, vector.Z);
        }

        public static float ToRadians(float d)
        {
            return MathUtil.DegreesToRadians(d);
        }

        public static float Clamp(float value, float min, float max)
        {
            return value > max ? max : value < min ? min : value;
        }
    }
}