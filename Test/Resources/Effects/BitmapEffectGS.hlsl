Texture2D shaderTexture; 
SamplerState SampleType
{
	AddressU = Mirror;
	AddressV = Mirror;
};

struct VTexIn
{
	float2 position : POSITION;
	float2 size    : SIZE;
};

struct VTextureOutput 
{
  float4 position : SV_POSITION;
  float2 tex    : TEXCOORD0;
};

VTexIn VS(VTexIn outt)
{
	return outt;
}

[maxvertexcount(4)]
void GS(point VTexIn texin[1], inout TriangleStream<VTextureOutput> triStream) {
	VTextureOutput v;

	float bottom = texin[0].position.y - texin[0].size.y, right = texin[0].position.x + texin[0].size.x;

	v.position = float4(texin[0].position.x, bottom, 0, 1);
	v.tex = float2(0, 1);
	triStream.Append(v);

	v.position.y = texin[0].position.y;
	v.tex.y = 0;
	triStream.Append(v);

	v.position.xy = float2(right, bottom);
	v.tex = float2(1, 1);
	triStream.Append(v);

	v.position.y = texin[0].position.y;
	v.tex.y = 0;
	triStream.Append(v);

}

float4 PS( VTextureOutput input ) : SV_Target
{
  return shaderTexture.Sample(SampleType, input.tex);
}

technique
{
	VertexShader	= VS;
	GeometryShader	= GS;
	PixelShader		= PS;
}