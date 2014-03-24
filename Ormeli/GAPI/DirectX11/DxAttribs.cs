using System;
using System.Runtime.InteropServices;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace Ormeli.DirectX11
{
    public class DxAttribs : IAttribsContainer
    {
        private InputLayout _inputLayout;
        private readonly DeviceContext _deviceContext;

        public DxAttribs(DeviceContext dC)
        {
            _deviceContext = dC;
        }

        public void Initialize(Attrib[] attribs, IntPtr blobPointer)
        {
            var elements = new InputElement[attribs.Length];
            for (int i = 0; i < attribs.Length; i++)
            {
                int d = 41;
                switch (attribs[i].Num)
                {
                    case 4:
                        d = 2; break;
                    case 3:
                        d = 6; break;
                    case 2:
                        d = 16; break;
                }
                elements[i] = new InputElement(attribs[i].AttribIndex.ToString().ToUpper(), 0,
                    (Format)(d + attribs[i].Type),
                    attribs[i].Offset, 0, InputClassification.PerVertexData, 0);
            }
            var pVsBlob = new Blob(blobPointer);
            var byteArr = new byte[pVsBlob.BufferSize];
            Marshal.Copy(pVsBlob.BufferPointer, byteArr, 0, byteArr.Length);
            _inputLayout = new InputLayout(_deviceContext.Device, byteArr, elements);
        }

        public void Accept()
        {
            _deviceContext.InputAssembler.InputLayout = _inputLayout;
        }
    }
}
