﻿using System;
using Ormeli.Cg;
using Ormeli.Core;
using Ormeli.Core.Patterns;
using Ormeli.Graphics;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using Buffer = Ormeli.Graphics.Buffer;
using Color = Ormeli.Math.Color;
using Device = SharpDX.Direct3D11.Device;
using Resource = SharpDX.Direct3D11.Resource;

namespace Ormeli.DirectX11
{
    internal sealed class DxRender : Disposable, IRender
    {
        private readonly Color4 _blendFactor = new Color4(0, 0, 0, 0);

        public Color BackColor
        {
            get
            {
                return new Color((byte) (color.Red*255), (byte) (color.Green*255), (byte) (color.Blue*255),
                    (byte) (color.Alpha*255));
            }
            set
            {
                color = new Color4(value.R, value.G, value.B, value.A);
            }
        }

        private Color4 color;
        public Device Device;
        public DeviceContext DeviceContext;
        public SwapChain SwapChain;
        private RenderForm _renderForm;
        private DxCreator creator;
        public BlendState AlphaDisableBlendingState { get; set; }

        public BlendState AlphaEnableBlendingState { get; set; }

        public DepthStencilState DepthDisabledStencilState { get; set; }

        public Texture2D DepthStencilBuffer { get; set; }

        public DepthStencilState DepthStencilState { get; set; }

        public DepthStencilView DepthStencilView { get; set; }

        public bool Enable4xMSAA { get; set; }

        public RasterizerState RasterState { get; set; }

        public Rational Rational { get; set; }

        public RenderTargetView RenderTargetView { get; set; }

        private int _lastAttrNum = -1;

        public void AlphaBlending(bool turn)
        {
            DeviceContext.OutputMerger.SetBlendState(turn ? AlphaEnableBlendingState : AlphaDisableBlendingState,
                _blendFactor);
        }

        public void BeginDraw()
        {
            DeviceContext.ClearDepthStencilView(DepthStencilView,
                DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1, 0);
            DeviceContext.ClearRenderTargetView(RenderTargetView, color);
        }

        public void CreateWindow()
        {
            _renderForm = new RenderForm
            {
                Height = Config.Height,
                Width = Config.Width
            };
        }

        public void Draw(CgEffect.TechInfo techInfo, Buffer vertexBuffer, Buffer indexBuffer, int vertexStride, int indexCount)
        {
            DeviceContext.InputAssembler.SetVertexBuffers(0,
                new VertexBufferBinding(CppObject.FromPointer<SharpDX.Direct3D11.Buffer>(vertexBuffer.Handle),
                    vertexStride, 0));
            DeviceContext.InputAssembler.SetIndexBuffer(
                CppObject.FromPointer<SharpDX.Direct3D11.Buffer>(indexBuffer.Handle), Format.R32_UInt, 0);

            var pass = CG.GetFirstPass(techInfo.Technique);
            if (_lastAttrNum != techInfo.AttribsContainerNumber)
            {
                EffectManager.AttribsContainers[techInfo.AttribsContainerNumber].Accept();
                _lastAttrNum = techInfo.AttribsContainerNumber;
            }

            while (pass)
            {
                CG.SetPassState(pass);

                DeviceContext.DrawIndexed(indexCount, 0, 0);

                CG.ResetPassState(pass);
                pass = CG.GetNextPass(pass);
            }
        }

        public void EndDraw()
        {
            SwapChain.Present(Config.VerticalSyncEnabled ? 1 : 0, PresentFlags.None);
        }

