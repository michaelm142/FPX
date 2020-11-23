#ifndef _TEXTURES_H
#define _TEXTURES_H

texture2D DiffuseMap	: register(t0);
texture2D NormalMap		: register(t1);
texture2D SpecularMap	: register(t2);
texture2D DepthMap		: register(t3);
texture2D OcclusionMap	: register(t4);
texture2D HeightMap		: register(t5);
TextureCube Skybox		: register(t6);

SamplerState DiffuseMapSampler
{
    Texture = <DiffuseMap>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
	AddressU = Wrap;
	AddressV = Wrap;
};
SamplerState NormalMapSampler
{
    Texture = <NormalMap>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
	AddressU = Wrap;
	AddressV = Wrap;
};
SamplerState SpecularMapSampler
{
	Texture = <SpecularMap>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
	AddressU = Wrap;
	AddressV = Wrap;
};
SamplerState DepthMapSampler
{
	Texture = <DepthMap>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
	AddressU = Wrap;
	AddressV = Wrap;
};
SamplerState HeightMapSampler
{
	Texture = <HeightMap>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
	AddressU = Wrap;
	AddressV = Wrap;
};

samplerCUBE SkyboxSampler : register(s5);
#endif