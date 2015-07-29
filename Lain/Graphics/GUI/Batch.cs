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
        private const int MinBatchSize = 128;

        private Effect Effect;
        private Buffer Vb { get; }
        private Sprite[] _spriteQueue;
        private int _spriteQueueCount;

        private int _vertexBufferPosition;


        public SpriteBatch()
        {
            Effect = Effect.FromFile("SpriteBatchGS.hlsl");
            Vb = Buffer.Create(new Bitmap[MaxBatchSize], BindFlag.VertexBuffer);
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

        public void End()
        {
            if (_spriteQueueCount > 0)
            {
                var _z = App.Render.IsDepthEnabled;

                App.Render.ZBuffer(false);
                int offset = 0;
                var previousTexture = Texture.Null;
                for (int i = 0; i < _spriteQueueCount; i++)
                {
                    var texture = _spriteQueue[i].Texture;

                    if (texture != previousTexture)
                    {
                        if (i > offset)
                            DrawBatchPerTexture(previousTexture, offset, i - offset);
                        offset = i;
                        previousTexture = texture;
                    }
                }

                DrawBatchPerTexture(previousTexture, offset, _spriteQueueCount - offset);
                App.Render.ZBuffer(_z);
            }
        }

        private unsafe void DrawBatchPerTexture(Texture texture, int offset, int count)
        {
            Effect.SetTexture(texture);
            var noOverwrite = SetDataOptions.NoOverwrite;
            
            var p = (Bitmap*)Vb.MapBuffer(_vertexBufferPosition * Bitmap.SizeInBytes, noOverwrite).ToPointer();
            for (int i = 0; i < count; i++)
                *p++ = _spriteQueue[offset + i].Bitmap;
            Vb.UnmapBuffer();

            App.Render.SetVertexBuffer(Vb, Marshal.SizeOf<Bitmap>());

            Effect.Render(_spriteQueue[offset].Tech, count, _vertexBufferPosition);

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