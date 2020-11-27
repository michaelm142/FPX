#include "Headers/LightParameters.h"
#include "Headers/Textureing.h"

LightPsOut PixelShaderFunction(GB_VS_OUT input) : COLOR0
{

	float4 diffuse = DiffuseMap.Sample(DiffuseMapSampler, input.uv);
	float3 normal = NormalMap.Sample(NormalMapSampler, input.uv).xyz;
	float4 specular = SpecularMap.Sample(SpecularMapSampler, input.uv);
	float4 misc = DepthMap.Sample(DepthMapSampler, input.uv);
	if (misc.x == 0.0f || misc.y == 0.0f)
		discard;
	float depth = misc.r / misc.g;
	float SpecularIntensity = misc.b * 10.0f;
	float SpecularPower = misc.a * 10.0f;

	normal = (normal * 2.f) - 1.f;

	float nDotL = dot(normal, LightDirection);

	float4 posWorld = CalculateWorldSpacePosition(input.scrPos.xy, depth, gInvViewProj);
	float3 directionToCamera = normalize(gCameraPos - posWorld);
	float3 reflectionVector = reflect(normal, -LightDirection);
	float4 specular_nDotL = saturate(dot(reflectionVector, directionToCamera));
	float SpecularMod = SpecularIntensity * pow(specular_nDotL, SpecularPower);

	float4 finalDiffuse = saturate(nDotL) * diffuse * DiffuseColor;
	float4 finalSpecular = specular * saturate(SpecularMod) * specular_nDotL *  (specular_nDotL > 0.0f ? float4(SpecularColor.xyz, 1.f) : (float4)0);

	LightPsOut output = (LightPsOut)0;
	float3 cVal = (finalDiffuse + finalSpecular).xyz;
	output.color = float4(cVal * Intensity, diffuse.a);
	output.depth = depth;

	return output;
}

technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_5_0 VertexShaderFunction();
        PixelShader = compile ps_5_0 PixelShaderFunction();
    }
}
