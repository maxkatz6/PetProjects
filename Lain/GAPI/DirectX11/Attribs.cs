#if DX
using System;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace Lain.GAPI
{
	public struct AttribsContainer
	{
		private readonly InputLayout _inputLayout;

		public AttribsContainer(string atbsFromHLSL, byte[] shaderBytecode)
		{
			var atbs = atbsFromHLSL.Trim().Split(new[] {';', ',', ':'}, StringSplitOptions.RemoveEmptyEntries);
			var elements = new InputElement[atbs.Length/2];
			var offset = 0;
			for (var i = 0; i < atbs.Length; i += 2)
			{
				Format f = 0;
				var o = 0;
				switch (atbs[i].Trim().Split(' ')[0])
				{
					case "uint":
					{
						f = Format.R32_UInt;
						o = sizeof (uint);
						break;
					}
					case "int":
					{
						f = Format.R32_SInt;
						o = sizeof (int);
						break;
					}
					case "float":
					{
						f = Format.R32_Float;
						o = sizeof (float);
						break;
					}
					case "float2":
					{
						f = Format.R32G32_Float;
						o = Vector2.SizeInBytes;
						break;
					}
					case "float3":
					{
						f = Format.R32G32B32_Float;
						o = Vector3.SizeInBytes;
						break;
					}
					case "float4":
					{
						f = Format.R32G32B32A32_Float;
						o = Vector4.SizeInBytes;
						break;
					}
				}
				var tag = atbs[i + 1].Trim();
				var num = 0;
				if (tag.Contains("TEXCOORD"))
				{
					num = tag[tag.Length - 1] - 48;
					tag = "TEXCOORD";
				}
				elements[i/2] = new InputElement(tag, num, f, offset, 0, InputClassification.PerVertexData, 0);
				offset += o;
			}
			_inputLayout = new InputLayout(App.Render.Device, shaderBytecode, elements);
		}

		public void Accept()
		{
			App.Render.DeviceContext.InputAssembler.InputLayout = _inputLayout;
		}
	}
}

#endif