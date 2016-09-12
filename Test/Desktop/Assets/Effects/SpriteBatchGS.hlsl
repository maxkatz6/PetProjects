Texture2D shaderTexture;
SamplerState SampleType
{
	AddressU = Mirror;
	AddressV = Mirror;
};

struct VTexIn
{
	float4 dest : POSITION;
	float4 source : SIZE;
	float4 color : COLOR;
};

struct VTextureOutput
{
	float4 position : SV_POSITION;
	float2 tex    : TEXCOORD0;
	float4 color : COLOR;
};

VTexIn VS(VTexIn outt)
{
	return outt;
}

[maxvertexcount(4)]
void GS(point VTexIn texin[1], inout TriangleStream<VTextureOutput> triStream) {
	VTextureOutput v;
	v.color = texin[0].color;

	v.position = float4(texin[0].dest.x, texin[0].dest.w, 0, 1);
	v.tex = float2(texin[0].source.x, texin[0].source.w);
	triStream.Append(v);

	v.position.x = texin[0].dest.z;
	v.tex.x = texin[0].source.z;
	triStream.Append(v);

	v.position.xy = float2(texin[0].dest.x, texin[0].dest.y);
	v.tex = float2(texin[0].source.x, texin[0].source.y);
	triStream.Append(v);

	v.position.x = texin[0].dest.z;
	v.tex.x = texin[0].source.z;
	triStream.Append(v);
}

float4 PS(VTextureOutput input) : SV_Target
{
	return shaderTexture.Sample(SampleType, input.tex)*input.color;
}

static const float spread = 4;
static const float scale = 0.35;
static const float smoothing = 0.25 / (spread * scale);
float4 PS_SDF(VTextureOutput input) : SV_Target
{

	float distance = shaderTexture.Sample(SampleType, input.tex).a;
	//clip(distance - (0.5f - smoothing));
	return float4(input.color.rgb, smoothstep(0.5 - smoothing, 0.5 + smoothing, distance) * input.color.a);
}

technique
{
	VertexShader = VS;
	GeometryShader = GS;
	PixelShader = PS;
}
technique SDF
{
	VertexShader = VS;
	GeometryShader = GS;
	PixelShader = PS_SDF;
}

/*		v.position = float4(texin[0].dest.x, texin[0].dest.w, 0, 1);
v.tex = float2(texin[0].source.x, texin[0].source.w);
triStream.Append(v);

v.position = float4(texin[0].dest.z, texin[0].dest.w, 0, 1);
v.tex = float2(texin[0].source.z, texin[0].source.w);
triStream.Append(v);

v.position = float4(texin[0].dest.x, texin[0].dest.y, 0, 1);
v.tex = float2(texin[0].source.x, texin[0].source.y);
triStream.Append(v);

v.position = float4(texin[0].dest.z, texin[0].dest.y, 0, 1);
v.tex = float2(texin[0].source.z, texin[0].source.y);
triStream.Append(v);*/