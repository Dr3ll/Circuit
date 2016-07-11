float4x4 World;
float4x4 View;
float4x4 Projection;
float3 CameraPosition;

texture BasicTexture;

sampler BasicTextureSampler = sampler_state {
    texture = <BasicTexture>;
    MinFilter = Anisotropic; // Minification Filter
    MagFilter = Anisotropic; // Magnification Filter
    MipFilter = Linear; // Mip-mapping
    AddressU = Wrap; // Address Mode for U Coordinates
    AddressV = Wrap; // Address Mode for V Coordinates
};

bool TextureEnabled = false;

texture NormalMap;

sampler NormalMapSampler = sampler_state {
    texture = <NormalMap>;
    MinFilter = Anisotropic;
    MagFilter = Anisotropic;
    MipFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};

float3 DiffuseColor = float3(1, 1, 1);
float3 AmbientColor = float3(0.1, 0.1, 0.1);
//float3 LightDirection = float3(1, 1, 1);
float3 LightPosition = float3(1,1,1);
float3 LightColor = float3(0.9, 0.9, 0.9);
float SpecularPower = 32;
float3 SpecularColor = float3(1, 1, 1);

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float2 UV : TEXCOORD0;
	float3 Normal : NORMAL0;
	float3 Binormal : BINORMAL0;
	float3 Tangent: TANGENT0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float2 UV : TEXCOORD0;
    float3 ViewDirection : TEXCOORD2;
	float3 LightDirection : TEXCOORD3;
	float3x3 WorldToTangentSpace : TEXCOORD4;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
    
    float4 worldPosition = mul(input.Position, World);
    float4x4 viewProjection = mul(View, Projection);
    
    output.Position = mul(worldPosition, viewProjection);
	output.LightDirection = normalize(LightPosition - worldPosition);

    output.UV = input.UV;

    output.ViewDirection = worldPosition - CameraPosition;

	output.WorldToTangentSpace[0] = mul(normalize(input.Tangent), World);
	output.WorldToTangentSpace[1] = -mul(normalize(input.Binormal), World);
	output.WorldToTangentSpace[2] = mul(normalize(input.Normal), World);

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    // Start with diffuse color
    float3 color = DiffuseColor;

    // Texture if necessary
    if (TextureEnabled)
        color *= tex2D(BasicTextureSampler, input.UV);

	// Start with ambient lighting
	float3 lighting = AmbientColor;

	//float3 lightDir = normalize(LightDirection);
	//float3 lightDir = normalize(LightPosition - mul(input.Position, World));
	float3 lightDir = input.LightDirection;

	// Extract the normals from the normal map
	float3 normal = tex2D(NormalMapSampler, input.UV).rgb;
	normal = normal * 2 - 1; // Move from [0, 1] to [-1, 1] range
	normal = normalize(mul(normal, input.WorldToTangentSpace));

	// Add lambertian lighting
	lighting += saturate(dot(lightDir, normal)) * LightColor;

    float3 refl = reflect(lightDir, normal);
    float3 view = normalize(input.ViewDirection);
    
    // Add specular highlights
    lighting += pow(saturate(dot(refl, view)), SpecularPower) * SpecularColor;

    // Calculate final color
    float3 output = saturate(lighting) * color;

    return float4(output, 1);
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_1_1 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}