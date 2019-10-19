#if DX

using System;
using Lain.Core;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using Device = SharpDX.Direct3D11.Device;
using ResultCode = SharpDX.DXGI.ResultCode;

namespace Lain.GAPI
{
    public class Render1 : Render
    {
        internal object RenderSource { get; set; }

        public void Suspend()
        {
            using (var dxgiDevice = Device.QueryInterface<SharpDX.DXGI.Device3>())
                dxgiDevice.Trim();
        }

        public new static Render1 Create(object renderSource)
        {
            try
            {
                var render = new Render1
                {
                    Device = new Device(DriverType.Hardware,
                        (Config.IsDebug ? DeviceCreationFlags.Debug : DeviceCreationFlags.None) |
                        DeviceCreationFlags.BgraSupport, FeatureLevel.Level_11_0,
                        FeatureLevel.Level_10_1,
                        FeatureLevel.Level_10_0, FeatureLevel.Level_9_3),
                    RenderSource = renderSource
                };
                // 1011 0001 0000 0000 -> 1011 0001 -> 11 0001 -> 4 -> 5
                render.ShaderModel = (((int) render.Device.FeatureLevel >> 8) & 0x33)/10 + 1;
                render.DeviceContext = render.Device.ImmediateContext;

                // Get the Direct3D 11.1 API device.
                using (var dxgiDevice = render.Device.QueryInterface<SharpDX.DXGI.Device>())
                using (var sisNative = ComObject.QueryInterface<ISurfaceImageSourceNative>(render.RenderSource))
                    sisNative.Device = dxgiDevice;

                render.DeviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

                render.DeviceContext.OutputMerger.SetRenderTargets(render.DepthStencilView, render.RenderTargetView);
                render.DeviceContext.Rasterizer.State = new RasterizerState(render.Device,
                    new RasterizerStateDescription
                    {
                        IsAntialiasedLineEnabled = false,
                        CullMode = CullMode.None,
                        DepthBias = 0,
                        DepthBiasClamp = 0,
                        IsDepthClipEnabled = true,
                        FillMode = FillMode.Solid,
                        IsFrontCounterClockwise = false,
                        IsMultisampleEnabled = false,
                        IsScissorEnabled = true,
                        SlopeScaledDepthBias = 0
                    });
                // Set viewport to the target area in the surface, taking into account the offset returned by BeginDraw.
                render.DeviceContext.Rasterizer.SetViewport(new ViewportF(0, 0, Config.Width, Config.Height));

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
            catch (Exception ex)
            {
                Log.SendError(ex, true);
                return null;
            }
        }

        public override void BeginDraw()
        {
            Utilities.Dispose(ref RenderTargetView);
            Utilities.Dispose(ref DepthStencilView);

            // Express target area as a native RECT type.
            var updateRectNative = new Rectangle(0, 0, Config.Width, Config.Height);

            // Query for ISurfaceImageSourceNative interface.
            using (var sisNative = ComObject.QueryInterface<ISurfaceImageSourceNative>(RenderSource))
            {
                // Begin drawing - returns a target surface and an offset to use as the top left origin when drawing.
                try
                {
                    RawPoint offset;
                    using (var surface = sisNative.BeginDraw(updateRectNative, out offset))
                    {
                        // QI for target texture from DXGI surface.
                        using (var d3DTexture = surface.QueryInterface<Texture2D>())
                        {
                            RenderTargetView = new RenderTargetView(Device, d3DTexture);
                        }

                        // Set viewport to the target area in the surface, taking into account the offset returned by BeginDraw.
                        DeviceContext.Rasterizer.SetViewport(new ViewportF(offset.X, offset.Y, Config.Width, Config.Height));

                        // Get the surface description in order to determine its size. The size of the depth/stencil buffer and the RenderTargetView must match, so the 
                        // depth/stencil buffer must be the same size as the surface. Since the whole surface returned by BeginDraw can potentially be much larger than 
                        // the actual update rect area passed into BeginDraw, it may be preferable for some apps to include an intermediate step which instead creates a 
                        // separate smaller D3D texture and renders into it before calling BeginDraw, then simply copies that texture into the surface returned by BeginDraw. 
                        // This would prevent needing to create a depth/stencil buffer which is potentially much larger than required, thereby saving memory at the cost of 
                        // additional overhead due to the copy operation.
                        var surfaceDesc = surface.Description;

                        // Create depth/stencil buffer descriptor.
                        var depthStencilDesc = new Texture2DDescription
                        {
                            Format = Format.D24_UNorm_S8_UInt,
                            Width = surfaceDesc.Width,
                            Height = surfaceDesc.Height,
                            ArraySize = 1,
                            MipLevels = 1,
                            BindFlags = BindFlags.DepthStencil,
                            SampleDescription = new SampleDescription(1, 0),
                            Usage = ResourceUsage.Default
                        };

                        // Allocate a 2-D surface as the depth/stencil buffer.
                        using (var depthStencil = new Texture2D(Device, depthStencilDesc))
                            DepthStencilView = new DepthStencilView(Device, depthStencil);

                        // Set render target.
                        DeviceContext.OutputMerger.SetRenderTargets(DepthStencilView, RenderTargetView);
                    }
                }
                catch (SharpDXException ex)
                {
                    if (ex.ResultCode == ResultCode.DeviceRemoved ||
                        ex.ResultCode == ResultCode.DeviceReset)
                    {
                        // If the device has been removed or reset, attempt to recreate it and continue drawing.
                        App.Render = Create(RenderSource);
                        BeginDraw();
                    }
                    else Log.SendError(ex, true);
                }
            }

            DeviceContext.ClearDepthStencilView(DepthStencilView,
                DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1, 8);
            DeviceContext.ClearRenderTargetView(RenderTargetView, BackColor);
        }

        public override void EndDraw()
        {
            using (var sisNative = ComObject.QueryInterface<ISurfaceImageSourceNative>(RenderSource))
                sisNative.EndDraw();
        }
    }
}

#endif