        public RenderType Initialize()
        {
            #region Создаем фабрику, адаптер и получаем данные о видеокарте

            // Создаем фабрику, адаптер и получаем данные о видеокарте
            var factory = new Factory1();
            Adapter1 adapter = factory.GetAdapter1(0);

            if (Config.VerticalSyncEnabled)
            {
                var modes = adapter.Outputs[0].GetDisplayModeList(Format.R8G8B8A8_UNorm,
                    DisplayModeEnumerationFlags.Interlaced);
                for (int i = 0; i < modes.Length; i++)
                {
                    if (modes[i].Width != Config.Width || modes[i].Height != Config.Height) continue;
                    Rational = new Rational(modes[i].RefreshRate.Numerator, modes[i].RefreshRate.Denominator);
                    break;
                }
            }

            HardwareDescription.VideoCardMemory = adapter.Description.DedicatedVideoMemory >> 10 >> 10;
            HardwareDescription.VideoCardDescription = adapter.Description.Description;

            #endregion Создаем фабрику, адаптер и получаем данные о видеокарте

            #region Создаем SwapChain, Device, и DeviceContext

            //TODO DriverType
            Device = new Device(DriverType.Hardware,
                Config.IsDebug ? DeviceCreationFlags.Debug : DeviceCreationFlags.None, FeatureLevel.Level_11_0,
                FeatureLevel.Level_10_1,
                FeatureLevel.Level_10_0, FeatureLevel.Level_9_3, FeatureLevel.Level_9_2, FeatureLevel.Level_9_1);

            DeviceContext = Device.ImmediateContext;
            creator = new DxCreator(Device);
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
                OutputHandle = _renderForm.Handle,
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

            #endregion Создаем SwapChain, Device, и DeviceContext

            #region Initialize buffers

            // Get the pointer to the back buffer.
            using (var backBuffer = Resource.FromSwapChain<Texture2D>(SwapChain, 0))
                // Create the render target view with the back buffer pointer.
                RenderTargetView = new RenderTargetView(Device, backBuffer);


            // Create the texture for the depth buffer 
            DepthStencilBuffer = new Texture2D(Device, new Texture2DDescription
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
            });

            #endregion Initialize buffers

            #region Initialize Depth Stencil

            var desc = new DepthStencilStateDescription
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

            // Set the depth stencil state.
            DeviceContext.OutputMerger.SetDepthStencilState(DepthStencilState, 1);

            ////////////////////
            // Create the depth stencil state.
            DepthStencilState = new DepthStencilState(Device, desc);

            desc.IsDepthEnabled = false;

            // Create the depth stencil state.
            DepthDisabledStencilState = new DepthStencilState(Device, desc);

            #endregion Initialize Depth Enabled Stencil

            #region Initialize Output Merger

            // Create the depth stencil view.
            DepthStencilView = new DepthStencilView(Device, DepthStencilBuffer, new DepthStencilViewDescription
            {
                Format = Format.D24_UNorm_S8_UInt,
                Dimension = Enable4xMSAA
                    ? DepthStencilViewDimension.Texture2DMultisampled
                    : DepthStencilViewDimension.Texture2D,
                Texture2D = new DepthStencilViewDescription.Texture2DResource
                {
                    MipSlice = 0
                },
            });

            // Bind the render target view and depth stencil buffer to the output render pipeline.
            DeviceContext.OutputMerger.SetTargets(DepthStencilView, RenderTargetView);

            #endregion Initialize Output Merger

            #region Initialize Raster State

            // Create the rasterizer state from the description
            RasterState = new RasterizerState(Device, new RasterizerStateDescription
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
            });

            #endregion Initialize Raster State

            #region Initialize Rasterizer

            // Now set the rasterizer state.
            DeviceContext.Rasterizer.State = RasterState;

            // Setup and create the viewport for rendering.

            DeviceContext.Rasterizer.SetViewport(0, 0, Config.Width, Config.Height);

            #endregion Initialize Rasterizer

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

            DeviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
         
/*   var vp = new Viewport { Width = (Config.Width), Height = (Config.Height), MinDepth = 0.0f, MaxDepth = 1.0f, X = 0, Y = 0 };
            DeviceContext.Rasterizer.SetViewport(vp);*/

            CG.DX11.SetDevice(CgEffect.Context, Device.NativePointer);
            CG.DX11.RegisterStates(CgEffect.Context);
            CG.DX11.SetManageTextureParameters(CgEffect.Context, CG.True);



            return RenderType.DirectX11;
        }

        public ICreator GetCreator()
        {
            if (creator == null) ErrorProvider.SendError("ICreator is not inited", true);
            return creator;
        }

        public void Run(Action act)
        {
            RenderLoop.Run(_renderForm, () => act());
        }

        public void ZBuffer(bool turn)
        {
            DeviceContext.OutputMerger.SetDepthStencilState(turn ? DepthStencilState : DepthDisabledStencilState, 1);
        }

        public void SetBackBufferRenderTarget()
        {
            // Bind the render target view and depth stencil buffer to the output render pipeline.
            DeviceContext.OutputMerger.SetTargets(DepthStencilView, RenderTargetView);
        }

        protected override void OnDispose()
        {
            _renderForm.Close();
            if (SwapChain != null)
            {
                SwapChain.SetFullscreenState(false, null);
                SwapChain.Dispose();
                SwapChain = null;
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
    }
}