#region License

/*
MIT License
OpenCg v1.0 Copyright (c) 2011 Péter Primusz
primuszpeter.blogspot.com
All rights reserved.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */

#endregion License

using System;
using System.Runtime.InteropServices;

namespace Ormeli.Cg
{
    public partial class CG
    {
        /// <summary>
        /// Represent a Cg annotation object.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Annotation
        {
            /// <summary>
            /// Keeps the struct from being garbage collected prematurely.
            /// </summary>
            private IntPtr Data;
        }

        /// <summary>
        /// Represent a Cg bool.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Bool
        {

            /// <summary>
            /// Keeps the struct from being garbage collected prematurely.
            /// </summary>
            private Int32 Data; //если что - в жопу риадонли

            public Bool(int boolean)
            {
                Data = boolean;
            }

            public static implicit operator Bool(int boolean)
            {
                return new Bool(boolean);
            }

            public static implicit operator bool(Bool boolean)
            {
                return boolean.Data == 1;
            }

            public static Bool operator ==(Bool me, Bool other)
            {
                return me.Data == other.Data ? (1) : (0);
            }

            public static Bool operator !=(Bool me, Bool other)
            {
                return me.Data != other.Data ? (1) : (0);
            }

            public override bool Equals(object other)
            {
                return (Equals(this, other));
            }

            public override int GetHashCode()
            {
                return Data.GetHashCode();
            }
        }

        /// <summary>
        /// Represent a Cg Buffer object.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Buffer
        {
            /// <summary>
            /// Keeps the struct from being garbage collected prematurely.
            /// </summary>
            private readonly IntPtr Data;
        }

        /// <summary>
        /// Represent a Cg context.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Context
        {
            /// <summary>
            /// Keeps the struct from being garbage collected prematurely.
            /// </summary>
            private readonly IntPtr Data;
        }

        /// <summary>
        /// Represent a Cg effect.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Effect
        {
            /// <summary>
            /// Keeps the struct from being garbage collected prematurely.
            /// </summary>
            private readonly IntPtr Data;

            public Parameter GetParameterByName(string s)
            {
                return GetNamedEffectParameter(this, s);
            }
        }

        /// <summary>
        /// Represent a Cg handle.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Handle
        {
            /// <summary>
            /// Keeps the struct from being garbage collected prematurely.
            /// </summary>
            private readonly IntPtr Data;
        }

        /// <summary>
        /// Represent a Cg object.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Obj
        {
            /// <summary>
            /// Keeps the struct from being garbage collected prematurely.
            /// </summary>
            private readonly IntPtr Data;
        }

        /// <summary>
        /// Represent a Cg parameter object.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Parameter 
        {
            /// <summary>
            /// Keeps the struct from being garbage collected prematurely.
            /// </summary>
            private readonly IntPtr Data;

            public Parameter(Effect ef, string s)
            {
                Data = ef.GetParameterByName(s).Data;
            }
            public Parameter(IntPtr intPtr)
            {
                Data = intPtr;
            }
            public static implicit operator string(Parameter param)
            {
                return GetParameterName(param).ToStr();
            }
            public static implicit operator IntPtr(Parameter param)
            {
                return param.Data;
            }
            public static implicit operator Parameter(IntPtr param)
            {
                return new Parameter(param);
            }
        }

        /// <summary>
        /// Represent a Cg pass object.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Pass 
        {
            /// <summary>
            /// Keeps the struct from being garbage collected prematurely.
            /// </summary>
            private readonly IntPtr Data;

            public static implicit operator bool(Pass pass)
            {
                return pass.Data != IntPtr.Zero;
            }
            public static implicit operator string(Pass pass)
            {
                return GetPassName(pass).ToStr();
            }

            public Pass GetNextPass()
            {
                return CG.GetNextPass(this);
            }

            public void Enable()
            {
                SetPassState(this);
            }

            public void Disable()
            {
                ResetPassState(this);
            }

            public IntPtr GetSignature()
            {
                return App.RenderType == RenderType.DirectX11
                    ? DX11.GetIASignatureByPass(this)
                    : IntPtr.Zero;
            }
        }

        /// <summary>
        /// Represent a Cg program.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Program
        {
            /// <summary>
            /// Keeps the struct from being garbage collected prematurely.
            /// </summary>
            private IntPtr Data;

            public void Compile()
            {
                CompileProgram(this);
            }
            public static Program Combine(params Program[] prgs)
            {
                switch (prgs.Length)
                {
                    case 2:
                        return CombinePrograms2(prgs[0], prgs[1]);
                    case 3:
                        return CombinePrograms3(prgs[0], prgs[1], prgs[2]);
                    case 4:
                        return CombinePrograms4(prgs[0], prgs[1], prgs[2], prgs[3]);
                    case 5:
                        return CombinePrograms5(prgs[0], prgs[1], prgs[2], prgs[3], prgs[4]);
                    default:
                        return CombinePrograms(prgs.Length, prgs);
                }
            }

            public Program Copy()
            {
                return CopyProgram(this);
            }

            public static Program Create(Context context, Enum programType, [In] string program,
            Profile profile, string entry, string[] args)
            {
                return CreateProgram(context, programType, program, profile, entry, args);
            }
        }

        /// <summary>
        /// Represent a Cg state object.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct State
        {
            /// <summary>
            /// Keeps the struct from being garbage collected prematurely.
            /// </summary>
            private IntPtr Data;
        }

        /// <summary>
        /// Represent a Cg stateassignment object.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct StateAssignment
        {
            /// <summary>
            /// Keeps the struct from being garbage collected prematurely.
            /// </summary>
            private IntPtr Data;
        }

        /// <summary>
        /// Represent a Cg technique object.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Technique
        {
            /// <summary>
            /// Keeps the struct from being garbage collected prematurely.
            /// </summary>
            private IntPtr Data;

            public static implicit operator bool(Technique boolean)
            {
                return boolean.Data != IntPtr.Zero;
            }
            public static implicit operator string(Technique tech)
            {
                return GetTechniqueName(tech).ToStr();
            }

            public Pass GetFirstPass()
            {
                return CG.GetFirstPass(this);
            }

            public IntPtr GetSignatureByFirstPass()
            {
                return GetFirstPass().GetSignature();
            }
        }
    }
}