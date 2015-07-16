using SharpDX;

namespace Ormeli.Graphics.Cameras
{
	public class TargetCamera : Camera
	{
		public TargetCamera(Transform pos, Transform target)
		{
			Transform = pos;
			Target = target;
		}

		public Transform Target { get; set; }

		public override void Update()
		{
			ViewRotation = Matrix.LookAtLH(Vector3.Zero, Target.Position, Vector3.Zero)*Projection;
            ViewProjection = Matrix.LookAtLH(Transform.Position, Target.Position, Vector3.Zero)*Projection;
		}
	}
}