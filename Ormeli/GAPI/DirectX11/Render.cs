#if DX
using System;
using Ormeli.Core.Patterns;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Device = SharpDX.Direct3D11.Device;
using Resource = SharpDX.Direct3D11.Resource;

namespace Ormeli.GAPI
{
	public sealed class Render : Disposable
	{
		private IntPtr _lastVB, _lastIB;
		private BlendState AlphaBlendingTrue, AlphaBlendingFalse;
		private DepthStencilState DepthStencilStateTrue, DepthStencilStateFalse;
		internal DepthStencilView DepthStencilView;
		internal Device Device;
		internal DeviceContext DeviceContext;
		internal RenderTargetView RenderTargetView;
		internal SwapChain SwapChain;
		public Color4 BackColor { get; set; }
		public bool IsBlendEnabled { get; private set; }
		public bool IsDepthEnabled { get; private set; }

		public static Render Create()
		{
			var render = new Render
			{
				Device = new Device(DriverType.Hardware,
					Config.IsDebug ? DeviceCreationFlags.Debug : DeviceCreationFlags.None, FeatureLevel.Level_11_0,
					FeatureLevel.Level_10_1,
					FeatureLevel.Level_10_0, FeatureLevel.Level_9_3, FeatureLevel.Level_9_2, FeatureLevel.Level_9_1)
			};


			render.DeviceContext = render.Device.ImmediateContext;
			var sampleDescription = Config.Enable4xMSAA
				? new SampleDescription(4, render.Device.CheckMultisampleQualityLevels(Format.R8G8B8A8_UNorm, 4) - 1)
				: new SampleDescription(1, 0);

			var rational = Rational.Empty;
			var factory = new Factory1();
			var adapter = factory.GetAdapter1(0);
			if (Config.VerticalSyncEnabled)
			{
				var modes = adapter.Outputs[0].GetDisplayModeList(Format.R8G8B8A8_UNorm,
					DisplayModeEnumerationFlags.Interlaced);
				for (var i = 0; i < modes.Length; i++)
				{
					if (modes[i].Width != Config.Width || modes[i].Height != Config.Height) continue;
					rational = new Rational(modes[i].RefreshRate.Numerator,
						modes[i].RefreshRate.Denominator);
					break;
				}
			}

			//HardwareDescription.VideoCardMemory = adapter.Description.DedicatedVideoMemory >> 10 >> 10;
			//HardwareDescription.VideoCardDescription = adapter.Description.Description;

			adapter.Dispose();

			render.SwapChain = new SwapChain(factory, render.Device, new SwapChainDescription
			{
				BufferCount = 1,
				ModeDescription = new ModeDescription
				{
					Format = Format.R8G8B8A8_UNorm,
					Width = Config.Width,
					Height = Config.Height,
					Scaling = DisplayModeScaling.Unspecified,
					ScanlineOrdering = DisplayModeScanlineOrder.Unspecified,
					RefreshRate = rational
				},
				Usage = Usage.RenderTargetOutput,
				OutputHandle = App.Window.Handle,
				SampleDescription = sampleDescription,
				IsWindowed = !Config.FullScreen,
				Flags = SwapChainFlags.AllowModeSwitch,
				SwapEffect = SwapEffect.Discard
			});


			using (var backBuffer = Resource.FromSwapChain<Texture2D>(render.SwapChain, 0))
				render.RenderTargetView = new RenderTargetView(render.Device, backBuffer);

			render.DepthStencilView = new DepthStencilView(render.Device, new Texture2D(render.Device,
				new Texture2DDescription
				{
					Width = Config.Width,
					Height = Config.Height,
					MipLevels = 1,
					ArraySize = 1,
					Format = Format.D24_UNorm_S8_UInt,
					SampleDescription = sampleDescription,
					Usage = ResourceUsage.Default,
					BindFlags = BindFlags.DepthStencil,
					CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.None,
					OptionFlags = ResourceOptionFlags.None
				}),
				new DepthStencilViewDescription
				{
					Format = Format.D24_UNorm_S8_UInt,
					Dimension = Config.Enable4xMSAA
						? DepthStencilViewDimension.Texture2DMultisampled
						: DepthStencilViewDimension.Texture2D,
					Texture2D = new DepthStencilViewDescription.Texture2DResource
					{
						MipSlice = 0
					}
				});

			render.DeviceContext.OutputMerger.SetTargets(render.DepthStencilView, render.RenderTargetView);

			render.DeviceContext.Rasterizer.State = new RasterizerState(render.Device, new RasterizerStateDescription
			{
				IsAntialiasedLineEnabled = Config.Enable4xMSAA,
				CullMode = CullMode.None,
				DepthBias = 0,
				DepthBiasClamp = 0,
				IsDepthClipEnabled = true,
				FillMode = FillMode.Solid,
				IsFrontCounterClockwise = false,
				IsMultisampleEnabled = Config.Enable4xMSAA,
				IsScissorEnabled = false,
				SlopeScaledDepthBias = 0
			});

			render.DeviceContext.Rasterizer.SetViewport(0, 0, Config.Width, Config.Height);

			render.DeviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

			var dssDesk = new DepthStencilStateDescription
			{
				IsDepthEnabled = true,
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
			};
			render.DepthStencilStateTrue = new DepthStencilState(render.Device, dssDesk);
			dssDesk.IsDepthEnabled = false;
			render.DepthStencilStateFalse = new DepthStencilState(render.Device, dssDesk);

			var blendStateDesc = new BlendStateDescription();
			blendStateDesc.RenderTarget[0] = new RenderTargetBlendDescription(
				true,
				BlendOption.SourceAlpha,
				BlendOption.InverseSourceAlpha,
				BlendOperation.Add,
				BlendOption.One,
				BlendOption.Zero,
				BlendOperation.Add,
				ColorWriteMaskFlags.All);

			render.AlphaBlendingTrue = new BlendState(render.Device, blendStateDesc);

			blendStateDesc.RenderTarget[0].IsBlendEnabled = false;
			render.AlphaBlendingFalse = new BlendState(render.Device, blendStateDesc);

			render.ZBuffer(true);
			render.AlphaBlending(true);
			Config.Render = Config.RenderType.DirectX11;
			return render;
		}

