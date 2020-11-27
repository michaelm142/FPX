#include "Headers//Camera.h"
#include "Headers//Textureing.h"
#include "Headers//GBuffers.h"

float2 iResolution;
float TerrainHeight;

GBufferVSOutput TerrainVS(in GBufferVSInput input)
{
	GBufferVSOutput output = (GBufferVSOutput)0;

	float4 worldPosition = mul(input.Position, World);
	int3 coord = int3(iResolution.x * input.uv.x, iResolution.y * input.uv.y, 0);
	float height = HeightMap.Load(coord).r * TerrainHeight;
	worldPosition.y += height;
	output.position = mul(worldPosition, ViewProjection);

	output.binormal = mul(input.binormal, (float3x3)World);
	output.normal = mul(input.Normal, (float3x3)World);
	output.uv = input.uv;
	output.depth = float4(output.position.z, output.position.w, 0, 0);

	return output;
}

GBufferPSOutput MainPS(GBufferVSOutput input)
{
	GBufferPSOutput output = (GBufferPSOutput)0;

	output.diffuse = float4(DiffuseMap.Sample(DiffuseMapSampler, input.uv).xyz, 1.0f);

	float3x3 tangentSpace = (float3x3)0;
	tangentSpace[2] = input.normal;
	tangentSpace[1] = input.binormal;
	tangentSpace[0] = cross(tangentSpace[1], tangentSpace[2]);

	float3 normal = NormalMap.Sample(NormalMapSampler, input.uv);
	normal = mul(normal, tangentSpace);

	output.normal = float4((normal + 1.f) / 2.f, 1.f);
	output.specular = SpecularMap.Sample(SpecularMapSampler, input.uv);
	output.depth.x = input.depth.x;
	output.depth.y = input.depth.y;

	return output;
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile vs_4_0 TerrainVS();
		PixelShader = compile ps_4_0 MainPS();
	}
};