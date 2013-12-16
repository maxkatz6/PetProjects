#region Using

using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System.Collections.Generic;
using TDF.Graphics.Data;

#endregion Using

namespace TDF.Graphics.Effects
{
    public static class InputElements
    {
        public static readonly List<int> Size =
            new List<int>
            {
                Utilities.SizeOf<ColorVertex>(),
                Utilities.SizeOf<TextureVertex>(),
                Utilities.SizeOf<LightVertex>(),
                Utilities.SizeOf<BumpVertex>()
            };

        public static List<InputElement[]> InputElementType = new List<InputElement[]>(4);

        public static void Initialize()
        {
            InputElementType.Add(new[]
            {
                new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0, InputClassification.PerVertexData, 0),
                new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 12, 0, InputClassification.PerVertexData, 0)
            });

            InputElementType.Add(new[]
            {
                new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0, InputClassification.PerVertexData, 0),
                new InputElement("TEXCOORD", 0, Format.R32G32_Float, 12, 0, InputClassification.PerVertexData, 0)
            });

            InputElementType.Add(new[]
            {
                new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0, InputClassification.PerVertexData, 0),
                new InputElement("TEXCOORD", 0, Format.R32G32_Float, 12, 0, InputClassification.PerVertexData, 0),
                new InputElement("NORMAL", 0, Format.R32G32B32_Float, 20, 0, InputClassification.PerVertexData, 0)
            });

            InputElementType.Add(new[]
            {
                new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0, InputClassification.PerVertexData, 0),
                new InputElement("TEXCOORD", 0, Format.R32G32_Float, 12, 0, InputClassification.PerVertexData, 0),
                new InputElement("NORMAL", 0, Format.R32G32B32_Float, 20, 0, InputClassification.PerVertexData, 0),
                new InputElement("TANGENT", 0, Format.R32G32B32_Float, 32, 0, InputClassification.PerVertexData, 0)
            });
        }
    }
}