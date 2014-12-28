using System;
using OpenTK.Graphics.OpenGL;
using Ormeli.GAPI.Interfaces;

namespace Ormeli.GAPI.OpenGL
{
    public struct GlAttribs : IAttribsContainer
    {
        private Attrib[] _attribs;
        private VertexAttribPointerType[] _types;
        private int _size;
        public void Initialize(Attrib[] attribs, IntPtr pointer)
        {
            _attribs = attribs;
            _types = new VertexAttribPointerType[attribs.Length];
            for (int i = 0; i < attribs.Length; i++)
            {
                _types[i] = (VertexAttribPointerType)(5126 - attribs[i].Type);
            }

            _size = _attribs[_attribs.Length - 1].Offset + _attribs[_attribs.Length - 1].Num * 4;

        }

        public void Accept()
        {
            for (int i = 0; i < _attribs.Length; i++)
            {
                var j = _attribs[i];
                GL.EnableVertexAttribArray((int)j.AttribIndex);
                GL.VertexAttribPointer((int)j.AttribIndex, j.Num, _types[i], false, _size, j.Offset);
            }
        }
    }
}
