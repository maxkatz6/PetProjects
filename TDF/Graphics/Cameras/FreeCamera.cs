using TDF.Core;
using TDF.Inputs;
using SharpDX;

namespace TDF.Graphics.Cameras
{
    public class FreeCamera : Camera
    {
        public bool RotateOnPress;

        public float Speed = 1;

        private Vector2 _lastMousePos;

        private int _negativeFactor;

        private Vector3 _translation;

        public float RotateFactor { get; set; }

        public FreeCamera(Vector3 position, float yaw, float pitch, bool pressedOnlyMouseRotate = false, bool negativeRot = true)
        {
            Position = position;
            Yaw = yaw;
            Pitch = pitch;

            _translation = Vector3.Zero;

            RotateOnPress = pressedOnlyMouseRotate;
            NegativeRotation = negativeRot;

            RotateFactor = 0.01f;
        }

        public bool NegativeRotation { set { _negativeFactor = value ? -1 : 1; } }

        public float Pitch { get; set; }

        public Vector3 Target { get; private set; }

        public float Yaw { get; set; }

        public void DirectionMove(Vector3 direction)
        {
            _translation = direction * Speed;
        }

        public void Move(Vector3 translation)
        {
            _translation += translation;
        }

        public void Rotate(float yawChange, float pitchChange)
        {
            Yaw += yawChange;
            Pitch += pitchChange;
        }

        public void RotateWithMouse(MouseState mouseState)
        {
            if (!(RotateOnPress && !mouseState.LeftButton) && mouseState.X >= 0 && mouseState.Y >= 0)
            {
                var deltaX = _lastMousePos.X - mouseState.X;
                var deltaY = _lastMousePos.Y - mouseState.Y;

                Rotate(_negativeFactor * deltaX * RotateFactor, _negativeFactor * deltaY * RotateFactor);
            }
            _lastMousePos = mouseState.Vector;
        }

        public override void Update()
        {
            // Вычисление матрицы вращения
            var rotation = Matrix.RotationYawPitchRoll(Yaw, Pitch, 0);
            // Смещение позиции и сброс переменной translation
            Position += Vector3.Transform(_translation, rotation).ToVector3();
            _translation = Vector3.Zero;
            // Вычисление новой точки обзора
            Target = Position + Vector3.Transform(Vector3.ForwardLH, rotation).ToVector3();
            // Вычисление вектора вертикальной ориентации
            var up = Vector3.Transform(Vector3.Up, rotation).ToVector3();
            // Вычисление матрицы вида
            View = Matrix.LookAtLH(Position, Target, up);
        }
    }
}