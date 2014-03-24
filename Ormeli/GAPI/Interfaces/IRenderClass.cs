using System;
using Ormeli.CG;
using Ormeli.Math;

namespace Ormeli
{
    public interface IRenderClass
    {
        void CreateWindow();
        RenderType Initialize();
        void Run(Action act);
        void BeginDraw();
        void ChangeBackColor(Color color);
        void Draw(CgEffect.TechInfo techInfo, Buffer vertexBuffer, Buffer indexBuffer, int vertexStride, int indexCount);
        void EndDraw();
        void ZBuffer(bool turn);
        void AlphaBlending(bool turn);
        void Dispose();
        IAttribsContainer InitAttribs(Attrib[] attribs, IntPtr ptr);

        Buffer CreateBuffer<T>(BindFlag bufferTarget, BufferUsage bufferUsage = BufferUsage.Dynamic,
            CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write) where T : struct;

        Buffer CreateBuffer<T>(T[] objs, BindFlag bufferTarget, BufferUsage bufferUsage = BufferUsage.Dynamic,
            CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write) where T : struct;
    }
}