using Ormeli.Graphics.Components;
using SharpDX;

namespace Ormeli.GAPI.Interfaces
{
    public interface IRender
    {
        Color4 BackColor { get; set; }
        RenderType Initialize();
        ICreator GetCreator();
        void CreateWindow();
        void Run(System.Action act);
        void BeginDraw();
        void SetBuffers(Buffer vertexBuffer, Buffer indexBuffer, int vertexStride);
        void Draw(int indexCount);
        void Draw(int indexCount, int startIndex);
        void EndDraw();
        /// <summary>
        /// Sets Z Buffer on
        /// </summary>
        /// <param name="turn">New state</param>
        /// <returns>Old state</returns>
        bool ZBuffer(bool turn);
        /// <summary>
        /// Sets Alpha Blending on
        /// </summary>
        /// <param name="turn">New state</param>
        /// <returns>Old state</returns>
        bool AlphaBlending(bool turn);
        void Dispose();
    }
}