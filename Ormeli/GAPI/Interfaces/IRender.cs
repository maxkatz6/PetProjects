using Ormeli.Math;
using Ormeli.Graphics;

namespace Ormeli
{
    public interface IRender
    {
        Color BackColor { get; set; }
        RenderType Initialize();
        ICreator GetCreator();
        void CreateWindow();
        void Run(System.Action act);
        void BeginDraw();
        void SetBuffers(Buffer vertexBuffer, Buffer indexBuffer, int vertexStride);
        void Draw(int indexCount);
        void EndDraw();
        void ZBuffer(bool turn);
        void AlphaBlending(bool turn);
        void Dispose();
    }
}