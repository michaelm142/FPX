#include "..\\Headers\\LightParameters.h"
#include "..\\Headers\\Textureing.h"

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

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 diffuse = DiffuseMap.Sample(DiffuseSampler, input.uv);
	float4 finalDiffuse = diffuse * DiffuseColor;
    return finalDiffuse;
}

technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
