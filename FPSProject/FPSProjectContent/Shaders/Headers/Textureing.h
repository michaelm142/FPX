#ifndef _TEXTURES_H
#define _TEXTURES_H

texture2D DiffuseMap : TEXTURE0;
texture2D NormalMap : TEXTURE1;
texture2D SpecularMap : TEXTURE2;
texture2D DepthMap : TEXTURE3;

sampler DiffuseSampler : SAMPLER0 = sampler_state
{
    Texture   = <DiffuseMap>;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
};

sampler NormalSampler : SAMPLER1 = sampler_state
{
    Texture   = <NormalMap>;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
};

sampler SpecularSampler : SAMPLER2 = sampler_state
{
    Texture   = <SpecularMap>;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
};


sampler DepthSampler : SAMPLER3 = sampler_state
{
    Texture   = <DepthMap>;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
};

#endif