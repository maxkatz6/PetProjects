using Buffer = Ormeli.Graphics.Buffer;
#pragma warning disable 618
using Ormeli.Graphics;
using OpenTK;
using OpenTK.Graphics;
using Ormeli.Cg;
using Ormeli.Core.Patterns;
using Ormeli.Math;
using System;

namespace Ormeli.OpenGL
{
    internal sealed class GlRender : Disposable, IRender
    {
        private GameWindow _gameWindow;

        private int lastAttrNum = -1;

        public Color BackColor
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                GL.ClearColor(_color.R, _color.G, _color.B, _color.A);
            }
        }

        private Color _color;
        public void CreateWindow()
        {
            _gameWindow = new GameWindow(Config.Width, Config.Height)
            {
                VSync = Config.VerticalSyncEnabled ? VSyncMode.On : VSyncMode.Off
            };
        }

        public RenderType Initialize()
        {
            GL.Enable(EnableCap.DepthTest);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.Viewport(0, 0, _gameWindow.Width, _gameWindow.Height);
            GL.Ortho(0, 0, _gameWindow.Width, _gameWindow.Height, -1, +1);

            CG.GL.SetDebugMode(0);
            CG.SetParameterSettingMode(CgEffect.Context, CG.Enum.DeferredParameterSetting);
            CG.GL.RegisterStates(CgEffect.Context);
            HardwareDescription.VideoCardDescription = GL.GetString(StringName.Vendor).Split(' ')[0] +
                                                       GL.GetString(StringName.Renderer).Split('/')[0];
            HardwareDescription.VideoCardMemory = 0;
            return RenderType.OpenGl3;
        }

        public ICreator GetCreator()
        {
            return new GlCreator();
        }


        public void Run(Action act)
        {
            _gameWindow.RenderFrame += (sender, args) => act();
            _gameWindow.Run();
        }

        public void BeginDraw()
        {
            GL.Viewport(0, 0, Config.Width, Config.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }


        public void EndDraw()
        {
            _gameWindow.SwapBuffers();
        }

        public void ZBuffer(bool turn)
        {
            //TODO not sure
            if (turn) GL.Enable(EnableCap.DepthTest);
            else GL.Disable(EnableCap.DepthTest);
        }

        public void AlphaBlending(bool turn)
        {
            //TODO not sure
            if (turn) GL.Enable(EnableCap.AlphaTest);
            else GL.Disable(EnableCap.AlphaTest);
        }

        //http://3dgep.com/?p=2665
        public void Draw(CgEffect.TechInfo techInfo, Buffer vertexBuffer, Buffer indexBuffer, int vertexStride, int indexCount)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, (uint)vertexBuffer.Handle);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, (uint)indexBuffer.Handle);

            var pass = CG.GetFirstPass(techInfo.Technique);
            if (lastAttrNum != techInfo.AttribsContainerNumber)
            {
                EffectManager.AttribsContainers[techInfo.AttribsContainerNumber].Accept();
                lastAttrNum = techInfo.AttribsContainerNumber;
            }

            while (pass)
            {
                CG.SetPassState(pass);

                GL.DrawElements(BeginMode.Triangles, indexCount,
                    DrawElementsType.UnsignedInt, IntPtr.Zero);

                CG.ResetPassState(pass);
                pass = CG.GetNextPass(pass);
            }
        }
    }
}

#pragma warning restore 618