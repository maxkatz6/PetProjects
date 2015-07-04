using System;
using Ormeli.Core;
using Ormeli.Core.Patterns;
using Ormeli.Graphics.Drawable;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace Ormeli.GAPI
{
#if DX
	public class Effect : Disposable
	{
		private Texture LastTexture;

		public Effect(string s, Attrib[] a)
		{
			try
			{
				s = Config.GetEffectPath(s);
				CompilationResult VSShaderByteCode = ShaderBytecode.CompileFromFile(s, "VS", "vs_4_0"),
					PSShaderByteCode = ShaderBytecode.CompileFromFile(s, "PS", "ps_4_0");
				if (VSShaderByteCode.HasErrors || PSShaderByteCode.HasErrors)
					throw new Exception(VSShaderByteCode.Message + "\r\n" + PSShaderByteCode.Message);
				VertexShader = new VertexShader(App.Render.Device, VSShaderByteCode);
				PixelShader = new PixelShader(App.Render.Device, PSShaderByteCode);
				ConstantMatrixBuffer = new Buffer(App.Render.Device, new BufferDescription
				{
					Usage = ResourceUsage.Dynamic,
					SizeInBytes = Utilities.SizeOf<Matrix>(),
					BindFlags = BindFlags.ConstantBuffer,
					CpuAccessFlags = CpuAccessFlags.Write,
					OptionFlags = ResourceOptionFlags.None,
					StructureByteStride = 0
				});
				SampleState = new SamplerState(App.Render.Device, new SamplerStateDescription
				{
					AddressU = TextureAddressMode.Wrap,
					AddressV = TextureAddressMode.Wrap,
					AddressW = TextureAddressMode.Wrap,
					MipLodBias = 0,
					MaximumAnisotropy = 1,
					ComparisonFunction = Comparison.Always,
					BorderColor = new Color4(0, 0, 0, 0),
					MinimumLod = 0,
					MaximumLod = float.MaxValue
				});
				AttribsContainer = new AttribsContainer(a, ShaderSignature.GetInputSignature(VSShaderByteCode));

				VSShaderByteCode.Dispose();
				PSShaderByteCode.Dispose();
			}
			catch (Exception ex)
			{
				ErrorProvider.SendError(ex);
			}
		}

		private PixelShader PixelShader { get; }
		private SamplerState SampleState { get; }
		private VertexShader VertexShader { get; }
		public AttribsContainer AttribsContainer { get; }
		private Buffer ConstantMatrixBuffer { get; }

		public void Render(Mesh mesh)
		{
			App.Render.SetBuffers(mesh.Vb, mesh.Ib, mesh.VertexSize);
			App.Render.DeviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
			AttribsContainer.Accept();
			App.Render.DeviceContext.VertexShader.Set(VertexShader);
			App.Render.DeviceContext.PixelShader.Set(PixelShader);
			App.Render.DeviceContext.PixelShader.SetSampler(0, SampleState);
			App.Render.DeviceContext.DrawIndexed(mesh.IndexCount, 0, 0);
		}

		public unsafe void SetMatrix(Matrix mt)
		{
			var box = App.Render.DeviceContext.MapSubresource(ConstantMatrixBuffer, 0, MapMode.WriteDiscard, MapFlags.None);
			*(Matrix*) box.DataPointer.ToPointer() = Matrix.Transpose(mt);
			App.Render.DeviceContext.UnmapSubresource(ConstantMatrixBuffer, 0);
			App.Render.DeviceContext.VertexShader.SetConstantBuffer(0, ConstantMatrixBuffer);
		}

		public void SetTexture(Texture tex)
		{
			if (tex == LastTexture) return;
			App.Render.DeviceContext.PixelShader.SetShaderResource(0, CppObject.FromPointer<ShaderResourceView>(tex));
			LastTexture = tex;
		}

		public void SetTextures(Texture[] tex)
		{
			var arr = new ShaderResourceView[tex.Length];
			for (var i = 0; i < tex.Length; i++)
				arr[i] = CppObject.FromPointer<ShaderResourceView>(tex[i]);
			App.Render.DeviceContext.PixelShader.SetShaderResources(0, arr);
		}
	}
#endif
}