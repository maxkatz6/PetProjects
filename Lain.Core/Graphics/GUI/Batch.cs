using System;
using System.Runtime.InteropServices;
using Lain.GAPI;
using SharpDX;
using Buffer = Lain.GAPI.Buffer;

namespace Lain.Graphics.GUI
{
    public class SpriteBatch
    {
        private const int MaxBatchSize = 4096;

        private const int VerticesPerSprite = 4;
        private const int IndicesPerSprite = 6;
        private const int MaxVertexCount = MaxBatchSize * VerticesPerSprite;
        private const int MaxIndexCount = MaxBatchSize * IndicesPerSprite;

        private Effect Effect;
        private Buffer Vb { get; }
        private Buffer Ib { get; }
        private Sprite[] _spriteQueue;
        private int _spriteQueueCount;

        private int _vertexBufferPosition;
        public bool UsingGS { get; }

        private static readonly int[] indices;
        static SpriteBatch()
        {
            indices = new int[MaxIndexCount];
            for (int i = 0, k = 0; i < MaxIndexCount; k += VerticesPerSprite)
            {
                indices[i++] = (k + 0);
                indices[i++] = (k + 1);
                indices[i++] = (k + 2);
                indices[i++] = (k + 0);
                indices[i++] = (k + 2);
                indices[i++] = (k + 3);
            }
        }

        public SpriteBatch(bool useGS = true)
        {
            UsingGS = useGS && App.Render.ShaderModel >= 4;

            Effect = Effect.FromFile(UsingGS ? "SpriteBatchGS.hlsl" : "SpriteBatch.hlsl");
            if (UsingGS)
                Vb = Buffer.Create(new Bitmap[MaxBatchSize], BindFlag.VertexBuffer);
            else
            {
                Vb = Buffer.Create(new BitmapVertex[MaxVertexCount], BindFlag.VertexBuffer);
                Ib = Buffer.Create(indices, BindFlag.IndexBuffer, BufferUsage.Default, CpuAccessFlag.None);
            }
            _spriteQueue = new Sprite[MaxBatchSize];
        }

        public void Begin()
        {
            _spriteQueueCount = 0;
            _vertexBufferPosition = 0;
        }

        public void DrawText(Font font, string text, Vector2? loc = null, Color4? color = null)
        {
            if (_spriteQueueCount >= _spriteQueue.Length)
                Array.Resize(ref _spriteQueue, _spriteQueue.Length * 2);

            var sprts = font.BuildSpriteArray(text, loc ?? Vector2.Zero, color ?? Color.White);
            for (int i = 0; i < sprts.Length; i++)
                _spriteQueue[_spriteQueueCount++] = sprts[i];
        }

        public void Draw(Texture texture, RectangleF dest, RectangleF? source = null, Color4? color = null)
        {
            if (_spriteQueueCount >= _spriteQueue.Length)
                Array.Resize(ref _spriteQueue, _spriteQueue.Length * 2);

            float deltaX = 1f / (texture.Width);
            float deltaY = 1f / (texture.Height);

            RectangleF sr;
            if (source.HasValue)
            {
                var s = source.Value;
                sr = new RectangleF(s.X * deltaX, s.Y * deltaY, s.Width * deltaX, s.Height * deltaY);
            }
            else sr = new RectangleF(0, 0, 1, 1);
            _spriteQueue[_spriteQueueCount++] = new Sprite(texture,
                new Bitmap(new RectangleF(dest.X / Config.Width * 2 - 1, -(dest.Y / Config.Height * 2 - 1), dest.Width / Config.Width * 2, -(dest.Height / Config.Height * 2)), sr, color ?? Color.White));
        }

        public unsafe void End()
        {
            if (_spriteQueueCount > 0)
            {
                var _z = App.Render.IsDepthEnabled;

                if (UsingGS)
                {
                    var p = (Bitmap*)Vb.MapBuffer(0, SetDataOptions.NoOverwrite).ToPointer();
                    for (int i = 0; i < _spriteQueueCount; i++)
                        *p++ = _spriteQueue[i].Bitmap;
                    Vb.UnmapBuffer();
                    App.Render.SetVertexBuffer(Vb, Bitmap.SizeInBytes);
                }
                else
                {
                    var p = (BitmapVertex*)Vb.MapBuffer(0, SetDataOptions.NoOverwrite).ToPointer();
                    for (int i = 0; i < _spriteQueueCount; i++)
                    {
                        var sp = _spriteQueue[i].Bitmap;

                        p->Color = sp.Color;
                        p->Location = new Vector2(sp.Destination.Left, sp.Destination.Top);
                        p++->TexCoord = new Vector2(sp.Source.Left, sp.Source.Top);

                        p->Color = sp.Color;
                        p->Location = new Vector2(sp.Destination.Left, sp.Destination.Bottom);
                        p++->TexCoord = new Vector2(sp.Source.Left, sp.Source.Bottom);

                        p->Color = sp.Color;
                        p->Location = new Vector2(sp.Destination.Right, sp.Destination.Bottom);
                        p++->TexCoord = new Vector2(sp.Source.Right, sp.Source.Bottom);
                        
                        p->Color = sp.Color;
                        p->Location = new Vector2(sp.Destination.Right, sp.Destination.Top);
                        p++->TexCoord = new Vector2(sp.Source.Right, sp.Source.Top);
                    }
                    Vb.UnmapBuffer();
                    
                    App.Render.SetIndexBuffer(Ib);
                    App.Render.SetVertexBuffer(Vb, BitmapVertex.SizeInBytes);
                }

                App.Render.ZBuffer(false);
                int offset = 0;
                var previousTexture = Texture.Null;
                for (int i = 0; i < _spriteQueueCount; i++)
                {
                    var texture = _spriteQueue[i].Texture;

                    if (texture != previousTexture)
                    {
                        if (i > offset) DrawBatchPerTexture(previousTexture, offset, i - offset);
                        offset = i;
                        previousTexture = texture;
                    }
                }

                DrawBatchPerTexture(previousTexture, offset, _spriteQueueCount - offset);
                App.Render.ZBuffer(_z);
            }
        }

        private void DrawBatchPerTexture(Texture texture, int offset, int count)
        {
            Effect.SetTexture(texture);
            if (UsingGS) Effect.Render(_spriteQueue[offset].Tech, count, _vertexBufferPosition);
            else Effect.Render(_spriteQueue[offset].Tech, count * IndicesPerSprite, _vertexBufferPosition * IndicesPerSprite);
            _vertexBufferPosition += count;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Sprite
        {
            public Texture Texture;
            public Bitmap Bitmap;
            public string Tech;
            public Sprite(Texture t, Bitmap b, string tc = "")
            {
                Texture = t;
                Bitmap = b;
                Tech = tc;
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct Bitmap
        {
            public static int SizeInBytes = Marshal.SizeOf<Bitmap>();
            public RectangleF Destination;
            public RectangleF Source;
            public Color4 Color;
            public Bitmap(RectangleF d, RectangleF s, Color4 c)
            {
                Destination = d;
                Source = s;
                Color = c;
            }
        }
    }
}