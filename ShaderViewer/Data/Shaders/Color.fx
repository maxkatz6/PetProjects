///////////////////////
////   GLOBALS
///////////////////////
cbuffer MatrixBuffer
{
	matrix WorldViewProj;
};

//////////////////////
////   TYPES
//////////////////////
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

/////////////////////////////////////
/////   Vertex Shader
/////////////////////////////////////
PixelInputType VS(VertexInputType vin)
{
	vin.position = mul(vin.position, WorldViewProj);
	return vin;
}

//////////////////////
////   Pixel Shader
/////////////////////
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
}