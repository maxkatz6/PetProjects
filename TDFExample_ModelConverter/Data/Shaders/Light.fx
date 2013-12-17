//***************************************************************************************
// LightHelper.fx by Frank Luna (C) 2011 All Rights Reserved.
//
// Structures and functions for lighting calculations.
//***************************************************************************************

struct DirectionalLight
{
	float4 Ambient;
	float4 Diffuse;
	float4 Specular;
	float3 Direction;
	float pad;
};

struct PointLight
{
	float4 Ambient;
	float4 Diffuse;
	float4 Specular;

	float3 Position;
	float Range;

	float3 Att;
	float pad;
};

struct SpotLight
{
	float4 Ambient;
	float4 Diffuse;
	float4 Specular;

	float3 Position;
	float Range;

	float3 Direction;
	float Spot;

	float3 Att;
	float pad;
};

struct Material
{
	float4 Ambient;
	float4 Diffuse;
	float4 Specular; // w = SpecPower
	float4 Reflect;
};

//---------------------------------------------------------------------------------------
// Computes the ambient, diffuse, and specular terms in the lighting equation
// from a directional light.  We need to output the terms separately because
// later we will modify the individual terms.
//---------------------------------------------------------------------------------------
void ComputeDirectionalLight(Material mat, DirectionalLight L,
	float3 normal, float3 toEye,
	out float4 ambient,
	out float4 diffuse,
	out float4 spec)
{
	// Initialize outputs.
	ambient = float4(0.0f, 0.0f, 0.0f, 0.0f);
	diffuse = float4(0.0f, 0.0f, 0.0f, 0.0f);
	spec = float4(0.0f, 0.0f, 0.0f, 0.0f);

	// The light vector aims opposite the direction the light rays travel.
	float3 lightVec = -L.Direction;

		// Add ambient term.
		ambient = mat.Ambient * L.Ambient;

	// Add diffuse and specular term, provided the surface is in 
	// the line of site of the light.

	float diffuseFactor = dot(lightVec, normal);

	// Flatten to avoid dynamic branching.
	[flatten]
	if (diffuseFactor > 0.0f)
	{
		float3 v = reflect(-lightVec, normal);
			float specFactor = pow(max(dot(v, toEye), 0.0f), mat.Specular.w);

		diffuse = diffuseFactor * mat.Diffuse * L.Diffuse;
		spec = specFactor * mat.Specular * L.Specular;
	}
}

//---------------------------------------------------------------------------------------
// Computes the ambient, diffuse, and specular terms in the lighting equation
// from a point light.  We need to output the terms separately because
// later we will modify the individual terms.
//---------------------------------------------------------------------------------------
void ComputePointLight(Material mat, PointLight L, float3 pos, float3 normal, float3 toEye,
	out float4 ambient, out float4 diffuse, out float4 spec)
{
	// Initialize outputs.
	ambient = float4(0.0f, 0.0f, 0.0f, 0.0f);
	diffuse = float4(0.0f, 0.0f, 0.0f, 0.0f);
	spec = float4(0.0f, 0.0f, 0.0f, 0.0f);

	// The vector from the surface to the light.
	float3 lightVec = L.Position - pos;

		// The distance from surface to light.
		float d = length(lightVec);

	// Range test.
	if (d > L.Range)
		return;

	// Normalize the light vector.
	lightVec /= d;

	// Ambient term.
	ambient = mat.Ambient * L.Ambient;

	// Add diffuse and specular term, provided the surface is in 
	// the line of site of the light.

	float diffuseFactor = dot(lightVec, normal);

	// Flatten to avoid dynamic branching.
	[flatten]
	if (diffuseFactor > 0.0f)
	{
		float3 v = reflect(-lightVec, normal);
			float specFactor = pow(max(dot(v, toEye), 0.0f), mat.Specular.w);

		diffuse = diffuseFactor * mat.Diffuse * L.Diffuse;
		spec = specFactor * mat.Specular * L.Specular;
	}

	// Attenuate
	float att = 1.0f / dot(L.Att, float3(1.0f, d, d*d));

	diffuse *= att;
	spec *= att;
}

//---------------------------------------------------------------------------------------
// Computes the ambient, diffuse, and specular terms in the lighting equation
// from a spotlight.  We need to output the terms separately because
// later we will modify the individual terms.
//---------------------------------------------------------------------------------------
void ComputeSpotLight(Material mat, SpotLight L, float3 pos, float3 normal, float3 toEye,
	out float4 ambient, out float4 diffuse, out float4 spec)
{
	// Initialize outputs.
	ambient = float4(0.0f, 0.0f, 0.0f, 0.0f);
	diffuse = float4(0.0f, 0.0f, 0.0f, 0.0f);
	spec = float4(0.0f, 0.0f, 0.0f, 0.0f);

	// The vector from the surface to the light.
	float3 lightVec = L.Position - pos;

		// The distance from surface to light.
		float d = length(lightVec);

	// Range test.
	if (d > L.Range)
		return;

	// Normalize the light vector.
	lightVec /= d;

	// Ambient term.
	ambient = mat.Ambient * L.Ambient;

	// Add diffuse and specular term, provided the surface is in 
	// the line of site of the light.

	float diffuseFactor = dot(lightVec, normal);

	// Flatten to avoid dynamic branching.
	[flatten]
	if (diffuseFactor > 0.0f)
	{
		float3 v = reflect(-lightVec, normal);
			float specFactor = pow(max(dot(v, toEye), 0.0f), mat.Specular.w);

		diffuse = diffuseFactor * mat.Diffuse * L.Diffuse;
		spec = specFactor * mat.Specular * L.Specular;
	}

	// Scale by spotlight factor and attenuate.
	float spot = pow(max(dot(-lightVec, L.Direction), 0.0f), L.Spot);

	// Scale by spotlight factor and attenuate.
	float att = clamp(
		0.0,
		1.0,
		1.0 / (
		L.Att.x +
		(L.Att.y * d) +
		(L.Att.z * d * d)
		)
		);

	ambient *= spot;
	diffuse *= att;
	spec *= att;
}

