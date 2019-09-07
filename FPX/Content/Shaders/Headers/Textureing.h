#ifndef _TEXTURES_H
#define _TEXTURES_H

texture2D DiffuseMap	: register(t0);
texture2D NormalMap		: register(t0);
texture2D SpecularMap	: register(t0);
texture2D DepthMap		: register(t0);
TextureCube Skybox		: register(t0);

sampler2D DiffuseMapSampler : register(s0);

samplerCUBE SkyboxSampler : register(s0);
#endif