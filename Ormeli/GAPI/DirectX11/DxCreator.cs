using System;
using System.Runtime.InteropServices;
using Ormeli.Graphics;
using SharpDX.Direct3D11;
using Buffer = Ormeli.Graphics.Buffer;
using CpuAccessFlags = Ormeli.Graphics.CpuAccessFlags;
using Device = SharpDX.Direct3D11.Device;
using Resource = SharpDX.Direct3D11.Resource;

namespace Ormeli.DirectX11
{
    public class DxCreator : ICreator
    {
        private readonly Device _device;

        public DxCreator(Device device)
        {
            _device = device;
        }

        public unsafe Texture LoadTexture(string fileName)
        {
            fileName = Config.TextureDirectory + fileName;
            var fileInfo = ImageInformation.FromFile(fileName).Value;

            var v = new ImageLoadInformation
            {
                Width = fileInfo.Width,
                Height = fileInfo.Height,
                FirstMipLevel = 0,
                MipLevels = fileInfo.MipLevels,
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = 0,
                OptionFlags = 0,
                Format = fileInfo.Format,
                Filter = FilterFlags.None,
                MipFilter = FilterFlags.None,
                PSrcInfo = new IntPtr(&fileInfo)
            };
            return new Texture
            {
                Handle = Resource.FromFile(_device, fileName, v).NativePointer
            };
        }

        public IAttribsContainer InitAttribs(Attrib[] attribs, IntPtr ptr)
        {
            var a = new DxAttribs(_device.ImmediateContext);
            a.Initialize(attribs, ptr);
            return a;
        }


        public Buffer CreateBuffer<T>(BindFlag bufferTarget, BufferUsage bufferUsage = BufferUsage.Dynamic,
            CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write) where T : struct
        {
            var vbd = new BufferDescription(
                Marshal.SizeOf(typeof(T)),
                (ResourceUsage)bufferUsage,
                (BindFlags)bufferTarget,
                (SharpDX.Direct3D11.CpuAccessFlags)((long)cpuAccessFlags * 65536),
                ResourceOptionFlags.None,
                0
                );
            return new Buffer(new SharpDX.Direct3D11.Buffer(_device, vbd).NativePointer, bufferTarget, bufferUsage,
                cpuAccessFlags);
        }

        public Buffer CreateBuffer<T>(T[] objs, BindFlag bufferTarget,
            BufferUsage bufferUsage = BufferUsage.Dynamic,
            CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write) where T : struct
        {
            var vbd = new BufferDescription(
                objs.Length * Marshal.SizeOf(typeof(T)),
                (ResourceUsage)bufferUsage,
                (BindFlags)bufferTarget,
                (SharpDX.Direct3D11.CpuAccessFlags)((long)cpuAccessFlags * 65536),
                ResourceOptionFlags.None, 0
                );
            return new Buffer(SharpDX.Direct3D11.Buffer.Create(_device, objs, vbd).NativePointer, bufferTarget,
                bufferUsage, cpuAccessFlags);
        }
    }
}