float3 NormalSampleToWorldSpace(float3 normalMapSample, float3 unitNormalW, float3 tangentW)
{
	// Uncompress each component from [0,1] to [-1,1].
	float3 normalT = 2.0f*normalMapSample - 1.0f;

		// Build orthonormal basis.
		float3 N = unitNormalW;
		float3 T = normalize(tangentW - dot(tangentW, N)*N);
		float3 B = cross(N, T);

		float3x3 TBN = float3x3(T, B, N);

		// Transform from tangent space to world space.
		float3 bumpedNormalW = mul(normalT, TBN);

		return bumpedNormalW;
}

///////////////////////
////   GLOBALS
///////////////////////
#define DLMaxCount  5
#define PLMaxCount  10
#define SLMaxCount  5

Texture2D shaderTexture;
Texture2D gNormalMap;

SamplerState SampleType
{
	Filter = MIN_MAG_MIP_LINEAR;
	AddressU = Wrap;
	AddressV = Wrap;
	Filter = ANISOTROPIC;
	MaxAnisotropy = 4;
	BorderColor = float4(0.0f, 0.0f, 1.0f, 1.0f);
};

cbuffer BuffersPerFrame
{
	float3 LightCount;
	DirectionalLight gDirLight[DLMaxCount];
	PointLight gPointLight[PLMaxCount];
	SpotLight gSpotLight[SLMaxCount];
	float3 gEyePosW;
};

cbuffer BuffersPerObject
{
	float4x4 World;
	float4x4 WorldInvTranspose;
	float4x4 WorldViewProj;
	//float4x4 TexTransform;
	Material gMaterial;
};

struct VertexIn
{
	float3 position : POSITION;
	float2 tex : TEXCOORD;
	float3 normal : NORMAL;
	float3 tangent : TANGENT;
};

struct VertexOut
{
	float4 position    : SV_POSITION;
	float3 positionW : POSITION;
	float2 tex : TEXCOORD;
	float3 normal : NORMAL;
	float3 tangent : TANGENT;
};

VertexOut VS(VertexIn vin)
{
	VertexOut vout;
	vout.position = mul(float4(vin.position, 1.0f), WorldViewProj);
	vout.positionW = mul(float4(vin.position, 1.0f), World).xyz;
	vout.tex = vin.tex;// mul(float4(vin.tex, 0.0f, 1.0f), TexTransform).xy;
	vout.normal = mul(vin.normal, (float3x3)WorldInvTranspose);
	vout.tangent = mul(vin.tangent, (float3x3)World);

	return vout;
}

float4 PS(VertexOut pin) : SV_Target
{
	// Interpolating normal can unnormalize it, so normalize it.
	pin.normal = normalize(pin.normal);

	// The toEye vector is used in lighting.
	float3 toEye = gEyePosW - pin.positionW;

		// Cache the distance to the eye from this surface point.
		float distToEye = length(toEye);

	// Normalize.
	toEye /= distToEye;

	// Sample the pixel color from the texture using the sampler at this texture coordinate location.
	float4 textureColor = shaderTexture.Sample(SampleType, pin.tex);

		float3 normalMapSample = gNormalMap.Sample(SampleType, pin.tex).rgb;
		float3 bumpedNormalW = NormalSampleToWorldSpace(normalMapSample, pin.normal , pin.tangent);

	float4 litColor = textureColor;

	float4 ambient = float4(0.0f, 0.0f, 0.0f, 0.0f);
	float4 diffuse = float4(0.0f, 0.0f, 0.0f, 0.0f);
	float4 spec = float4(0.0f, 0.0f, 0.0f, 0.0f);
	// Sum the light contribution from each light source.

	[unroll]
	for (int i = 0; i < LightCount.x; i++){
		float4 A, D, S;
		ComputeDirectionalLight(gMaterial, gDirLight[i], bumpedNormalW, toEye, A, D, S);

		ambient += A;
		diffuse += D;
		spec += S;
	}
	[unroll]
	for ( i = 0; i < LightCount.y; i++){
		float4 A, D, S;
		ComputePointLight(gMaterial, gPointLight[i], pin.positionW, bumpedNormalW, toEye, A, D, S);

		ambient += A;
		diffuse += D;
		spec += S;
	}
	[unroll]
	for (i = 0; i < LightCount.z; i++){
		float4 A, D, S;
		ComputeSpotLight(gMaterial, gSpotLight[i], pin.positionW, bumpedNormalW, toEye, A, D, S);

		ambient += A;
		diffuse += D;
		spec += S;
	}

	// Modulate with late add.
	litColor = textureColor*(ambient + diffuse) + spec;
	// Common to take alpha from diffuse material and texture.
	litColor.a = gMaterial.Diffuse.a * textureColor.a;

	return litColor;
}

technique11 Light_DirectX10
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_4_1, VS()));
		SetPixelShader(CompileShader(ps_4_1, PS()));
	}
}

technique11 Light_DirectX11
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_5_0, VS()));
		SetPixelShader(CompileShader(ps_5_0, PS()));
	}
}

technique11 Light_DirectX9
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_4_0_level_9_3, VS()));
		SetPixelShader(CompileShader(ps_4_0_level_9_3, PS()));
	}
}


