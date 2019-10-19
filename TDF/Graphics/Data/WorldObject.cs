using SharpDX;
using TDF.Core;

namespace TDF.Graphics.Data
{
    public class WorldObject
    {
        public Matrix Matrix;
        public int ModelNum;
        public string ModelPath;

        public BoundingBox BoundingBox { get; private set; }

        public Vector3 Position { get; private set; }

        public Vector3 Rotation { get; private set; }

        public Vector3 Scale { get; private set; }

        public void CalculateMatrix()
        {
            var matrix1 = Matrix.Translation(Position);
            var matrix2 = Matrix.Scaling(Scale);
            var matrix3 = Matrix.RotationYawPitchRoll(MathHelper.ToRadians(Rotation.X), MathHelper.ToRadians(Rotation.Y),
                MathHelper.ToRadians(Rotation.Z));
            Matrix = Matrix.Multiply(Matrix.Multiply(matrix1, matrix2), matrix3);
        }

        public BoundingBox GetBoundingBox()
        {
            return new BoundingBox(
                Vector3.TransformCoordinate(BoundingBox.Minimum, Matrix),
                Vector3.TransformCoordinate(BoundingBox.Maximum, Matrix)
                );
        }

        public void SetBoundingBox(BoundingBox bb)
        {
            BoundingBox = bb;
        }

        public void SetPosition(Vector3 ve3)
        {
            Position = ve3;
        }

        public void SetPosition(float x, float y, float z)
        {
            SetPosition(new Vector3(x, y, z));
        }

        public void SetRotation(Vector3 ve3)
        {
            Rotation = ve3;
        }

        public void SetRotation(float x, float y, float z)
        {
            SetRotation(new Vector3(x, y, z));
        }

        public void SetScale(Vector3 ve3)
        {
            Scale = ve3;
        }

        public void SetScale(float x, float y, float z)
        {
            Scale = new Vector3(x, y, z);
        }
    }
}