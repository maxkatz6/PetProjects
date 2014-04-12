using System;
using Ormeli.Cg;
using Ormeli.Math;
using Buffer = Ormeli.Graphics.Buffer;

namespace Ormeli
{
    public interface IRender
    {
        Color BackColor { get; set; }
        void CreateWindow();
        RenderType Initialize();
        ICreator GetCreator();
        void Run(Action act);
        void BeginDraw();
        void Draw(CgEffect.TechInfo techInfo, Buffer vertexBuffer, Buffer indexBuffer, int vertexStride, int indexCount);
        void EndDraw();
        void ZBuffer(bool turn);
        void AlphaBlending(bool turn);
        void Dispose();
    }
}