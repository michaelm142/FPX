#include "Headers/LightParameters.h"
#include "Headers/Textureing.h"

LightPsOut PixelShaderFunction(GB_VS_OUT input) : COLOR0
{
	float4 diffuse = DiffuseMap.Sample(DiffuseMapSampler, input.uv);
	float4 depth = DepthMap.Sample(DepthMapSampler, input.uv);
	if (depth.x / depth.y == 0.0f)
		discard;

	float4 finalDiffuse = diffuse * DiffuseColor;

	LightPsOut output = (LightPsOut)0;
	output.color.xyz = (finalDiffuse * Intensity).xyz;
	output.color.a = diffuse.a;
	output.depth = depth.x / depth.y;

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
