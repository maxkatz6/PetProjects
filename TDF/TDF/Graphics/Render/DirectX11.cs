﻿#region Using

using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using TDF.Core;
using TDF.Graphics.Cameras;
using TDF.Graphics.Data;
using TDF.Graphics.Effects;
using TDF.Inputs;
using Device = SharpDX.Direct3D11.Device;
using Resource = SharpDX.Direct3D11.Resource;

#endregion Using

namespace TDF.Graphics.Render
{
    public static class DirectX11
    {
        #region Variables & Properties

        public static BlendState AlphaDisableBlendingState { get; private set; }

        public static BlendState AlphaEnableBlendingState { get; private set; }

        public static DepthStencilState DepthDisabledStencilState { get; private set; }

        public static DepthStencilState DepthStencilState { get; private set; }

        public static DepthStencilView DepthStencilView { get; private set; }

        public static Device Device { get; private set; }

        public static DeviceContext DeviceContext { get; private set; }

        public static bool Enable4xMSAA { get; set; }

        public static string VideoCardDescription { get; private set; }

        public static int VideoCardMemory { get; private set; }

        public static Matrix OrthoMatrix
        {
            get { return CurrentCamera.Ortho; }
        }

        public static Matrix ProjectionMatrix
        {
            get { return CurrentCamera.Projection; }
        }

        public static Matrix ViewMatrix
        {
            get { return CurrentCamera.View; }
        }
        internal static Camera CurrentCamera;

        private static Texture2D DepthStencilBuffer { get; set; }

        private static RasterizerState RasterState { get; set; }

        private static RenderTargetView RenderTargetView { get; set; }

        private static SwapChain SwapChain { get; set; }

        private static bool VerticalSyncEnabled { get; set; }

        private static Rational Rational { get; set; }

        private static bool isCameraInited ;
        #endregion Variables & Properties

        #region Methods

        public static void BeginScene(float red, float green, float blue, float alpha)
        {
            BeginScene(new Color4(red, green, blue, alpha));
        }

        public static void BeginScene(Color4 color)
        {
            // Clear the depth buffer.
            DeviceContext.ClearDepthStencilView(DepthStencilView, DepthStencilClearFlags.Depth, 1, 0);

            // Clear the back buffer.
            DeviceContext.ClearRenderTargetView(RenderTargetView, color);
        }

        public static void ChangePrimitiveTopology(PrimitiveTopology primitiveTopology)
        {
            DeviceContext.InputAssembler.PrimitiveTopology = primitiveTopology;
        }

        public static void EndScene()
        {
            // Present the back buffer to the screen since rendering is complete.
            SwapChain.Present(VerticalSyncEnabled ? 1 : 0, PresentFlags.None);
        }

        public static bool Initialize(IntPtr windowHandle, Camera camera, string configFile, bool _4xMSAA = false)
        {
            Config.Initialize(configFile);
            Initialize(windowHandle, camera, _4xMSAA);
            return true;
        }

        public static bool Initialize(IntPtr windowHandle, Camera camera, bool _4xMSAA = false)
        {
            camera.UpdateScrennMatrices();
            CurrentCamera = camera;
            isCameraInited = true;
            return Initialize(windowHandle, _4xMSAA);
        }

        public static void Initialize(IntPtr windowHandle, string configIni, bool _4xMSAA)
        {
            Config.Initialize(configIni);
            Initialize(windowHandle, _4xMSAA);
        }

