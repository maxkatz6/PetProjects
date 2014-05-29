using Ormeli.Graphics;
using Ormeli.Math;

namespace Ormeli
{
    public interface ICreator
    {
        Texture LoadTexture(string path);
        Texture CreateTexture(Color[,] array);

        IAttribsContainer InitAttribs(Attrib[] attribs, System.IntPtr ptr);

        Buffer CreateBuffer<T>(BindFlag bufferTarget, BufferUsage bufferUsage = BufferUsage.Dynamic,
            CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write) where T : struct;

        Buffer CreateBuffer<T>(T[] objs, BindFlag bufferTarget, BufferUsage bufferUsage = BufferUsage.Dynamic,
            CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write) where T : struct;

        EffectBase CreateEffect(string file);
    }
}
