using TDF.Core;
using SharpDX;

namespace TDF.Graphics.Cameras
{
    public class Camera
    {
        public Matrix View { get; set; }
        public Matrix Projection { get; set; }
        public Matrix Ortho { get; set; }

        public Vector3 Position { get; set; }

        public Camera()
        {
            UpdateScrennMatrices();
        }

        public void UpdateScrennMatrices()
        {
            Projection = Matrix.PerspectiveFovLH(MathHelper.ToRadians(45), (float)Config.Width / Config.Height, Config.ScreenNear, Config.ScreenDepth);

            Ortho = Matrix.OrthoLH(Config.Width, Config.Height, Config.ScreenNear, Config.ScreenDepth);
        }

        public virtual void Update() { }
    }
}