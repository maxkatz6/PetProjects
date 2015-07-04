Texture2D shaderTexture; 
SamplerState SampleType
{
	AddressU = Mirror;
	AddressV = Mirror;
};
cbuffer MatrixBuffer
{
	matrix WVP;
};

struct VTextureOutput 
{
  float4 position : SV_POSITION;
  float2 tex    : TEXCOORD0;
};

VTextureOutput VS( float3 position : POSITION , float2 tex : TEXCOORD0)
{	
  VTextureOutput OUT;
  
  OUT.position =mul( float4( position, 1 ), WVP);
  OUT.tex = tex;

  return OUT;
}

float4 PS( VTextureOutput input ) : SV_Target
{
  return shaderTexture.Sample(SampleType, input.tex);
}