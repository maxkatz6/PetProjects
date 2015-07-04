using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace Ormeli.GAPI
{
#if DX
	public struct AttribsContainer
    {
        private readonly InputLayout _inputLayout;

		public AttribsContainer(Attrib[] attribs, byte[] shaderBytecode)
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
				elements[i] = new InputElement(attribs[i].Index.ToString().ToUpper(), 0,
					(Format)(d + attribs[i].Type),
					attribs[i].Offset, 0, InputClassification.PerVertexData, 0);
			}
			_inputLayout = new InputLayout(App.Render.Device, shaderBytecode, elements);
		}

        public void Accept()
        {
			App.Render.DeviceContext.InputAssembler.InputLayout = _inputLayout;
        }
    }
#endif
}
