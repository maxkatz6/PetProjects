using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using Ormeli.Cg;
using Ormeli.Core;
using Ormeli.Graphics;
using Bitmap = System.Drawing.Bitmap;
using Buffer = Ormeli.Graphics.Buffer;
using Color = Ormeli.Math.Color;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace Ormeli.OpenGL
{
    public class GlCreator : ICreator
    {
        private static int _pointer;
        public Texture LoadTexture(string fileName)
        {
            //TODO support DDS
            _pointer = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _pointer);
            var bmp = new Bitmap(Config.GetTexturePath(fileName));
            var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmpData.Width, bmpData.Height, 0,
                PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);

            bmp.UnlockBits(bmpData);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            return new Texture(new IntPtr(_pointer), bmpData.Width, bmpData.Height);
        }

        public unsafe Texture CreateTexture(Color[,] array)
        {
            _pointer = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _pointer);

            var v = array.GetLength(0);
            var s = array.GetLength(1);
            fixed (void* p = &array[0,0])
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, v, s, 0,
                PixelFormat.Rgba, PixelType.Float, new IntPtr(p));


            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            return new Texture(new IntPtr(_pointer), v,s);
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
            GL.GenBuffers(1, out _pointer);
            return new Buffer((IntPtr)_pointer, Reflection.GetStatic<int, T>("Number"));
        }

        public Buffer CreateBuffer<T>(T[] objs, BindFlag bufferTarget, BufferUsage bufferUsage = BufferUsage.Dynamic,
            CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write) where T : struct
        {
            var bf = (BufferTarget)(bufferTarget == BindFlag.ConstantBuffer ? 0x8a11 : 34961 + (int)bufferTarget);
            var bu = GetFromOrmeliEnum(bufferUsage, cpuAccessFlags);
            
            GL.GenBuffers(1, out _pointer);
            GL.BindBuffer(bf, _pointer);
            int l = Marshal.SizeOf(typeof(T));
            GL.BufferData(bf,
                new IntPtr(objs.Length * l),
                objs, bu);

            GL.GenVertexArrays(1, out _pointer);
            if (bf == BufferTarget.ArrayBuffer)
            {
                GL.BindVertexArray(_pointer);
                EffectManager.AcceptAttributes(Reflection.GetStatic<int, T>("Number"));
                return new Buffer((IntPtr)_pointer, Reflection.GetStatic<int, T>("Number"));
            }

            GL.BindBuffer(bf, 0);
            return new Buffer((IntPtr)_pointer);
        }

        public unsafe void SetDynamicData(Buffer buf, Action<IntPtr> fromData, int offsetInBytes = 0, SetDataOptions options = SetDataOptions.Discard)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, buf);
            var p = GL.MapBuffer(BufferTarget.ArrayBuffer, BufferAccess.WriteOnly);
            fromData((IntPtr) ((byte*) p + offsetInBytes));
            GL.UnmapBuffer(BufferTarget.ArrayBuffer);
        }

        public EffectBase CreateEffect()
        {
            EffectBase _base;
            switch (App.EffectLanguage)
            {
                case EffectLanguage.GLSL:
                    throw new Exception("Not supported effect yet");
                case EffectLanguage.CG:
                    _base = new CgEffectBase();
                    break;
                default:
                    throw new Exception("Not supported effect language");
            }
            return _base;
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
