#include "Headers/LightParameters.h"
#include "Headers/Textureing.h"

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 diffuse = DiffuseMap.Sample(DiffuseMapSampler, input.uv);
	float4 finalDiffuse = diffuse * DiffuseColor;
    return finalDiffuse;
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
