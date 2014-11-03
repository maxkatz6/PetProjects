﻿using System;
using System.Runtime.InteropServices;
using Ormeli.Core.Patterns;
﻿using Ormeli.Graphics.Cameras;
﻿using Ormeli.Graphics.Effects;
﻿using Ormeli.Math;
﻿using SharpDX;
/*
namespace Ormeli.Graphics
{
    public class Batch : Disposable
    {
        private const int MaxBatchSize = 2048;
        private const int MinBatchSize = 128;
        private const int VerticesPerSprite = 4;
        private const int IndicesPerSprite = 6;
        private const int MaxVertexCount = MaxBatchSize * VerticesPerSprite;
        private const int MaxIndexCount = MaxBatchSize * IndicesPerSprite;

        private static readonly Buffer IndexBuffer;
        private Buffer _vertexBuffer;
        public Effect2D Effect { get; set; }

        private SpriteInfo[] _spriteQueue;
        private int _spriteQueueCount;

        private int _vertexBufferPosition;

        static Batch()
        {
            var indices = new int[MaxIndexCount];
            for (int i = 0, k = 0; i < MaxIndexCount; k += VerticesPerSprite)
            {
                indices[i++] = (k + 0);
                indices[i++] = (k + 1);
                indices[i++] = (k + 2);
                indices[i++] = (k + 1);
                indices[i++] = (k + 3);
                indices[i++] = (k + 2);
            }

            IndexBuffer = App.Creator.CreateBuffer(indices, BindFlag.IndexBuffer, BufferUsage.Default, CpuAccessFlags.None);
        }

        public Batch()
        {
            _spriteQueue = new SpriteInfo[MaxBatchSize];

            Effect = Ormeli.Effect.Get<Effect2D>(1);

            _vertexBuffer = App.Creator.CreateBuffer(new BitmapVertex[MaxVertexCount], BindFlag.VertexBuffer);  
        }

        public void Begin()
        {
            _spriteQueueCount = 0;
            _vertexBufferPosition = 0;
        }

        public void Draw(int texture, RectangleF destinationRectangle, Rectangle? sourceRectangle, Color color)
        {     
            // Resize the buffer of SpriteInfo
            if (_spriteQueueCount >= _spriteQueue.Length)
            {
                Array.Resize(ref _spriteQueue, _spriteQueue.Length * 2);
            }

            _spriteQueue[_spriteQueueCount++] = new SpriteInfo
            {
                Color = color,
                Texture = texture,
                CustomSource = sourceRectangle.HasValue,
                Source = sourceRectangle.HasValue ? new RectangleF(sourceRectangle.Value) : new RectangleF(0, 0, Texture.Get(texture).Width, Texture.Get(texture).Height),
                Destination = destinationRectangle
            }; 
        }

        bool _a, _z;
        public void End()
        {
            if (_spriteQueueCount > 0)
            {
                _a = App.Render.AlphaBlending(true);
                _z = App.Render.ZBuffer(false);

                Effect.SetMatrix(Camera.Current.Ortho);

                App.Render.SetBuffers(_vertexBuffer, IndexBuffer, BitmapVertex.SizeInBytes);
                
                FlushBatch();
            }
            App.Render.AlphaBlending(_a);
            App.Render.ZBuffer(_z);
        }

        private void FlushBatch()
        {
            // Iterate on all sprites and group batch per texture.
            int offset = 0;
            int previousTexture = -1;
            for (int i = 0; i < _spriteQueueCount; i++)
            {
                int texture = _spriteQueue[i].Texture;
                

                if (texture != previousTexture)
                {
                    if (i > offset)
                    {
                        DrawBatchPerTexture(previousTexture, offset, i - offset);
                    }
                    offset = i;
                    previousTexture = texture;
                }
            }

            // Draw the last batch
            DrawBatchPerTexture(previousTexture, offset, _spriteQueueCount - offset);
        }

        private void DrawBatchPerTexture(int texture, int offset, int count)
        {
            Effect.SetTexture(texture);
            Effect.Render("Texture", () => DrawBatchPerTextureAndPass(texture, offset, count));
        }

        private unsafe void DrawBatchPerTextureAndPass(int texture, int offset, int count)
        {
            float deltaX = 1f / (Texture.Get(texture).Width);
            float deltaY = 1f / (Texture.Get(texture).Height);
            while (count > 0)
            {
                var noOverwrite = SetDataOptions.NoOverwrite;

                // How many sprites do we want to draw?
                int batchSize = count;

                // How many sprites does the D3D vertex buffer have room for?
                int remainingSpace = MaxBatchSize - _vertexBufferPosition;
                if (batchSize > remainingSpace)
                {
                    if (remainingSpace < MinBatchSize)
                    {
                        _vertexBufferPosition = 0;
                        noOverwrite = SetDataOptions.Discard;
                        batchSize = (count < MaxBatchSize) ? count : MaxBatchSize;
                    }
                    else
                        batchSize = remainingSpace;
                }

                // Sets the data directly to the buffer in memory
                int offsetInBytes = _vertexBufferPosition * VerticesPerSprite * BitmapVertex.SizeInBytes;
                _vertexBuffer.SetDynamicData(ptr =>
                {
                    var p = (BitmapVertex*)ptr;
                    for (int i = 0; i < batchSize; i++)
                    {
                        var sp = _spriteQueue[offset + i];
                        int left = (-(Config.Width >> 1)) + (int)sp.Destination.X;
                        int right = left + (int)sp.Destination.Width;
                        int top = (Config.Height >> 1) + (int)sp.Destination.Y;
                        int bottom = top - (int)sp.Destination.Height;

                        p++->Position = new Vector2(left, top);
                        p++->Position = new Vector2(right, top);
                        p++->Position = new Vector2(left, bottom);
                        p->Position = new Vector2(right, bottom);
                        p -= 3;

                      //  if (!sp.CustomSource) continue;
                        var v = new Vector2(sp.Source.X, sp.Source.Y);
                        var w = sp.Source.Width*deltaX;
                        var h = sp.Source.Height * deltaY;
                        p++->TexCoord = v;
                        p++->TexCoord = v + new Vector2(w, 0);
                        p++->TexCoord = v + new Vector2(0, h);
                        p->TexCoord = v + new Vector2(w, h);
                    }
                }, offsetInBytes, noOverwrite);

                // Draw from the specified index
                int startIndex = _vertexBufferPosition * IndicesPerSprite;
                int indexCount = batchSize * IndicesPerSprite;
                App.Render.Draw(indexCount,startIndex);

                // Update position, offset and remaining count
                _vertexBufferPosition += batchSize;
                offset += batchSize;
                count -= batchSize;
            }
        }
        #region Nested type: SpriteInfo

        [StructLayout(LayoutKind.Sequential)]
        public struct SpriteInfo // TODO данные о вершинах хранить в этой структуре (переименновать в Sprite). При изменении данных, редактировать вершины.
        {
            public int Texture;
            public RectangleF Source;
            public bool CustomSource;
            public RectangleF Destination;
            public Color Color;
        }

        #endregion

    }
}*/