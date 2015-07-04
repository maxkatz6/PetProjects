using System;
using Ormeli.Core.Patterns;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using Buffer = Ormeli.Graphics.Buffer;
using Device = SharpDX.Direct3D11.Device;

namespace Ormeli.GAPI
{
#if DX
	public sealed class Render : Disposable
	{
		private IntPtr _lastBuffer;
		public DepthStencilView DepthStencilView;
		public Device Device;
		public DeviceContext DeviceContext;
		public RenderForm RenderForm;
		public RenderTargetView RenderTargetView;
		public SwapChain SwapChain;
		public Color4 BackColor { get; set; }
		public bool IsBlendEnabled { get; private set; }
		public bool IsDepthEnabled { get; private set; }

		public void AlphaBlending(bool isBlendEnabled)
		{
			if (IsBlendEnabled == isBlendEnabled) return;
			var blendStateDesc = new BlendStateDescription();
			blendStateDesc.RenderTarget[0] = new RenderTargetBlendDescription(
				isBlendEnabled,
				BlendOption.SourceAlpha,
				BlendOption.InverseSourceAlpha,
				BlendOperation.Add,
				BlendOption.One,
				BlendOption.Zero,
				BlendOperation.Add,
				ColorWriteMaskFlags.All);
			DeviceContext.OutputMerger.SetBlendState(new BlendState(Device, blendStateDesc),
				new Color4(0, 0, 0, 0f));
			IsBlendEnabled = isBlendEnabled;
		}

		public void ZBuffer(bool isDepthEnabled)
		{
			if (IsDepthEnabled == isDepthEnabled) return;
			DeviceContext.OutputMerger.SetDepthStencilState(new DepthStencilState(Device, new DepthStencilStateDescription
			{
				IsDepthEnabled = isDepthEnabled,
				DepthWriteMask = DepthWriteMask.All,
				DepthComparison = Comparison.Less,
				IsStencilEnabled = true,
				StencilReadMask = 0xFF,
				StencilWriteMask = 0xFF,
				// Stencil operation if pixel front-facing.
				FrontFace = new DepthStencilOperationDescription
				{
					FailOperation = StencilOperation.Keep,
					DepthFailOperation = StencilOperation.Increment,
					PassOperation = StencilOperation.Keep,
					Comparison = Comparison.Always
				},
				// Stencil operation if pixel is back-facing.
				BackFace = new DepthStencilOperationDescription
				{
					FailOperation = StencilOperation.Keep,
					DepthFailOperation = StencilOperation.Decrement,
					PassOperation = StencilOperation.Keep,
					Comparison = Comparison.Always
				}
			}), 1);
			IsDepthEnabled = isDepthEnabled;
		}

		public void BeginDraw()
		{
			DeviceContext.ClearDepthStencilView(DepthStencilView,
				DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1, 8);
			DeviceContext.ClearRenderTargetView(RenderTargetView, BackColor);
		}

		public void SetBuffers(Buffer vertexBuffer, Buffer indexBuffer, int vertexStride)
		{
			if (_lastBuffer == vertexBuffer)
				return;
			_lastBuffer = vertexBuffer;

			DeviceContext.InputAssembler.SetVertexBuffers(0,
				new VertexBufferBinding(CppObject.FromPointer<SharpDX.Direct3D11.Buffer>(vertexBuffer),
					vertexStride, 0));
			DeviceContext.InputAssembler.SetIndexBuffer(
				CppObject.FromPointer<SharpDX.Direct3D11.Buffer>(indexBuffer), Format.R32_UInt, 0);
		}

		public void Draw(int indexCount, int startIndex = 0)
		{
			DeviceContext.DrawIndexed(indexCount, startIndex, 0);
		}

		public void EndDraw()
		{
			SwapChain.Present(Config.VerticalSyncEnabled ? 1 : 0, PresentFlags.None);
		}

		public void Run(Action act)
		{
			RenderLoop.Run(RenderForm, () => act());
		}

		protected override void OnDispose()
		{
			RenderForm.Close();
			SwapChain?.SetFullscreenState(false, null);
			SwapChain?.Dispose();
			DepthStencilView?.Dispose();
			RenderTargetView?.Dispose();
			SwapChain?.Dispose();
			Device?.Dispose();
		}
	}
#endif
}