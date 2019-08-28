#include "..\\Headers\\LightParameters.h"
#include "..\\Headers\\Textureing.h"

float4x4 gInvViewProj;

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float3 Normal : NORMAL0;
	float2 uv : TEXCOORD0;
	float3 binormal : BINORMAL0;
};

struct VertexShaderOutput
{
	float4 position : POSITION0;
	float2 scrPos : TEXCOORD1;
	float2 uv : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(float4 position : POSITION0, float2 uv : TEXCOORD0)
{
	VertexShaderOutput output;

	output.position = position;
	output.scrPos = position.xy;
	output.uv = uv;

	return output;
}

float4 CalculateWorldSpacePosition(float2 pixelPosition, float pixelDepth, 
								   float4x4 inverseViewProjection)
{
	float4 viewPos = mul(float4(pixelPosition, pixelDepth, 1.f), inverseViewProjection);

	return viewPos / viewPos.w;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 diffuse = DiffuseMap.Sample(DiffuseSampler, input.uv);
	float3 normal = NormalMap.Sample(NormalSampler, input.uv).xyz;
	float4 specular = SpecularMap.Sample(SpecularSampler, input.uv);
	float4 misc = DepthMap.Sample(DepthSampler, input.uv);
	float depth = misc.r / misc.g;
	float SpecularIntensity = misc.b * 10.0f;
	float SpecularPower = misc.a * 10.0f;

	normal = (normal * 2.f) - 1.f;

	float nDotL = dot(normal, LightDirection);

	float4 posWorld = CalculateWorldSpacePosition(input.scrPos.xy, depth, gInvViewProj);
	float3 directionToCamera = normalize(gCameraPos - posWorld);
	float3 reflectionVector = reflect(normal, -LightDirection);
	float specular_nDotL = saturate(dot(reflectionVector, directionToCamera));
	float SpecularMod = SpecularIntensity * pow(specular_nDotL, SpecularPower);

	float4 finalDiffuse = saturate(nDotL) * diffuse * DiffuseColor;
	float4 finalSpecular = specular * saturate(SpecularMod) * specular_nDotL *  (specular_nDotL > 0.0f ? float4(SpecularColor.xyz, 1.f) : (float4)0);

    return finalDiffuse + finalSpecular;
}

technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
