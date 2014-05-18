using Ormeli.Math;

namespace Ormeli.Graphics.Cameras
{
    public class TargetCamera : Camera
    {
        public Vector3 Target { get; set; }

        public TargetCamera(Vector3 position, Vector3 target)
        {
            Position = position;
            Target = target;
        }

        public override void Update()
        {
            ViewProjection = Matrix.LookAt(Position, Target, Vector3.Zero) * Projection;
        }
    }
}
