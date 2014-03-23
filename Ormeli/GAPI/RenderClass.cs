using System;
using Ormeli.CG;
using Ormeli.Math;

namespace Ormeli
{
    public interface IRenderClass
    {
        void CreateWindow();
        RenderType Initialize();
        CgEffect InitCgShader(string file);
        void Run(Action act);
        void BeginDraw();
        void ChangeBackColor(Color color);
        void Draw(CgEffect cgEffect, Buffer vertexBuffer, Buffer indexBuffer, int vertexStride, int indexCount);
        void EndDraw();
        void ZBuffer(bool turn);
        void AlphaBlending(bool turn);
        void Dispose();

        Buffer CreateBuffer<T>(BindFlag bufferTarget, BufferUsage bufferUsage = BufferUsage.Dynamic,
            CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write) where T : struct;

        Buffer CreateBuffer<T>(T obj, BindFlag bufferTarget, BufferUsage bufferUsage = BufferUsage.Dynamic,
            CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write) where T : struct;

        Buffer CreateBuffer<T>(T[] objs, BindFlag bufferTarget, BufferUsage bufferUsage = BufferUsage.Dynamic,
            CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write) where T : struct;
    }
}