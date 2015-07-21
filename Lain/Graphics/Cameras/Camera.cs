using SharpDX;

namespace Lain.Graphics.Cameras
{
	public abstract class Camera
	{
		protected Camera()
		{
			UpdateScreenMatrices();
		}
		public Matrix ViewRotation { get; protected set; }
		public Matrix ViewProjection { get; protected set; }
		public Matrix Projection { get; protected set; }
		public Matrix Ortho { get; protected set; }
		public Transform Transform { get; protected set; }

		public void UpdateScreenMatrices()
		{
			UpdateScreenMatrices(Config.Width, Config.Height);
		}

		public void UpdateScreenMatrices(int width, int height)
		{
			Projection = Matrix.PerspectiveFovRH(MathHelper.ToRadians(60),
				(float) width/height, Config.ScreenNear,
				Config.ScreenDepth);

			Ortho = Matrix.OrthoRH(width, height, Config.ScreenNear, Config.ScreenDepth);
		}

		public abstract void Update();
	}
}