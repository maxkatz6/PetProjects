using Ormeli.Math;
using SharpDX;

namespace Ormeli.Graphics.Cameras
{
    public class FreeCamera : Camera
    {
        public bool RotateOnPress;

        public float Speed = 1;

        private Vector2 _lastMousePos;

        private int _negativeFactor;

        private Vector3 _translation;
        private bool _needViewUpdate = true;

        public float RotateFactor { get; set; }

        public FreeCamera(Vector3 position, float yaw = 0, float pitch = 0, bool pressedOnlyMouseRotate = true, bool negativeRot = true)
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

        public float Yaw { get; set; }

        public void DirectionMove(Vector3 direction)
        {
            Move(direction * Speed);
        }

        public void Move(Vector3 translation)
        {
            _translation += translation;
            _needViewUpdate = true;
        }

        public void Rotate(float yawChange, float pitchChange)
        {
            Yaw = (Yaw > MathHelper.Pi ? Yaw - MathHelper.TwoPi : (Yaw < -MathHelper.Pi ? Yaw + MathHelper.TwoPi : Yaw)) + yawChange;
            Pitch = (Pitch > MathHelper.Pi ? Pitch - MathHelper.TwoPi : (Pitch < -MathHelper.Pi ? Pitch + MathHelper.TwoPi : Pitch)) + pitchChange;
            _needViewUpdate = true;
        }

        private void RotateWithMouse()
        {
            var mouseState = Input.Input.MouseState;
            if (!(RotateOnPress && !mouseState.LeftButton) && mouseState.X >= 0 && mouseState.Y >= 0)
            {
                var deltaX = _lastMousePos.X - mouseState.X;
                var deltaY = _lastMousePos.Y - mouseState.Y;

                if (System.Math.Abs(deltaX) < MathHelper.ZeroTolerance && System.Math.Abs(deltaY) < MathHelper.ZeroTolerance) return;

                Rotate(_negativeFactor * deltaX * RotateFactor, _negativeFactor * deltaY * RotateFactor);
            }
            _lastMousePos = mouseState.Vector;
        }


        public override void Update()
        {
            RotateWithMouse();
            if (!_needViewUpdate) return;
            var rotation = Matrix.RotationYawPitchRoll(Yaw, Pitch,0);
            // Смещение позиции и сброс переменной translation
            Position += Vector3.TransformCoordinate(_translation, rotation);
            _translation = Vector3.Zero;

            //Some optimisations.
            float cosPitch = rotation[1, 1],
                sinPitch = -rotation[2, 1],
                cosYaw = rotation[0, 0],
                sinYaw = -rotation[0, 2];

            var xaxis = new Vector3(cosYaw, 0, -sinYaw);
            var yaxis = new Vector3(sinYaw * sinPitch, cosPitch, cosYaw * sinPitch);
            var zaxis = new Vector3(sinYaw * cosPitch, -sinPitch, cosPitch * cosYaw);

            ViewProjection = new Matrix(
                xaxis.X, yaxis.X, zaxis.X, 0,
                xaxis.Y, yaxis.Y, zaxis.Y, 0,
                xaxis.Z, yaxis.Z, zaxis.Z, 0,
                -Vector3.Dot(xaxis, Position), -Vector3.Dot(yaxis, Position), -Vector3.Dot(zaxis, Position), 1)*
                             Projection;
            _needViewUpdate = false;
        }
    }
}