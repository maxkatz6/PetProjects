using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Ormeli.Core;
using Ormeli.Graphics;
using Ormeli.Graphics.Drawable;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using Buffer = Ormeli.Graphics.Buffer;
using CpuAccessFlags = Ormeli.Graphics.CpuAccessFlags;
using Device = SharpDX.Direct3D11.Device;
using Resource = SharpDX.Direct3D11.Resource;

namespace Ormeli.GAPI
{
#if DX
	public class Creator
	{
		private Device _device;

		public static void InitGAPI(out Render render, out Creator creator)
		{
			render = new Render();
			creator = new Creator
			{
				_device = render.Device = new Device(DriverType.Hardware,
					Config.IsDebug ? DeviceCreationFlags.Debug : DeviceCreationFlags.None, FeatureLevel.Level_11_0,
					FeatureLevel.Level_10_1,
					FeatureLevel.Level_10_0, FeatureLevel.Level_9_3, FeatureLevel.Level_9_2, FeatureLevel.Level_9_1)
			};


			render.DeviceContext = render.Device.ImmediateContext;
			var sampleDescription = Config.Enable4xMSAA
				? new SampleDescription(4, render.Device.CheckMultisampleQualityLevels(Format.R8G8B8A8_UNorm, 4) - 1)
				: new SampleDescription(1, 0);

			render.RenderForm = creator.CreateWindow();

			render.SwapChain = creator.CreateSwapChain(new SwapChainDescription
			{
				BufferCount = 1,
				ModeDescription = new ModeDescription
				{
					Format = Format.R8G8B8A8_UNorm,
					Width = Config.Width,
					Height = Config.Height,
					Scaling = DisplayModeScaling.Unspecified,
					ScanlineOrdering = DisplayModeScanlineOrder.Unspecified
				},
				Usage = Usage.RenderTargetOutput,
				OutputHandle = render.RenderForm.Handle,
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

			render.ZBuffer(true);
			render.AlphaBlending(true);
			Config.Render = Config.RenderType.DirectX11;
		}

		private SwapChain CreateSwapChain(SwapChainDescription swapChainDesc)
		{
			var factory = new Factory1();
			var adapter = factory.GetAdapter1(0);
			if (Config.VerticalSyncEnabled)
			{
				var modes = adapter.Outputs[0].GetDisplayModeList(Format.R8G8B8A8_UNorm,
					DisplayModeEnumerationFlags.Interlaced);
				for (var i = 0; i < modes.Length; i++)
				{
					if (modes[i].Width != Config.Width || modes[i].Height != Config.Height) continue;
					swapChainDesc.ModeDescription.RefreshRate = new Rational(modes[i].RefreshRate.Numerator,
						modes[i].RefreshRate.Denominator);
					break;
				}
			}

			//HardwareDescription.VideoCardMemory = adapter.Description.DedicatedVideoMemory >> 10 >> 10;
			//HardwareDescription.VideoCardDescription = adapter.Description.Description;

			adapter.Dispose();

			return new SwapChain(factory, _device, swapChainDesc);
		}

		public RenderForm CreateWindow()
		{
			var _renderForm = new RenderForm(Config.Tittle)
			{
				Height = Config.Height,
				Width = Config.Width
			};
			_renderForm.KeyPress += (s, e) => Input.CharInput(e.KeyChar);
			_renderForm.KeyDown += (s, e) => Input.KeyDown((Input.Key) e.KeyValue);
			_renderForm.KeyUp += (s, e) => Input.KeyUp((Input.Key) e.KeyValue);
			_renderForm.MouseMove += (s, e) => Input.SetMouseCoord(e.X, e.Y);
			_renderForm.MouseDown += (s, e) =>
			{
				switch (e.Button)
				{
					case MouseButtons.Left:
						Input.PressLButton(true);
						break;
					case MouseButtons.Right:
						Input.PressRButton(true);
						break;
					case MouseButtons.Middle:
						Input.PressMButton(true);
						break;
				}
			};
			_renderForm.MouseUp += (s, e) =>
			{
				switch (e.Button)
				{
					case MouseButtons.Left:
						Input.PressLButton(false);
						break;
					case MouseButtons.Right:
						Input.PressRButton(false);
						break;
					case MouseButtons.Middle:
						Input.PressMButton(false);
						break;
				}
			};
			return _renderForm;
		}

		public unsafe Texture LoadTexture(string fileName)
		{
			var fi = ImageInformation.FromFile(fileName);
			if (!fi.HasValue)
				return Texture.Null;
			var fileInfo = fi.Value;

			return new Texture(ShaderResourceView.FromFile(_device, fileName, new ImageLoadInformation
			{
				Width = fileInfo.Width,
				Height = fileInfo.Height,
				FirstMipLevel = 0,
				MipLevels = fileInfo.MipLevels,
				Usage = ResourceUsage.Default,
				BindFlags = BindFlags.ShaderResource,
				CpuAccessFlags = 0,
				OptionFlags = 0,
				Format = fileInfo.Format,
				Filter = FilterFlags.None,
				MipFilter = FilterFlags.None,
				PSrcInfo = new IntPtr(&fileInfo)
			}).NativePointer, fileInfo.Width, fileInfo.Height);
		}

		public unsafe Texture CreateTexture(Color4[,] array, Format format = Format.R32G32B32A32_Float)
		{
			var w = array.GetLength(1);
			var h = array.GetLength(0);

			DataBox[] v;
			fixed (void* p = &array[0, 0])
				v = new[]
				{
					new DataBox(new DataStream(new IntPtr(p), array.Length*16, true, true).DataPointer,
						w*FormatHelper.SizeOfInBytes(format), 0)
				};
			return new Texture(new ShaderResourceView(_device, new Texture2D(_device, new Texture2DDescription
			{
				ArraySize = 1,
				BindFlags = BindFlags.ShaderResource,
				CpuAccessFlags = 0,
				Format = format,
				Height = h,
				MipLevels = 1,
				OptionFlags = 0,
				Usage = ResourceUsage.Default,
				Width = w,
				SampleDescription = new SampleDescription(1, 0)
			}, v)).NativePointer, w, h);
		}

		public Buffer CreateBuffer<T>(BindFlag bufferTarget, BufferUsage bufferUsage = BufferUsage.Dynamic,
			CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write) where T : struct
		{
			var vbd = new BufferDescription(
				Marshal.SizeOf(typeof (T)),
				(ResourceUsage) bufferUsage,
				(BindFlags) bufferTarget,
				(SharpDX.Direct3D11.CpuAccessFlags) ((long) cpuAccessFlags*65536),
				ResourceOptionFlags.None,
				0
				);
			return new Buffer(new SharpDX.Direct3D11.Buffer(_device, vbd).NativePointer,
				bufferTarget == BindFlag.VertexBuffer ? Reflection.GetStatic<int, T>("Number") : -1);
		}

		public Buffer CreateBuffer<T>(T[] objs, BindFlag bufferTarget,
			BufferUsage bufferUsage = BufferUsage.Dynamic,
			CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write) where T : struct
		{
			var vbd = new BufferDescription(
				objs.Length*Marshal.SizeOf(typeof (T)),
				(ResourceUsage) bufferUsage,
				(BindFlags) bufferTarget,
				(SharpDX.Direct3D11.CpuAccessFlags) ((long) cpuAccessFlags*65536),
				ResourceOptionFlags.None, 0
				);
			return new Buffer(SharpDX.Direct3D11.Buffer.Create(_device, objs, vbd).NativePointer,
				bufferTarget == BindFlag.VertexBuffer ? Reflection.GetStatic<int, T>("Number") : -1);
		}
	}
#endif
}