///////////////////////
////   GLOBALS
///////////////////////
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

//////////////////////
////   TYPES
//////////////////////
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


/////////////////////////////////////
/////   Vertex Shader
/////////////////////////////////////
PixelInputType VS(VertexInputType vin)
{
	PixelInputType vout;
	vout.position = mul(float4(vin.position,1.0f), WorldViewProj);
	vout.tex = vin.tex;
	return vout;
}

//////////////////////
////   Pixel Shader
/////////////////////
float4 PS(PixelInputType pin) : SV_Target
{
	float4 textureColor;

	// Sample the pixel color from the texture using the sampler at this texture coordinate location.
	textureColor = shaderTexture.Sample(SampleType, pin.tex);
	textureColor += float4(1,0.5,6,4);
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
}