		public void AlphaBlending(bool isBlendEnabled)
		{
			if (IsBlendEnabled == isBlendEnabled) return;
			DeviceContext.OutputMerger.SetBlendState(isBlendEnabled ? AlphaBlendingTrue : AlphaBlendingFalse,
				new Color4(0, 0, 0, 0f));
			IsBlendEnabled = isBlendEnabled;
		}

		public void ZBuffer(bool isDepthEnabled)
		{
			if (IsDepthEnabled == isDepthEnabled) return;
			DeviceContext.OutputMerger.SetDepthStencilState(isDepthEnabled ? DepthStencilStateTrue : DepthStencilStateFalse, 1);
			IsDepthEnabled = isDepthEnabled;
		}

		public void BeginDraw()
		{
			DeviceContext.ClearDepthStencilView(DepthStencilView,
				DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1, 8);
			DeviceContext.ClearRenderTargetView(RenderTargetView, BackColor);
		}

		public void SetVertexBuffer(Buffer vb, int vertexStride)
		{
			if (_lastVB == vb)
				return;
			_lastVB = vb;

			DeviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vb, vertexStride, 0));
		}

		public void SetIndexBuffer(Buffer ib)
		{
			if (_lastIB == ib)
				return;
			_lastIB = ib;

			DeviceContext.InputAssembler.SetIndexBuffer(ib, Format.R32_UInt, 0);
		}

		public void Draw(int indexCount, int startIndex = 0)
		{
			DeviceContext.DrawIndexed(indexCount, startIndex, 0);
		}

		public void EndDraw()
		{
			SwapChain.Present(Config.VerticalSyncEnabled ? 1 : 0, PresentFlags.None);
		}

		protected override void OnDispose()
		{
			SwapChain?.SetFullscreenState(false, null);
			SwapChain?.Dispose();
			DepthStencilView?.Dispose();
			RenderTargetView?.Dispose();
			SwapChain?.Dispose();
			Device?.Dispose();
		}
	}
}

#endif