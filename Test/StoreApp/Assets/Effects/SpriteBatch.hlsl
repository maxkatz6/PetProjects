Texture2D shaderTexture;
SamplerState SampleType
{
	AddressU = Mirror;
	AddressV = Mirror;
};

struct VTextureOutput
{
	float4 position : SV_POSITION;
	float2 tex    : TEXCOORD0;
	float4 color : COLOR;
};

VTextureOutput VS(float2 pos : POSITION, float2 tex : TEXCOORD0, float4 color : COLOR)
{
	VTextureOutput v;
	v.position = float4(pos, 0, 1);
	v.color = color;
	v.tex = tex;
	return v;
}

float4 PS(VTextureOutput input) : SV_Target
{
	return shaderTexture.Sample(SampleType, input.tex)*input.color;
}

static const float spread = 4;
static const float scale = 0.45;
static const float smoothing = 0.25 / (spread * scale);
float4 PS_SDF(VTextureOutput input) : SV_Target
{

	float distance = shaderTexture.Sample(SampleType, input.tex).a;
	clip(distance - (0.5f - smoothing));
	return float4(input.color.rgb, smoothstep(0.5 - smoothing, 0.5 + smoothing, distance) * input.color.a);
}

technique
{
	VertexShader = VS;
	PixelShader = PS;
}
technique SDF
{
	VertexShader = VS;
	PixelShader = PS_SDF;
}