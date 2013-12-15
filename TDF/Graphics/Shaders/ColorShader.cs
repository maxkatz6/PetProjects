using TDF.Graphics.Data;
using TDF.Core;
using TDF.Graphics.Render;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Runtime.InteropServices;
using Buffer = SharpDX.Direct3D11.Buffer;
using MapFlags = SharpDX.Direct3D11.MapFlags;

namespace TDF.Graphics.Shaders
{
    public static class ColorShader
    {
        #region Структуры

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct MatrixBuffer
        {
            public Matrix WVP;
        }

        #endregion Структуры

        static private Buffer ConstantMartixBuffer { get; set; }

        static private InputLayout Layout { get; set; }

        static private PixelShader PixelShader { get; set; }

        static private VertexShader VertexShader { get; set; }

        static public bool Initialize()
        {//Инициализируем шейдеры
            return InitializeShader("color.vs", "color.ps");
        }

        static public bool Render(int indexCount, Matrix wvp)
        {
            //передаем параметры
            if (!SetShaderParameters(wvp)) return false;

            RenderShader(indexCount);
            return true;
        }

        static public void Shutdown()
        {
            ShutdownShader();
        }

        static private bool InitializeShader(string vsFileName, string psFileName)
        {
            try
            {
                //Компилирем шейдеры
                var vertexShaderByteCode = ShaderBytecode.CompileFromFile(Config.ShadersFilePath + vsFileName, "ColorVertexShader", Config.VShaderVersion);
                var pixelShaderByteCode = ShaderBytecode.CompileFromFile(Config.ShadersFilePath + psFileName, "ColorPixelShader", Config.PShaderVersion);

                //Создаем шейдеры с буфера
                VertexShader = new VertexShader(DirectX11.Device, vertexShaderByteCode);
                PixelShader = new PixelShader(DirectX11.Device, pixelShaderByteCode);

                // Теперь ставим расположение данных, которые идут в шейдер.
                // Эта установка должна соответствовать структуре VertexType в модели и в шефдере.
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
                                SemanticName = "COLOR",
                                SemanticIndex = 0,
                                Format = Format.R32G32B32A32_Float,
                                Slot = 0,
                                AlignedByteOffset = 12,
                                Classification = InputClassification.PerVertexData,
                                InstanceDataStepRate = 0
                            }
                    };

                //Создание входного вершиного слоя
                Layout = new InputLayout(DirectX11.Device, ShaderSignature.GetInputSignature(vertexShaderByteCode), inputElements);

                //Выгружаем шейдерные байт-коды
                vertexShaderByteCode.Dispose();
                pixelShaderByteCode.Dispose();

                //Настройка динамической матрицы постоянного буфера, который находится в вершинных шейдерах.
                var matrixBufferDesc = new BufferDescription
                {
                    Usage = ResourceUsage.Dynamic,
                    SizeInBytes = Utilities.SizeOf<Matrix>(),
                    BindFlags = BindFlags.ConstantBuffer,
                    CpuAccessFlags = CpuAccessFlags.Write,
                    OptionFlags = ResourceOptionFlags.None,
                    StructureByteStride = 0
                };
                // Создать постоянный буфер, чтобы мы могли получить доступ к вершинным шейдерам буфера внутри этого класса.
                ConstantMartixBuffer = new Buffer(DirectX11.Device, matrixBufferDesc);
                return true;
            }
            catch (Exception ex)
            {
                ErrorProvider.Send(ex);
                return false;
            }
        }

        static private void RenderShader( int indexCount)
        {
            DirectX11.DeviceContext.InputAssembler.InputLayout = Layout;//Передаем вершинный входной макет

            //Передаем шейдеры, какие будут использованы для рендера триугольник
            DirectX11.DeviceContext.VertexShader.Set(VertexShader);
            DirectX11.DeviceContext.PixelShader.Set(PixelShader);

            //Рисуем триугольник
            DirectX11.DeviceContext.DrawIndexed(indexCount, 0, 0);
        }

        static private bool SetShaderParameters( Matrix wvp)
        {
            try
            {
                //Блокируем константный буфер, поэтому он может быть записан
                DataStream mappedResource;
                DirectX11.DeviceContext.MapSubresource(ConstantMartixBuffer, MapMode.WriteDiscard, MapFlags.None,
                                             out mappedResource);

                //Копируем матрицы в константный буфер
                var matrixBuffer = new MatrixBuffer
                {
                    WVP = Matrix.Transpose(wvp)
                };
                mappedResource.Write(matrixBuffer);

                //Разблокируем константный буфер
                DirectX11.DeviceContext.UnmapSubresource(ConstantMartixBuffer, 0);

                //И наконец передаем константный буфер в вершинный шефдер с загруженными значениями
                DirectX11.DeviceContext.VertexShader.SetConstantBuffer(0, ConstantMartixBuffer);

                return true;
            }
            catch (Exception ex)
            {
                ErrorProvider.Send(ex);
                return false;
            }
        }

        static private void ShutdownShader()
        {
            if (ConstantMartixBuffer != null)
            {
                ConstantMartixBuffer.Dispose();
                ConstantMartixBuffer = null;
            }
            if (Layout != null)
            {
                Layout.Dispose();
                Layout = null;
            }
            if (PixelShader != null)
            {
                PixelShader.Dispose();
                PixelShader = null;
            }
            if (VertexShader != null)
            {
                VertexShader.Dispose();
                VertexShader = null;
            }
        }
    }
}