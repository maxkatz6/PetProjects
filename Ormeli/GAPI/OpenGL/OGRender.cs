#pragma warning disable 618

using OpenTK;
using OpenTK.Graphics;
using Ormeli.CG;
using Ormeli.Core;
using Ormeli.Core.Patterns;
using Ormeli.Math;
using System;
using System.Runtime.InteropServices;

namespace Ormeli.OpenGL
{
    internal sealed class OGRender : Disposable, IRenderClass
    {
        private GameWindow _gameWindow;

        public void CreateWindow()
        {
            _gameWindow = new GameWindow(Config.Width, Config.Height)
            {
                VSync = Config.VerticalSyncEnabled ? VSyncMode.On : VSyncMode.Off
            };
        }

        public RenderType Initialize()
        {
            // Other state
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
            return RenderType.OpneGl3;
        }

        public CgEffect InitCgShader(string file)
        {
            CGeffect cgEffect = CgImports.cgCreateEffectFromFile(CgEffect.CGcontext, Config.ShadersDirectory + file,
                null);
            CGtechnique myCgTechnique = CgImports.cgGetFirstTechnique(cgEffect);

            while (myCgTechnique && CgImports.cgValidateTechnique(myCgTechnique) != 1)
            {
                Console.WriteLine("Ormeli: Technique {0} did not validate.  Skipping.\n",
                    CgImports.cgGetTechniqueName(myCgTechnique).ToStr());
                myCgTechnique = CgImports.cgGetNextTechnique(myCgTechnique);
            }

            if (myCgTechnique)
            {
                Console.WriteLine("Ormeli: Use technique {0}.\n", CgImports.cgGetTechniqueName(myCgTechnique).ToStr());
            }
            else
            {
                ErrorProvider.SendError("Ormeli: No valid technique.", true);
            }

            return new CgEffect
            {
                CGeffect = cgEffect,
                CGtechnique = myCgTechnique
            };
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
        public void Draw(CgEffect cgEffect, Buffer vertexBuffer, Buffer indexBuffer, int vertexStride, int indexCount)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, (uint)vertexBuffer.Handle);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, (uint)indexBuffer.Handle);

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(3);
            GL.VertexAttribPointer((int)AttribIndex.Position, 3, VertexAttribPointerType.Float, false, 28, 0);
            GL.VertexAttribPointer((int)AttribIndex.Color0, 4, VertexAttribPointerType.Float, false, 28, 12);

            CGpass pass = CgImports.cgGetFirstPass(cgEffect.CGtechnique);
            while (pass)
            {
                CgImports.cgSetPassState(pass);

                GL.DrawElements(BeginMode.Triangles, indexCount,
                    DrawElementsType.UnsignedInt, IntPtr.Zero);

                CgImports.cgResetPassState(pass);
                pass = CgImports.cgGetNextPass(pass);
            }
        }

        public Buffer CreateBuffer<T>(BindFlag bufferTarget, BufferUsage bufferUsage = BufferUsage.Dynamic,
            CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write) where T : struct
        {
            uint pointer;
            GL.GenBuffers(1, out pointer);
            return new Buffer((IntPtr)pointer, bufferTarget, bufferUsage, cpuAccessFlags);
        }

        public Buffer CreateBuffer<T>(T obj, BindFlag bufferTarget, BufferUsage bufferUsage = BufferUsage.Dynamic,
            CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write) where T : struct
        {
            var bf = (BufferTarget)(bufferTarget == BindFlag.ConstantBuffer ? 0x8a11 : 34961 + (int)bufferTarget);
            BufferUsageHint bu = GetFromOrmeliEnum(bufferUsage, cpuAccessFlags);
            uint pointer;
            GL.GenBuffers(1, out pointer);
            GL.BindBuffer(bf, pointer);
            GL.BufferData(bf,
                new IntPtr(Marshal.SizeOf(typeof(T))),
                new[] { obj }, bu);
            GL.BindBuffer(bf, 0);

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

        private enum AttribIndex
        {
            // ReSharper disable UnusedMember.Local
            Position = 0, //Input Vertex, Generic Attribute 0

            BlendWeight = 1, //	Input vertex weight, Generic Attribute 1
            Normal = 2, //Input normal, Generic Attribute 2
            Diffuse = 3,
            Color0 = 3, // 	Input primary color, Generic Attribute 3
            Specular = 4,
            Color1 = 4, // 	Input secondary color, Generic Attribute 4
            TessFactor = 5,
            FogCoord = 5, // 	Input fog coordinate, Generic Attribute 5
            PSize = 6, //Input point size, Generic Attribute 6
            BlendIindices = 7, //	Generic Attribute 7
            TexCoord0 = 8,
            TexCoord1 = 9,
            TexCoord2 = 10,
            TexCoord3 = 11,
            TexCoord4 = 12,
            TexCoord5 = 13,
            TexCoord6 = 14,
            TexCoord7 = 15, //Input texture coordinates (texcoord0-texcoord7), Generic Attributes 8-15
            Tangent = 14, //Generic Attribute 14
            Binormal = 15, //Generic Attribute 15
            // ReSharper restore UnusedMember.Local
        }
    }
}

#pragma warning restore 618