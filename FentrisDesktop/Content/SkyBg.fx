#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
float4 tint1;
float4 tint2;
float timeScale;
float distScale;
float time;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 texCol = tex2D(SpriteTextureSampler,input.TextureCoordinates);
	float2 center = float2(0.2126, 0.14371);
	float2 diff = input.TextureCoordinates;
	diff.x *= 3840.0/2679.0;

	float dist = distance(center, diff);
	float colorVar = (sin(dist * distScale + time * timeScale) + 1.0) / 2.0;
	float3 gradientColor = lerp(tint1.rgb, tint2.rgb, colorVar);
	
	if (texCol.r > 0.0)
	{
		return float4(gradientColor * clamp(texCol.r, 0.0, 1.0) * texCol.rgb, 1.0);
	}
	else
	{
		return texCol;
	}
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};