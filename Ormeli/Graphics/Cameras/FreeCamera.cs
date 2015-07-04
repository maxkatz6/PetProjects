using Ormeli.Math;
using SharpDX;

namespace Ormeli.Graphics.Cameras
{
	public class FreeCamera : Camera
	{
		private Point _lastMousePos;
		private bool _needViewUpdate = true;
		private Vector3 _translation;

		public FreeCamera(Transform transform)
		{
			Transform = transform;
		}

		public float Speed { get; set; } = 1;
		public bool RotateOnPress { get; set; } = true;
		public float RotateFactor { get; set; } = 1;
		public bool NegativeRotation { get; } = true;

		public void MoveWASD()
		{
			if (Input.IsKeyDown(Input.Key.W))
				DirectionMove(Vector3.ForwardRH);
			else if (Input.IsKeyDown(Input.Key.S))
				DirectionMove(Vector3.BackwardRH);
			if (Input.IsKeyDown(Input.Key.A))
				DirectionMove(Vector3.Left);
			else if (Input.IsKeyDown(Input.Key.D))
				DirectionMove(Vector3.Right);
		}

		public void DirectionMove(Vector3 direction)
		{
			Move(direction*Speed);
		}

		public void Move(Vector3 translation)
		{
			_translation += translation;
			_needViewUpdate = true;
		}

		private void RotateWithMouse()
		{
			var mouseState = Input.Mouse;
			if (!(RotateOnPress && !mouseState.RightButton) && mouseState.X >= 0 && mouseState.Y >= 0)
			{
				var deltaX = _lastMousePos.X - mouseState.X;
				var deltaY = _lastMousePos.Y - mouseState.Y;

				if (System.Math.Abs(deltaX) < MathHelper.ZeroTolerance && System.Math.Abs(deltaY) < MathHelper.ZeroTolerance)
					return;

				Transform.Rotation += new Vector3(deltaX, deltaY, 0)*0.01f*RotateFactor*(NegativeRotation ? -1 : 1);
				_needViewUpdate = true;
			}
			_lastMousePos = mouseState.Point;
		}

		public override void Update()
		{
			RotateWithMouse();
			if (!_needViewUpdate) return;
			var rotation = Matrix.RotationYawPitchRoll(Transform.Rotation.X, Transform.Rotation.Y, 0);
			// Смещение позиции и сброс переменной translation
			Transform.Position += Vector3.TransformCoordinate(_translation, rotation);
			_translation = Vector3.Zero;

			Transform.Rotation = new Vector3(Transform.Rotation.X%MathHelper.TwoPi, Transform.Rotation.Y%MathHelper.TwoPi, 0);

			//Some optimisations.
			float cosPitch = rotation[1, 1],
				sinPitch = -rotation[2, 1],
				cosYaw = rotation[0, 0],
				sinYaw = -rotation[0, 2];

			Vector3 xaxis = new Vector3(cosYaw, 0, -sinYaw),
				yaxis = new Vector3(sinYaw*sinPitch, cosPitch, cosYaw*sinPitch),
				zaxis = new Vector3(sinYaw*cosPitch, -sinPitch, cosPitch*cosYaw);

			ViewProjection = new Matrix(
				xaxis.X, yaxis.X, zaxis.X, 0,
				xaxis.Y, yaxis.Y, zaxis.Y, 0,
				xaxis.Z, yaxis.Z, zaxis.Z, 0,
				-Vector3.Dot(xaxis, Transform.Position), -Vector3.Dot(yaxis, Transform.Position),
				-Vector3.Dot(zaxis, Transform.Position), 1)*Projection;
			_needViewUpdate = false;
		}
	}
}