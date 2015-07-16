using SharpDX;

namespace Ormeli.Graphics
{
	public class Transform
	{
		private Vector3 position;
		private Vector3 rotation;
		private Vector3 scale = Vector3.One;

		public Vector3 Rotation
		{
			get { return rotation; }
			set
			{
				rotation = value;
				Update();
			}
		}

		public Vector3 Scale
		{
			get { return scale; }
			set
			{
				scale = value;
				Update();
			}
		}

		public Vector3 Position
		{
			get { return position; }
			set
			{
				position = value;
				Update();
			}
		}

		public Matrix WorldMatrix { get; private set; } = Matrix.Identity;

		private void Update()
		{
			WorldMatrix = Matrix.RotationYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z)*
			              new Matrix(
				              Scale.X, 0, 0, 0,
				              0, Scale.Y, 0, 0,
				              0, 0, Scale.Z, 0,
				              Position.X, Position.Y, Position.Z, 1);
			/*	Matrix.Scaling(Scale.X,Scale.Y,Scale.Z) * 
				Matrix.RotationYawPitchRoll(Rotation.X,Rotation.Y,Rotation.Z) *
				Matrix.Translation(Position);*/
		}
	}
}