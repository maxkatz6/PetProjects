#region Using

using System.Collections.Generic;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using TDF.Graphics.Data;

#endregion

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

            EffectManager.Initialize();
        }
    }

    public static class EffectManager
    {
        #region ColorEffect Code
        private const string ColorShader = @"
cbuffer MatrixBuffer
{
	matrix WorldViewProj;
};

struct VertexInputType
{
	float4 position : POSITION;
	float4 color : COLOR;
};

struct PixelInputType
{
	float4 position : SV_POSITION;
	float4 color : COLOR;
};

PixelInputType VS(VertexInputType vin)
{
	vin.position = mul(vin.position, WorldViewProj);
	return vin;
}

float4 PS(PixelInputType pin) : SV_Target
{
	return pin.color;
}

technique11 Color_DirectX10
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_4_1, VS()));
		SetPixelShader(CompileShader(ps_4_1, PS()));
	}
}

technique11 Color_DirectX11
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_5_0, VS()));
		SetPixelShader(CompileShader(ps_5_0, PS()));
	}
}

technique11 Color_DirectX9
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_4_0_level_9_3, VS()));
		SetPixelShader(CompileShader(ps_4_0_level_9_3, PS()));
	}
}";
        #endregion
        #region TextureEffect Code
        private const string TextureShader = @"
Texture2D shaderTexture;
SamplerState SampleType
{
	Filter = MIN_MAG_MIP_LINEAR; 
	AddressU = Wrap;
	AddressV = Wrap;
	Filter = ANISOTROPIC; 
	MaxAnisotropy = 4;
	BorderColor = float4(0.0f, 0.0f, 1.0f, 1.0f);
};

cbuffer MatrixBuffer
{
	matrix WorldViewProj;
};

struct VertexInputType
{
	float3 position : POSITION;
	float2 tex : TEXCOORD0;
};

struct PixelInputType
{
	float4 position : SV_POSITION;
	float2 tex : TEXCOORD0;
};

PixelInputType VS(VertexInputType vin)
{
	PixelInputType vout;
	vout.position = mul(float4(vin.position,1.0f), WorldViewProj);
	vout.tex = vin.tex;
	return vout;
}

float4 PS(PixelInputType pin) : SV_Target
{
	float4 textureColor;
	textureColor = shaderTexture.Sample(SampleType, pin.tex);
	return textureColor;
}

technique11 Texture_DirectX10
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_4_1, VS()));
		SetPixelShader(CompileShader(ps_4_1, PS()));
	}
}

technique11 Texture_DirectX11
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_5_0, VS()));
		SetPixelShader(CompileShader(ps_5_0, PS()));
	}
}

technique11 Texture_DirectX9
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_4_0_level_9_3, VS()));
		SetPixelShader(CompileShader(ps_4_0_level_9_3, PS()));
	}
}";
        #endregion

        public static Effect[] Effects = new Effect[4];

        public static void Initialize()
        {
            var ce = new ColorEffect();
            var te = new TextureEffect();
            ce.InitializeFromMemory(ColorShader);
            te.InitializeFromMemory(TextureShader);

            Effects[0] = (ce);
            Effects[1] = (te);
        }
    }
}