#include "Headers//Camera.h"
#include "Headers//Textureing.h"
#include "Headers/MaterialInfo.h"
#include "Headers//GBuffers.h"

GBufferPSOutput PixelShaderFunction(GBufferVSOutput input)
{
	GBufferPSOutput output;

	output.diffuse = DiffuseMap.Sample(DiffuseMapSampler, input.uv) * DiffuseColor;
	output.diffuse.a = 1.0f;

	float3x3 tangentSpace = (float3x3)0;
	tangentSpace[2] = input.normal;
	tangentSpace[1] = input.binormal;
	tangentSpace[0] = cross(tangentSpace[1], tangentSpace[2]);

	float3 normal = NormalMap.Sample(NormalMapSampler, input.uv) * Roughness;
	normal = mul(normal, tangentSpace);

	output.normal = float4((normal + 1.f) / 2.f * Roughness, 1.f);
	output.specular = float4(SpecularMap.Sample(SpecularMapSampler, input.uv).rgb * SpecularColor.rgb, 1.0f);
	output.depth.r = input.depth.x;
	output.depth.g = input.depth.y;
	output.depth.b = SpecularPower;
	output.depth.a = SpecularIntensity;

	return output;
}

technique Technique1
{
	pass Pass1
	{
		// TODO: set renderstates here.

		VertexShader = compile vs_5_0 GBufferVS();
		PixelShader = compile ps_5_0 PixelShaderFunction();
	}
}
