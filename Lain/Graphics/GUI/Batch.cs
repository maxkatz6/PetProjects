﻿using System;
using System.Runtime.InteropServices;
﻿using Lain.GAPI;
﻿using SharpDX;
﻿using Buffer = Lain.GAPI.Buffer;

namespace Lain.Graphics
{
	//TODO//
    public class SpriteBatch
    {
        private const int MaxBatchSize = 2048;
        private const int MinBatchSize = 128;
        private const int VerticesPerSprite = 4;
        private const int IndicesPerSprite = 6;
        private const int MaxVertexCount = MaxBatchSize * VerticesPerSprite;
        private const int MaxIndexCount = MaxBatchSize * IndicesPerSprite;

	    private Effect Effect;
		public Buffer Vb { get; }
		public Buffer Ib { get; }
		private Sprite[] _spriteQueue;
        private int _spriteQueueCount;

        private int _vertexBufferPosition;
	    private static readonly int[] indices;
        static SpriteBatch()
        {
            indices = new int[MaxIndexCount];
            for (int i = 0, k = 0; i < MaxIndexCount; k += VerticesPerSprite)
            {
                indices[i++] = (k + 0);
                indices[i++] = (k + 1);
                indices[i++] = (k + 2);
                indices[i++] = (k + 1);
                indices[i++] = (k + 3);
                indices[i++] = (k + 2);
            }
        }

        public SpriteBatch()
        {
			Effect = Effect.FromFile("BitmapEffect.hlsl");
			Vb = Buffer.Create(new BitmapVertex[MaxVertexCount], BindFlag.VertexBuffer);
			Ib = Buffer.Create(indices, BindFlag.IndexBuffer, BufferUsage.Default, CpuAccessFlag.None);
            _spriteQueue = new Sprite[MaxBatchSize];
        }

	    public void Begin()
        {
            _spriteQueueCount = 0;
            _vertexBufferPosition = 0;
        }

        public void Draw(Texture texture, RectangleF destinationRectangle, RectangleF? sourceRectangle = null/*, Color color*/)
        {     
            // Resize the buffer of SpriteInfo
            if (_spriteQueueCount >= _spriteQueue.Length)
            {
                Array.Resize(ref _spriteQueue, _spriteQueue.Length * 2);
            }

            _spriteQueue[_spriteQueueCount++] = new Sprite
            {
                //Color = color,
                Texture = texture,
                CustomSource = true,// sourceRectangle.HasValue,
                Source = sourceRectangle ?? new RectangleF(0, 0, texture.Width, texture.Height),
                Destination = destinationRectangle
            }; 
        }
		
        public void End()
        {
            if (_spriteQueueCount > 0)
			{
				var _z = App.Render.IsDepthEnabled;
				App.Render.ZBuffer(false);

				//Draw(Scene.Current.Camera.Ortho);
				
                FlushBatch();
				App.Render.ZBuffer(_z);
			}
        }

        private void FlushBatch()
        {
            // Iterate on all sprites and group batch per texture.
            int offset = 0;
            Texture previousTexture = Texture.Null;
            for (int i = 0; i < _spriteQueueCount; i++)
            {
                Texture texture = _spriteQueue[i].Texture;
                

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

        private void DrawBatchPerTexture(Texture texture, int offset, int count)
        {
            Effect.Render(count, false);
            Effect.SetTexture(texture);
            DrawBatchPerTextureAndPass(texture, offset, count);
        }

        private unsafe void DrawBatchPerTextureAndPass(Texture texture, int offset, int count)
        {
            float deltaX = 1f / (texture.Width);
            float deltaY = 1f / (texture.Height);
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
				var ptr = Vb.MapBuffer(_vertexBufferPosition * VerticesPerSprite * BitmapVertex.SizeInBytes, noOverwrite).ToPointer();
                var p = (BitmapVertex*)ptr;
                for (int i = 0; i < batchSize; i++)
                {
                    var sp = _spriteQueue[offset + i];

                    var left = sp.Destination.X / Config.Width * 2 - 1;
                    var right = left + sp.Destination.Width / Config.Width * 2;
                    var top = -(sp.Destination.Y / Config.Height * 2 - 1);
                    var bottom = top - (sp.Destination.Height / Config.Height * 2); //є[-1;1]

                    p++->Location = new Vector2(left, top);
                    p++->Location = new Vector2(right, top);
                    p++->Location = new Vector2(left, bottom);
                    p++->Location = new Vector2(right, bottom);

                    if (sp.CustomSource) p -= 4;
					else continue;

                    var v = new Vector2(sp.Source.X, sp.Source.Y);
                    var w = sp.Source.Width*deltaX;
                    var h = sp.Source.Height * deltaY;
                    p++->TexCoord = v;
                    p++->TexCoord = v + new Vector2(w, 0);
                    p++->TexCoord = v + new Vector2(0, h);
                    p++->TexCoord = v + new Vector2(w, h);
                }
				Vb.UnmapBuffer();
				
                // Draw from the specified index
                int startIndex = _vertexBufferPosition * IndicesPerSprite;
                int indexCount = batchSize * IndicesPerSprite;
                App.Render.SetIndexBuffer(Ib);
                App.Render.SetVertexBuffer(Vb, BitmapVertex.SizeInBytes);
                App.Render.Draw(indexCount,startIndex);

                // Update position, offset and remaining count
                _vertexBufferPosition += batchSize;
                offset += batchSize;
                count -= batchSize;
            }
        }
        #region Nested type: SpriteInfo

        [StructLayout(LayoutKind.Sequential)]
        public struct Sprite
        {
            public Texture Texture;
            public RectangleF Source;
            public bool CustomSource;
            public RectangleF Destination;
            public Color Color;
        }
        #endregion
    }
}