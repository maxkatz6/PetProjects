using System;
using Ormeli.Graphics;
using Buffer = Ormeli.Graphics.Buffer;

namespace Ormeli
{
    public interface ICreator
    {
        Texture LoadTexture(string path);

        IAttribsContainer InitAttribs(Attrib[] attribs, IntPtr ptr);

        Buffer CreateBuffer<T>(BindFlag bufferTarget, BufferUsage bufferUsage = BufferUsage.Dynamic,
            CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write) where T : struct;

        Buffer CreateBuffer<T>(T[] objs, BindFlag bufferTarget, BufferUsage bufferUsage = BufferUsage.Dynamic,
            CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write) where T : struct;
    }
}