        public static bool Initialize(IntPtr windowHandle, bool _4xMSAA = false)
        {
            LibManager.SaveFromResource();
            if (!Config.IsInited) Config.Initialize("Default Title",1260,720);
            if (!isCameraInited) CurrentCamera = new Camera();
            Enable4xMSAA = _4xMSAA;
            try
            {
                #region Environment Config

                // Store the vsync setting.
                VerticalSyncEnabled = Config.VerticalSyncEnabled;

                // Create a DirectX graphics interface factory.
                var factory = new Factory1();
                // Use the factory to create an adapter for the primary graphics interface (video card).
                Adapter1 adapter = factory.GetAdapter1(0);
                // Get the primary adapter output (monitor).
                Output monitor = adapter.Outputs[0];
                // Get modes that fit the DXGI_FORMAT_R8G8B8A8_UNORM display format for the adapter output (monitor).
                ModeDescription[] modes = monitor.GetDisplayModeList(Format.R8G8B8A8_UNorm,
                    DisplayModeEnumerationFlags.Interlaced);
                // Now go through all the display modes and find the one that matches the screen width and height.
                // When a match is found store the the refresh rate for that monitor, if vertical sync is enabled.
                // Otherwise we use maximum refresh rate.
                if (VerticalSyncEnabled)
                {
                    foreach (var mode in modes)
                    {
                        if (mode.Width == Config.Width && mode.Height == Config.Height)
                        {
                            Rational = new Rational(mode.RefreshRate.Numerator, mode.RefreshRate.Denominator);
                            break;
                        }
                    }
                }

                // Get the daapter (video card) description.
                AdapterDescription adapterDescription = adapter.Description;

                // Store the dedicated video card memory in megabytes.
                VideoCardMemory = adapterDescription.DedicatedVideoMemory >> 10 >> 10;

                // Convert the name of the video card to a character array and store it.
                VideoCardDescription = adapterDescription.Description;

                //factory.MakeWindowAssociation(windowHandle,WindowAssociationFlags.None);

                monitor.Dispose();

                #endregion Environment Config

                #region Initialize swap chain and d3d device

                // Create the swap chain, Direct3D device, and Direct3D device context.

#if DEBUG
                Device = new Device(adapter, DeviceCreationFlags.None, FeatureLevel.Level_11_0, FeatureLevel.Level_10_1,
                    FeatureLevel.Level_10_0, FeatureLevel.Level_9_3, FeatureLevel.Level_9_2, FeatureLevel.Level_9_1);
#else
                Device = new Device(adapter, DeviceCreationFlags.None,  FeatureLevel.Level_11_0, FeatureLevel.Level_10_1, FeatureLevel.Level_10_0, FeatureLevel.Level_9_3,FeatureLevel.Level_9_2, FeatureLevel.Level_9_1 );
#endif
                adapter.Dispose();
                Config.InitializeFeature();
                DeviceContext = Device.ImmediateContext;
                ChangePrimitiveTopology(PrimitiveTopology.TriangleList);

                int m4XMsaaQuality = Device.CheckMultisampleQualityLevels(Format.R8G8B8A8_UNorm, 4);

                // Initialize the swap chain description.
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
                    OutputHandle = windowHandle,
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

                // Release the factory.
                factory.Dispose();

                #endregion Initialize swap chain and d3d device

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
                    CpuAccessFlags = CpuAccessFlags.None,
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

                #region Initialize other

                Texture.CreateNullTexture();
                InputElements.Initialize();
                Input.Initialize();

                #endregion Initialize other

                #region Prepare for render

                TurnZBufferOn();
                TurnOnAlphaBlending();

                #endregion Prepare for render

                return true;
            }
            catch (Exception ex)
            {
                ErrorProvider.Send(ex);
                return false;
            }
        }

        public static void Resize()
        {
     /*       var modeDescription = new ModeDescription
            {
                Format = Format.R8G8B8A8_UNorm,
                Width = Config.Width,
                Height = Config.Height,
                RefreshRate = Rational,
                Scaling = DisplayModeScaling.Unspecified,
                ScanlineOrdering = DisplayModeScanlineOrder.Unspecified
            };
            SwapChain.ResizeTarget(ref modeDescription);
            //         SwapChain.ResizeBuffers(3,Config.Width,Config.Height, Format.R8G8B8A8_UNorm, SwapChainFlags.AllowModeSwitch);
            CurrentCamera.UpdateScrennMatrices();*/
        }

        public static void SetBackBufferRenderTarget()
        {
            // Bind the render target view and depth stencil buffer to the output render pipeline.
            DeviceContext.OutputMerger.SetTargets(DepthStencilView, RenderTargetView);
        }

        public static void Shutdown()
        {
            // Before shutting down set to windowed mode or when you release the swap chain it will throw an exception.
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

            Log.Shutdown();
        }

        public static void TurnOffAlphaBlending()
        {
            // Setup the blend factor.
            var blendFactor = new Color4(0, 0, 0, 0);

            // Turn on the alpha blending.
            DeviceContext.OutputMerger.SetBlendState(AlphaDisableBlendingState, blendFactor, -1);
        }

        public static void TurnOnAlphaBlending()
        {
            // Setup the blend factor.
            var blendFactor = new Color4(0, 0, 0, 0);

            // Turn on the alpha blending.
            DeviceContext.OutputMerger.SetBlendState(AlphaEnableBlendingState, blendFactor, -1);
        }

        public static void TurnZBufferOff()
        {
            DeviceContext.OutputMerger.SetDepthStencilState(DepthDisabledStencilState, 1);
        }

        public static void TurnZBufferOn()
        {
            DeviceContext.OutputMerger.SetDepthStencilState(DepthStencilState, 1);
        }

        public static void UpdateScreenMode()
        {
            SwapChain.SetFullscreenState(Config.FullScreen, null);
        }

        #endregion Methods
    }
}