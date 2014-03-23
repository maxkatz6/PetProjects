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

namespace Ormeli.CG
{
    /// <summary>
    /// Represent a Cg annotation object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CGannotation
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
    public struct CGbool
    {
        /// <summary>
        /// Keeps the struct from being garbage collected prematurely.
        /// </summary>
        private Int32 Data; //если что - в жопу риадонли

        public CGbool(int boolean)
        {
            Data = boolean;
        }

        public static implicit operator CGbool(int boolean)
        {
            return new CGbool(boolean);
        }

        public static implicit operator bool(CGbool boolean)
        {
            return boolean.Data == 1;
        }

        public static CGbool operator ==(CGbool me, CGbool other)
        {
            return me.Data == other.Data ? (1) : (0);
        }

        public static CGbool operator !=(CGbool me, CGbool other)
        {
            return me.Data != other.Data ? (1) : (0);
        }

        public bool Equals(CGbool other)
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
    public struct CGbuffer
    {
        /// <summary>
        /// Keeps the struct from being garbage collected prematurely.
        /// </summary>
        private IntPtr Data;
    }

    /// <summary>
    /// Represent a Cg context.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CGcontext
    {
        /// <summary>
        /// Keeps the struct from being garbage collected prematurely.
        /// </summary>
        private IntPtr Data;
    }

    /// <summary>
    /// Represent a Cg effect.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CGeffect
    {
        /// <summary>
        /// Keeps the struct from being garbage collected prematurely.
        /// </summary>
        private IntPtr Data;
    }

    /// <summary>
    /// Represent a Cg handle.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CGhandle
    {
        /// <summary>
        /// Keeps the struct from being garbage collected prematurely.
        /// </summary>
        private IntPtr Data;
    }

    /// <summary>
    /// Represent a Cg object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CGobj
    {
        /// <summary>
        /// Keeps the struct from being garbage collected prematurely.
        /// </summary>
        private IntPtr Data;
    }

    /// <summary>
    /// Represent a Cg parameter object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CGparameter
    {
        /// <summary>
        /// Keeps the struct from being garbage collected prematurely.
        /// </summary>
        private IntPtr Data;
    }

    /// <summary>
    /// Represent a Cg pass object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CGpass
    {
        /// <summary>
        /// Keeps the struct from being garbage collected prematurely.
        /// </summary>
        private IntPtr Data;

        public static implicit operator bool(CGpass boolean)
        {
            return boolean.Data != IntPtr.Zero;
        }
    }

    /// <summary>
    /// Represent a Cg program.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CGprogram
    {
        /// <summary>
        /// Keeps the struct from being garbage collected prematurely.
        /// </summary>
        private IntPtr Data;
    }

    /// <summary>
    /// Represent a Cg state object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CGstate
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
    public struct CGstateassignment
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
    public struct CGtechnique
    {
        /// <summary>
        /// Keeps the struct from being garbage collected prematurely.
        /// </summary>
        private IntPtr Data;

        public static implicit operator bool(CGtechnique boolean)
        {
            return boolean.Data != IntPtr.Zero;
        }
    }
}