#include "Headers/LightParameters.h"
#include "Headers/Textureing.h"

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float2 pixelPosition = float2(input.scrPos.x, input.scrPos.y);
	float2 texCoord = input.uv;


	float4 diffuse = DiffuseMap.Sample(DiffuseMapSampler, texCoord);
	float4 specular = SpecularMap.Sample(SpecularMapSampler, texCoord);
	float3 normal = NormalMap.Sample(NormalMapSampler, texCoord).xyz;
	float4 misc = DepthMap.Sample(DepthMapSampler, texCoord);
	float depth = misc.r / misc.g;


	float4 posWorld = CalculateWorldSpacePosition(input.scrPos.xy, depth, gInvViewProj);
	float3 toLight = LightPosition - posWorld;
	float distanceToLight = length(toLight);

	toLight = normalize(toLight);
	float attenuation = CalculateAttenuation(Intensity, distanceToLight, Range);

	normal = (normal - 0.5f) * 2.f;
	float nDotL = saturate(dot(normal, toLight)) * attenuation;

	float4 finalDiffuse = diffuse * nDotL * DiffuseColor;
	float4 finalSpecular = specular * nDotL;

	return finalDiffuse + finalSpecular;
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile vs_5_0 VertexShaderFunction();
		PixelShader = compile ps_5_0 PixelShaderFunction();
	}
};