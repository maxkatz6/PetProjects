#pragma warning disable 618
using OpenTK;
using OpenTK.Graphics;
using Ormeli.CG;
using Ormeli.Core.Patterns;
using Ormeli.Math;
using System;
using System.Runtime.InteropServices;

namespace Ormeli.OpenGL
{
    internal sealed class OGRender : Disposable, IRenderClass
    {
        private GameWindow _gameWindow;

        private int lastAttrNum = -1;
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

            CgImports.cgGLSetDebugMode(0);
            CgImports.cgSetParameterSettingMode(CgEffect.CGcontext, CGenum.DeferredParameterSetting);
            CgImports.cgGLRegisterStates(CgEffect.CGcontext);
            HardwareDescription.VideoCardDescription = GL.GetString(StringName.Vendor).Split(' ')[0] +
                                                       GL.GetString(StringName.Renderer).Split('/')[0];
            HardwareDescription.VideoCardMemory = 0;
            return RenderType.OpenGl3;
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

        public void ChangeBackColor(Color color)
        {
            GL.ClearColor(color.R, color.G, color.B, color.A);
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

            var pass = CgImports.cgGetFirstPass(techInfo.CGtechnique);
            if (lastAttrNum != techInfo.AttribsContainerNumber)
            {
                EffectManager.AttribsContainers[techInfo.AttribsContainerNumber].Accept();
                lastAttrNum = techInfo.AttribsContainerNumber;
            }

            while (pass)
            {
                CgImports.cgSetPassState(pass);

                GL.DrawElements(BeginMode.Triangles, indexCount,
                    DrawElementsType.UnsignedInt, IntPtr.Zero);

                CgImports.cgResetPassState(pass);
                pass = CgImports.cgGetNextPass(pass);
            }
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

#pragma warning restore 618