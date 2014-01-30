using System;
using OpenTK.Graphics;
using Ormeli.App;
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

        public override void BeginDraw(Color color)
        {
            GL.ClearColor(color.R, color.G, color.B, color.A);
            BeginDraw();
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

        public override void Render(Buffer vertexBuffer, Buffer indexBuffer, int vertexStride, int indexCount)
        {
            throw new NotImplementedException();
        }

        private static Color4 ToGLColor(Color d)
        {
            return new Color4(d.R, d.G, d.B, d.A);
        }
    }
}
