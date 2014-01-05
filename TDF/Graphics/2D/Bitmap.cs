using SharpDX;
using System;
using TDF.Core;
using TDF.Graphics.Data;
using TDF.Graphics.Models;
using TDF.Graphics.Render;

namespace Engine.Graphics.GUI
{
    public class Bitmap : Mesh
    {
        #region Свойства

        public Point Position { get; private set; }

        public Size2 Size { get; private set; }

        private Matrix WVP { get; set; }

        #endregion Свойства

        #region Методы

        public void Initialize(Size2 size, string textureFileName = null)
        {
            Size = size;
            SetPosition(new Point(-size.Width, -size.Height));

            InitializeBuffers();
            LoadTexture(textureFileName);
        }

        public void Initialize(Size2 size, Point pos, string textureFileName = null)
        {
            Size = size;
            SetPosition(pos);

            InitializeBuffers();
            LoadTexture(textureFileName);
        }

        public void Initialize(Size2 size, Texture tex)
        {
            Size = size;
            SetPosition(new Point(-size.Width, -size.Height));

            InitializeBuffers();
            SetMaterial(new Material { DiffuseTexture = tex });
        }

        public void Initialize(Size2 size, Point pos, Texture tex)
        {
            Size = size;
            SetPosition(pos);

            InitializeBuffers();
            SetMaterial(new Material { DiffuseTexture = tex });
        }

        public void Initialize(string textureFileName = null)
        {
            LoadTexture(textureFileName);
            Size = new Size2(Material.DiffuseTexture.Width, Material.DiffuseTexture.Height);

            SetPosition(new Point(-Size.Width, -Size.Height));

            InitializeBuffers();
        }

        public void Initialize(Point pos, string textureFileName = null)
        {
            LoadTexture(textureFileName);
            Size = new Size2(Material.DiffuseTexture.Width, Material.DiffuseTexture.Height);

            SetPosition(pos);

            InitializeBuffers();
        }

        public void Initialize(Texture tex)
        {
            Size = new Size2(tex.Width, tex.Height);
            SetPosition(new Point(-Size.Width, -Size.Height));

            InitializeBuffers();
            SetMaterial(new Material { DiffuseTexture = tex });
        }

        public void Initialize(Texture tex, Point pos)
        {
            Size = new Size2(tex.Width, tex.Height);
            SetPosition(pos);

            InitializeBuffers();
            SetMaterial(new Material { DiffuseTexture = tex });
        }

        public void LoadTexture(string textureFileName)
        {
            SetMaterial(new Material { DiffuseTexture = new Texture(textureFileName) });
        }

        public new void Render()
        {
            Render(WVP);
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
            if (height != Size.Height | width != Size.Width | height != -1 | width != -1)
            {
                Size = new Size2(width, height);

                UpdateBuffers();
            }
            Render(positionX, positionY);
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

        public void SetTexture(Texture texture)
        {
            SetMaterial(new Material { DiffuseTexture = texture });
        }

        private void InitializeBuffers()
        {
            try
            {
                EffectNumber = TextureVertex.VertexType;
                UpdateBuffers();
                CreateIndexBuffer(new uint[] { 0, 1, 2, 0, 3, 1 });
            }
            catch (Exception ex)
            {
                ErrorProvider.Send(ex);
            }
        }

        private void UpdateBuffers(float x1 = 0, float y1 = 0, float x2 = 1, float y2 = 1)
        {
            int left = (-(Config.Width >> 1));
            int right = left + Size.Width;
            int top = (Config.Height >> 1);
            int bottom = top - Size.Height;

            SetVertices(new[]
            {
                new TextureVertex
                {
                    Position = new Vector3(left, top, 1),
                    Texture = new Vector2(x1, y1)
                },
                new TextureVertex
                {
                    Position = new Vector3(right, bottom, 1),
                    Texture = new Vector2(x2, y2)
                },
                new TextureVertex
                {
                    Position = new Vector3(left, bottom, 1),
                    Texture = new Vector2(x1, y2)
                },
                new TextureVertex
                {
                    Position = new Vector3(right, top, 1),
                    Texture = new Vector2(x2, y1)
                }
            });
        }

        #endregion Методы
    }
}