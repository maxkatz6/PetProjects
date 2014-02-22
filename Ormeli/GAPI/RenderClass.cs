using System;
using Ormeli.Core.Patterns;
using Ormeli.Math;

namespace Ormeli
{
    public abstract class RenderClass : Disposable
    {
        public abstract void CreateWindow();
        public abstract RenderType Initialize();
        public abstract void InitCG();
        public abstract void Run(Action act);
        public abstract void BeginDraw();
        public abstract void ChangeBackColor(Color color);
        public abstract void DrawBuffer(Buffer vertexBuffer, Buffer indexBuffer, int vertexStride, int indexCount);
        public abstract void EndDraw();
        public abstract void TurnZBufferOff();
        public abstract void TurnZBufferOn();
        public abstract void TurnOnAlphaBlending();
        public abstract void TurnOffAlphaBlending();
        public abstract Buffer CreateBuffer<T>(BindFlag bufferTarget, BufferUsage bufferUsage = BufferUsage.Dynamic, CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write) where T : struct;
        public abstract Buffer CreateBuffer<T>(T obj, BindFlag bufferTarget, BufferUsage bufferUsage = BufferUsage.Dynamic, CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write) where T : struct;
        public abstract Buffer CreateBuffer<T>(T[] objs, BindFlag bufferTarget, BufferUsage bufferUsage = BufferUsage.Dynamic, CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write) where T : struct;
    }
}
