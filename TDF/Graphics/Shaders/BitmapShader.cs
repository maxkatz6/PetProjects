using TDF.Graphics;
using TDF.Core;
using TDF.Graphics.Render;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Runtime.InteropServices;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace TDF.Graphics.Shaders
{
    public static class BitmapShader
    {
        #region Структуры



        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct MatrixBuffer
        {
            public Matrix WVP;
        }

        #endregion Структуры

        #region Свойства

        private static Buffer ConstantMatrixBuffer { get; set; }

        private static InputLayout Layout { get; set; }

        private static PixelShader PixelShader { get; set; }

        private static SamplerState SampleState { get; set; }

        private static VertexShader VertexShader { get; set; }

        #endregion Свойства

        #region Методы

        static public bool Initialize()
        {
            // Инициализируем шейдеры
            return InitializeShader("texture.vs", "texture.ps");
        }

        static public bool Render(int indexCount, Matrix wvp, ShaderResourceView texture)
        {
            // Set the shader parameters that it will use for rendering.
            if (!SetShaderParameters(wvp, texture))
                return false;

            // Now render the prepared buffers with the shader.
            RenderShader(indexCount);

            return true;
        }

        static public void Shutdown()
        {
            // Release the sampler state.
            if (SampleState != null)
            {
                SampleState.Dispose();
                SampleState = null;
            }

            // Release the matrix constant buffer.
            if (ConstantMatrixBuffer != null)
            {
                ConstantMatrixBuffer.Dispose();
                ConstantMatrixBuffer = null;
            }

            // Release the layout.
            if (Layout != null)
            {
                Layout.Dispose();
                Layout = null;
            }

            // Release the pixel shader.
            if (PixelShader != null)
            {
                PixelShader.Dispose();
                PixelShader = null;
            }

            // Release the vertex shader.
            if (VertexShader != null)
            {
                VertexShader.Dispose();
                VertexShader = null;
            }
        }

        static private bool InitializeShader(string vsFileName, string psFileName)
        {
            try
            {
                // Setup full pathes
                vsFileName = Config.ShadersFilePath + vsFileName;
                psFileName = Config.ShadersFilePath + psFileName;

                // Compile the vertex shader code.
                var vertexShaderByteCode = ShaderBytecode.CompileFromFile(vsFileName, "TextureVertexShader", "vs_4_0_level_9_1");
                // Compile the pixel shader code.
                var pixelShaderByteCode = ShaderBytecode.CompileFromFile(psFileName, "TexturePixelShader", Config.PShaderVersion);

                // Create the vertex shader from the buffer.
                VertexShader = new VertexShader(DirectX11.Device, vertexShaderByteCode);
                // Create the pixel shader from the buffer.
                PixelShader = new PixelShader(DirectX11.Device, pixelShaderByteCode);

                // Now setup the layout of the data that goes into the shader.
                // This setup needs to match the VertexType structure in the NModel and in the shader.
                var inputElements = new[]
				{
					new InputElement
					{
						SemanticName = "POSITION",
						SemanticIndex = 0,
						Format = Format.R32G32B32_Float,
						Slot = 0,
						AlignedByteOffset = 0,
						Classification = InputClassification.PerVertexData,
						InstanceDataStepRate = 0
					},
					new InputElement
					{
						SemanticName = "TEXCOORD",
						SemanticIndex = 0,
						Format = Format.R32G32_Float,
						Slot = 0,
						AlignedByteOffset = 12,
						Classification = InputClassification.PerVertexData,
						InstanceDataStepRate = 0
					}
				};

                // Create the vertex input the layout.
                Layout = new InputLayout(DirectX11.Device, ShaderSignature.GetInputSignature(vertexShaderByteCode), inputElements);

                // Release the vertex and pixel shader buffers, since they are no longer needed.
                vertexShaderByteCode.Dispose();
                pixelShaderByteCode.Dispose();

                // Setup the description of the dynamic matrix constant buffer that is in the vertex shader.
                var matrixBufferDesc = new BufferDescription()
                {
                    Usage = ResourceUsage.Dynamic,
                    SizeInBytes = Utilities.SizeOf<MatrixBuffer>(),
                    BindFlags = BindFlags.ConstantBuffer,
                    CpuAccessFlags = CpuAccessFlags.Write,
                    OptionFlags = ResourceOptionFlags.None,
                    StructureByteStride = 0
                };

                // Create the constant buffer pointer so we can access the vertex shader constant buffer from within this class.
                ConstantMatrixBuffer = new Buffer(DirectX11.Device, matrixBufferDesc);

                // Create a texture sampler state description.
                var samplerDesc = new SamplerStateDescription()
                {
                    Filter = Filter.MinMagMipLinear,
                    AddressU = TextureAddressMode.Wrap,
                    AddressV = TextureAddressMode.Wrap,
                    AddressW = TextureAddressMode.Wrap,
                    MipLodBias = 0,
                    MaximumAnisotropy = 1,
                    ComparisonFunction = Comparison.Always,
                    BorderColor = new Color4(0, 0, 0, 0),
                    MinimumLod = 0,
                    MaximumLod = float.MaxValue
                };

                // Create the texture sampler state.
                SampleState = new SamplerState(DirectX11.Device, samplerDesc);
                return true;
            }
            catch (Exception ex)
            {
                ErrorProvider.Send(ex);
                return false;
            }
        }

        static private void RenderShader(int indexCount)
        {
            // Set the vertex input layout.
            DirectX11.DeviceContext.InputAssembler.InputLayout = Layout;

            // Set the vertex and pixel shaders that will be used to render this triangle.
            DirectX11.DeviceContext.VertexShader.Set(VertexShader);
            DirectX11.DeviceContext.PixelShader.Set(PixelShader);

            // Set the sampler state in the pixel shader.
            DirectX11.DeviceContext.PixelShader.SetSampler(0, SampleState);
            // Render the triangle.
            DirectX11.DeviceContext.DrawIndexed(indexCount, 0, 0);
        }

        static private bool SetShaderParameters(Matrix wvp, ShaderResourceView texture)
        {
            try
            {
                // Lock the constant buffer so it can be written to.
                DataStream mappedResource;
                DirectX11.DeviceContext.MapSubresource(ConstantMatrixBuffer, MapMode.WriteDiscard, SharpDX.Direct3D11.MapFlags.None, out mappedResource);

                mappedResource.Write(new MatrixBuffer
                {
                    WVP = Matrix.Transpose(wvp),
                });

                // Unlock the constant buffer.
                DirectX11.DeviceContext.UnmapSubresource(ConstantMatrixBuffer, 0);

                // Finally set the constant buffer in the vertex shader with the updated values.
                DirectX11.DeviceContext.VertexShader.SetConstantBuffer(0, ConstantMatrixBuffer);

                // Set shader resource in the pixel shader.
                DirectX11.DeviceContext.PixelShader.SetShaderResource(0, texture);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion Методы
    }
}