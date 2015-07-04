using SharpDX;

namespace Ormeli.Graphics.Cameras
{
	public abstract class Camera
	{
		protected Camera()
		{
			UpdateScreenMatrices();
		}

		public Matrix ViewProjection { get; set; }
		public Matrix Projection { get; set; }
		public Matrix Ortho { get; set; }
		public Transform Transform { get; set; }

		public void UpdateScreenMatrices()
		{
			UpdateScreenMatrices(Config.Width, Config.Height);
		}

		public void UpdateScreenMatrices(int width, int height)
		{
			Projection = Matrix.PerspectiveFovRH(1, //MathHelper.ToRadians(45);
				(float) width/height, Config.ScreenNear,
				Config.ScreenDepth);

			Ortho = Matrix.OrthoRH(width, height, Config.ScreenNear, Config.ScreenDepth);
		}

		public abstract void Update();
	}
}