using Ormeli.CG;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Runtime.InteropServices;
using SharpDX.Windows;
using Color = Ormeli.Math.Color;
using Device = SharpDX.Direct3D11.Device;
using Resource = SharpDX.Direct3D11.Resource;

namespace Ormeli.DirectX11
{
    internal class DXRender : RenderClass
    {
        public Color4 BackColor;

        public Device Device;

        public DeviceContext DeviceContext;

        public SwapChain SwapChain;


        public DepthStencilState DepthStencilState { get; set; }

        public RenderTargetView RenderTargetView { get; set; }

        public Texture2D DepthStencilBuffer { get; set; }

        public DepthStencilView DepthStencilView { get; set; }

        public bool Enable4xMSAA { get; set; }

        public Rational Rational { get; set; }

        public RasterizerState RasterState { get; set; }

        public BlendState AlphaDisableBlendingState { get; set; }

        public BlendState AlphaEnableBlendingState { get; set; }

        public DepthStencilState DepthDisabledStencilState { get; set; }

        private RenderForm renderForm;

        public override void CreateWindow()
        {
            renderForm = new RenderForm
            {
                Height = Config.Height,
                Width = Config.Width
            };
        }

        public override RenderType Initialize()
        {
            #region Создаем фабрику, адаптер и получаем данные о видеокарте
            // Создаем фабрику, адаптер и получаем данные о видеокарте
            var factory = new Factory1();
            var adapter = factory.GetAdapter1(0);

            if (Config.VerticalSyncEnabled)
            {
                var modes = adapter.Outputs[0].GetDisplayModeList(Format.R8G8B8A8_UNorm,
                    DisplayModeEnumerationFlags.Interlaced);
                for (int i = 0; i < modes.Length; i++)
                {
                    if (modes[i].Width == Config.Width && modes[i].Height == Config.Height)
                    {
                        Rational = new Rational(modes[i].RefreshRate.Numerator, modes[i].RefreshRate.Denominator);
                        break;
                    }
                }
            }

            HardwareDescription.VideoCardMemory = adapter.Description.DedicatedVideoMemory >> 10 >> 10;
            HardwareDescription.VideoCardDescription = adapter.Description.Description;

            #endregion 
            #region Создаем SwapChain, Device, и DeviceContext

            Device = new Device(adapter, Config.IsDebug ? DeviceCreationFlags.Debug : DeviceCreationFlags.None , FeatureLevel.Level_11_0, FeatureLevel.Level_10_1,
                FeatureLevel.Level_10_0, FeatureLevel.Level_9_3, FeatureLevel.Level_9_2, FeatureLevel.Level_9_1);

            DeviceContext = Device.ImmediateContext;

            int m4XMsaaQuality = Device.CheckMultisampleQualityLevels(Format.R8G8B8A8_UNorm, 4);

            var swapChainDesc = new SwapChainDescription
            {
                // Set to a single back buffer.
                BufferCount = 1,
                // Set the width and height of the back buffer.
                ModeDescription = new ModeDescription
                {
                    Format = Format.R8G8B8A8_UNorm,
                    Width = Config.Width,
                    Height = Config.Height,
                    RefreshRate = Rational,
                    Scaling = DisplayModeScaling.Unspecified,
                    ScanlineOrdering = DisplayModeScanlineOrder.Unspecified
                },
                // Set the usage of the back buffer.
                Usage = Usage.RenderTargetOutput,
                // Set the handle for the window to render to.
                OutputHandle = renderForm.Handle,
                // Turn multisampling off.
                SampleDescription =
                    (Enable4xMSAA ? new SampleDescription(4, m4XMsaaQuality - 1) : new SampleDescription(1, 0)),
                // Set to full screen or windowed mode.
                IsWindowed = !Config.FullScreen,
                // Don't set the advanced flags.
                Flags = SwapChainFlags.AllowModeSwitch,
                // Discard the back buffer content after presenting.
                SwapEffect = SwapEffect.Discard
            };

            SwapChain = new SwapChain(factory, Device, swapChainDesc);

            // Удаляем фабрику и адаптер из памяти.
            factory.Dispose();
            adapter.Dispose();

            #endregion

            #region Initialize buffers

            // Get the pointer to the back buffer.
            using (var backBuffer = Resource.FromSwapChain<Texture2D>(SwapChain, 0))
            {
                // Create the render target view with the back buffer pointer.
                RenderTargetView = new RenderTargetView(Device, backBuffer);
            }

            // Initialize and set up the description of the depth buffer.
            var depthBufferDesc = new Texture2DDescription
            {
                Width = Config.Width,
                Height = Config.Height,
                MipLevels = 1,
                ArraySize = 1,
                Format = Format.D24_UNorm_S8_UInt,
                SampleDescription =
                    (Enable4xMSAA ? new SampleDescription(4, m4XMsaaQuality - 1) : new SampleDescription(1, 0)),
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            };

            // Create the texture for the depth buffer using the filled out description.
            DepthStencilBuffer = new Texture2D(Device, depthBufferDesc);

            #endregion Initialize buffers

            #region Initialize Depth Enabled Stencil

            // Initialize and set up the description of the stencil state.
            var depthStencilDesc = new DepthStencilStateDescription
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

            // Create the depth stencil state.
            DepthStencilState = new DepthStencilState(Device, depthStencilDesc);

            #endregion Initialize Depth Enabled Stencil

            #region Initialize Output Merger

            // Set the depth stencil state.
            DeviceContext.OutputMerger.SetDepthStencilState(DepthStencilState, 1);

            // Initialize and set up the depth stencil view.
            var depthStencilViewDesc = new DepthStencilViewDescription
            {
                Format = Format.D24_UNorm_S8_UInt,
                Dimension = Enable4xMSAA
                    ? DepthStencilViewDimension.Texture2DMultisampled
                    : DepthStencilViewDimension.Texture2D,
                Texture2D = new DepthStencilViewDescription.Texture2DResource
                {
                    MipSlice = 0
                },
            };

            // Create the depth stencil view.
            DepthStencilView = new DepthStencilView(Device, DepthStencilBuffer, depthStencilViewDesc);

            // Bind the render target view and depth stencil buffer to the output render pipeline.
            DeviceContext.OutputMerger.SetTargets(DepthStencilView, RenderTargetView);

            #endregion Initialize Output Merger

            #region Initialize Raster State

            // Setup the raster description which will determine how and what polygon will be drawn.
            var rasterDesc = new RasterizerStateDescription
            {
                IsAntialiasedLineEnabled = Enable4xMSAA,
                CullMode = CullMode.Back,
                DepthBias = 0,
                DepthBiasClamp = .0f,
                IsDepthClipEnabled = true,
                FillMode = FillMode.Solid,
                IsFrontCounterClockwise = false,
                IsMultisampleEnabled = Enable4xMSAA,
                IsScissorEnabled = false,
                SlopeScaledDepthBias = .0f
            };

            // Create the rasterizer state from the description we just filled out.
            RasterState = new RasterizerState(Device, rasterDesc);

            #endregion Initialize Raster State

            #region Initialize Rasterizer

            // Now set the rasterizer state.
            DeviceContext.Rasterizer.State = RasterState;

            // Setup and create the viewport for rendering.

            DeviceContext.Rasterizer.SetViewport(0, 0, Config.Width, Config.Height);

            #endregion Initialize Rasterizer

            #region Initialize Depth Disabled Stencil

            // Now create a second depth stencil state which turns off the Z buffer for 2D rendering.
            // The difference is that DepthEnable is set to false.
            // All other parameters are the same as the other depth stencil state.
            var depthDisabledStencilDesc = new DepthStencilStateDescription
            {
                IsDepthEnabled = false,
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

            // Create the depth stencil state.
            DepthDisabledStencilState = new DepthStencilState(Device, depthDisabledStencilDesc);

            #endregion Initialize Depth Disabled Stencil

            #region Initialize Blend States

            // Create an alpha enabled blend state description.
            var blendStateDesc = new BlendStateDescription();
            blendStateDesc.RenderTarget[0].IsBlendEnabled = true;
            blendStateDesc.RenderTarget[0].SourceBlend = BlendOption.SourceAlpha;
            blendStateDesc.RenderTarget[0].DestinationBlend = BlendOption.InverseSourceAlpha;
            blendStateDesc.RenderTarget[0].BlendOperation = BlendOperation.Add;
            blendStateDesc.RenderTarget[0].SourceAlphaBlend = BlendOption.One;
            blendStateDesc.RenderTarget[0].DestinationAlphaBlend = BlendOption.Zero;
            blendStateDesc.RenderTarget[0].AlphaBlendOperation = BlendOperation.Add;
            blendStateDesc.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.All;

            // Create the blend state using the description.
            AlphaEnableBlendingState = new BlendState(Device, blendStateDesc);

            // Modify the description to create an disabled blend state description.
            blendStateDesc.RenderTarget[0].IsBlendEnabled = false;
            // Create the blend state using the description.
            AlphaDisableBlendingState = new BlendState(Device, blendStateDesc);

            #endregion Initialize Blend States

            return RenderType.DirectX11;
        }

        public override void InitCG()
        {
            var v = CgImports.cgCreateContext();
            CgImports.cgD3D11SetDevice(v, Device.NativePointer);
            CgErrorProvider.CheckForCgError(v, "setting Direct3D device");
            var myCgVertexProfile = CgImports.cgD3D11GetLatestVertexProfile();
            CgErrorProvider.CheckForCgError(v, "getting latest profile");
            var profileOpts = CgImports.cgD3D11GetOptimalOptions(myCgVertexProfile);
            CgErrorProvider.CheckForCgError(v, "getting latest profile options");
            var myCgVertexProgram = CgImports.cgCreateProgramFromFile(v, CGenum.SOURCE, "Ormeli (3).exe", myCgVertexProfile,
               "j", profileOpts);
            CgErrorProvider.CheckForCgError(v, "creating vertex program from file");
            CgImports.cgD3D11LoadProgram(myCgVertexProgram, 0);
            CgErrorProvider.CheckForCgError(v, "loading vertex program");
        }

        public override void Run(Action act)
        {
            RenderLoop.Run(renderForm,()=> act());
        }

        public override void BeginDraw()
        {
            DeviceContext.ClearDepthStencilView(DepthStencilView, DepthStencilClearFlags.Depth, 1, 0);
            DeviceContext.ClearRenderTargetView(RenderTargetView, BackColor);
        }

        public override void ChangeBackColor(Color color)
        {
            BackColor = ToDXColor(color);
        }

        public override void EndDraw()
        {
            SwapChain.Present(Config.VerticalSyncEnabled ? 1 : 0, PresentFlags.None);
        }

        private static Color4 ToDXColor(Color d)
        {
            return new Color4(d.R, d.G, d.B, d.A);
        }

        protected override void OnDispose()
        
        {
            renderForm.Close();
            if (SwapChain != null)
            {
                SwapChain.SetFullscreenState(false, null);
            }

            if (AlphaEnableBlendingState != null)
            {
                AlphaEnableBlendingState.Dispose();
                AlphaEnableBlendingState = null;
            }

            if (AlphaDisableBlendingState != null)
            {
                AlphaDisableBlendingState.Dispose();
                AlphaDisableBlendingState = null;
            }

            if (DepthDisabledStencilState != null)
            {
                DepthDisabledStencilState.Dispose();
                DepthDisabledStencilState = null;
            }

            if (RasterState != null)
            {
                RasterState.Dispose();
                RasterState = null;
            }

            if (DepthStencilView != null)
            {
                DepthStencilView.Dispose();
                DepthStencilView = null;
            }

            if (DepthStencilState != null)
            {
                DepthStencilState.Dispose();
                DepthStencilState = null;
            }

            if (DepthStencilBuffer != null)
            {
                DepthStencilBuffer.Dispose();
                DepthStencilBuffer = null;
            }

            if (RenderTargetView != null)
            {
                RenderTargetView.Dispose();
                RenderTargetView = null;
            }

            if (Device != null)
            {
                Device.Dispose();
                Device = null;
            }

            if (SwapChain != null)
            {
                SwapChain.Dispose();
                SwapChain = null;
            }
        }

        public void SetBackBufferRenderTarget()
        {
            // Bind the render target view and depth stencil buffer to the output render pipeline.
            DeviceContext.OutputMerger.SetTargets(DepthStencilView, RenderTargetView);
        }

        public override void TurnOffAlphaBlending()
        {
            // Setup the blend factor.
            var blendFactor = new Color4(0, 0, 0, 0);

            // Turn on the alpha blending.
            DeviceContext.OutputMerger.SetBlendState(AlphaDisableBlendingState, blendFactor);
        }

        public override void DrawBuffer(Buffer vertexBuffer, Buffer indexBuffer, int vertexStride, int indexCount)
        {
            DeviceContext.InputAssembler.SetVertexBuffers(0,
                new VertexBufferBinding(CppObject.FromPointer<SharpDX.Direct3D11.Buffer>(vertexBuffer.Handle),
                    vertexStride, 0));
            DeviceContext.InputAssembler.SetIndexBuffer(
                CppObject.FromPointer<SharpDX.Direct3D11.Buffer>(indexBuffer.Handle), Format.R32_UInt, 0);
            DeviceContext.DrawIndexed(indexCount, 0, 0);
        }

        public override Buffer CreateBuffer<T>(BindFlag bufferTarget, BufferUsage bufferUsage = BufferUsage.Dynamic,
            CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write)
        {
            var vbd = new BufferDescription(
                Marshal.SizeOf(typeof(T)),
                (ResourceUsage)bufferUsage,
                (BindFlags)bufferTarget,
                (SharpDX.Direct3D11.CpuAccessFlags)((long)cpuAccessFlags * 65536),
                ResourceOptionFlags.None,
                0
                );
            return new Buffer(new SharpDX.Direct3D11.Buffer(Device, vbd).NativePointer, bufferTarget, bufferUsage,
                cpuAccessFlags);
        }

        public override Buffer CreateBuffer<T>(T obj, BindFlag bufferTarget,
            BufferUsage bufferUsage = BufferUsage.Dynamic,
            CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write)
        {
            var vbd = new BufferDescription(
                Marshal.SizeOf(typeof(T)),
                (ResourceUsage)bufferUsage,
                (BindFlags)bufferTarget,
                (SharpDX.Direct3D11.CpuAccessFlags)((long)cpuAccessFlags * 65536),
                ResourceOptionFlags.None,
                0
                );
            return new Buffer(SharpDX.Direct3D11.Buffer.Create(Device, ref obj, vbd).NativePointer, bufferTarget,
                bufferUsage, cpuAccessFlags);
        }

        public override Buffer CreateBuffer<T>(T[] objs, BindFlag bufferTarget,
            BufferUsage bufferUsage = BufferUsage.Dynamic,
            CpuAccessFlags cpuAccessFlags = CpuAccessFlags.Write)
        {
            var vbd = new BufferDescription(
                objs.Length * Marshal.SizeOf(typeof(T)),
                (ResourceUsage)bufferUsage,
                (BindFlags)bufferTarget,
                (SharpDX.Direct3D11.CpuAccessFlags)((long)cpuAccessFlags * 65536),
                ResourceOptionFlags.None, 0
                );
            return new Buffer(SharpDX.Direct3D11.Buffer.Create(Device, objs, vbd).NativePointer, bufferTarget,
                bufferUsage, cpuAccessFlags);
        }

        public override void TurnOnAlphaBlending()
        {
            // Setup the blend factor.
            var blendFactor = new Color4(0, 0, 0, 0);

            // Turn on the alpha blending.
            DeviceContext.OutputMerger.SetBlendState(AlphaEnableBlendingState, blendFactor);
        }

        public override void TurnZBufferOff()
        {
            DeviceContext.OutputMerger.SetDepthStencilState(DepthDisabledStencilState, 1);
        }

        public override void TurnZBufferOn()
        {
            DeviceContext.OutputMerger.SetDepthStencilState(DepthStencilState, 1);
        }

        private static void Save_Release(ref DisposeBase db)
        {
            if (db == null) return;
            db.Dispose();
            db = null;
        }
    }
}