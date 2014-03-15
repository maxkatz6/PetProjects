#pragma warning disable 618
using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics;
using Ormeli.CG;
using Ormeli.Core.Patterns;
using Ormeli.Math;
using Tao = Tao.OpenGl;

namespace Ormeli.OpenGL3
{
    public class OGRender : Disposable, IRenderClass
    {
        private GameWindow _gameWindow;

        public  void CreateWindow()
        {
            _gameWindow = new GameWindow(Config.Width, Config.Height)
            {
                VSync = Config.VerticalSyncEnabled ? VSyncMode.On : VSyncMode.Off
            };
        }

        public  RenderType Initialize()
        {
            // Other state
            GL.Enable(EnableCap.DepthTest);
    //        GL.ClearColor(System.Drawing.Color.MidnightBlue);
            GL.BlendColor(0,1,1,1);

      /*      GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();*/

   /*         GL.Viewport(0, 0, _gameWindow.Width, _gameWindow.Height);
            GL.Ortho(0, 0, _gameWindow.Width, _gameWindow.Height, -1, +1);
*/
         /*   GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();*/

            CgImports.cgGLSetDebugMode(0);
            CgImports.cgSetParameterSettingMode(CgShader.CGcontext, CGenum.DeferredParameterSetting);

            return RenderType.OpneGl3;
        }

        public CgShader InitCgShader(string v, string p)
        {
            var myCgVertexProfile = CgImports.cgGLGetLatestProfile(CgGLenum.Vertex);
            var myCgVertexProgram =
                CgImports.cgCreateProgramFromFile(
                    CgShader.CGcontext,
                    CGenum.Source,
                    Config.ShadersDirectory + v,
                    myCgVertexProfile,
                    "main",
                    CgImports.GetOptimalOptions(myCgVertexProfile));

            CgImports.cgGLLoadProgram(myCgVertexProgram);

            var myCgFragmentProfile = CgImports.cgGLGetLatestProfile(CgGLenum.Fragment);

           var  myCgFragmentProgram =
                CgImports.cgCreateProgramFromFile(
                    CgShader.CGcontext, /* Cg runtime context */
                    CGenum.Source, /* Program in human-readable form */
                    Config.ShadersDirectory + p, /* Name of file containing program */
                    myCgFragmentProfile, /* Profile: OpenGL ARB vertex program */
                    "main", /* Entry function name */
                    CgImports.GetOptimalOptions(myCgFragmentProfile));

            CgImports.cgGLLoadProgram(myCgFragmentProgram);

            return new CgShader
            {
                FragmentProgram = myCgFragmentProgram,
                VertexProgram = myCgVertexProgram
            };
        }


        public  void Run(Action act)
        {
            _gameWindow.RenderFrame += (sender, args) => act();
            _gameWindow.Run();
        }

        public  void BeginDraw()
        {
            GL.Viewport(0, 0, Config.Width, Config.Height);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        }

        public  void ChangeBackColor(Color color)
        {
            GL.ClearColor(color.R, color.G, color.B, color.A);
        }

        public  void EndDraw()
        {
            _gameWindow.SwapBuffers();
        }

        public  void ZBuffer(bool turn)
        {
            throw new NotImplementedException();
        }

        public  void AlphaBlending(bool turn)
        {
            throw new NotImplementedException();
        }


        //http://3dgep.com/?p=2665
        public void Draw(CgShader cgShader, Buffer vertexBuffer, Buffer indexBuffer, int vertexStride, int indexCount)
        {
            CgImports.cgGLBindProgram(cgShader.VertexProgram);
            CgImports.cgGLBindProgram(cgShader.FragmentProgram);

            GL.BindBuffer(BufferTarget.ArrayBuffer, (uint)vertexBuffer.Handle);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, (uint)indexBuffer.Handle);

            var p = CgImports.cgGetNamedParameter(cgShader.VertexProgram, "position");
            CgImports.cgGLEnableClientState(p);
            CgImports.cgGLSetParameterPointer(p, 3, (int)VertexAttribPointerType.Float, 28, new IntPtr(0));

            p = CgImports.cgGetNamedParameter(cgShader.VertexProgram, "color");
            CgImports.cgGLEnableClientState(p);
            CgImports.cgGLSetParameterPointer(p, 4, (int)VertexAttribPointerType.Float, 28, new IntPtr(12));

            
            GL.DrawElements(BeginMode.Triangles, indexCount,
                DrawElementsType.UnsignedInt, IntPtr.Zero);

        }

        public  Buffer CreateBuffer<T>(BindFlag bufferTarget, BufferUsage bufferUsage = BufferUsage.Dynamic, CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write) where T:struct 
        {
            uint pointer;
            GL.GenBuffers(1, out pointer);
            return new Buffer((IntPtr)pointer,bufferTarget,bufferUsage,cpuAccessFlags);
        }

        public  Buffer CreateBuffer<T>(T obj, BindFlag bufferTarget, BufferUsage bufferUsage = BufferUsage.Dynamic,
            CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write) where T : struct 
        {
            var bf = (BufferTarget)(bufferTarget == BindFlag.ConstantBuffer ? 0x8a11 : 34961+(int)bufferTarget);
            var bu = GetFromOrmeliEnum(bufferUsage, cpuAccessFlags);
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
            var bu = GetFromOrmeliEnum(bufferUsage,cpuAccessFlags);
            uint pointer;
            GL.GenBuffers(1, out pointer);
            GL.BindBuffer(bf, pointer);
            var l = Marshal.SizeOf(typeof (T));
            GL.BufferData(bf,
                new IntPtr(objs.Length * l),
                objs, bu);
            GL.BindBuffer(bf,0);
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