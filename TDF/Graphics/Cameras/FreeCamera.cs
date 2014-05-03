using System;
using TDF.Core;
using TDF.Inputs;
using SharpDX;

namespace TDF.Graphics.Cameras
{
    public class FreeCamera : Camera
    {
        public bool RotateOnPress { get; set; }

        public float Speed = 1;

        private Point _lastMousePos;

        private int _negativeFactor;

        private Vector3 _translation;

        public float RotateFactor { get; set; }

        public FreeCamera(Vector3 position, float yaw, float pitch, bool pressedOnlyMouseRotate = false,
            bool negativeRot = true)
        {
            Position = position;
            Yaw = yaw;
            Pitch = pitch;

            _translation = Vector3.Zero;

            RotateOnPress = pressedOnlyMouseRotate;
            NegativeRotation = negativeRot;

            RotateFactor = 0.01f;
        }

        public bool NegativeRotation
        {
            set { _negativeFactor = value ? -1 : 1; }
        }

        public float Pitch { get; set; }

        public Vector3 Target { get; private set; }

        public float Yaw { get; set; }

        public void DirectionMove(Vector3 direction)
        {
            _translation = direction*Speed;
        }

        public void Move(Vector3 translation)
        {
            _translation += translation;
        }

        public void Rotate(float yawChange, float pitchChange)
        {
            Yaw += yawChange;
            Pitch += pitchChange;

            if (Yaw < -PI)
                Yaw += PI2;
            if (Yaw > PI)
                Yaw -= PI2;

            if (Pitch < -PI)
                Pitch += PI2;
            if (Pitch > PI)
                Yaw -= PI2;
        }

        public void RotateWithMouse()
        {
            if (!(RotateOnPress && !Input.MouseState.LeftButton) && Input.MouseState.X >= 0 && Input.MouseState.Y >= 0)
            {
                var deltaX = _lastMousePos.X - Input.MouseState.X;
                var deltaY = _lastMousePos.Y - Input.MouseState.Y;

                Rotate(_negativeFactor*deltaX*RotateFactor, _negativeFactor*deltaY*RotateFactor);
            }
            _lastMousePos = new Point(Input.MouseState.X, Input.MouseState.Y);
        }

        public override void Update()
        {
            // Вычисление матрицы вращения
            var rotation = FromYawPitch(Yaw, Pitch);
            // Смещение позиции и сброс переменной translation
            Position += Vector3.Transform(_translation, rotation).ToVector3();
            _translation = Vector3.Zero;
            // Вычисление новой точки обзора
            float cosPitch = rotation[1, 1],
                sinPitch = -rotation[2, 1],
                cosYaw = rotation[0, 0],
                sinYaw = -rotation[0, 2];

            var xaxis = new Vector3(cosYaw, 0, -sinYaw);
            var yaxis = new Vector3(sinYaw * sinPitch, cosPitch, cosYaw * sinPitch);
            var zaxis = new Vector3(sinYaw * cosPitch, -sinPitch, cosPitch * cosYaw);

            View = new Matrix(
                xaxis.X, yaxis.X, zaxis.X, 0,
                xaxis.Y, yaxis.Y, zaxis.Y, 0,
                xaxis.Z, yaxis.Z, zaxis.Z, 0,
                -Vector3.Dot(xaxis, Position), -Vector3.Dot(yaxis, Position), -Vector3.Dot(zaxis, Position),
                    1
                );
        }

        private static Matrix FromYawPitch(float yaw, float pitch)
        {
            var matrix = new Matrix();

            float cYaw = Cos(yaw),
                sYaw = Sin(yaw),
                sPitch = Sin(pitch),
                cPitch = Cos(pitch);

            matrix[0, 0] = cYaw;
            matrix[0, 2] = -sYaw;

            matrix[1, 0] = sPitch*sYaw;
            matrix[1, 1] = cPitch;
            matrix[1, 2] = sPitch*cYaw;

            matrix[2, 0] = cPitch*sYaw;
            matrix[2, 1] = -sPitch;
            matrix[2, 2] = cPitch*cYaw;

            matrix[3, 3] = 0;
            return matrix;
        }

        private const float PI = (float) Math.PI;
        private const float PI2 = 2 * PI;
        private const float HalfPI = PI / 2;
        private const float QuadPI = 4 / PI;
        private const float HalfPI2 = 4/(PI * PI);

        public static float Sin(float x)
        {
            if (x < 0)
                return x * (QuadPI + HalfPI2 * x);
            return x * (QuadPI - HalfPI2 * x);
        }
        public static float Cos(float x)
        {
            x += HalfPI;
            if (x > PI)
                x -= PI2;

            if (x < 0)
                return x * (QuadPI + HalfPI2 * x);
            return x * (QuadPI - HalfPI2 * x);
        }

    }
}