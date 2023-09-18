// ---------------------------------------------------------------------------
// Includes
// ---------------------------------------------------------------------------

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

// ---------------------------------------------------------------------------
// Functions
// ---------------------------------------------------------------------------


// Computes the world space view direction (pointing towards the viewer).
float3 GetWorldSpaceViewDirection(float3 positionWS)
{
    if (unity_OrthoParams.w == 0)
    {
		// Perspective
        return _WorldSpaceCameraPos - positionWS;
    }
    else
    {
		// Orthographic
        float4x4 viewMat = GetWorldToViewMatrix();
        return viewMat[2].xyz;
    }
}

half3 GetWorldSpaceNormalizeViewDirection(float3 positionWS)
{
    float3 viewDir = GetWorldSpaceViewDirection(positionWS);
    if (unity_OrthoParams.w == 0)
    {
		// Perspective
        return half3(normalize(viewDir));
    }
    else
    {
		// Orthographic
        return half3(viewDir);
    }
}

// ---------------------------------------------------------------------------
// InputData : holds information about the position and orientation of the mesh at the current fragment
// ---------------------------------------------------------------------------

void InitializeInputData(Varyings IN, half3 normalTS, out InputData lightingInput)
{
    lightingInput = (InputData) 0; // avoids errors

    lightingInput.positionWS = IN.positionWS;

#ifdef _NORMALMAP
		half3 viewDirWS = half3(IN.normalWS.w, IN.tangentWS.w, IN.bitangentWS.w); // viewDir has been stored in w components of these in vertex shader
		lightingInput.normalWS = TransformTangentToWorld(normalTS, half3x3(IN.tangentWS.xyz, IN.bitangentWS.xyz, IN.normalWS.xyz));
#else
    half3 viewDirWS = GetWorldSpaceNormalizeViewDirection(lightingInput.positionWS);
    lightingInput.normalWS = IN.normalWS;
#endif

    lightingInput.normalWS = NormalizeNormalPerPixel(lightingInput.normalWS);

    viewDirWS = SafeNormalize(viewDirWS); //normalizes the values, can remove if it is expensive
    lightingInput.viewDirectionWS = viewDirWS;

#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
		lightingInput.shadowCoord = IN.shadowCoord;
#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
		lightingInput.shadowCoord = TransformWorldToShadowCoord(lightingInput.positionWS);
#else
    lightingInput.shadowCoord = float4(0, 0, 0, 0);
#endif

    lightingInput.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(IN.positionCS);
    //inputData.shadowMask = SAMPLE_SHADOWMASK(input.lightmapUV);
}