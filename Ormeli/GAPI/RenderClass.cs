using System;
using Ormeli.Core.Patterns;
using Ormeli.Math;

namespace Ormeli
{
    public abstract class RenderClass : Disposable
    {
        public abstract void Initialize(IntPtr handle);
        public abstract void BeginDraw();
        public abstract void BeginDraw(Color color);
        public abstract void EndDraw();
        public abstract void TurnZBufferOff();
        public abstract void TurnZBufferOn();
        public abstract void TurnOnAlphaBlending();
        public abstract void TurnOffAlphaBlending();
        public abstract void Render(Buffer vertexBuffer, Buffer indexBuffer, int vertexStride, int indexCount);
        public abstract Buffer CreateBuffer<T>(BindFlag bufferTarget, BufferUsage bufferUsage = BufferUsage.Dynamic, CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write) where T : struct;
        public abstract Buffer CreateBuffer<T>(T obj, BindFlag bufferTarget, BufferUsage bufferUsage = BufferUsage.Dynamic, CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write) where T : struct;
        public abstract Buffer CreateBuffer<T>(T[] objs, BindFlag bufferTarget, BufferUsage bufferUsage = BufferUsage.Dynamic, CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write) where T : struct;
    }
}
