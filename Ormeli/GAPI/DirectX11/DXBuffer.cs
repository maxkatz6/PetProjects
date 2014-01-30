using System;

namespace Ormeli.DirectX11
{
    internal class DXBuffer : Buffer
    {
        public SharpDX.Direct3D11.Buffer Buffer { get; private set; }
        public override void Initalize()
        {
            throw new NotImplementedException();
        }
    }
}
