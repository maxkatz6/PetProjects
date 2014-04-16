using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using OpenTK.Graphics;
using Ormeli.Graphics;
using Buffer = Ormeli.Graphics.Buffer;

namespace Ormeli.OpenGL
{
    public class GlCreator : ICreator
    {

        public Texture LoadTexture(string fileName)
        {
            //TODO support DDS
            var id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);
            var bmp = new Bitmap(Config.TextureDirectory + fileName);
            var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmpData.Width, bmpData.Height, 0,
                OpenTK.Graphics.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);

            bmp.UnlockBits(bmpData);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            return new Texture { Handle = new IntPtr(id) };
        }

        public IAttribsContainer InitAttribs(Attrib[] attribs, IntPtr ptr)
        {
            var a = new GlAttribs();
            a.Initialize(attribs, ptr);
            return a;
        }

        public Buffer CreateBuffer<T>(BindFlag bufferTarget, BufferUsage bufferUsage = BufferUsage.Dynamic,
            CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write) where T : struct
        {
            uint pointer;
            GL.GenBuffers(1, out pointer);
            return new Buffer((IntPtr)pointer, bufferTarget, bufferUsage, cpuAccessFlags);
        }

        public Buffer CreateBuffer<T>(T[] objs, BindFlag bufferTarget, BufferUsage bufferUsage = BufferUsage.Dynamic,
            CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write) where T : struct
        {
            var bf = (BufferTarget)(bufferTarget == BindFlag.ConstantBuffer ? 0x8a11 : 34961 + (int)bufferTarget);
            BufferUsageHint bu = GetFromOrmeliEnum(bufferUsage, cpuAccessFlags);
            uint pointer;
            GL.GenBuffers(1, out pointer);
            GL.BindBuffer(bf, pointer);
            int l = Marshal.SizeOf(typeof(T));
            GL.BufferData(bf,
                new IntPtr(objs.Length * l),
                objs, bu);
            GL.BindBuffer(bf, 0);
            return new Buffer((IntPtr)pointer, bufferTarget, bufferUsage, cpuAccessFlags);
        }

        private static BufferUsageHint GetFromOrmeliEnum(BufferUsage bu, CpuAccessFlags cpu)
        {
            switch (bu)
            {
                case BufferUsage.Dynamic:
                    switch (cpu)
                    {
                        case CpuAccessFlags.Read:
                            return BufferUsageHint.DynamicRead;

                        case CpuAccessFlags.Write:
                            return BufferUsageHint.DynamicDraw;

                        default:
                            return BufferUsageHint.DynamicCopy;
                    }
                case BufferUsage.Immutable:
                    switch (cpu)
                    {
                        case CpuAccessFlags.Read:
                            return BufferUsageHint.StreamRead;

                        case CpuAccessFlags.Write:
                            return BufferUsageHint.StreamDraw;

                        default:
                            return BufferUsageHint.StreamCopy;
                    }
                default:
                    switch (cpu)
                    {
                        case CpuAccessFlags.Read:
                            return BufferUsageHint.StaticRead;

                        case CpuAccessFlags.Write:
                            return BufferUsageHint.StaticDraw;

                        default:
                            return BufferUsageHint.StaticCopy;
                    }
            }
        }
    }
}
