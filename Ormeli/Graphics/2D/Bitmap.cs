using Ormeli.Graphics.Cameras;
using Ormeli.Math;

namespace Ormeli.Graphics
{
    public class Bitmap : TextureMesh
    {
        #region Свойства

        public Point Position { get; private set; }

        public Point Size { get; private set; }

        private Matrix WVP { get; set; }

        #endregion Свойства

        #region Методы

        public void Initialize(Point size, Point pos, string fileName)
        {
            Initialize(size, pos);
            SetTexture(fileName);
        }
        public void Initialize(Point size, Point pos, int index)
        {
            Initialize(size, pos);
            SetTexture(index);
        }

        public void Initialize(Point size, Point pos)
        {
            Size = size;
            SetPosition(pos);

            UpdateBuffers();
        }

        public void Render()
        {
            base.Render(WVP);
        }

        public void Render(int positionX, int positionY)
        {
            if (!(positionX == Position.X && positionY == Position.Y))
            {
                SetPosition(new Point(positionX, positionY));
            }

            Render(WVP);
        }

        public void Render(int positionX, int positionY, int height, int width)
        {
            if (height != Size.X | width != Size.Y | height != -1 | width != -1)
            {
                Size = new Point(width, height);

                UpdateBuffers();
            }
            Render(positionX, positionY);
        }

        public void SetPosition(Point pos)
        {
            Position = pos;
            WVP = Matrix.Multiply(Matrix.Translation(pos.X, -pos.Y, 0), Camera.Current.Ortho);
        }

        public void SetSize(Point size)
        {
            Size = size;
            UpdateBuffers();
        }

        private void UpdateBuffers(float x1 = 0, float y1 = 0, float x2 = 1, float y2 = 1)
        {
            int left = (-(Config.Width >> 1));
            int right = left + Size.X;
            int top = (Config.Height >> 1);
            int bottom = top - Size.Y;

            Initalize(new[] {0, 1, 2, 0, 3, 1}, new[]
            {
                new BitmapVertex
                {
                    Position = new Vector2(left, top),
                    TexCoord = new Vector2(x1, y1)
                },
                new BitmapVertex
                {
                    Position = new Vector2(right, bottom),
                    TexCoord = new Vector2(x2, y2)
                },
                new BitmapVertex
                {
                    Position = new Vector2(left, bottom),
                    TexCoord = new Vector2(x1, y2)
                },
                new BitmapVertex
                {
                    Position = new Vector2(right, top),
                    TexCoord = new Vector2(x2, y1)
                }
            }, 1, "Texture");
        }

        #endregion Методы
    }
}