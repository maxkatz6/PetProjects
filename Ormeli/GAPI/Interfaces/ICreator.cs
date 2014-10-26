﻿using System;
using Ormeli.Graphics;
using Ormeli.Math;
using Buffer = Ormeli.Graphics.Buffer;

namespace Ormeli
{
    public enum SetDataOptions
    {
        NoOverwrite,
        Discard
    }
    public interface ICreator
    {
        Texture LoadTexture(string path);
        Texture CreateTexture(Color[,] array);

        IAttribsContainer InitAttribs(Attrib[] attribs, IntPtr ptr);

        Buffer CreateBuffer<T>(BindFlag bufferTarget, BufferUsage bufferUsage = BufferUsage.Dynamic,
            CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write) where T : struct;

        Buffer CreateBuffer<T>(T[] objs, BindFlag bufferTarget, BufferUsage bufferUsage = BufferUsage.Dynamic,
            CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write) where T : struct;

        void SetDynamicData(Buffer buf, Action<IntPtr> fromData, int offsetInBytes = 0,
            SetDataOptions options = SetDataOptions.Discard);

        EffectBase CreateEffect();
    }
}
