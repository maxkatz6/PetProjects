using System;
using System.Runtime.InteropServices;
using Ormeli.Core;
using Ormeli.Graphics;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Buffer = Ormeli.Graphics.Buffer;
using Color = Ormeli.Math.Color;
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
            fileName = Config.GetTexturePath(fileName);
            ImageInformation fileInfo = ImageInformation.FromFile(fileName).Value;

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
            return new Texture(Resource.FromFile(_device, fileName, v).NativePointer);
        
        }

        public unsafe Texture CreateTexture(Color[,] array)
        {
            //ToDo next
            var boxTexDesc = new Texture2DDescription
            {
                ArraySize = 1,
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = 0,
                Format = Format.R32G32B32A32_Float,
                Height = array.GetLength(0),
                MipLevels = 1,
                OptionFlags = 0,
                Usage = ResourceUsage.Default,
                Width = array.GetLength(1),
                SampleDescription = new SampleDescription(1, 0),
            };

            DataBox[] v;
            fixed (void* p = &array[0, 0])
                v = new[]
                {
                    new DataBox(
                        new DataStream(new IntPtr(p), array.Length*16, true, true).DataPointer,
                        boxTexDesc.Width*(int) FormatHelper.SizeOfInBytes(boxTexDesc.Format), 0)
                };

            return new Texture (new Texture2D(_device, boxTexDesc, v).NativePointer);
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
                Marshal.SizeOf(typeof (T)),
                (ResourceUsage) bufferUsage,
                (BindFlags) bufferTarget,
                (SharpDX.Direct3D11.CpuAccessFlags) ((long) cpuAccessFlags*65536),
                ResourceOptionFlags.None,
                0
                );
            return new Buffer(new SharpDX.Direct3D11.Buffer(_device, vbd).NativePointer,
                bufferTarget == BindFlag.VertexBuffer ? Reflection.GetStatic<int, T>("Number") : -1);
        }

        public Buffer CreateBuffer<T>(T[] objs, BindFlag bufferTarget,
            BufferUsage bufferUsage = BufferUsage.Dynamic,
            CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write) where T : struct
        {
            var vbd = new BufferDescription(
                objs.Length*Marshal.SizeOf(typeof (T)),
                (ResourceUsage) bufferUsage,
                (BindFlags) bufferTarget,
                (SharpDX.Direct3D11.CpuAccessFlags) ((long) cpuAccessFlags*65536),
                ResourceOptionFlags.None, 0
                );
            return new Buffer(SharpDX.Direct3D11.Buffer.Create(_device, objs, vbd).NativePointer,
                bufferTarget == BindFlag.VertexBuffer ? Reflection.GetStatic<int, T>("Number") : -1);
        }
    }
}