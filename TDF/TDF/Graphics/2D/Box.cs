using SharpDX;
using System;
using TDF.Core;
using TDF.Graphics.Data;
using TDF.Graphics.Models;
using TDF.Graphics.Render;

namespace Engine.Graphics.GUI
{
    public sealed class Box : Mesh
    {
        #region Свойства и поля

        public Color4 Color { get; private set; }

        public Point Position { get; private set; }

        public Size2 Size { get; private set; }

        private Matrix WVP { get; set; }

        #endregion Свойства и поля

        #region Методы

        public void Initialize(Size2 size)
        {
            Size = size;
            SetPosition(new Point(-size.Width, -size.Height));

            Color = new Color4(1, 1, 1, 1);

            InitializeBuffers();
        }

        public void Initialize(Size2 size, Color4 color)
        {
            Size = size;
            SetPosition(new Point(-size.Width, -size.Height));

            Color = color;

            InitializeBuffers();
        }

        public void Initialize(Size2 size, Color4 color, Point pos)
        {
            Size = size;
            SetPosition(pos);
            Color = color;

            InitializeBuffers();
        }

        public new void Render()
        {
            Render(WVP);
        }

        public void Render(int positionX, int positionY)
        {
            if (!(positionX == Position.X && positionY == Position.Y))
            {
                WVP = Matrix.Multiply(Matrix.Translation(positionX, -positionY, 0), DirectX11.OrthoMatrix);

                Position = new Point(positionX, positionY);
            }

            Render(WVP);
        }

        public void Render(int positionX, int positionY, int height, int width)
        {
            if (height != Size.Height | width != Size.Width | height != -1 | width != -1)
            {
                Size = new Size2(width, height);

                UpdateBuffers();
            }
            Render(positionX, positionY);
        }

        public void SetColor(Color4 color)
        {
            Color = color;
            UpdateBuffers();
        }

        public void SetColor(Color color, float a = 1)
        {
            Color = new Color4(color.ToVector3(), a);
            UpdateBuffers();
        }

        public void SetPosition(Point pos)
        {
            Position = pos;
            WVP = Matrix.Multiply(Matrix.Translation(pos.X, -pos.Y, 0), DirectX11.OrthoMatrix);
        }

        public void SetSize(Size2 size)
        {
            Size = size;
            UpdateBuffers();
        }

        private void InitializeBuffers()
        {
            try
            {
                EffectNumber = ColorVertex.VertexType;
                UpdateBuffers();
                CreateIndexBuffer(new uint[] { 0, 1, 2, 0, 3, 1 });
            }
            catch (Exception ex)
            {
                ErrorProvider.Send(ex);
            }
        }

        private void UpdateBuffers()
        {
            //Считаем координаты левой,правой, нижней,верхней стороны битмапа (начало координат в центре)
            int left = (-(Config.Width >> 1));
            int right = left + Size.Width;
            int top = (Config.Height >> 1);
            int bottom = top - Size.Height;

            SetVertices(new[]
            {
                new ColorVertex
                {
                    Position = new Vector3(left, top, 1),
                    Color = Color
                },
                new ColorVertex
                {
                    Position = new Vector3(right, bottom, 1),
                    Color = Color
                },
                new ColorVertex
                {
                    Position = new Vector3(left, bottom, 1),
                    Color = Color
                },
                new ColorVertex
                {
                    Position = new Vector3(right, top, 1),
                    Color = Color
                }
            });
        }

        #endregion Методы
    }
}