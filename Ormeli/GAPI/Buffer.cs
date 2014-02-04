using System;

namespace Ormeli
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
  }*/
    //dx
}