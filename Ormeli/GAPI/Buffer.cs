using System;
using Ormeli.Core.Patterns;

namespace Ormeli
{
    public enum BindFlag
    {
        VertexBuffer = 1,
        IndexBuffer = 2,
        ConstantBuffer = 4
    }
  /*  public enum BindFlag
     {
         ArrayBuffer = 34961 + 1 ,
         ElementArrayBuffer = 34961 + 2,
         PixelPackBuffer = 34961 + 90,
         PixelUnpackBuffer = 34961 + 91,
         UniformBuffer =0x8a11,
         TextureBuffer =34961 + 921,
         TransformFeedbackBuffer = 34961 + 1021,
         CopyReadBuffer = 34961 + 1701,
         CopyWriteBuffer = 34961 + 1702,
     } //og
 /*   public enum BindFlag
    {
        None = 0,
        VertexBuffer = 1,
        IndexBuffer = 2,
        ConstantBuffer = 4,
        ShaderResource = 8,
        StreamOutput = 16,
        RenderTarget = 32,
        DepthStencil = 64,
        UnorderedAccess = 128,
        Decoder = 512,
        VideoEncoder = 1024
    }*/ //dx
    /// <summary>
    /// Staging is ONLY for DX.
    /// </summary>
    public enum BufferUsage
    {
        Default = 0,
        Immutable = 1,
        Dynamic = 2,
        Staging = 3
    }
    public enum CpuAccessFlags
    {
        Write = 1, //DX     *65536
        Read = 2,
        None
    } 

    public class Buffer : Disposable
    {
        internal Buffer(IntPtr handle, BindFlag bufferTarget, BufferUsage bufferUsage, CpuAccessFlags cpuAccessFlags)
        {
            Handle = handle;
            BindFlag = bufferTarget;
            BufferUsage = bufferUsage;
            CpuAccessFlags = cpuAccessFlags;
        }

        public IntPtr Handle { get; private set; }
        public BindFlag BindFlag { get; private set; }
        public BufferUsage BufferUsage { get; private set; }
        public CpuAccessFlags CpuAccessFlags { get; private set; }
    }
}