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
};

VTextureOutput VS(float2 position : POSITION, float2 tex : TEXCOORD0)
{
	VTextureOutput OUT;
	OUT.position = float4(position, 0, 1);
	OUT.tex = tex;
	return OUT;
}

float4 PS( VTextureOutput input ) : SV_Target
{
  return shaderTexture.Sample(SampleType, input.tex);
}

technique
{
	VertexShader = VS;
	PixelShader = PS;
}