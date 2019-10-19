using SharpDX;

namespace TDF.Graphics.Cameras
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
            View = Matrix.LookAtLH(Position, Target, new Vector3(0,0,0));
        }
    }
}
