using System;
using Ormeli.GAPI.Interfaces;

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
        private readonly IntPtr _handle;

        public int VertexType { get { return _vertexType; }}
        private readonly int _vertexType;

        public Buffer(IntPtr handle, int vertexType = -1)
        {
            _handle = handle;
            _vertexType = vertexType;
        }

        public static implicit operator IntPtr(Buffer buf)
        {
            return buf._handle;
        }

        public static implicit operator int(Buffer buf)
        {
            return (int)buf._handle;
        }

        public void SetDynamicData(Action<IntPtr> action, int offsetInBytes, SetDataOptions options = SetDataOptions.Discard)
        {
            App.Creator.SetDynamicData(this, action, offsetInBytes, options);
        }
    }

}