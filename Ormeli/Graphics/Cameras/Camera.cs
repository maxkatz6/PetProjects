using Ormeli.Math;

namespace Ormeli.Graphics.Cameras
{
    public abstract class Camera
    {
        public static Camera Current;
        public Matrix ViewProjection { get; set; }
        public Matrix Projection { get; set; }
        public Matrix Ortho { get; set; }

        public Vector3 Position { get; set; }

        protected Camera()
        {
            if (Current == null) SetCurrent();
        }

        public void UpdateScrennMatrices()
        {
            UpdateScrennMatrices(Config.Width, Config.Height);
        }

        public void UpdateScrennMatrices(int width, int height)
        {
            Projection = Matrix.PerspectiveFov(1, //MathHelper.ToRadians(45);
                (float)width / height, Config.ScreenNear,
                Config.ScreenDepth);

            Ortho = Matrix.Ortho(width, height, Config.ScreenNear, Config.ScreenDepth);
        }
        public void SetCurrent()
        {
            UpdateScrennMatrices();
            Current = this;
        }

        public virtual void Update() { }
    }
}