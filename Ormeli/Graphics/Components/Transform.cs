using SharpDX;

namespace Ormeli.Graphics
{
	public class Transform
	{
		public Vector3 Position { get; set; }
		public Vector3 Rotation { get; set; }
		public Vector3 Scale { get; set; } = new Vector3(1,1,1);

		//TODO оптимизировать, чтобы не создавало новую матрицу при каждом вызове, а только при изменении трансформаций
		public Matrix WorldMatrix => Matrix.RotationYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z) * new Matrix(
			Scale.X,	0,			0,			0,
			0,			Scale.Y,	0,			0,
			0,			0,			Scale.Z,	0,
			Position.X, Position.Y, Position.Z, 1);
		/*	Matrix.Scaling(Scale.X,Scale.Y,Scale.Z) * 
			Matrix.RotationYawPitchRoll(Rotation.X,Rotation.Y,Rotation.Z) *
			Matrix.Translation(Position);*/
	}
}
