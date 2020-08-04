float4x4 World;
float4x4 ViewProjection;
float3 CameraForward;

#include "Headers//Textureing.h"
#include "Headers/MaterialInfo.h"

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float3 Normal : NORMAL0;
	float2 uv : TEXCOORD0;
	float3 binormal : BINORMAL0;
};

struct VertexShaderOutput
{
	float4 position : SV_Position;
	float4 depth : NORMAL1;
	float3 binormal : BINORMAL0;
	float3 normal : NORMAL0;
	float2 uv : TEXCOORD0;
};

struct PixelShaderOutput
{
	float4 diffuse :	SV_Target0;
	float4 normal :		SV_Target1;
	float4 specular :	SV_Target2;
	float4 depth :		SV_Target3;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	output.position = mul(worldPosition, ViewProjection);

	output.binormal = mul(input.binormal, (float3x3)World);
	output.normal = mul(input.Normal, (float3x3)World);
	output.uv = input.uv;
	output.depth = float4(output.position.z, output.position.w, 0, 0);

	return output;
}

PixelShaderOutput PixelShaderFunction(VertexShaderOutput input)
{
	PixelShaderOutput output;

	output.diffuse = DiffuseMap.Sample(DiffuseMapSampler, input.uv) * DiffuseColor;
	output.diffuse.a = 1.0f;

	float3x3 tangentSpace = (float3x3)0;
	tangentSpace[2] = input.normal;
	tangentSpace[1] = input.binormal;
	tangentSpace[0] = cross(tangentSpace[1], tangentSpace[2]);

	float3 normal = float3(0, 0, 1.f);
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

		VertexShader = compile vs_5_0 VertexShaderFunction();
		PixelShader = compile ps_5_0 PixelShaderFunction();
	}
}
