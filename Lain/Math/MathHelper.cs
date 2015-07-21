using System.Runtime.InteropServices;

namespace Lain
{
	public static class MathHelper
	{
		public const float ZeroTolerance = 1e-6f;
		public const float Log10E = 0.4342944819032f;
		public const float Log2E = 1.442695040888f;
		public const float Pi = (float) System.Math.PI;
		public const float PiOver2 = Pi/2.0f;
		public const float PiOver4 = Pi/4.0f;
		public const float TwoPi = Pi*2.0f;
		private const float QuadPi = 4/Pi;
		private const float PiConst = 4/(Pi*Pi);

		public static float FastSqrt(float z)
		{
			FloatIntUnion u;
			u.tmp = 0;
			var xhalf = 0.5f*z;
			u.f = z;
			u.tmp = 0x5f375a86 - (u.tmp >> 1); // what the fuck?
			u.f = u.f*(1.5f - xhalf*u.f*u.f);
			return u.f*z;
		}

		public static float FastestSqrt(float z)
		{
			FloatIntUnion u;
			u.tmp = 0;
			u.f = z;
			u.tmp -= 1 << 23; /* Subtract 2^m. */
			u.tmp >>= 1; /* Divide by 2. */
			u.tmp += 1 << 29; /* Add ((b + 1) / 2) * 2^m. */
			return u.f;
		}

		public static float FastestSin(float x)
		{
			if (x < -Pi)
				x += TwoPi;
			else if (x > Pi)
				x -= TwoPi;

			return (x < 0 ? x*(QuadPi + PiConst*x) : x*(QuadPi - PiConst*x));
		}

		public static float FastestCos(float x)
		{
			x += PiOver2;
			if (x < -Pi)
				x += TwoPi;
			else if (x > Pi)
				if ((x -= TwoPi) > Pi)
					x -= TwoPi;

			return (x < 0 ? x*(QuadPi + PiConst*x) : x*(QuadPi - PiConst*x));
		}

		public static float FastSin(float x)
		{
			x = WrapAngle(x);

			var sin = (x < 0 ? x*(QuadPi + PiConst*x) : x*(QuadPi - PiConst*x));
			return (sin < 0 ? .225f*(sin*-sin - sin) + sin : .225f*(sin*sin - sin) + sin);
		}

		public static float FastCos(float x)
		{
			x = WrapAngle(x);
			var cos = (x < 0 ? x*(QuadPi + PiConst*x) : x*(QuadPi - PiConst*x));
			return (cos < 0 ? .225f*(cos*-cos - cos) + cos : .225f*(cos*cos - cos) + cos);
		}

		public static float Barycentric(float value1, float value2, float value3, float amount1, float amount2)
		{
			return value1 + (value2 - value1)*amount1 + (value3 - value1)*amount2;
		}

		public static float CatmullRom(float value1, float value2, float value3, float value4, float amount)
		{
			// http://stephencarmody.wikispaces.com/Catmull-Rom+splines

			//value1 *= ((-amount + 2.0f) * amount - 1) * amount * 0.5f;
			//value2 *= (((3.0f * amount - 5.0f) * amount) * amount + 2.0f) * 0.5f;
			//value3 *= ((-3.0f * amount + 4.0f) * amount + 1.0f) * amount * 0.5f;
			//value4 *= ((amount - 1.0f) * amount * amount) * 0.5f;
			//
			//return value1 + value2 + value3 + value4;

			// http://www.mvps.org/directx/articles/catmull/

			var amountSq = amount*amount;
			var amountCube = amountSq*amount;

			// value1..4 = P0..3
			// amount = t
			return ((2.0f*value2 +
			         (-value1 + value3)*amount +
			         (2.0f*value1 - 5.0f*value2 + 4.0f*value3 - value4)*amountSq +
			         (3.0f*value2 - 3.0f*value3 - value1 + value4)*amountCube)*0.5f);
		}

		public static float Clamp(float value, float min, float max)
		{
			return System.Math.Min(System.Math.Max(min, value), max);
		}

		public static float Distance(float value1, float value2)
		{
			return System.Math.Abs(value1 - value2);
		}

		public static float Hermite(float value1, float tangent1, float value2, float tangent2, float amount)
		{
			//http://www.cubic.org/docs/hermite.htm
			var s = amount;
			var s2 = s*s;
			var s3 = s2*s;
			var h1 = 2*s3 - 3*s2 + 1;
			var h2 = -2*s3 + 3*s2;
			var h3 = s3 - 2*s2 + s;
			var h4 = s3 - s2;
			return value1*h1 + value2*h2 + tangent1*h3 + tangent2*h4;
		}

		public static float Lerp(float value1, float value2, float amount)
		{
			return value1 + (value2 - value1)*amount;
		}

		public static float Max(float value1, float value2)
		{
			return System.Math.Max(value1, value2);
		}

		public static float Min(float value1, float value2)
		{
			return System.Math.Min(value1, value2);
		}

		public static float SmoothStep(float value1, float value2, float amount)
		{
			//FIXME: check this
			//the function is Smoothstep (http://en.wikipedia.org/wiki/Smoothstep) but the usage has been altered
			// to be similar to Lerp
			amount = amount*amount*(3f - 2f*amount);
			return value1 + (value2 - value1)*amount;
		}

		public static float ToDegrees(float radians)
		{
			return radians*(180f/Pi);
		}

		public static float ToRadians(float degrees)
		{
			return degrees*(Pi/180f);
		}

		public static float WrapAngle(float angle)
		{
			angle = angle%TwoPi;
			if (angle > Pi)
				return angle - TwoPi;
			if (angle < -Pi)
				return angle + TwoPi;
			return angle;
		}

		[StructLayout(LayoutKind.Explicit)]
		private struct FloatIntUnion
		{
			[FieldOffset(0)] public float f;

			[FieldOffset(0)] public int tmp;
		}
	}
}