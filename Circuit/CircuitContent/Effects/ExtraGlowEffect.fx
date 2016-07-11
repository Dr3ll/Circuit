float4x4 World;
float4x4 View;
float4x4 Projection;

texture GlowTexture;

float3 GlowColor1;
float3 GlowColor2;

sampler2D GlowSampler = sampler_state {
	texture = <GlowTexture>;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 UV : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float2 UV : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	output.Position = mul(input.Position, mul(World,
	mul(View, Projection)));

	output.UV = input.UV;

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 temp = tex2D(GlowSampler, input.UV);
	if(temp.r > 0.5f)
	{
		return float4(GlowColor1.r, GlowColor1.g, GlowColor1.b, 1);
	}

	if(temp.g > 0.5f)
	{
		return float4(GlowColor2.r, GlowColor2.g, GlowColor2.b, 1);
	}

	return temp;
}

technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}