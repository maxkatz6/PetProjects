using System;
using System.Runtime.InteropServices;
using OpenTK.Graphics;
using Ormeli.Math;

namespace Ormeli.OpenGL3
{
    public class OGRender : RenderClass
    {
        public override void Initialize(IntPtr handle)
        {
            throw new NotImplementedException();
        }

        public override void BeginDraw()
        {
            GL.Viewport(0, 0, Config.Width, Config.Height);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        //    SwapBuffers();
        }

        public override void ChangeBackColor(Color color)
        {
            GL.ClearColor(color.R, color.G, color.B, color.A);
        }

        public override void EndDraw()
        {
            throw new NotImplementedException();
        }

        public override void TurnZBufferOff()
        {
            throw new NotImplementedException();
        }

        public override void TurnZBufferOn()
        {
            throw new NotImplementedException();
        }

        public override void TurnOnAlphaBlending()
        {
            throw new NotImplementedException();
        }

        public override void TurnOffAlphaBlending()
        {
            throw new NotImplementedException();
        }

        public override void DrawBuffer(Buffer vertexBuffer, Buffer indexBuffer, int vertexStride, int indexCount)
        {
            throw new NotImplementedException();
        }

        public override Buffer CreateBuffer<T>(BindFlag bufferTarget, BufferUsage bufferUsage = BufferUsage.Dynamic, CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write)
        {
            uint pointer;
            GL.GenBuffers(1, out pointer);
            return new Buffer((IntPtr)pointer,bufferTarget,bufferUsage,cpuAccessFlags);
        }

        public override Buffer CreateBuffer<T>(T obj, BindFlag bufferTarget, BufferUsage bufferUsage = BufferUsage.Dynamic,
            CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write)
        {
            //TODO DESTROY with fire THIS SHIT
            //how i did it?
            var bf = (BufferTarget)(bufferTarget == BindFlag.ConstantBuffer ? 0x8a11 : 34961+(int)bufferTarget);
            var bu =
                (BufferUsageHint)
                    (35040 + ((int)bufferUsage == 3 ? 0 : (int)bufferUsage * 3) + cpuAccessFlags == CpuAccessFlags.None
                        ? 3 : ((int)cpuAccessFlags - 1));
            uint pointer;
            GL.GenBuffers(1, out pointer);
            GL.BindBuffer(bf, pointer);
            GL.BufferData(bf,
               new IntPtr(Marshal.SizeOf(typeof(T))),
               new []{obj}, bu);

            return new Buffer((IntPtr)pointer, bufferTarget, bufferUsage, cpuAccessFlags);
        }

        public override Buffer CreateBuffer<T>(T[] objs, BindFlag bufferTarget, BufferUsage bufferUsage = BufferUsage.Dynamic,
            CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write)
        {
            var bf = (BufferTarget)(bufferTarget == BindFlag.ConstantBuffer ? 0x8a11 : 34961 + (int)bufferTarget);
            var bu =
                (BufferUsageHint)
                    (35040 + ((int) bufferUsage == 3 ? 0 : (int) bufferUsage*3) + cpuAccessFlags == CpuAccessFlags.None
                        ? 3
                        : ((int) cpuAccessFlags - 1));
            uint pointer;
            GL.GenBuffers(1, out pointer);
            GL.BindBuffer(bf, pointer);
            GL.BufferData(bf,
                new IntPtr(objs.Length * Marshal.SizeOf(typeof(T))),
                objs, bu);
            return new Buffer((IntPtr)pointer, bufferTarget, bufferUsage, cpuAccessFlags);
        }

        private static Color4 ToGLColor(Color d)
        {
            return new Color4(d.R, d.G, d.B, d.A);
        }
    }
}
