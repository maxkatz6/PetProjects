using System;

namespace Ormeli.Graphics
{
    public enum BindFlag
    {
        VertexBuffer = 1,
        IndexBuffer = 2,
        ConstantBuffer = 4
    }

    public enum BufferUsage
    {
        Default = 0,
        Immutable = 1,
        Dynamic = 2,
        DXStaging = 3
    }

    public enum CpuAccessFlags
    {
        Write = 1,
        Read = 2,
        None = 0
    }

    public struct Buffer
    {
        public BindFlag BindFlag;

        public BufferUsage BufferUsage;

        public CpuAccessFlags CpuAccessFlags;

        public IntPtr Handle;

        public Buffer(IntPtr handle, BindFlag bufferTarget, BufferUsage bufferUsage, CpuAccessFlags cpuAccessFlags)
        {
            Handle = handle;
            BindFlag = bufferTarget;
            BufferUsage = bufferUsage;
            CpuAccessFlags = cpuAccessFlags;
        }
    }
}