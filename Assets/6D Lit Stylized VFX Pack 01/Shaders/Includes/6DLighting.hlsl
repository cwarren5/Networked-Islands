#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

half SixDirectionLighting(half3 SixPointFront, half3 SixPointBack, half3x3 tangentMatrix, half3 Direction, half2 lightMultPow, half LightingSteps)
{
    half3 tangentLightDir = TransformWorldToTangent(Direction, tangentMatrix); // get tangent light dir
    half3 tangentLightDirLerp = ceil(saturate(tangentLightDir)); // and transform it to a 0-1 lerp for back/front
    float dirX = lerp(SixPointBack.x, SixPointFront.x, tangentLightDirLerp.x);
    float dirY = lerp(SixPointBack.y, SixPointFront.y, tangentLightDirLerp.y);
    float dirZ = lerp(SixPointBack.z, SixPointFront.z, tangentLightDirLerp.z);

    half lightContribution = saturate(dirX * pow(tangentLightDir.x, 2) + dirY * pow(tangentLightDir.y, 2) + dirZ * pow(tangentLightDir.z, 2)); //weighted contribution of light

    return round(saturate(pow(lightContribution * lightMultPow.x, lightMultPow.y)) * LightingSteps) / LightingSteps;
}


void MainLight_float(half3 SixPointFront, half3 SixPointBack, half3x3 tangentMatrix, half3 WorldPosition, half2 lightMultPow, half LightingSteps, out half3 Diffuse)
{
    #if SHADERGRAPH_PREVIEW
        Diffuse = 1;
    #else
        #if SHADOWS_SCREEN
            float4 clipPos = TransformWorldToHClip(WorldPosition);
            float4 shadowCoord = ComputeScreenPos(clipPos);
        #else
            float4 shadowCoord = TransformWorldToShadowCoord(WorldPosition);
        #endif
        Light mainLight = GetMainLight(shadowCoord);

        half sixDContribution = SixDirectionLighting(SixPointFront, SixPointBack, tangentMatrix, -mainLight.direction, lightMultPow, LightingSteps);

        Diffuse = mainLight.color * (mainLight.distanceAttenuation * mainLight.shadowAttenuation) * sixDContribution;
    #endif
    }

void MainLight_half(half3 SixPointFront, half3 SixPointBack, half3x3 tangentMatrix, half3 WorldPosition, half2 lightMultPow, half LightingSteps, out half3 Diffuse)
{
    #if SHADERGRAPH_PREVIEW
        Diffuse = 1;
    #else
        #if SHADOWS_SCREEN
            float4 clipPos = TransformWorldToHClip(WorldPosition);
            float4 shadowCoord = ComputeScreenPos(clipPos);
        #else
            float4 shadowCoord = TransformWorldToShadowCoord(WorldPosition);
        #endif
        Light mainLight = GetMainLight(shadowCoord);

        half sixDContribution = SixDirectionLighting(SixPointFront, SixPointBack, tangentMatrix, -mainLight.direction, lightMultPow, LightingSteps);

        Diffuse = mainLight.color * (mainLight.distanceAttenuation * mainLight.shadowAttenuation) * sixDContribution;
    #endif
    }
    

void AmbientColor_float(half3 SixPointFront, half3 SixPointBack, half3x3 tangentMatrix, half3 WorldPosition, half2 lightMultPow, half LightingSteps, out half2 VectorUpDown)
{
    #if SHADERGRAPH_PREVIEW
        VectorUpDown = 1;
    #else
        #if SHADOWS_SCREEN
            float4 clipPos = TransformWorldToHClip(WorldPosition);
            float4 shadowCoord = ComputeScreenPos(clipPos);
        #else
            float4 shadowCoord = TransformWorldToShadowCoord(WorldPosition);
        #endif

        half sixDContributionUp = SixDirectionLighting(SixPointFront, SixPointBack, tangentMatrix, float3(0,-1,0), lightMultPow, LightingSteps);
        half sixDContributionDown = SixDirectionLighting(SixPointFront, SixPointBack, tangentMatrix, float3(0,1,0), lightMultPow, LightingSteps);

        VectorUpDown.x =  sixDContributionUp;
        VectorUpDown.y =  sixDContributionDown;

    #endif
    }


void AmbientColor_half(half3 SixPointFront, half3 SixPointBack, half3x3 tangentMatrix, half3 WorldPosition, half2 lightMultPow, half LightingSteps, out half2 VectorUpDown)
{
    #if SHADERGRAPH_PREVIEW
        VectorUpDown = 1;
    #else
        #if SHADOWS_SCREEN
            float4 clipPos = TransformWorldToHClip(WorldPosition);
            float4 shadowCoord = ComputeScreenPos(clipPos);
        #else
            float4 shadowCoord = TransformWorldToShadowCoord(WorldPosition);
        #endif

        half sixDContributionUp = SixDirectionLighting(SixPointFront, SixPointBack, tangentMatrix, float3(0,-1,0), lightMultPow, LightingSteps);
        half sixDContributionDown = SixDirectionLighting(SixPointFront, SixPointBack, tangentMatrix, float3(0,1,0), lightMultPow, LightingSteps);

        VectorUpDown.x =  sixDContributionUp;
        VectorUpDown.y =  sixDContributionDown;

    #endif
    }


void AdditionalLights_float(half3 SixPointFront, half3 SixPointBack, half3x3 tangentMatrix, half3 WorldPosition, half2 lightMultPow, half LightingSteps, out half3 Diffuse)
{
    half3 diffuseColor = 0;

    #ifndef SHADERGRAPH_PREVIEW

        int pixelLightCount = GetAdditionalLightsCount();
        for (int i = 0; i < pixelLightCount; ++i)
        {
            Light light = GetAdditionalLight(i, WorldPosition);

            half sixDContribution = SixDirectionLighting(SixPointFront, SixPointBack, tangentMatrix, light.direction, lightMultPow, LightingSteps);

            half3 attenuatedLightColor = light.color * (light.distanceAttenuation * light.shadowAttenuation) * sixDContribution;

            diffuseColor += attenuatedLightColor;
        }
    #endif

    Diffuse = diffuseColor;
}

void AdditionalLights_half(half3 SixPointFront, half3 SixPointBack, half3x3 tangentMatrix, half3 WorldPosition, half2 lightMultPow, half LightingSteps, out half3 Diffuse)
{
    half3 diffuseColor = 0;

    #ifndef SHADERGRAPH_PREVIEW

        int pixelLightCount = GetAdditionalLightsCount();
        for (int i = 0; i < pixelLightCount; ++i)
        {
            Light light = GetAdditionalLight(i, WorldPosition);

            half sixDContribution = SixDirectionLighting(SixPointFront, SixPointBack, tangentMatrix, light.direction, lightMultPow, LightingSteps);

            half3 attenuatedLightColor = light.color * (light.distanceAttenuation * light.shadowAttenuation) * sixDContribution;

            diffuseColor += attenuatedLightColor;
        }
    #endif

    Diffuse = diffuseColor;
}

#endif
