﻿#if DX
using System;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.Direct3D11;

namespace Lain.GAPI
{
    public enum BindFlag
    {
        VertexBuffer = 1,
        IndexBuffer = 2,
        ConstantBuffer = 4
    }

    public enum SetDataOptions
    {
        NoOverwrite = MapMode.WriteNoOverwrite,
        Discard = MapMode.WriteDiscard
    }

    public enum BufferUsage
    {
        Default = 0,
        Immutable = 1,
        Dynamic = 2,
        DXStaging = 3
    }

    public enum CpuAccessFlag
    {
        Write = CpuAccessFlags.Write,
        Read = CpuAccessFlags.Read,
        None = CpuAccessFlags.None
    }

    public struct Buffer
    {
        private readonly SharpDX.Direct3D11.Buffer _buffer;

        private Buffer(SharpDX.Direct3D11.Buffer handle)
        {
            _buffer = handle;
        }

        public static implicit operator SharpDX.Direct3D11.Buffer(Buffer buf)
        {
            return buf._buffer;
        }

        public static implicit operator IntPtr(Buffer buf)
        {
            return buf._buffer.NativePointer;
        }

        public static implicit operator int(Buffer buf)
        {
            return (int) buf._buffer.NativePointer;
        }

        public static Buffer Create<T>(BindFlag bufferTarget, BufferUsage bufferUsage = BufferUsage.Dynamic,
            CpuAccessFlag cpuAccessFlags = CpuAccessFlag.Write) where T : struct
        {
            var vbd = new BufferDescription(
                Marshal.SizeOf<T>(),
                (ResourceUsage) bufferUsage,
                (BindFlags) bufferTarget,
                (CpuAccessFlags) cpuAccessFlags,
                ResourceOptionFlags.None,
                0
                );
            return new Buffer(new SharpDX.Direct3D11.Buffer(App.Render.Device, vbd));
        }

        public static Buffer Create<T>(T[] objs, BindFlag bufferTarget,
            BufferUsage bufferUsage = BufferUsage.Dynamic,
            CpuAccessFlag cpuAccessFlags = CpuAccessFlag.Write) where T : struct
        {
            var vbd = new BufferDescription(
                objs.Length*Marshal.SizeOf<T>(),
                (ResourceUsage) bufferUsage,
                (BindFlags) bufferTarget,
                (CpuAccessFlags) cpuAccessFlags,
                ResourceOptionFlags.None, 0
                );
            return new Buffer(SharpDX.Direct3D11.Buffer.Create(App.Render.Device, objs, vbd));
        }

        public void Write<T>(T[] arr, SetDataOptions mapmode = SetDataOptions.Discard) where T : struct
        {
            DataStream s;
            App.Render.DeviceContext.MapSubresource(_buffer, 0, (MapMode) mapmode, MapFlags.None, out s);
            s.WriteRange(arr);
            UnmapBuffer();
        }

        public unsafe IntPtr MapBuffer(int offset = 0, SetDataOptions mapmode = SetDataOptions.Discard)
        {
            return
                (IntPtr)
                    ((byte*)
                        App.Render.DeviceContext.MapSubresource(_buffer, 0, (MapMode) mapmode, MapFlags.None)
                            .DataPointer + offset);
        }

        public void UnmapBuffer()
        {
            App.Render.DeviceContext.UnmapSubresource(_buffer, 0);
        }
    }
}

#endif